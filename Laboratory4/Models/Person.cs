using Laboratory4.Exceptions;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laboratory4.Models
{
    internal class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }

        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => _dateOfBirth = value;
        }

        public string DateOfBirthStr
        {
            get
            {
                return _dateOfBirth.ToLongDateString();
            }
        }

        private bool _isAdult;
        public bool IsAdult
        {
            get
            {
                return _isAdult;
            }
        }

        private string _sunSign;
        public string SunSign
        {
            get
            {
                return _sunSign;
            }
        }
        private string _chineseSign;
        public string ChineseSign
        {
            get
            {
                return _chineseSign;
            }
        }

        private bool _isBirthday;
        public bool IsBirthday
        {
            get
            {
                return _isBirthday;
            }
        }

        public Person(string firstName, string lastName, string? email, DateTime dateOfBirth, bool isBirthday, bool isAdult, string sunSign, string chineseSign)
        {
            if (email != null && !EmailIsValid(email))
                throw new InvalidEmailException("Email is invalid");
            if (Age(dateOfBirth) >= 135)
                throw new TooOldExcpetion("Your age is >= 135");
            if (Age(dateOfBirth) < 0)
                throw new NegativeAgeException("Your age is negative");
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = dateOfBirth;
            _isAdult = isAdult;
            _isBirthday = isBirthday;
            _sunSign = sunSign;
            _chineseSign = chineseSign;
        }

        public static int Age(DateTime dateOfBith)
        {
            if (DateTime.Today.Year == dateOfBith.Year)
            {
                if (DateTime.Today.Month > dateOfBith.Month || (DateTime.Today.Month == dateOfBith.Month && DateTime.Today.Day >= dateOfBith.Day))
                    return 0;
                return -1;
            }
            if (DateTime.Today.Month > dateOfBith.Month || (DateTime.Today.Month == dateOfBith.Month && DateTime.Today.Day >= dateOfBith.Day))
                return DateTime.Today.Year - dateOfBith.Year;
            return DateTime.Today.Year - dateOfBith.Year - 1;
        }

        private bool EmailIsValid(string email)
        {
            Regex regex = new Regex(@"\w+@\w+\.\w+");
            return regex.IsMatch(email);
        }

        public static bool CalcIsAdult(DateTime date)
        {
            return Age(date) >= 18;
        }

        public static string CalcSunSign(DateTime dateOfBirth)
        {
            if (dateOfBirth.Month == 3 && dateOfBirth.Day >= 21 || dateOfBirth.Month == 4 && dateOfBirth.Day <= 20)
                return "Aries ♈︎";
            if (dateOfBirth.Month == 4 && dateOfBirth.Day >= 21 || dateOfBirth.Month == 5 && dateOfBirth.Day <= 20)
                return "Taurus ♉︎";
            if (dateOfBirth.Month == 5 && dateOfBirth.Day >= 21 || dateOfBirth.Month == 6 && dateOfBirth.Day <= 21)
                return "Gemini ♊︎";
            if (dateOfBirth.Month == 6 && dateOfBirth.Day >= 22 || dateOfBirth.Month == 7 && dateOfBirth.Day <= 22)
                return "Cancer ♋︎";
            if (dateOfBirth.Month == 7 && dateOfBirth.Day >= 23 || dateOfBirth.Month == 8 && dateOfBirth.Day <= 22)
                return "Leo ♌︎";
            if (dateOfBirth.Month == 8 && dateOfBirth.Day >= 23 || dateOfBirth.Month == 9 && dateOfBirth.Day <= 22)
                return "Virgo ♍︎";
            if (dateOfBirth.Month == 9 && dateOfBirth.Day >= 23 || dateOfBirth.Month == 10 && dateOfBirth.Day <= 22)
                return "Libra ♎︎";
            if (dateOfBirth.Month == 10 && dateOfBirth.Day >= 23 || dateOfBirth.Month == 11 && dateOfBirth.Day <= 22)
                return "Scorpio ♏︎";
            if (dateOfBirth.Month == 11 && dateOfBirth.Day >= 23 || dateOfBirth.Month == 12 && dateOfBirth.Day <= 21)
                return "Sagittarius ♐︎";
            if (dateOfBirth.Month == 12 && dateOfBirth.Day >= 22 || dateOfBirth.Month == 1 && dateOfBirth.Day <= 20)
                return "Capricorn ♑︎";
            if (dateOfBirth.Month == 1 && dateOfBirth.Day >= 21 || dateOfBirth.Month == 2 && dateOfBirth.Day <= 19)
                return "Aquarius ♒︎";
            else
                return "Pisces ♓︎";
        }

        public static string CalcChineseSign(DateTime dateOfBirth)
        {
                switch (dateOfBirth.Year % 12)
                {
                    case 0: return "Monkey"; break;
                    case 1: return "Rooster"; break;
                    case 2: return "Dog"; break;
                    case 3: return "Pig"; break;
                    case 4: return "Rat"; break;
                    case 5: return "Ox"; break;
                    case 6: return "Tiger"; break;
                    case 7: return "Rabbit"; break;
                    case 8: return "Dragon"; break;
                    case 9: return "Snake"; break;
                    case 10: return "Horse"; break;
                    default: return "Goat"; break;
                }
        }

        public static bool CalcIsBirthDay(DateTime dateOfBirth)
        {
            return dateOfBirth.Month == DateTime.Today.Month && dateOfBirth.Day == DateTime.Today.Day;
        }
    }
}
