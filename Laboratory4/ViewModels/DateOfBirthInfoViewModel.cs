using Laboratory4.Models;
using System;
using Laboratory4.Tools;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Laboratory4.Navigation;
using System.Collections.ObjectModel;
using Laboratory4.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratory4.ViewModels
{
    internal class DateOfBirthInfoViewModel : INotifyPropertyChanged, INavigatable
    {
        private bool _isFiltered = false;
        private static PersonRepository _personRepository = new PersonRepository();
        private Action _goToCredentialsInput;
        private Action<PersonViewModel> _gotoRedactPerson;
        public event PropertyChangedEventHandler? PropertyChanged;
        private static ObservableCollection<PersonViewModel> _allPersons;
        public ObservableCollection<PersonViewModel> AllPersons
        {
            get
            {
                return _allPersons;
            }
            set
            {
                _allPersons = value;
                OnPropertyChanged();
            }
        }

        private static ObservableCollection<PersonViewModel> _persons;
        public ObservableCollection<PersonViewModel> Persons
        {
            get
            {
                return _persons;
            }
            set
            {
                _persons = value;
                OnPropertyChanged();
            }
        }

        public PersonViewModel SelectedPerson
        {
            get;
            set;
        }

        private string filterMessage = "Filter by IsAdult";
        public string FilterMessage
        {
            get
            {
                return filterMessage;
            }
            set 
            { 
                filterMessage = value; 
                OnPropertyChanged(); 
            }
        }

        public NavigationTypes ViewType
        {
            get
            {
                return NavigationTypes.DateOfBirthInfo;
            }
        }

        private RelayCommand<object> filterCommand;
        public RelayCommand<object> FilterCommand
        {
            get
            {
                return filterCommand ??= new RelayCommand<object>(o => Filter());
            }
        }

        private RelayCommand<object> backCommand;
        public RelayCommand<object> BackCommand
        {
            get
            {
                return backCommand ??= new RelayCommand<object>(o => GoToCredentialsInput());
            }
        }

        private RelayCommand<object> sortCommand;
        public RelayCommand<object> SortCommand
        {
            get
            {
                return sortCommand ??= new RelayCommand<object>(o => sortByLastName());
            }
        }

        private RelayCommand<object> redactCommand;
        public RelayCommand<object> RedactCommand
        {
            get
            {
                return redactCommand ??= new RelayCommand<object>(o => GoToRedact(), CanExecute);
            }
        }

        private RelayCommand<object> deleteCommand;
        public RelayCommand<object> DeleteCommand
        {
            get
            {
                return deleteCommand ??= new RelayCommand<object>(o => DeleteSelected(), CanExecute);
            }
        }

        public bool CanExecute(Object o)
        {
            return SelectedPerson != null;
        }

        public DateOfBirthInfoViewModel(Action goToCredentialsInput, Action<PersonViewModel> gotoRedactPerson, Action goToDateOfBirthInfo)
        {
            if(_allPersons == null)
            {
                _allPersons = new ObservableCollection<PersonViewModel>(new PersonRepository().GetAll(goToDateOfBirthInfo));
                _persons = new ObservableCollection<PersonViewModel>(_allPersons);
            }
            _goToCredentialsInput = goToCredentialsInput;
            _gotoRedactPerson = gotoRedactPerson;
        }

        public void GoToRedact()
        {
            _gotoRedactPerson.Invoke(SelectedPerson);
        }

        public void GoToCredentialsInput()
        {
            _goToCredentialsInput.Invoke();
        }

        public void Filter()
        {
            if(!_isFiltered)
            {
                var filteredPersons =
                    from p in _allPersons
                    where p.IsAdult
                    select p;
                Persons = new ObservableCollection<PersonViewModel>(filteredPersons.ToList());
                _isFiltered = true;
                FilterMessage = "Unfilter";
            }
            else
            {
                Persons = new ObservableCollection<PersonViewModel>(_allPersons);
                _isFiltered = false;
                FilterMessage = "Filter by IsAdult";
            }
        }

        public static void addPerson(PersonViewModel person)
        {
            if(_persons != null)
            {
                _persons.Add(person);
                _allPersons.Add(person);
            }
        }

        public async Task DeleteSelected()
        {
            await _personRepository.Delete(SelectedPerson.ThePerson);
            AllPersons.Remove(SelectedPerson);
            Persons.Remove(SelectedPerson);
            OnPropertyChanged();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void sortByLastName()
        {
            var sortedPersons =
                from p in _persons
                orderby p.LastName
                select p;
            _persons = new ObservableCollection<PersonViewModel>(sortedPersons.ToList());
            OnPropertyChanged(nameof(Persons));
        }
    }
}
