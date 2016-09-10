using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Common;
using Reminder.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Reminder.Wrappers;
using System.Linq;

namespace Reminder
{
    internal class PeopleDb
    {

        public PeopleDb()
        {
            using (var db = new DataConnection())
            {
                try { db.CreateTable<Person>(); } catch (Exception) { }
            }
        }

        public void AddPerson(Person person)
        {
            using (var db = new DataConnection())
            {
                db.Insert(person);
            }
        }

        public IEnumerable<PersonWrapper> GetPeople()
        {
            using (var db = new DataConnection())
            {
                var people = from p in db.GetTable <Person>()
                             select p;
                var peopleWrapped = people.Select(p => new PersonWrapper(p));
                return peopleWrapped;
            }
        }
    }
}
