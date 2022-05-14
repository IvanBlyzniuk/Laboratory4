using Laboratory4.Exceptions;
using Laboratory4.Models;
using Laboratory4.Navigation;
using Laboratory4.Repositories;
using Laboratory4.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Laboratory4.ViewModels
{
    internal class PersonViewModel : INavigatable
    {
        private Person _person;
        public Person ThePerson
        {
            get { return _person; }
        }
        private Action _goToDateOfBirthInfo;
        private static PersonRepository personRepository = new PersonRepository();

        private RelayCommand<object> acceptCommand;
        public RelayCommand<object> AcceptCommand
        {
            get
            {
                return acceptCommand ??= new RelayCommand<object>(o => GoToDateOfBirthInfo(), CanExecute);
            }
        }

        private RelayCommand<object> cancelCommand;
        public RelayCommand<object> CancelCommand
        {
            get
            {
                return cancelCommand ??= new RelayCommand<object>(o => Cancel());
            }
        }

        public string FirstName {
            get
            {
                return _person.FirstName;
            }
        }
        public string LastName {
            get
            {
                return _person.LastName;
            }
         }
        public string? Email
        { 
            get
            {
                return _person.Email;
            }
        }

        public DateTime DateOfBirth
        {
            get => _person.DateOfBirth;
            set => _person.DateOfBirth = value;
        }
        public string NewFirstName { get; set; }
        public string NewLastName { get; set; }

        private DateTime newDateOfBirth = DateTime.Today;
        public DateTime NewDateOfBirth
        {
            get
            {
                return newDateOfBirth;
            }
            set
            {
                newDateOfBirth = value;
            }
        }

        public string DateOfBirthStr
        {
            get
            {
                return _person.DateOfBirth.ToLongDateString();
            }
        }

        public bool IsAdult
        {
            get
            {
                return _person.IsAdult;
            }
        }

        public string SunSign
        {
            get
            {
                return _person.SunSign;
            }
        }

        public string ChineseSign
        {
            get
            {
                return _person.ChineseSign;
            }
        }

        public bool IsBirthday
        {
            get
            {
                return _person.IsBirthday;
            }
        }

        public NavigationTypes ViewType
        {
            get
            {
                return NavigationTypes.Person;
            }
        }

        public PersonViewModel(Person person, Action goToDateOfBirthInfo)
        {
            _person = person;
            _goToDateOfBirthInfo = goToDateOfBirthInfo;
        }

        private bool CanExecute(Object o)
        {
            return !String.IsNullOrWhiteSpace(NewFirstName) && !String.IsNullOrWhiteSpace(NewLastName);
        }

        public void Cancel()
        {
            _goToDateOfBirthInfo.Invoke();
        }

        public async Task GoToDateOfBirthInfo()
        {
            Person person;
            Task<bool> t1 = Task.Run(() => Person.CalcIsBirthDay(NewDateOfBirth));
            Task<bool> t2 = Task.Run(() => Person.CalcIsAdult(NewDateOfBirth));
            Task<string> t3 = Task.Run(() => Person.CalcSunSign(NewDateOfBirth));
            Task<string> t4 = Task.Run(() => Person.CalcChineseSign(NewDateOfBirth));
            bool isBirthday = await t1;
            bool isAdult = await t2;
            string sunSign = await t3;
            string chineseSign = await t4;
            try
            {
                person = new Person(NewFirstName, NewLastName, Email, NewDateOfBirth, isBirthday, isAdult, sunSign, chineseSign);
            }
            catch (InvalidEmailException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (NegativeAgeException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (TooOldExcpetion ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _person = person;
            await personRepository.AddOrUpdateAsync(_person);
            _goToDateOfBirthInfo.Invoke();
        }
    }
}
