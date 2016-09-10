using LinqToDB.Mapping;
using System;
using System.Globalization;

namespace Reminder.Model
{
    [Table("People")]
    public class Person
    {
        public Person(Person person)
        {
            this.Id = person.Id;
            this.Name = person.Name;
            this.Birthday = person.Birthday;
            this.Nameday = person.Nameday;
        }

        public Person() { }

        public Person(string name, DateTime birthday, DateTime nameday)
        {
            this.Name = name;
            this.Birthday = birthday;
            this.Nameday = nameday;
        }

        [PrimaryKey, Identity]
        public int Id { get; set; }
        [Column, NotNull]
        public string Name { get; set; }
        [Column, NotNull]
        public DateTime Birthday { get; set; }
        [Column, NotNull]
        public DateTime Nameday { get; set; }

        public override string ToString()
        {
            return $"{this.Name} Birthday:{this.Birthday.ToString("yyyy.MMMM.dd", CultureInfo.CurrentCulture)} Nameday:{this.Nameday.ToString("MMMM dd", CultureInfo.CurrentCulture)}";
        }
    }
}
