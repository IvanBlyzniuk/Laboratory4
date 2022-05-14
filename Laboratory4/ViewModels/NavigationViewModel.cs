using Laboratory4.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Laboratory4.ViewModels
{

    enum NavigationTypes
    {
        CredentialsInput = 1,
        DateOfBirthInfo = 2,
        Person = 3,
    }

    internal class NavigationViewModel: INotifyPropertyChanged
    {
        private List<INavigatable> _viewModels = new List<INavigatable>();

        public event PropertyChangedEventHandler? PropertyChanged;

        private INavigatable _currentViewModel;
        public INavigatable CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            { 
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public NavigationViewModel()
        {
            Navigate(NavigationTypes.DateOfBirthInfo);
        }

        internal void Navigate(NavigationTypes type)
        {
            if (CurrentViewModel != null && CurrentViewModel.ViewType == type)
            {
                return;
            }
                
            INavigatable viewModel = GetViewModel(type);
            if (viewModel == null)
                return;

            CurrentViewModel = viewModel;
        }

        internal void NavigateToPerson(PersonViewModel pvm)
        {
            CurrentViewModel = pvm;
        }

        private INavigatable GetViewModel(NavigationTypes type)
        {
            INavigatable viewModel = _viewModels.FirstOrDefault(vm => vm.ViewType == type);
            if (viewModel != null)
                return viewModel;
            switch(type)
            {
                case NavigationTypes.DateOfBirthInfo:
                    viewModel = new DateOfBirthInfoViewModel(() => Navigate(NavigationTypes.CredentialsInput), p => NavigateToPerson(p), () => Navigate(NavigationTypes.DateOfBirthInfo));
                    break;
                case NavigationTypes.CredentialsInput:
                    viewModel = new CredentialsInputViewModel(() => Navigate(NavigationTypes.DateOfBirthInfo));
                    break;
                default:
                    return null;
            }
            _viewModels.Add(viewModel);
            return viewModel;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
