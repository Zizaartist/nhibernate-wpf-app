using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using happy_water_carrier_test.Models;
using happy_water_carrier_test.Models.LocalModels;
using happy_water_carrier_test.Services.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace happy_water_carrier_test.ViewModels
{
    public class OrderViewModel : TemplateViewModel
    {
        private readonly IOrderDataAccess _orderDataAccess;

        public ObservableCollection<ListElementModel> Orders { get; } = new ObservableCollection<ListElementModel>();
        public ObservableCollection<ListElementModel> Employees { get; } = new ObservableCollection<ListElementModel>();
        private ObservableCollection<ListElementModel> Tags { get; } = new ObservableCollection<ListElementModel>();
        public ObservableCollection<ListElementModel> ExcludedTags { get; } = new ObservableCollection<ListElementModel>();

        private OrderLocal currentOrder = new OrderLocal();

        public OrderLocal CurrentOrder 
        {
            get => currentOrder;
            set 
            {
                var actualValue = value ?? new OrderLocal();
                if (currentOrder == actualValue) return;
                currentOrder = actualValue;
                OnPropertyChanged();
                RefreshExcludedTags();
            }
        }

        public RelayCommand Refresh { get; }
        public RelayCommand<ListElementModel> Get { get; }
        public RelayCommand Add { get; }
        public RelayCommand Update { get; }
        public RelayCommand Remove { get; }

        public RelayCommand<string> AddTag { get; }
        public RelayCommand<object[]> UpdateTag { get; }
        public RelayCommand<ListElementModel> RemoveTag { get; }
        public RelayCommand<IEnumerable<object>> IncludeTags { get; }
        public RelayCommand<IEnumerable<object>> ExcludeTags { get; }

        public OrderViewModel(IOrderDataAccess orderDataAccess)
        {
            _orderDataAccess = orderDataAccess;

            Refresh = new RelayCommand(() => BusyAction(() =>
            {
                RefreshOrdersAction();
                RefreshEmployeesAction();
                RefreshTagsAction();
            }), CanExecuteCommands);
            Get = new RelayCommand<ListElementModel>((param) => BusyAction(GetAction, param), CanExecuteCommands);
            Add = new RelayCommand(() => BusyAction(AddAction), CanExecuteCommands);
            Update = new RelayCommand(() => BusyAction(UpdateAction), CanExecuteCommands);
            Remove = new RelayCommand(() => BusyAction(RemoveAction), CanExecuteCommands);

            AddTag = new RelayCommand<string>((param) => BusyAction(AddTagAction, param), CanExecuteCommands);
            UpdateTag = new RelayCommand<object[]>((param) => BusyAction(UpdateTagAction, param), CanExecuteCommands);
            RemoveTag = new RelayCommand<ListElementModel>((param) => BusyAction(RemoveTagAction, param), CanExecuteCommands);
            IncludeTags = new RelayCommand<IEnumerable<object>>((param) => BusyAction(IncludeTagsAction, param), CanExecuteCommands);
            ExcludeTags = new RelayCommand<IEnumerable<object>>((param) => BusyAction(ExcludeTagsAction, param), CanExecuteCommands);

            OnBusyChanged += (_, _) => Refresh.NotifyCanExecuteChanged();
            OnBusyChanged += (_, _) => Get.NotifyCanExecuteChanged();
            OnBusyChanged += (_, _) => Add.NotifyCanExecuteChanged();
            OnBusyChanged += (_, _) => Update.NotifyCanExecuteChanged();
            OnBusyChanged += (_, _) => Remove.NotifyCanExecuteChanged();
            OnBusyChanged += (_, _) => AddTag.NotifyCanExecuteChanged();
            OnBusyChanged += (_, _) => UpdateTag.NotifyCanExecuteChanged();
            OnBusyChanged += (_, _) => RemoveTag.NotifyCanExecuteChanged();


        }

        private void RefreshExcludedTags()
        {
            ExcludedTags.Clear();
            foreach (var tag in Tags.Where(tag => !currentOrder.IncludedTags.Any(it => it.Id == tag.Id)))
                ExcludedTags.Add(tag);
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
                CurrentOrder = null;
            }
        }

        private void RefreshOrdersAction()
        {
            Orders.Clear();

            var subdivisionList = _orderDataAccess.GetIdList();
            foreach (var listElement in subdivisionList)
                Orders.Add(listElement);
        }

        private void RefreshEmployeesAction()
        {
            Employees.Clear();

            var defaultOption = new ListElementModel { Value = "-" };
            Employees.Add(defaultOption);
            CurrentOrder.Employee = defaultOption;

            var employeeList = _orderDataAccess.GetEmployeesIdList();
            foreach (var listElement in employeeList)
                Employees.Add(listElement);
        }

        private void RefreshTagsAction()
        {
            Tags.Clear();

            var tagsList = _orderDataAccess.GetTagsIdList();
            foreach (var listElement in tagsList)
                Tags.Add(listElement);
        }

        private void GetAction(ListElementModel selectedOrder)
        {
            if (selectedOrder != null)
            {
                var orderData = _orderDataAccess.Get(selectedOrder.Id);
                var localOrder = ConvertDataAccessToLocal(orderData);
                CurrentOrder = localOrder;
            }
        }

        private void AddAction()
        {
            var orderData = ConvertLocalToDataAccess(CurrentOrder);
            orderData.Id = 0;
            _orderDataAccess.Add(orderData);
            _orderDataAccess.SaveChanges();

            CurrentOrder.Id = orderData.Id;

            RefreshOrdersAction();
        }

        private void UpdateAction()
        {
            if (CurrentOrder.Id != 0)
            {
                var orderData = ConvertLocalToDataAccess(CurrentOrder);
                _orderDataAccess.Update(orderData);
                _orderDataAccess.SaveChanges();
            }
        }

        private void RemoveAction()
        {
            if (CurrentOrder.Id != 0)
            {
                _orderDataAccess.Remove(CurrentOrder.Id);
                _orderDataAccess.SaveChanges();

                CurrentListSelection = null;

                RefreshOrdersAction();
            }
        }

        private void AddTagAction(string name) 
        {
            if (string.IsNullOrEmpty(name)) return;

            var newTag = new Tag { Name = name };
            _orderDataAccess.AddTag(newTag);
            _orderDataAccess.SaveChanges();

            RefreshTagsAction();
            RefreshExcludedTags();
        }

        private void UpdateTagAction(object[] param) 
        {
            var selectedTag = param[0] as ListElementModel;
            var name = param[1] as string;

            if (selectedTag == null || string.IsNullOrEmpty(name)) return;

            var updatedTag = new Tag 
            { 
                Id = selectedTag.Id, 
                Name = name 
            };
            _orderDataAccess.UpdateTag(updatedTag);
            _orderDataAccess.SaveChanges();

            RefreshTagsAction();
            selectedTag.Value = name;
        }

        private void RemoveTagAction(ListElementModel toRemove) 
        {
            if (toRemove == null) return;

            _orderDataAccess.RemoveTag(toRemove.Id);
            _orderDataAccess.SaveChanges();

            RefreshTagsAction();
            RefreshExcludedTags();
        }

        private void IncludeTagsAction(IEnumerable<object> transferedTags)
        {
            var tags = transferedTags.Select(tag => tag as ListElementModel).ToList();
            foreach (var tag in tags)
            {
                var toRemove = ExcludedTags.FirstOrDefault(et => et.Id == tag.Id);
                ExcludedTags.Remove(toRemove);
                CurrentOrder.IncludedTags.Add(tag);
            }
        }

        private void ExcludeTagsAction(IEnumerable<object> transferedTags)
        {
            var tags = transferedTags.Select(tag => tag as ListElementModel).ToList();
            foreach (var tag in tags)
            {
                var toRemove = CurrentOrder.IncludedTags.FirstOrDefault(et => et.Id == tag.Id);
                CurrentOrder.IncludedTags.Remove(toRemove);
                ExcludedTags.Add(tag);
            }
        }

        private OrderLocal ConvertDataAccessToLocal(Order model) 
        {
            var result = new OrderLocal
            {
                Id = model.Id,
                ProductName = model.ProductName,
                Employee = Employees.FirstOrDefault(em => em.Id == model.EmployeeId) ?? Employees[0]
            };
            foreach (var tag in model.Tags)
                result.IncludedTags.Add(new ListElementModel
                {
                    Id = tag.Id,
                    Value = tag.Name
                });
            return result;
        } 

        private Order ConvertLocalToDataAccess(OrderLocal model) => new Order
        {
            Id = model.Id,
            ProductName = model.ProductName,
            EmployeeId = model.Employee != null && model.Employee.Id > 0 ? model.Employee.Id : null,
            Tags = model.IncludedTags.Select(tag => new Tag { Id = tag.Id }).ToList()
        };
    }
}