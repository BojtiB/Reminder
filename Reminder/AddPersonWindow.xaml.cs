using Reminder.Model;
using Reminder.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for AddPersonWindow.xaml
    /// </summary>
    public partial class AddPersonWindow : Window
    {
        private static readonly PeopleDb db = new PeopleDb();
        public AddPersonWindow()
        {
            InitializeComponent();
            this.SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            name.Text = "";
            birthdayPicker.SelectedDate = DateTime.Now;
            namedayPicker.SelectedDate = DateTime.Now;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (name.Text == null)
            {
                this.statusBarTextBlock.Text = "Please fill the name!";

            }
            else
            {
                var person = new Person(
                    name.Text,
                    birthdayPicker.SelectedDate.Value,
                    namedayPicker.SelectedDate.Value
                );
                this.statusBarTextBlock.Text = "Saving...";
                db.AddPerson(person);
                this.SetDefaultValues();
                this.statusBarTextBlock.Text = $"{person.ToString()} is saved!";
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
