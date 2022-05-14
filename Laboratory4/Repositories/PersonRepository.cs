using Laboratory4.Models;
using Laboratory4.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Laboratory4.Repositories
{
    internal class PersonRepository
    {
        private static readonly string BaseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Laboratory4_Blyzniuk");

        public PersonRepository()
        {
            if(!Directory.Exists(BaseFolder))
            {
                Directory.CreateDirectory(BaseFolder);
                for(int i = 1; i <= 50; i++)
                {
                    DateTime dateOfBirth = new DateTime(2022 - i, i % 12 + 1, i % 28 + 1);
                    string firstName;
                    string lastName;
                    if (i < 10)
                    {
                        firstName = $"User0{i}";
                        lastName = $"LastName0{i}";
                    }
                    else
                    {
                        firstName = $"User{i}";
                        lastName = $"LastName{i}";
                    }
                    Person curPerson = new Person(firstName, lastName, $"{firstName}@gmail.com", dateOfBirth, Person.CalcIsBirthDay(dateOfBirth), Person.CalcIsAdult(dateOfBirth), Person.CalcSunSign(dateOfBirth), Person.CalcChineseSign(dateOfBirth));
                    AddOrUpdateAsync(curPerson);
                }
            }
        }

        public async Task AddOrUpdateAsync(Person obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);

            using (var sw = new StreamWriter(Path.Combine(BaseFolder, obj.Email), false))
            {
                await sw.WriteAsync(stringObj);
            }
        }

        public async Task<Person> GetAsync(string email)
        {
            string stringObj = null;
            string filepath = Path.Combine(BaseFolder, email);

            if (!File.Exists(filepath))
                return null;

            using(var sr = new StreamReader(filepath))
            {
                stringObj = await sr.ReadToEndAsync();
            }

            return JsonSerializer.Deserialize<Person>(stringObj);
        }

        public List<PersonViewModel> GetAll(Action goToDateOfBirthInfo)
        {
            var res = new List<PersonViewModel>();
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string stringObj = null;
                using (var sr = new StreamReader(file))
                {
                    stringObj = sr.ReadToEnd();
                }

                res.Add(new PersonViewModel(JsonSerializer.Deserialize<Person>(stringObj), goToDateOfBirthInfo));
            }
            return res;
        }

        public async Task Delete(Person obj)
        {
            await Task.Run(() => File.Delete(Path.Combine(BaseFolder,obj.Email)));
        }
    }
}
