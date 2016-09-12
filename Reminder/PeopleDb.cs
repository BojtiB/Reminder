using Reminder.Model;
using System.Collections.Generic;
using Reminder.Wrappers;
using System.Linq;

namespace Reminder
{
    public class PeopleDb
    {

        public void AddPerson(Person person)
        {
            using (var db = new PeopleDbContext())
            {
                db.People.Add(person);
                db.SaveChanges();
            }
        }

        public IEnumerable<Person> GetPeople()
        {
            using (var db = new PeopleDbContext())
            {
                return db.People.ToList();
            }
        }

        public IEnumerable<PersonWrapper> GetPeopleWrapped()
        {
            return this.GetPeople().Select(p => new PersonWrapper(p));
        }

        public void UpdatePerson(Person person)
        {
            using (var db = new PeopleDbContext())
            {
                var original = db.People.Find(person.Id);
                db.Entry(original).CurrentValues.SetValues(person);
                db.SaveChanges();
            }
        }

        public void DeletePerson(Person person)
        {
            using (var db = new PeopleDbContext())
            {
                db.People.Attach(person);
                db.People.Remove(person);
                db.SaveChanges();
            }
        }
    }
}
