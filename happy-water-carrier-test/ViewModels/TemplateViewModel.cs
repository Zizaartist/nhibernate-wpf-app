﻿using CommunityToolkit.Mvvm.ComponentModel;
using happy_water_carrier_test.Models.LocalModels;
using System;

namespace happy_water_carrier_test.ViewModels
{
    public abstract class TemplateViewModel : ObservableObject
    {
        ~TemplateViewModel() 
        {
            foreach (var deleg in OnBusyChanged.GetInvocationList())
                OnBusyChanged -= (deleg as EventHandler<bool>);
        }

        protected abstract void OnCurrentListSelectionChanged(ListElementModel newSelection);

        protected event EventHandler<bool> OnBusyChanged;

        private ListElementModel currentListSelection;
        public ListElementModel CurrentListSelection
        {
            get => currentListSelection;
            set
            {
                if (currentListSelection == value) return;
                currentListSelection = value;
                OnPropertyChanged();
                OnCurrentListSelectionChanged(value);
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy == value) return;
                isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
                OnBusyChanged?.Invoke(null, value);
            }
        }

        public bool IsNotBusy
        {
            get => !IsBusy;
            set => IsBusy = !value;
        }

        protected void BusyAction(Action action)
        {
            IsBusy = true;
            try
            {
                action();
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected void BusyAction<T>(Action<T> action, T param)
        {
            IsBusy = true;
            try
            {
                action(param);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected bool CanExecuteCommands() => !IsBusy;
        protected bool CanExecuteCommands<T>(T _) => !IsBusy;
    }
}
