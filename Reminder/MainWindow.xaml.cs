using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using Reminder.Model;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    enum MainWindowStatus
    {
        ViewAll,
        UpcomingEvents
    }
    public partial class MainWindow : Window
    {
        private readonly PeopleDb db;
        private MainWindowStatus mainWindowStatus = MainWindowStatus.UpcomingEvents;

        public MainWindow(PeopleDb db)
        {
            this.db = db;
            InitializeComponent();
            this.RefreshUpcoming();
        }

        private void RefreshUpcoming()
        {
            this.RefreshUpcomingBirthDays();
            this.RefreshUpcomingNamedays();
        }

        private void RefreshUpcomingBirthDays()
        {
            this.dataGridBirthdays.ItemsSource = from pw in db.GetPeopleWrapped()
                                                 orderby pw.DayUntilBirthday
                                                 select pw;
        }

        private void RefreshUpcomingNamedays()
        {
            this.dataGridNamedays.ItemsSource = from pw in db.GetPeopleWrapped()
                                                orderby pw.DayUntilNameday
                                                select pw;
        }

        private void RefreshAllView()
        {
            this.dataGridAll.ItemsSource = db.GetPeople();
        }

        private void AddPeople_Clicked(object sender, RoutedEventArgs e)
        {
            var window = new AddOrUpdatePersonWindow(this.db);
            this.Visibility = Visibility.Hidden;
            window.ShowDialog();
            if (mainWindowStatus == MainWindowStatus.UpcomingEvents)
            {
                this.RefreshUpcoming();
            }
            else
            {
                this.RefreshAllView();
            }
            this.Visibility = Visibility.Visible;
        }

        private void UpcomingEvents_Clicked(object sender, RoutedEventArgs e)
        {
            if (mainWindowStatus != MainWindowStatus.UpcomingEvents)
            {
                this.dataGridAll.Visibility = Visibility.Hidden;
                this.dataGridBirthdays.Visibility = Visibility.Visible;
                this.dataGridNamedays.Visibility = Visibility.Visible;
                this.RefreshUpcoming();
                mainWindowStatus = MainWindowStatus.UpcomingEvents;
            }
        }

        private void ViewAll_Clicked(object sender, RoutedEventArgs e)
        {
            if (mainWindowStatus != MainWindowStatus.ViewAll)
            {
                this.dataGridBirthdays.Visibility = Visibility.Hidden;
                this.dataGridNamedays.Visibility = Visibility.Hidden;
                this.dataGridAll.Visibility = Visibility.Visible;
                this.RefreshAllView();
                mainWindowStatus = MainWindowStatus.ViewAll;
            }
        }

        private void Edit_Clicked(object sender, RoutedEventArgs e)
        {
            var person = ((FrameworkElement)sender).DataContext as Person;
            var window = new AddOrUpdatePersonWindow(this.db, person);
            this.Visibility = Visibility.Hidden;
            window.ShowDialog();
            this.Visibility = Visibility.Visible;
            this.RefreshAllView();
        }

        private void Delete_Clicked(object sender, RoutedEventArgs e)
        {
            var person = ((FrameworkElement)sender).DataContext as Person;
            var messageBoxResult = MessageBox.Show($"{Application.Current.Resources["are_you_sure_to_delete"]} {person.Name}?", $"{ Application.Current.Resources["please_confirm"]}", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                db.DeletePerson(person);
                MessageBox.Show($"{person} {Application.Current.Resources["deleted"]}");
                this.RefreshAllView();
            }
        }
    }
}
