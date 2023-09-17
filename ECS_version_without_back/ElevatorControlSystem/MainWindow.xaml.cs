using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ElevatorControlSystem
{
    public partial class MainWindow : Window
    {
        private TimeUpdater timeUpdater;
        private MediaPlayer mediaPlayer;
        private LoginWindow loginWindow;
        private TimeSpan savedMusicPosition;
        private DispatcherTimer timer;
        private Roles roles { get; set; }

        public enum Roles
        {
            Laboratory = 0,
            Production = 1,
            Accounting = 2,
            HR = 3,
            Director = 4,
            Def = 5,
            Dev = 6,
        }

        public MainWindow(Roles userRole)
        {
            InitializeComponent();
            InitializeUI();
            InitializeRole(userRole);
        
        }
        #region All initializer
        private void InitializeUI()
        {
            InitializeLoginWindow();
            InitializeWelcomingQuote();
            InitializeTimeUpdater();
            InitializeUserActivityMonitor();
            InitializeStateChangedHandler();
            InitializeMediaPlayer();
            InitializeUserTextBlock();
            InitializeTimer();
        }
        private void InitializeLoginWindow()
        {
            this.loginWindow = new LoginWindow();
        }
        private void InitializeWelcomingQuote()
        {
            tbForWelcomingQuote.Text = GoodWishes.GenerateGoodWishes();
        }
        private void InitializeTimeUpdater()
        {
            timeUpdater = new TimeUpdater(tbForTime, AppSettings.Default.Is24HourFormat);
        }
        private void InitializeUserActivityMonitor()
        {
            new UserActivityMonitor(StatusEllipse, this);
        }
        private void InitializeStateChangedHandler()
        {
            StateChanged += MainWindow_StateChanged;
        }
        private void InitializeMediaPlayer()
        {
            mediaPlayer = new MediaPlayer();
            PlayOrStopMusic(AppSettings.Default.IsMusicEnabled);
        }
        private void InitializeUserTextBlock()
        {
            InitializeUserWelcomingText();
            InitializeUserInfoText();
        }
        private void InitializeUserWelcomingText()
        {
            tbForUserWelcoming.Text = $"Добро пожаловать, {LoginWindow.UserFirstName}.";
        }
        private void InitializeUserInfoText()
        {
            tbForUserInfo.Text = $"{LoginWindow.UserLastName} {LoginWindow.UserFirstName}";
        }
        private void InitializeRole(Roles userRole)
        {
            roles = userRole;
            AppByRole(roles);
            if (roles == Roles.Director)
            {
                DirectorConfig();
            }
        }
        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        #endregion

        #region MusicSettings
        public void PlayOrStopMusic(bool isMusicEnabled)
        {
            if (isMusicEnabled)
            {
                PlayMusic();
            }
            else
            {
                StopMusic();
            }
        }


        public void PlayMusic(bool resumePlayback = false)
        {
            if (mediaPlayer.Source == null)
            {
                string musicFilePath = "Music\\Background_music.wav";
                string projectDirectory = System.IO.Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
                string fullMusicFilePath = System.IO.Path.Combine(projectDirectory, musicFilePath);
                mediaPlayer.Open(new Uri(fullMusicFilePath));
            }

            if (resumePlayback)
            {
                mediaPlayer.Position = savedMusicPosition;
            }

            mediaPlayer.Play();
        }

        public void StopMusic()
        {
            savedMusicPosition = mediaPlayer.Position;
            mediaPlayer.Stop();
        }

        public void SetMusicVolume(double volume)
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Volume = volume;
            }
        }
        #endregion

        #region TimeSettings
        public void UpdateTimeFormat(bool is24HourFormat)
        {

            bool wasRunning = timeUpdater.IsRunning;

            if (wasRunning)
            {
                timeUpdater.Stop();
            }

            timeUpdater = new TimeUpdater(tbForTime, is24HourFormat);

            if (wasRunning)
            {
                timeUpdater.Start();
            }
        }
        #endregion

        #region DateSettings
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            tbForDate.Text = currentDate.ToString("dd.MM.yyyy");
        }
        #endregion
        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = 0,
                    To = 1080,
                    Duration = TimeSpan.FromSeconds(0.5)
                };

                BeginAnimation(HeightProperty, animation);
            }
        }

        public void AppByRole(Roles role)
        {
            cbForChooseData.Items.Clear();
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
                case Roles.Dev:
                    DevConfig();
                    break;
                case Roles.Def:
                    DefaultConfig();
                    break;
            }
        }

       

        public void LaboratoryConfig()
        {
            StackPanelManager.ShowStackPanel(StackForAddNewLaboratoryCard);
            StackPanelManager.ShowStackPanel(StackForAddNewInvoce);
            AddUniqueItemToComboBox("Lab1");
            AddUniqueItemToComboBox("Lab2");

        }
        public void ProductionConfig()
        {
            StackPanelManager.ShowStackPanel(StackForAddNewRegister);
            StackPanelManager.ShowStackPanel(StackForAddNewCompletionReportProd);
            AddUniqueItemToComboBox("Prod1");
            AddUniqueItemToComboBox("Prod2");
        }
        public void AccountingConfig()
        {
            StackPanelManager.ShowStackPanel(StackForAddNewPrice);
            StackPanelManager.ShowStackPanel(StackForAddNewOutputInvoice);
        }
        public void HRConfig()
        {
            StackPanelManager.ShowStackPanel(StackForAddNewUser);
        }
        public void DirectorConfig()
        {
            StackForCreateAll.Visibility = Visibility.Collapsed;
        }

        public void DefaultConfig()
        {
            StackPanelManager.HideStackPanel(StackForAddNewInvoce);
            StackPanelManager.HideStackPanel(StackForAddNewLaboratoryCard);
            StackPanelManager.HideStackPanel(StackForAddNewRegister);
            StackPanelManager.HideStackPanel(StackForAddNewCompletionReportAcc);
            StackPanelManager.HideStackPanel(StackForAddNewCompletionReportProd);
            StackPanelManager.HideStackPanel(StackForAddNewPrice);
            StackPanelManager.HideStackPanel(StackForAddNewOutputInvoice);
            StackPanelManager.HideStackPanel(StackForAddNewUser);
            StackPanelManager.HideStackPanel(StackForCreateAll);
        }

        public void DevConfig()
        {
            AddUniqueItemToComboBox("Lab1");
            AddUniqueItemToComboBox("Lab2");
            AddUniqueItemToComboBox("Prod1");
            AddUniqueItemToComboBox("Prod2");
            AddUniqueItemToComboBox("etc");

            StackPanelManager.ShowStackPanel(StackForAddNewInvoce);
            StackPanelManager.ShowStackPanel(StackForAddNewLaboratoryCard);
            StackPanelManager.ShowStackPanel(StackForAddNewRegister);
            StackPanelManager.ShowStackPanel(StackForAddNewCompletionReportAcc);
            StackPanelManager.ShowStackPanel(StackForAddNewCompletionReportProd);
            StackPanelManager.ShowStackPanel(StackForAddNewPrice);
            StackPanelManager.ShowStackPanel(StackForAddNewOutputInvoice);
            StackPanelManager.ShowStackPanel(StackForAddNewUser);
        }

        private void roundedExpander_Expanded(object sender, RoutedEventArgs e)
        {
            StackPanelManager.HideStackPanel(StackForAddNewInvoce);
            StackPanelManager.HideStackPanel(StackForAddNewLaboratoryCard);
            StackPanelManager.HideStackPanel(StackForAddNewRegister);
            StackPanelManager.HideStackPanel(StackForAddNewCompletionReportAcc);
            StackPanelManager.HideStackPanel(StackForAddNewCompletionReportProd);
            StackPanelManager.HideStackPanel(StackForAddNewPrice);
            StackPanelManager.HideStackPanel(StackForAddNewOutputInvoice);
            StackPanelManager.HideStackPanel(StackForAddNewUser);
        }

        private void roundedExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            AppByRole(roles);
        }

        private void btnCreateNewInvoice_Click(object sender, RoutedEventArgs e)
        {

            ShowGridToCreate(GridForCreateInvioce);
        }

        private void btnCreateNewLaboratoryСard_Click(object sender, RoutedEventArgs e)
        {
            ShowGridToCreate(GridForCreateLaboratoryCard);
        }

        private void btnToViewData_Click(object sender, RoutedEventArgs e)
        {
            ShowGridToCreate(GridForViewData);
        }

        private void btnToViewNews_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View news", "Nice", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            ShowGridToCreate(GridForNews);
        }

        private void btnForSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new();
            settingWindow.Owner = this;
            settingWindow.Show();

        }

        private void btnForHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow help = new();
            help.Show();
        }

        private void btnForContact_Click(object sender, RoutedEventArgs e)
        {

            ContactUsWindow cswindow = new();
            cswindow.Show();
        }

        private void btnExitFromAcc_Click(object sender, RoutedEventArgs e)
        {
            loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
            StopMusic();

        }

        private void btnCreateNewRegister_Click(object sender, RoutedEventArgs e)
        {
            ShowGridToCreate(GridForCreateRegister);
        }

        private void btnCreateNewCompletionReportProd_Click(object sender, RoutedEventArgs e)
        {
            ShowGridToCreate(GridForCompletionReportProd);
        }

        private void btnCreateNewPrice_Click(object sender, RoutedEventArgs e)
        {
            ShowGridToCreate(GridForCreateNewPrice);
        }

        private void btnCreateNewOutputInvoice_Click(object sender, RoutedEventArgs e)
        {
            ShowGridToCreate(GridForCreateOutputInvoice);
        }

        private void btnAddNewUser_Click(object sender, RoutedEventArgs e)
        {
            ShowGridToCreate(GridForCreateNewUser);
        }
        private void btnCreateNewCompletionReportAcc_Click(object sender, RoutedEventArgs e)
        {
            ShowGridToCreate(GridForCompletionReportAcc);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();

        }

        public static void ShowGridToCreate(Grid gridToCreate)
        {
            if (gridToCreate == null)
                return;

            foreach (var child in LogicalTreeHelper.GetChildren(gridToCreate.Parent))
            {
                if (child is Grid grid)
                {
                    if (grid == gridToCreate)
                        grid.Visibility = Visibility.Visible;
                    else
                        grid.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void AddUniqueItemToComboBox(string item)
        {
            if (!cbForChooseData.Items.Contains(item))
            {
                cbForChooseData.Items.Add(item);
            }
        }
        private void btnGenaratePassByRole_Click(object sender, RoutedEventArgs e)
        {
            PasswordGenerator password = new PasswordGenerator();
            Roles role = password.ParseRole(cmbWithRoles.Text);
            tbForSavePass.Text = password.GeneratePassword(role);
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

        private async void btcCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(tbForSavePass.Text);
            tbCopiedNotification.Visibility = Visibility.Visible;
            await Task.Delay(TimeSpan.FromSeconds(1.5));
            tbCopiedNotification.Visibility = Visibility.Collapsed;
        }


        private void cbReadOnlyForGrid_Checked(object sender, RoutedEventArgs e)
        {
            dg.IsReadOnly = false;
            StackForEditInDataGrid.Visibility = Visibility.Visible;
        }

        private void cbReadOnlyForGrid_Unchecked(object sender, RoutedEventArgs e)
        {
            dg.IsReadOnly = true;
            StackForEditInDataGrid.Visibility = Visibility.Hidden;
        }

    

        private void btnToSearchInGrid_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void btnToSaveInfoFromDataGrid_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Saved!");
        }
   

        private void cbForChooseData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}

