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
    public partial class AddOrUpdatePersonWindow : Window
    {
        private readonly PeopleDb db;
        private bool update = false;
        private int personId;
        public AddOrUpdatePersonWindow(PeopleDb db, Person person = null)
        {
            this.db = db;
            InitializeComponent();
            
            if (person != null)
            {
                update = true;
            }
            this.SetDefaultValues(person);
        }

        private void SetDefaultValues(Person person = null)
        {
            if (update)
            {
                this.personId = person.Id;
                this.name.Text = person.Name;
                this.birthdayPicker.SelectedDate = person.Birthday;
                this.namedayPicker.SelectedDate = person.Nameday;
            }
            else
            {
                this.name.Text = "";
                this.birthdayPicker.SelectedDate = DateTime.Now;
                this.namedayPicker.SelectedDate = DateTime.Now;
            }
            this.name.Focus();
        }

        /// TODO MAGYARÍTANI
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (name.Text == null)
            {
                this.statusBarTextBlock.Text = $"{Application.Current.Resources["please_fill_the_name"]}";
            }
            else
            {
                var person = new Person(
                    name.Text,
                    birthdayPicker.SelectedDate.Value,
                    namedayPicker.SelectedDate.Value
                );
                this.statusBarTextBlock.Text = $"{Application.Current.Resources["saving"]}";

                if (update)
                {
                    person.Id = this.personId;
                    db.UpdatePerson(person);
                    MessageBox.Show($"{person.ToString()} {Application.Current.Resources["updated"]}");
                    this.Close();
                }
                else
                {
                    db.AddPerson(person);
                    this.SetDefaultValues();
                    this.statusBarTextBlock.Text = $"{person.ToString()} {Application.Current.Resources["saved"]}";
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
