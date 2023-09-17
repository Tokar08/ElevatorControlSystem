using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using static ElevatorControlSystem.MainWindow;

namespace ElevatorControlSystem
{


    public class DatabaseManager
    {
        private string connectionString = @"Server=.\SQLEXPRESS;Database=GrainElevatorDB;Trusted_Connection=True;Encrypt=False";

        public async Task<(Roles Role, string FirstName, string LastName)> GetUserRoleAsync(string username, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT role, first_name, last_name FROM Users WHERE email = @Email AND password = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", username);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                Roles role = Enum.TryParse(reader["role"].ToString(), out Roles parsedRole) ? parsedRole : Roles.Def;
                                string firstName = reader["first_name"].ToString();
                                string lastName = reader["last_name"].ToString();

                                return (role, firstName, lastName);
                            }
                            else
                            {
                                return (Roles.Def, null, null);
                            }
                        }
                    }
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
            dbManager = new DatabaseManager();
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
