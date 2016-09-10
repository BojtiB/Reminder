using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly PeopleDb db = new PeopleDb();

        public MainWindow()
        {
            InitializeComponent();
            this.RefreshUpcoming();
        }

        private void AddPeople_Clicked(object sender, RoutedEventArgs e)
        {
            var window = new AddPersonWindow();
            window.ShowDialog();
            this.RefreshUpcoming();
        }

        private void RefreshUpcoming()
        {
            this.RefreshUpcomingBirthDays();
            this.RefreshUpcomingNamedays();
        }
        private void RefreshUpcomingBirthDays()
        {
            this.dataGridBirthdays.ItemsSource = from pw in db.GetPeople()
                                                 orderby pw.DayUntilBirthday
                                                 select pw;
        }
        private void RefreshUpcomingNamedays()
        {
            this.dataGridNamedays.ItemsSource = from pw in db.GetPeople()
                                                orderby pw.DayUntilNameday
                                                select pw;
        }
    }
}
