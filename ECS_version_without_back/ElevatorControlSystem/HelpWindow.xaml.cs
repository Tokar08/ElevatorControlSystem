using ElevatorControlSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ElevatorControlSystem
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppDefect> AppDefects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=AppDefectsDB;Trusted_Connection=True;Encrypt=False");
        }
    }
    public partial class HelpWindow : Window
    {
        static LoginWindow login = new();
        MainWindow window = new MainWindow(login.userRole);
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void btnExitFromHelp_Click(object sender, RoutedEventArgs e)
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

        private async void bntSaveDsc_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var appDefect = new AppDefect
                {
                    Description = tbDescribeProblem.Text,
                    CreatedDate = DateTime.Now,
                    CreatedBy = window.tbForUserInfo.Text,
                    CompanyName = window.tbNameOfCompany.Text,
                    Status = true
                };


                using (var dbContext = new AppDbContext())
                {
                    dbContext.AppDefects.Add(appDefect);
                    dbContext.SaveChanges();
                }

                MessageBox.Show("Запись отправлена.\nСпасибо, что помогаете нам становится лучше!", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                btnExitFromHelp_Click(sender, e);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

