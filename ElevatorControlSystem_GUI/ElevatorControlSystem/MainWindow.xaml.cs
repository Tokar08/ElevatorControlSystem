using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ElevatorControlSystem.MainWindow;

namespace ElevatorControlSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isExpanded = true;
        public enum Roles
        {
            Laboratory = 0,
            Production = 1,
            Accounting = 2,
            HR = 3,
            Director = 4,
        }
        public Roles roles { get; set; } = Roles.Production;
        public MainWindow()
        {
            InitializeComponent();
            InitializeUI();


        }
        private void InitializeUI()
        {
            tbForUserWelcoming.Text = "Добро пожаловать, Василий.";
            tbForWelcomingQuote.Text = GoodWishes.GenerateGoodWishes();
            new TimeUpdater(tbForTime, true);
            new UserActivityMonitor(StatusEllipse);


            if (roles == Roles.Director)
            {
                DirectorConfig();
            }
        }
        public void AppByRole(Roles role)
        {
            switch (role)
            {
                case Roles.Laboratory:
                    LaboratoryConfig();
                    break;
                case Roles.Production:
                    ProductionConfig();
                    break;
                case Roles.Accounting:
                    AccountingConfig();
                    break;
                case Roles.HR:
                    HRConfig();
                    break;
                case Roles.Director:
                    DirectorConfig();
                    break;
            }
        }


        public void LaboratoryConfig()
        {
            StackForAddNewLaboratoryCard.Visibility = Visibility.Visible;
            StackForAddNewInvoce.Visibility = Visibility.Visible;
        }
        public void ProductionConfig()
        {
            StackForAddNewRegister.Visibility = Visibility.Visible;
            StackForAddNewDepotItem.Visibility = Visibility.Visible;
            StackForAddNewCompletionReport.Visibility = Visibility.Visible;
        }
        public void AccountingConfig() 
        {
            StackForAddNewPrice.Visibility = Visibility.Visible;
            StackForAddNewOutputInvoice.Visibility = Visibility.Visible;
        }
        public void HRConfig() 
        {
        StackForAddNewUser.Visibility = Visibility.Visible;
        }
        public void DirectorConfig()
        {
           StackForCreateAll.Visibility = Visibility.Collapsed;
        }


        private void btnExitFromApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnCollapse_Click(object sender, RoutedEventArgs e)
        {
            if (isExpanded)
            {
                Width = 1920;
                Height = 1079;
                ExpandOrCollapse.Source = new BitmapImage(new Uri("Images/Expand.png", UriKind.Relative));
            }
            else
            {
                Width = 1920;
                Height = 1080;
                ExpandOrCollapse.Source = new BitmapImage(new Uri("Images/Collapse.png", UriKind.Relative));
            }

            isExpanded = !isExpanded;
        }
        private void roundedExpander_Expanded(object sender, RoutedEventArgs e)
        {
            AppByRole(roles);
        }

        private void roundedExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            StackForAddNewInvoce.Visibility = Visibility.Collapsed;
            StackForAddNewLaboratoryCard.Visibility = Visibility.Collapsed;
            StackForAddNewRegister.Visibility = Visibility.Collapsed;
            StackForAddNewDepotItem.Visibility = Visibility.Collapsed;
            StackForAddNewCompletionReport.Visibility = Visibility.Collapsed;
            StackForAddNewPrice.Visibility = Visibility.Collapsed;
            StackForAddNewOutputInvoice.Visibility = Visibility.Collapsed;
            StackForAddNewUser.Visibility = Visibility.Collapsed;
        }

      
  




        private void btnCreateNewInvoice_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New Invoice","Nice",MessageBoxButton.OK,MessageBoxImage.Asterisk);
        }

        private void btnCreateNewLaboratoryСard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New Laboratory Сard", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void btnToViewData_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View data", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void btnToViewNews_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View news", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void btnForSetting_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Setting", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void btnForHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Help", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void btnForContact_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Contact us", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);

        }

        private void btnExitFromAcc_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Exit from acc", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void btnCreateNewRegister_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Register", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void btnCreateNewDepotItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DepotItem", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void btnCreateNewCompletionReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("CompletionReport", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void btnCreateNewPrice_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Price", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void btnCreateNewOutputInvoice_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("OutputInvoice", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void btnAddNewUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("User", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }


}
