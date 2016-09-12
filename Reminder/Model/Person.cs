using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Windows;

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

        [Key]
        public int Id { get; set; }
        [Column, Required]
        public string Name { get; set; }
        [Column, Required]
        public DateTime Birthday { get; set; }
        [Column, Required]
        public DateTime Nameday { get; set; }

        public override string ToString()
        {
            return $"{this.Name} {Application.Current.Resources["birthday"]}: {this.Birthday.ToString("yyyy.MMMM.dd", CultureInfo.CurrentCulture)} "+
                $"{Application.Current.Resources["nameday"]}: {this.Nameday.ToString("MMMM dd", CultureInfo.CurrentCulture)}";
        }
    }
}
