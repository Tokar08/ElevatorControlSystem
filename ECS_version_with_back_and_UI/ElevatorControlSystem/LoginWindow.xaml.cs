using GrainElevatorCS_ef.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using static ElevatorControlSystem.MainWindow;

namespace ElevatorControlSystem
{
    public class DatabaseManager
    {
        private Db dbContext;

        public DatabaseManager(Db context)
        {
            dbContext = context;
        }

        public async Task<(Roles Role, string FirstName, string LastName)> GetUserRoleAsync(string username, string password)
        {
            try
            {
                var user = await dbContext.Users
                    .Where(u => u.Email == username && u.Password == password)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    Roles role = user.Role; // Предполагается, что у модели User есть свойство Role.
                    string firstName = user.FirstName;
                    string lastName = user.LastName;

                    return (role, firstName, lastName);
                }
                else
                {
                    return (Roles.Def, null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке учетных данных: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return (Roles.Def, null, null);
            }
        }
    }
    public partial class LoginWindow : Window
    {
        public DatabaseManager dbManager;
        public static string UserFirstName { get; private set; }
        public static string UserLastName { get; private set; }

        public Roles userRole { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
            StateChanged += MainWindow_StateChanged;

            Db context = new Db();
            dbManager = new DatabaseManager(context);
        }

        private async void OpenApp_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = pbUserPassword.Password;

            (userRole, string firstName, string lastName) = await dbManager.GetUserRoleAsync(username, password);

            if (userRole != Roles.Def)
            {
                UserFirstName = firstName;
                UserLastName = lastName;
                MainWindow mainWindow = new MainWindow(userRole);
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверные логин или пароль. Попробуйте еще раз.", "Ошибка входа!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void btnExitFromApp_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation closeAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            closeAnimation.Completed += (s, _) => Close();

            BeginAnimation(OpacityProperty, closeAnimation);
        }

        private void btnCollapse_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = ActualHeight,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            animation.Completed += (s, _) => WindowState = WindowState.Minimized;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                BeginAnimation(HeightProperty, animation);
            }));
        }
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = 0,
                    To = 600,
                    Duration = TimeSpan.FromSeconds(0.5)
                };

                BeginAnimation(HeightProperty, animation);
            }
        }

        private void btnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            pbUserPassword.Visibility = Visibility.Collapsed;
            btnShowPassword.Visibility = Visibility.Collapsed;

            btnUnShowPassword.Visibility = Visibility.Visible;
            tbUserPassword.Visibility = Visibility.Visible;

            tbUserPassword.Text = pbUserPassword.Password;
        }
        private void btnUnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            pbUserPassword.Visibility = Visibility.Visible;
            btnShowPassword.Visibility = Visibility.Visible;

            btnUnShowPassword.Visibility = Visibility.Collapsed;
            tbUserPassword.Visibility = Visibility.Collapsed;

            pbUserPassword.Password = tbUserPassword.Text;
        }
    }
}
//Запрос на добавление пользователей: 
//INSERT INTO dbo.users (firstName, lastName, birthDate, email, phone, password, gender, city, country, role)
//VALUES
//    ('Vasia', 'Petrov', '2023-01-07', 'vasia@gmail.com', '123433245', 'zxc', 'M', 'Lviv', 'Ukraine', 4), --Director
//    ('Petya', 'Ivanov', '2023-01-06', 'petya@gmail.com', '5323231232', 'cxz', 'M', 'Lviv', 'Ukraine', 1), --Production
//    ('Olga', 'Sidorova', '2023-01-05', 'olga@gmail.com', '314242342', 'ewq', 'F', 'Kiev', 'Ukraine', 0), --Laboratory
//    ('Anna', 'Kuznetsova', '2023-01-04', 'anna@gmail.com', '213123142', 'asd', 'F', 'Kiev', 'Belarus', 2), --Accounting
//    ('Alex', 'Smirnov', '2023-01-03', 'alex@gmail.com', '213123213', 'dsa', 'M', 'Lviv', 'Ukraine', 3), --HR
//    ('Ivan', 'Fedorov', '2023-01-02', 'Ivan@gmail.com', '123321123', 'qwe', 'M', 'Kiev', 'Ukraine', 6); --Dev
