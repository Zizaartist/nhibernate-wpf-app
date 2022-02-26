using CommunityToolkit.Mvvm.Input;
using happy_water_carrier_test.Models;
using happy_water_carrier_test.Models.LocalModels;
using happy_water_carrier_test.Services.DataAccess;
using System.Collections.ObjectModel;
using System.Linq;

namespace happy_water_carrier_test.ViewModels
{
    public class SubdivisionViewModel : TemplateViewModel
    {
        private readonly ISubdivisionDataAccess _subdivisionDataAccess;

        public ObservableCollection<ListElementModel> Subdivisions { get; } = new ObservableCollection<ListElementModel>();
        public ObservableCollection<ListElementModel> Employees { get; } = new ObservableCollection<ListElementModel>();

        // Никогда не null, нужно куда-то записывать данные с полей
        private SubdivisionLocal currentSubdivision = new SubdivisionLocal();
        public SubdivisionLocal CurrentSubdivision
        {
            get => currentSubdivision;
            set
            {
                var actualValue = value ?? new SubdivisionLocal();
                if (currentSubdivision == actualValue) return;
                currentSubdivision = actualValue;
                OnPropertyChanged();
            }
        }

        public RelayCommand Refresh { get; }
        public RelayCommand<ListElementModel> Get { get; }
        public RelayCommand Add { get; }
        public RelayCommand Update { get; }
        public RelayCommand Remove { get; }


        public SubdivisionViewModel(ISubdivisionDataAccess subdivisionDataAccess)
        {
            _subdivisionDataAccess = subdivisionDataAccess;

            Refresh = new RelayCommand(() => BusyAction(() =>
            {
                RefreshSubdivisionsAction();
                RefreshEmployeesAction();
            }), CanExecuteCommands);
            Get = new RelayCommand<ListElementModel>((param) => BusyAction(GetAction, param), CanExecuteCommands);
            Add = new RelayCommand(() => BusyAction(AddAction), CanExecuteCommands);
            Update = new RelayCommand(() => BusyAction(UpdateAction), CanExecuteCommands);
            Remove = new RelayCommand(() => BusyAction(RemoveAction), CanExecuteCommands);
        }

        protected override void OnBusyChanged(bool value)
        {
            Refresh?.NotifyCanExecuteChanged();
            Get?.NotifyCanExecuteChanged();
            Add?.NotifyCanExecuteChanged();
            Update?.NotifyCanExecuteChanged();
            Remove?.NotifyCanExecuteChanged();
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
                CurrentSubdivision = null;
            }
        }

        private void RefreshSubdivisionsAction()
        {
            Subdivisions.Clear();

            var subdivisionList = _subdivisionDataAccess.GetIdList();
            foreach (var listElement in subdivisionList)
                Subdivisions.Add(listElement);
        }

        private void RefreshEmployeesAction()
        {
            Employees.Clear();

            var defaultOption = new ListElementModel { Value = "-" };
            Employees.Add(defaultOption);
            CurrentSubdivision.Director = defaultOption;

            var employeeList = _subdivisionDataAccess.GetEmployeesIdList();
            foreach (var listElement in employeeList)
                Employees.Add(listElement);
        }

        private void GetAction(ListElementModel selectedSubdivision)
        {
            if (selectedSubdivision != null)
            {
                var employeeData = _subdivisionDataAccess.Get(selectedSubdivision.Id);
                var localEmployee = ConvertDataAccessToLocal(employeeData);
                CurrentSubdivision = localEmployee;
            }
        }

        private void AddAction()
        {
            var subdivisionData = ConvertLocalToDataAccess(CurrentSubdivision);
            subdivisionData.Id = 0;
            _subdivisionDataAccess.Add(subdivisionData);
            _subdivisionDataAccess.SaveChanges();

            CurrentSubdivision.Id = subdivisionData.Id;

            RefreshSubdivisionsAction();
        }

        private void UpdateAction()
        {
            if (CurrentSubdivision.Id != 0)
            {
                var employeeData = ConvertLocalToDataAccess(CurrentSubdivision);
                _subdivisionDataAccess.Update(employeeData);
                _subdivisionDataAccess.SaveChanges();
            }
        }

        private void RemoveAction()
        {
            if (CurrentSubdivision.Id != 0)
            {
                _subdivisionDataAccess.Remove(CurrentSubdivision.Id);
                _subdivisionDataAccess.SaveChanges();

                CurrentListSelection = null;

                RefreshSubdivisionsAction();
            }
        }

        private SubdivisionLocal ConvertDataAccessToLocal(Subdivision model) => new SubdivisionLocal
        {
            Id = model.Id,
            Name = model.Name,
            Director = Employees.FirstOrDefault(em => em.Id == model.DirectorId) ?? Employees[0]
        };

        private Subdivision ConvertLocalToDataAccess(SubdivisionLocal model) => new Subdivision
        {
            Id = model.Id,
            Name = model.Name,
            DirectorId = model.Director != null && model.Director.Id > 0 ? model.Director.Id : null
        };
    }
}
