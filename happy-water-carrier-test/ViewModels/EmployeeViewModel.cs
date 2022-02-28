using CommunityToolkit.Mvvm.Input;
using happy_water_carrier_test.Models;
using happy_water_carrier_test.Models.Enum;
using happy_water_carrier_test.Models.LocalModels;
using happy_water_carrier_test.Services.DataAccess;
using System.Collections.ObjectModel;
using System.Linq;

namespace happy_water_carrier_test.ViewModels
{
    public class EmployeeViewModel : TemplateViewModel
    {
        private readonly IEmployeeDataAccess _employeeDataAccess;

        public ObservableCollection<ListElementModel> Employees { get; } = new ObservableCollection<ListElementModel>();
        public ObservableCollection<ListElementModel> Subdivisions { get; } = new ObservableCollection<ListElementModel>();

        // Никогда не null, нужно куда-то записывать данные с полей
        private EmployeeLocal currentEmployee = new EmployeeLocal { Gender = -1 };
        public EmployeeLocal CurrentEmployee
        {
            get => currentEmployee;
            set 
            {
                var actualValue = value ?? new EmployeeLocal { Gender = -1 };
                if (currentEmployee == actualValue) return;
                currentEmployee = actualValue;
                OnPropertyChanged();
            }
        }

        public RelayCommand Refresh { get; }
        public RelayCommand<ListElementModel> Get { get; }
        public RelayCommand Add { get; }
        public RelayCommand Update { get; }
        public RelayCommand Remove { get; }

        public EmployeeViewModel(IEmployeeDataAccess employeeDataAccess)
        {
            _employeeDataAccess = employeeDataAccess;

            Refresh = new RelayCommand(() => BusyAction(() => 
            { 
                RefreshEmployeesAction();
                RefreshSubdivisionsAction();
            }), CanExecuteCommands);
            Get = new RelayCommand<ListElementModel>((param) => BusyAction(GetAction, param), CanExecuteCommands);
            Add = new RelayCommand(() => BusyAction(AddAction), CanExecuteCommands);
            Update = new RelayCommand(() => BusyAction(UpdateAction), CanExecuteCommands);
            Remove = new RelayCommand(() => BusyAction(RemoveAction), CanExecuteCommands);

            OnBusyChanged += (_, _) => Refresh.NotifyCanExecuteChanged();
            OnBusyChanged += (_, _) => Get.NotifyCanExecuteChanged();
            OnBusyChanged += (_, _) => Add.NotifyCanExecuteChanged();
            OnBusyChanged += (_, _) => Update.NotifyCanExecuteChanged();
            OnBusyChanged += (_, _) => Remove.NotifyCanExecuteChanged();
        }

        protected override void OnCurrentListSelectionChanged(ListElementModel newSelection)
        {
            if (newSelection != null)
            {
                if (Get.CanExecute(newSelection))
                    Get.Execute(newSelection);
            }
            else
            {
                CurrentEmployee = null;
            }
        }

        private void RefreshEmployeesAction()
        {
            Employees.Clear();

            // Нагрузка из-за большого количества вызовов notifyOnPropertyChanged
            // в Xamarin можно было ObservableRangeCollection использовать для оптимизации
            // а в WPF пока не нашел альтернатив

            var employeeList = _employeeDataAccess.GetIdList();
            foreach (var listElement in employeeList)
                Employees.Add(listElement);
        }

        private void RefreshSubdivisionsAction()
        {
            Subdivisions.Clear();

            var defaultOption = new ListElementModel { Value = "-" };
            Subdivisions.Add(defaultOption);
            CurrentEmployee.Subdivision = defaultOption;

            var subdivisionList = _employeeDataAccess.GetSubdivisionsIdList();
            foreach (var listElement in subdivisionList)
                Subdivisions.Add(listElement);
        }

        private void GetAction(ListElementModel selectedEmployee)
        {
            if (selectedEmployee != null)
            {
                var employeeData = _employeeDataAccess.Get(selectedEmployee.Id);
                var localEmployee = ConvertDataAccessToLocal(employeeData);
                CurrentEmployee = localEmployee;
            }
        }

        private void AddAction()
        {
            var employeeData = ConvertLocalToDataAccess(CurrentEmployee);
            employeeData.Id = 0;
            _employeeDataAccess.Add(employeeData);
            _employeeDataAccess.SaveChanges();

            CurrentEmployee.Id = employeeData.Id;

            RefreshEmployeesAction();
        }

        private void UpdateAction()
        {
            if (CurrentEmployee.Id != 0)
            {
                var employeeData = ConvertLocalToDataAccess(CurrentEmployee);
                _employeeDataAccess.Update(employeeData);
                _employeeDataAccess.SaveChanges();
            }
        }

        private void RemoveAction()
        {
            if (CurrentEmployee.Id != 0)
            {
                _employeeDataAccess.Remove(CurrentEmployee.Id);
                _employeeDataAccess.SaveChanges();

                CurrentListSelection = null;

                RefreshEmployeesAction();
            }
        }

        private EmployeeLocal ConvertDataAccessToLocal(Employee model) => new EmployeeLocal
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PatronymicName = model.PatronymicName,
            DateOfBirth = model.DateOfBirth,
            Gender = ((int?) model.Gender) ?? -1,
            Subdivision = Subdivisions.FirstOrDefault(su => su.Id == model.SubdivisionId) ?? Subdivisions[0]
        };

        private Employee ConvertLocalToDataAccess(EmployeeLocal model) => new Employee
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PatronymicName = model.PatronymicName,
            DateOfBirth = model.DateOfBirth,
            Gender = model.Gender == -1 ? null : ((Gender) model.Gender),
            SubdivisionId = model.Subdivision != null && model.Subdivision.Id > 0 ? model.Subdivision.Id : null
        };
    }
}
