using Reminder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminder.Wrappers
{
    public class PersonWrapper
    {
        private Person person;
        private int dayUntilBirthday;
        private int dayUntilNameday;

        public PersonWrapper(Person person)
        {
            this.person = person;

            this.dayUntilBirthday = this.CalculateDayUntilEvent(person.Birthday);
            this.dayUntilNameday = this.CalculateDayUntilEvent(person.Nameday);
        }

        public string Name
        {
            get { return this.person.Name; }
            private set { }
        }

        public int Age
        {
            get { return DateTime.Now.Year - this.person.Birthday.Year; }
        }
        public string Birthday
        {
            get { return this.person.Birthday.ToString("yyyy-MM-dd"); }
            private set { }
        }

        public string Nameday
        {
            get { return this.person.Nameday.ToString("MMMM dd"); }
            private set { }
        }

        public int DayUntilBirthday
        {
            get { return this.dayUntilBirthday; }

            private set { }
        }

        public int DayUntilNameday
        {
            get { return this.dayUntilNameday; }

            private set { }
        }

        private int CalculateDayUntilEvent(DateTime date)
        {
            int deduction = 0;
            var nowYear = DateTime.Now.Year;
            if (!DateTime.IsLeapYear(nowYear))
            {
                if (date.Month == 2 && date.Day == 29)
                {
                    deduction = 1;
                }
            }
            var dayOfYearEvent = new DateTime(
                    nowYear,
                    date.Month,
                    date.Day - deduction).DayOfYear;

            var dayOfYearNow = DateTime.Now.DayOfYear;
            int untilNumberOfDays = 0;
            if (dayOfYearNow > dayOfYearEvent)
            {
                untilNumberOfDays = dayOfYearEvent + (new DateTime(nowYear, 12, 31).DayOfYear) - dayOfYearNow;
            }
            else
            {
                untilNumberOfDays = dayOfYearEvent - dayOfYearNow;
            }
            return untilNumberOfDays;
        }
    }
}
