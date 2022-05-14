using Laboratory4.Exceptions;
using Laboratory4.Models;
using Laboratory4.Navigation;
using Laboratory4.Repositories;
using Laboratory4.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Laboratory4.ViewModels
{
    internal class CredentialsInputViewModel : INavigatable
    {
        private static PersonRepository personRepository = new PersonRepository();

        private Action _goToDateOfBirthInfo;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public bool IsEnabled { get { return false; } set { IsEnabled = value; } }

        private DateTime dateOfBirth = DateTime.Today;
        public DateTime DateOfBirth{
            get
            {
                return dateOfBirth;
            }
            set
            {
                dateOfBirth = value;
            }
        }

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

        public NavigationTypes ViewType
        {
            get
            {
                return NavigationTypes.CredentialsInput;
            }
        }

        private bool CanExecute(Object o)
        {
            return !String.IsNullOrWhiteSpace(FirstName) && !String.IsNullOrWhiteSpace(LastName) && !String.IsNullOrWhiteSpace(Email);
        }

        public CredentialsInputViewModel(Action goToDateOfBirthInfo)
        {
            _goToDateOfBirthInfo = goToDateOfBirthInfo;
        }

        public void Cancel()
        {
            _goToDateOfBirthInfo.Invoke();
        }

        public async Task GoToDateOfBirthInfo()
        {
            Person person;
            Task<bool> t1 = Task.Run(() => Person.CalcIsBirthDay(DateOfBirth));
            Task<bool> t2 = Task.Run(() => Person.CalcIsAdult(DateOfBirth));
            Task<string> t3 = Task.Run(() => Person.CalcSunSign(DateOfBirth));
            Task<string> t4 = Task.Run(() => Person.CalcChineseSign(DateOfBirth));
            bool isBirthday = await t1;
            bool isAdult = await t2;
            string sunSign = await t3;
            string chineseSign = await t4;
            try
            {
                person = new Person(FirstName, LastName, Email, DateOfBirth, isBirthday, isAdult, sunSign, chineseSign);
            }
            catch (InvalidEmailException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(NegativeAgeException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch(TooOldExcpetion ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
            await personRepository.AddOrUpdateAsync(person);
            DateOfBirthInfoViewModel.addPerson(new PersonViewModel(person,_goToDateOfBirthInfo));
            //MessageBox.Show(CanExecute(person).ToString());
            _goToDateOfBirthInfo.Invoke();
        }


    }
}
