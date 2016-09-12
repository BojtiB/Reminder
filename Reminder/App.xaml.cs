using Reminder.Wrappers;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace Reminder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly PeopleDb db = new PeopleDb();

        private readonly BackgroundWorker worker = new BackgroundWorker();
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.SetLanguageDictionary();
            MainWindow = new MainWindow(this.db);
            MainWindow.Closing += MainWindow_Closing;

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            _notifyIcon.Icon = Reminder.Properties.Resources.AppIcon;
            _notifyIcon.Visible = true;

            this.CreateContextMenu();

            this.worker.DoWork += Worker_DoWork;
            this.worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                var birhtdays = from pw in db.GetPeopleWrapped()
                                where pw.DayUntilBirthday < 3
                                select pw;

                var namedays = from pw in db.GetPeopleWrapped()
                               where pw.DayUntilNameday < 3
                               select pw;

                StringBuilder stringBuilder = new StringBuilder();
                if (birhtdays.Count() > 0)
                {

                    stringBuilder.AppendLine($"{Application.Current.Resources["upcoming_birthdays"]}");
                    foreach (var bd in birhtdays)
                    {
                        stringBuilder.AppendLine($"{bd.Name} {bd.DayUntilBirthday} {Application.Current.Resources["day_left_"]}!");
                    }

                    stringBuilder.AppendLine();
                }

                if (namedays.Count() > 0)
                {
                    stringBuilder.AppendLine($"{Application.Current.Resources["upcoming_namedays"]}");

                    foreach (var nd in namedays)
                    {
                        stringBuilder.AppendLine($"{nd.Name} {nd.DayUntilNameday} {Application.Current.Resources["day_left_"]}!");
                    }
                }


                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"{Application.Current.Resources["remind_you_later"]}");

                if (namedays.Count() > 0 || birhtdays.Count() > 0)
                {
                    if (MessageBox.Show(stringBuilder.ToString(), $"{ Application.Current.Resources["reminder"]}", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        break;
                    }
                }

                // Remind every 20th minutes
                Thread.Sleep(1000 * 60 * 20);
            }
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "hu-HU":
                    dict.Source = new Uri("..\\Resources\\StringResources.hu-HU.xaml",
                                       UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml",
                                      UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip =
              new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add($"{Application.Current.Resources["upcoming_events"]}").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add($"{Application.Current.Resources["exit"]}").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            MainWindow.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
            App.Current.Shutdown();
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                MainWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
        }

    }
}
