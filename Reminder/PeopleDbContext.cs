using Reminder.Model;
using System.Data.Entity;

namespace Reminder
{
    public class PeopleDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
    }
}
