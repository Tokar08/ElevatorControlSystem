using ElevatorControlSystem.ControlsManager;
using ElevatorControlSystem.Repozitories;
using ElevatorControlSystem.SettingsModel;
using GrainElevatorCS_ef;
using GrainElevatorCS_ef.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Key = System.Windows.Input.Key;
using TextBox = System.Windows.Controls.TextBox;

namespace ElevatorControlSystem
{
    public partial class MainWindow : System.Windows.Window
    {
        private TimeUpdater timeUpdater;
        private MediaPlayer mediaPlayer;
        private LoginWindow loginWindow;
        private TimeSpan savedMusicPosition;
        private DispatcherTimer timer;

        private Roles roles { get; set; }

        private static Db db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions());
        private SupplierRepository supplierRep = new(db);
        private ProductTitleRepository productTitleRep = new(db);
        private InputInvoiceRepository inputInvoiceRep = new(db);
        private LaboratoryCardRepository labCardRep = new(db);
        private RegisterRepository registerRep = new(db);
        private CompletionReportRepository completionReportRep = new(db);
        private PriceListRepository priceListRepository = new(db);
        private DepotItemRepository depotItemRep = new(db);
        private OutputInvoiceRepository outputInvoiceRep = new(db);
        private UserRepository userRepository = new(db);
        private DataGridManager gridManager = new();

        private HashSet<int> usedLaboratoryCardIds = new HashSet<int>();
        private List<StackPanelWithEllipse> stackPanelsWithEllipses = new List<StackPanelWithEllipse>();
        private Dictionary<string, int> supplierDictionary = new Dictionary<string, int>();
        private Dictionary<string, int> productDictionary = new Dictionary<string, int>();
        private KeyBindingsManager keyBindingsManager;
        private PriceList pl;


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
        private Dictionary<string, string> genderMapping = new Dictionary<string, string>
        {
            { "Мужской", "Male" },
            { "Женский", "Female" }
        };
        private Dictionary<string, Roles> rolesMapping = new Dictionary<string, Roles>
        {
            { "Лаборатория", Roles.Laboratory },
            { "Отдел кадров", Roles.HR },
            { "Производственный отдел", Roles.Production },
            { "Бухгалтерия", Roles.Accounting }
        };

        public MainWindow(Roles userRole)
        {
            InitializeComponent();
            InitializeUI();
            InitializeRole(userRole);

        }
        #region All initializer
     
        private async void InitializeUI()
        {
            InitializeLoginWindow();
            InitializeWelcomingQuote();
            InitializeTimeUpdater();
            InitializeUserActivityMonitor();
            InitializeStateChangedHandler();
            InitializeMediaPlayer();
            InitializeUserTextBlock();
            InitializeTimer();
            InitializeListViewForSuppliersAndProduct();
            InitializeBar();
            InitializeStackPanel();
            InitializeKeyBindings();

        }
        private void InitializeKeyBindings()
        {
            keyBindingsManager = new KeyBindingsManager();

            keyBindingsManager.AddBinding(new KeyGesture(Key.D1, ModifierKeys.Control), () => btnCreateNewInvoice_Click(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.D2, ModifierKeys.Control), () => btnCreateNewLaboratoryСard_Click(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.D3, ModifierKeys.Control), () => btnCreateNewRegister_Click(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.D4, ModifierKeys.Control), () => btnCreateNewCompletionReportProd_Click(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.D5, ModifierKeys.Control), () => btnCreateNewPrice_Click(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.D6, ModifierKeys.Control), () => btnCreateNewCompletionReportAcc_Click(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.D7, ModifierKeys.Control), () => btnCreateNewOutputInvoice_Click(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.D8, ModifierKeys.Control), () => btnAddNewUser_Click(null, null));

            keyBindingsManager.AddBinding(new KeyGesture(Key.Q, ModifierKeys.Control), () => roundedExpander_Expanded(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.W, ModifierKeys.Control), () => roundedExpander_Collapsed(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.E, ModifierKeys.Control), () => btnToViewData_Click(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.R, ModifierKeys.Control), () => btnToViewNews_Click(null, null));

            keyBindingsManager.AddBinding(new KeyGesture(Key.Up, ModifierKeys.Control), () => roundedExpander_Expanded(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.Down, ModifierKeys.Control), () => roundedExpander_Collapsed(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.Left, ModifierKeys.Control), () => btnToViewData_Click(null, null));
            keyBindingsManager.AddBinding(new KeyGesture(Key.Right, ModifierKeys.Control), () => btnToViewNews_Click(null, null));


            keyBindingsManager.RegisterBindings(this);
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
            try
            {
                timeUpdater = new TimeUpdater(tbForTime, AppSettings.Settings.Is24HourFormat);
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Ошибка инициализации TimeUpdater: {ex.Message}");
            }
        }

        private void InitializeMediaPlayer()
        {
            try
            {
                mediaPlayer = new MediaPlayer();
                PlayOrStopMusic(AppSettings.Settings.IsMusicEnabled);
            }
            catch (Exception ex)
            {

                NotificationManager.ShowErrorMessageBox($"Ошибка инициализации MediaPlayer: {ex.Message}");
            }
        }

        private void InitializeUserActivityMonitor()
        {
            new UserActivityMonitor(StatusEllipse, this);
        }
        private void InitializeStateChangedHandler()
        {
            StateChanged += MainWindow_StateChanged;
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

        private void InitializeStackPanel()
        {
            stackPanelsWithEllipses.Add(new StackPanelWithEllipse(StackForAddNewInvoce, EllipseInNewInvoice));
            stackPanelsWithEllipses.Add(new StackPanelWithEllipse(StackForAddNewLaboratoryCard, EllipseInLaboratoryCard));
            stackPanelsWithEllipses.Add(new StackPanelWithEllipse(StackForAddNewRegister, EllipseInRegister));
            stackPanelsWithEllipses.Add(new StackPanelWithEllipse(StackForAddNewCompletionReportProd, EllipseInCompletionReportProd));
            stackPanelsWithEllipses.Add(new StackPanelWithEllipse(StackForAddNewPrice, EllipseInNewPrice));
            stackPanelsWithEllipses.Add(new StackPanelWithEllipse(StackForAddNewCompletionReportAcc, EllipseInNewCompletionReportAcc));
            stackPanelsWithEllipses.Add(new StackPanelWithEllipse(StackForAddNewOutputInvoice, EllipseInNewOutputInvoice));
            stackPanelsWithEllipses.Add(new StackPanelWithEllipse(StackForAddNewUser, EllipseInNewUser));
            stackPanelsWithEllipses.Add(new StackPanelWithEllipse(StackToViewData, EllipseInViewData));
            stackPanelsWithEllipses.Add(new StackPanelWithEllipse(StackToViewAnalyze, EllipseInViewAnalyze));
        }
        private async void InitializeBar()
        {
            List<ProductTitle> products = await productTitleRep.GetAllAsync();
            List<InputInvoice> invoices = await inputInvoiceRep.GetAllAsync();

            if (products.Count == 0 || invoices.Count == 0)
            {
                List<double> defaultTickValues = Enumerable.Repeat(100.00, 5).ToList();
                List<double> defaultData = Enumerable.Range(1, 5).Select(_ => (double)new Random().Next(1, 150)).ToList();
                List<string> defaultCategories = Enumerable.Range(1, 5).Select(i => $"Пример категории №{i}").ToList();

                barChart.Add(defaultTickValues.ToArray(), defaultCategories.ToArray(), defaultData.ToArray());
                return;
            }

            List<double> TickValues = new List<double>();
            List<double> Data = new List<double>();
            List<string> Categories = new List<string>();
            List<int> newMaxValues = new List<int>();

            foreach (var product in products)
            {
                int sumWeight = (int)invoices
                    .Where(invoice => invoice.ProductTitle.Id == product.Id)
                    .Select(invoice => (double)invoice.PhysicalWeight / 1000)
                    .Sum();

                newMaxValues.Add(100 < sumWeight ? (int)(sumWeight * 1.2) : 100);
                TickValues.Add(100);
                Data.Add(sumWeight);
                Categories.Add(product.Title);
            }

            var maxTickValue = (double)Math.Ceiling((decimal)newMaxValues.Max() / 100) * 100;
            var tickValuesArray = Enumerable.Repeat(maxTickValue, products.Count).ToArray();

            barChart.Add(tickValuesArray, Categories.ToArray(), Data.ToArray());
        }


        private async void InitializeListViewForSuppliersAndProduct()
        {
            using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
            {
                var supplierRepContext = new SupplierRepository(db);
                var productTitleRepContext = new ProductTitleRepository(db);

                var supplierItems = await supplierRepContext.GetAllAsync();
                var productTitleItems = await productTitleRepContext.GetAllAsync();

                supplierItems.ForEach(item => supplierDictionary.Add(item.Title, item.Id));
                productTitleItems.ForEach(item => productDictionary.Add(item.Title, item.Id));


            }

        }

        private async void InitializeListViewForSelectInputInvoice()
        {
            using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
            {

                InputInvoiceRepository inputInvoiceRepContext = new InputInvoiceRepository(db);
                List<InputInvoice> inputInvoices = await inputInvoiceRepContext.GetAllAsync();
                lvForSelectInputInvoice.ItemsSource = inputInvoices;

            }
        }

        private async void InitializelvForSelectLaboratoryCard()
        {
            using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
            {
                var laboratoryCardRepContext = new LaboratoryCardRepository(db);

                var laboratoryCards = await laboratoryCardRepContext.GetAllAsync();

                lvForSelectLaboratoryCard.ItemsSource = laboratoryCards;

            }
        }
        private async void InitializeListViewForRegister()
        {
            using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
            {
                RegisterRepository registerRepContext = new RegisterRepository(db);

                List<Register> registers = await registerRepContext.GetAllAsync();
                lvForSelectRegister.ItemsSource = registers;
            }
        }
        private async void InitializeListViewForSelectCompletionReport()
        {
            using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
            {
                var completionReportRepContext = new CompletionReportRepository(db);
                var completionReports = await completionReportRepContext.GetAllAsync();

                lvForSelectCompletionReport.ItemsSource = completionReports;

            }
        }
        private async void InitializeForSelectPriceList()
        {
            using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
            {
                var priceListRepositoryContext = new PriceListRepository(db);
                lvForSelectPriceList.ItemsSource = await priceListRepositoryContext.GetAllAsync();
            }
        }
        private async void InitializeListViewForSelectDepoItem()
        {
            using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
            {
                var supplierRepContext = new SupplierRepository(db);
                var productTitleRepContext = new ProductTitleRepository(db);

                var depoItemses = new DepotItemRepository(db);
                var depoItems = await depoItemses.GetAllAsync();

                lvForSelectDepoItem.ItemsSource = depoItems;

            }
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
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewLaboratoryCard);
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewInvoce);
            AddUniqueItemToComboBox("Входные накладные");
            AddUniqueItemToComboBox("Лабораторные карточки");

        }
        public void ProductionConfig()
        {
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewRegister);
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewCompletionReportProd);
            AddUniqueItemToComboBox("Произ. партия");
            AddUniqueItemToComboBox("Реестры");
            AddUniqueItemToComboBox("Акт доработки");
        }
        public void AccountingConfig()
        {
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewPrice);
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewOutputInvoice);
            AddUniqueItemToComboBox("Акт доработки");
            AddUniqueItemToComboBox("Тех. оперции");
            AddUniqueItemToComboBox("Цены операций");
            AddUniqueItemToComboBox("Прайс-лист");
            AddUniqueItemToComboBox("Складские единицы");
            AddUniqueItemToComboBox("Категории складских единиц");
            AddUniqueItemToComboBox("Расходные накладные");
        }
        public void HRConfig()
        {
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewUser);
            AddUniqueItemToComboBox("Пользователи");
        }
        public void DirectorConfig()
        {
            BorderManager.ShowBorder(BorderForBarButtons);
            StackPanelManager.HideStackPanel(StackForCreateAll);
            AddUniqueItemToComboBox("Входные накладные");
            AddUniqueItemToComboBox("Лабораторные карточки");
            AddUniqueItemToComboBox("Произ. партия");
            AddUniqueItemToComboBox("Реестры");
            AddUniqueItemToComboBox("Акт доработки");
            AddUniqueItemToComboBox("Тех. оперции");
            AddUniqueItemToComboBox("Цены операций");
            AddUniqueItemToComboBox("Прайс-лист");
            AddUniqueItemToComboBox("Складские единицы");
            AddUniqueItemToComboBox("Категории складских единиц");
            AddUniqueItemToComboBox("Расходные накладные");
            AddUniqueItemToComboBox("Продукция");
            AddUniqueItemToComboBox("Поставщики");
            AddUniqueItemToComboBox("Пользователи");
        }

        public void DefaultConfig()
        {
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewInvoce);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewLaboratoryCard);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewRegister);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewCompletionReportAcc);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewCompletionReportProd);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewPrice);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewOutputInvoice);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewUser);
            StackPanelManager.HideStackPanelWithAnimation(StackForCreateAll);
        }

        public void DevConfig()
        {
            BorderManager.ShowBorder(BorderForBarButtons);
            AddUniqueItemToComboBox("Входные накладные");
            AddUniqueItemToComboBox("Лабораторные карточки");
            AddUniqueItemToComboBox("Произ. партия");
            AddUniqueItemToComboBox("Реестры");
            AddUniqueItemToComboBox("Акт доработки");
            AddUniqueItemToComboBox("Тех. оперции");
            AddUniqueItemToComboBox("Цены операций");
            AddUniqueItemToComboBox("Прайс-лист");
            AddUniqueItemToComboBox("Складские единицы");
            AddUniqueItemToComboBox("Категории складских единиц");
            AddUniqueItemToComboBox("Расходные накладные");
            AddUniqueItemToComboBox("Продукция");
            AddUniqueItemToComboBox("Поставщики");
            AddUniqueItemToComboBox("Пользователи");

            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewInvoce);
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewLaboratoryCard);
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewRegister);
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewCompletionReportAcc);
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewCompletionReportProd);
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewPrice);
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewOutputInvoice);
            StackPanelManager.ShowStackPanelWithAnimation(StackForAddNewUser);


        }

        private void roundedExpander_Expanded(object sender, RoutedEventArgs e)
        {
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewInvoce);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewLaboratoryCard);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewRegister);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewCompletionReportAcc);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewCompletionReportProd);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewPrice);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewOutputInvoice);
            StackPanelManager.HideStackPanelWithAnimation(StackForAddNewUser);
        }

        private void roundedExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            AppByRole(roles);
        }

        private void btnToViewData_Click(object sender, RoutedEventArgs e)
        {
            gridManager.RefreshDataGridAsync(dg, cbForChooseData.Text);
            HighlightStackPanel(StackToViewData);
            ShowGridToCreate(GridForViewData);
        }

        private void btnToViewNews_Click(object sender, RoutedEventArgs e)
        {
            barChart.Reset();
            InitializeBar();
            HighlightStackPanel(StackToViewAnalyze);
            ShowGridToCreate(GridForBar);
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


        private void btnCreateNewInvoice_Click(object sender, RoutedEventArgs e)
        {
            HighlightStackPanel(StackForAddNewInvoce);
            ShowGridToCreate(GridForCreateInvioce);
        }

        private void btnCreateNewLaboratoryСard_Click(object sender, RoutedEventArgs e)
        {
            InitializeListViewForSelectInputInvoice();
            HighlightStackPanel(StackForAddNewLaboratoryCard);
            ShowGridToCreate(GridForCreateLaboratoryCard);
        }
        private void btnCreateNewRegister_Click(object sender, RoutedEventArgs e)
        {
            InitializelvForSelectLaboratoryCard();
            HighlightStackPanel(StackForAddNewRegister);
            ShowGridToCreate(GridForCreateRegister);
        }

        private void btnCreateNewCompletionReportProd_Click(object sender, RoutedEventArgs e)
        {
            InitializeListViewForRegister();
            HighlightStackPanel(StackForAddNewCompletionReportProd);
            ShowGridToCreate(GridForCompletionReportProd);
        }

        private void btnCreateNewPrice_Click(object sender, RoutedEventArgs e)
        {
            HighlightStackPanel(StackForAddNewPrice);
            ShowGridToCreate(GridForCreateNewPrice);
        }

        private void btnCreateNewOutputInvoice_Click(object sender, RoutedEventArgs e)
        {
            InitializeListViewForSelectDepoItem();
            HighlightStackPanel(StackForAddNewOutputInvoice);
            ShowGridToCreate(GridForCreateOutputInvoice);
        }

        private void btnAddNewUser_Click(object sender, RoutedEventArgs e)
        {
            HighlightStackPanel(StackForAddNewUser);
            ShowGridToCreate(GridForCreateNewUser);
        }
        private void btnCreateNewCompletionReportAcc_Click(object sender, RoutedEventArgs e)
        {
            InitializeListViewForSelectCompletionReport();
            InitializeForSelectPriceList();
            HighlightStackPanel(StackForAddNewCompletionReportAcc);
            ShowGridToCreate(GridForCompletionReportAcc);
        }


        private void HighlightStackPanel(StackPanel stackPanelToHighlight)
        {
            foreach (var stackPanelWithEllipse in stackPanelsWithEllipses)
            {
                if (stackPanelWithEllipse.StackPanel == stackPanelToHighlight)
                {
                    stackPanelWithEllipse.Highlight();
                }
                else
                {
                    stackPanelWithEllipse.Unhighlight();
                }
            }
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
            System.Windows.Clipboard.SetText(tbForSavePass.Text);
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

        private async void btnToSearchInGrid_Click(object sender, RoutedEventArgs e)
        {
            DateTime? dateFilter = null;
            if (DateTime.TryParse(btnShowCalendarViewData.Content.ToString(), out var parsedDate))
            {
                dateFilter = parsedDate;
            }

            DataGridFilterCriteria filterCriteria = new DataGridFilterCriteria
            {
                FirstArgumentFilter = tbForFirstArgument.Text,
                SecondArgumentFilter = tbForSecondArgument.Text,
                DateFilter = dateFilter
            };

            await gridManager.ConfigureDataGridColumns(dg, cbForChooseData.Text, filterCriteria);
        }
        private void btnClearFiltrationInViewDate_Click(object sender, RoutedEventArgs e)
        {
            gridManager.Reset(tbForFirstArgument, tbForSecondArgument, btnShowCalendarViewData, calendarViewData, dg, cbForChooseData.Text);
        }
        private async void btnToSaveInfoFromDataGrid_Click(object sender, RoutedEventArgs e)
        {
            gridManager.SaveChangesAsync(dg, cbForChooseData.Text);

        }


        private void cbForChooseData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbForChooseData.SelectedItem != null)
            {
                DataGridManager dgs = new();
                dgs.ConfigureDataGridColumns(dg, cbForChooseData.SelectedItem.ToString());
            }
        }
        private void tbUserInfoPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            string text = textBox.Text;

            if (!text.StartsWith("+380"))
            {
                text = new string(text.Where(char.IsDigit).ToArray());
                text = "+380" + text;
                textBox.Text = text;
                textBox.CaretIndex = textBox.Text.Length;
            }
        }
        private void btnShowCalendarInputInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (calendarInputInvoice.Visibility == Visibility.Collapsed)
            {
                calendarInputInvoice.Visibility = Visibility.Visible;
            }
            else
            {
                calendarInputInvoice.Visibility = Visibility.Collapsed;
            }
        }

        private void calendarInputInvoice_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

            DateTime selectedDate = calendarInputInvoice.SelectedDate ?? DateTime.Now;

            btnShowCalendarInputInvoice.Content = selectedDate.ToShortDateString();

            calendarInputInvoice.Visibility = Visibility.Collapsed;
        }

        private void btnShowCalendarRegister_Click(object sender, RoutedEventArgs e) => CalendarManager.ShowCalendar(calendarRegister);
        private void calendarRegister_SelectedDatesChanged(object sender, SelectionChangedEventArgs e) => CalendarManager.SendaDate(calendarRegister, btnShowCalendarRegister);
        private void btnShowCalendarCompletionReportProd_Click(object sender, RoutedEventArgs e) => CalendarManager.ShowCalendar(calendarCompletionReportProd);
        private void calendarCompletionReportProd_SelectedDatesChanged(object sender, SelectionChangedEventArgs e) => CalendarManager.SendaDate(calendarCompletionReportProd, btnShowCalendarCompletionReportProd);
        private void btnShowCalendarNewUser_Click(object sender, RoutedEventArgs e) => CalendarManager.ShowCalendar(calendarNewUser);
        private void calendarNewUser_SelectedDatesChanged(object sender, SelectionChangedEventArgs e) => CalendarManager.SendaDate(calendarNewUser, btnShowCalendarNewUser);
        private void btnShowCalendarViewData_Click(object sender, RoutedEventArgs e) => CalendarManager.ShowCalendar(calendarViewData);
        private void calendarViewData_SelectedDatesChanged(object sender, SelectionChangedEventArgs e) => CalendarManager.SendaDate(calendarViewData, btnShowCalendarViewData);
        private void AddNewColumn_Click(object sender, RoutedEventArgs e)
        {
            NewColumnModalWindow newColumn = new NewColumnModalWindow(barChart);
            newColumn.Show();
        }
        private void ResetAllColumns_Click(object sender, RoutedEventArgs e) => barChart.Reset();

        private void btnSortDataDescending_Click(object sender, RoutedEventArgs e) => barChart.SortDataDescending();
        private void btnSortDataAscending_Click(object sender, RoutedEventArgs e) => barChart.SortDataAscending();

        public async void btnCreateNewInputInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
                {
                    var supplierRepContext = new SupplierRepository(db);
                    var productTitleRepContext = new ProductTitleRepository(db);

                    var supplierItems = await supplierRepContext.GetAllAsync();
                    var productTitleItems = await productTitleRepContext.GetAllAsync();

                    var inputInvoiceRepContext = new InputInvoiceRepository(db);
                    var inputInvoices = await inputInvoiceRepContext.GetAllAsync();

                    if (CheckManager.IsNotNullOrWhiteSpace(tbInputInvoiceNumber.Text) &&
                        CheckManager.IsDate(btnShowCalendarInputInvoice.Content.ToString()!) &&
                        CheckManager.IsNotNullOrWhiteSpace(tbInputInvoiceVehicleNumber.Text) &&
                        CheckManager.IsInt(tbInputInvoicePhysicalWeight.Text))
                    {
                        DateTime arrivalDate = DateTime.Parse(btnShowCalendarInputInvoice.Content.ToString()!);
                        string supplierName = tbInputInvoiceSupplier.Text;
                        string productName = tbInputInvoiceProductName.Text.ToLower();

                        if (!supplierDictionary.ContainsKey(supplierName))
                        {
                            await supplierRepContext.CreateAsync(new Supplier() { Title = supplierName });
                            supplierItems = await supplierRepContext.GetAllAsync();
                            var newSupplier = supplierItems.FirstOrDefault(s => s.Title == supplierName);
                            if (newSupplier != null)
                            {
                                supplierDictionary.Add(newSupplier.Title, newSupplier.Id);
                            }
                        }
                        if (!productDictionary.ContainsKey(productName))
                        {
                            await productTitleRepContext.CreateAsync(new ProductTitle() { Title = productName });
                            productTitleItems = await productTitleRepContext.GetAllAsync();
                            var newProduct = productTitleItems.FirstOrDefault(p => p.Title == productName);
                            if (newProduct != null)
                            {
                                productDictionary.Add(newProduct.Title, newProduct.Id);
                            }
                        }

                        await inputInvoiceRepContext.CreateAsync(new InputInvoice()
                        {
                            InvNumber = tbInputInvoiceNumber.Text,
                            ArrivalDate = arrivalDate,
                            VehicleNumber = tbInputInvoiceVehicleNumber.Text,
                            SupplierId = supplierDictionary[supplierName],
                            ProductTitleId = productDictionary[productName],
                            PhysicalWeight = int.Parse(tbInputInvoicePhysicalWeight.Text),

                        });

                        FieldManager.ClearFields(
                        tbInputInvoiceNumber,
                        btnShowCalendarInputInvoice,
                        tbInputInvoiceVehicleNumber,
                        tbInputInvoiceSupplier,
                        tbInputInvoiceProductName,
                        tbInputInvoicePhysicalWeight
                        );

                        await ListViewManager.RefreshListView(lvForSelectInputInvoice, inputInvoiceRepContext);
                        NotificationManager.ShowSuccessMessageBox("Входящая накладная успешно создана Вами!");
                    }
                    else
                    {
                        NotificationManager.ShowErrorMessageBox("Пожалуйста, заполните все поля корректными данными.");
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка: {ex.Message}");
            }
        }




        private async void btnForCreateNewLaboratoryCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedInputInvoice = lvForSelectInputInvoice.SelectedItem as InputInvoice;
                if (CheckManager.IsInt(tbLaboratoryCardAnalysisNumber.Text) &&
                    CheckManager.IsDouble(tbLaboratoryCardWeedness.Text) &&
                    CheckManager.IsDouble(tbLaboratoryCardHumidity.Text) &&
                    selectedInputInvoice != null)
                {
                    var existingLabCard = await labCardRep.GetAsync(selectedInputInvoice.Id);

                    if (existingLabCard != null)
                    {
                        NotificationManager.ShowErrorMessageBox($"Лабораторная карта уже создана для выбранной входящей накладной.");
                    }
                    else
                    {
                        await labCardRep.CreateAsync(new LaboratoryCard(
                            selectedInputInvoice,
                            int.Parse(tbLaboratoryCardAnalysisNumber.Text),
                            double.Parse(tbLaboratoryCardWeedness.Text),
                            double.Parse(tbLaboratoryCardHumidity.Text),
                            !string.IsNullOrEmpty(tbLaboratoryCardGrainImpurity.Text) ? double.Parse(tbLaboratoryCardGrainImpurity.Text) : 0,
                            tbLaboratoryCardSpecialMark.Text,
                            cbLaboratoryCardIsProduction.IsChecked
                        ));;

                        FieldManager.ClearFields(
                         tbLaboratoryCardAnalysisNumber,
                         tbLaboratoryCardWeedness,
                         tbLaboratoryCardHumidity,
                         tbLaboratoryCardGrainImpurity,
                         tbLaboratoryCardSpecialMark,
                         cbLaboratoryCardIsProduction
                       );

                        await ListViewManager.RefreshListView(lvForSelectLaboratoryCard, labCardRep);
                        NotificationManager.ShowSuccessMessageBox("Лабораторная карта успешно создана.");
                    }

                }
                else
                {
                    NotificationManager.ShowErrorMessageBox("Пожалуйста, заполните все поля корректными данными.");
                }
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка: {ex.Message}");
            }
        }




        private async void btnForCreateNewRegister_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
            {
                try
                {
                    var registersRep = new RegisterRepository(db);

                    var depotItemsReps = new DepotItemRepository(db);

                    if (!CheckManager.IsInt(tbRegisterNumber.Text) || !CheckManager.IsDouble(tbRegisterImpurity.Text) || !CheckManager.IsDouble(tbRegisterHumidity.Text))
                    {
                        NotificationManager.ShowErrorMessageBox("Пожалуйста, введите корректные данные.");
                        return;
                    }

                    var selectedItems = lvForSelectLaboratoryCard.SelectedItems;

                    if (selectedItems == null)
                    {
                        NotificationManager.ShowErrorMessageBox("Выберите хотя бы одну карточку лаборатории.");
                        return;
                    }


                    List<LaboratoryCard> selectedLaboratoryCards = selectedItems.Cast<LaboratoryCard>().ToList();

                    List<LaboratoryCard> usedLaboratoryCards = selectedLaboratoryCards.Where(item => usedLaboratoryCardIds.Contains(item.Id)).ToList();
                    if (usedLaboratoryCards.Count > 0)
                    {
                        string usedCardsMessage = string.Join(", ", usedLaboratoryCards.Select(card => card.Id.ToString()));
                        NotificationManager.ShowErrorMessageBox($"Лабораторные карты с ID {usedCardsMessage} уже использованы.");
                        return;
                    }

                    if (selectedLaboratoryCards.Any(labCard =>
                        labCard.IdNavigation.ArrivalDate != selectedLaboratoryCards[0].IdNavigation.ArrivalDate ||
                        labCard.IdNavigation.Supplier.Title != selectedLaboratoryCards[0].IdNavigation.Supplier.Title ||
                        labCard.IdNavigation.ProductTitle.Title != selectedLaboratoryCards[0].IdNavigation.ProductTitle.Title))
                    {
                        NotificationManager.ShowErrorMessageBox("Выбранные Лабораторные карты должны иметь одинаковые:\n• Дата прихода\n• Поставщик\n• Наименование продукции");
                        return;
                    }


                    usedLaboratoryCardIds.UnionWith(selectedLaboratoryCards.Select(item => item.Id));

                    var reg = new Register(
                        int.Parse(tbRegisterNumber.Text),
                        double.Parse(tbRegisterImpurity.Text),
                        double.Parse(tbRegisterHumidity.Text),
                        selectedLaboratoryCards);

                    await registersRep.CreateAsync(reg);

                    using (var dbs = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
                    {
                        var registersReps = new RegisterRepository(dbs);
                        await ListViewManager.RefreshListView(lvForSelectRegister, registersReps);
                        NotificationManager.ShowSuccessMessageBox("Регистр успешно создан!");
                    }

                    List<Register> regList = await registersRep.GetAllAsync();
                    List<DepotItem> diList = await depotItemRep.GetAllAsync();

                    if (diList.Count > 0)
                    {
                        bool isAddedInclude = false;

                        foreach (var item in diList)
                        {
                            if (item.IsAddedRegister(reg))
                            {
                                await depotItemRep.UpdateAsync(item);
                                isAddedInclude = true;
                                break;
                            }
                        }

                        if (!isAddedInclude)
                            await depotItemRep.CreateAsync(new DepotItem(reg));
                    }
                    else
                        await depotItemRep.CreateAsync(new DepotItem(reg));

                    FieldManager.ClearFields(lvForSelectLaboratoryCard, tbRegisterNumber, tbRegisterHumidity, tbRegisterImpurity);
                }
                catch (Exception ex)
                {
                    NotificationManager.ShowErrorMessageBox("Произошла ошибка: " + ex.Message);
                }
            }
        }

        private async void btnForCreateNewCompletionReportProd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dbs = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
                {
                    if (CheckManager.IsInt(tbForReportNumCompletionReportProd.Text) &&
                    CheckManager.IsDate(btnShowCalendarCompletionReportProd.Content.ToString()))
                    {
                        List<Register> selectedRegisters = lvForSelectRegister.SelectedItems.Cast<Register>().ToList();
                        CompletionReportRepository completionReportRep = new CompletionReportRepository(dbs);

                        if (selectedRegisters.Any(register =>
                        register.Supplier.Title != selectedRegisters[0].Supplier.Title ||
                        register.ProductTitle.Title != selectedRegisters[0].ProductTitle.Title))
                        {
                            NotificationManager.ShowErrorMessageBox("Выбранные Реестры должны иметь одинаковые:\n• Поставщик\n• Наименование продукции");
                            return;
                        }

                        var ourNewCompletionReport = new CompletionReport(
                        int.Parse(tbForReportNumCompletionReportProd.Text),
                        DateTime.Parse(btnShowCalendarCompletionReportProd.Content.ToString()),
                        selectedRegisters);

                        await completionReportRep.UpdateAsync(ourNewCompletionReport);



                        FieldManager.ClearFields(
                         tbForReportNumCompletionReportProd,
                         btnShowCalendarCompletionReportProd
                    );

                        await ListViewManager.RefreshListView(lvForSelectCompletionReport, completionReportRep);
                        NotificationManager.ShowSuccessMessageBox("Отчет о завершении успешно создан!");
                    }
                    else
                    {
                        NotificationManager.ShowErrorMessageBox("Пожалуйста, введите корректные данные.");
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка при создании отчета: {ex.Message}");
            }
        }




        private void btnForCreateNewTitlePriceList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CheckManager.IsNotNullOrWhiteSpace(tbPriceProductName.Text))
                {
                    NotificationManager.ShowErrorMessageBox("Пожалуйста, введите название продукта.");
                    return;
                }

                pl = new PriceList(tbPriceProductName.Text);
                NotificationManager.ShowSuccessMessageBox($"Прайс-лист для `{tbPriceProductName.Text}` успешно создан!");

            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка при создании прайс-листа: {ex.Message}");
            }
        }

        private void btnForCreateNewOperation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pl == null)
                {
                    NotificationManager.ShowErrorMessageBox("Сначала создайте прайс-лист и укажите название продукта.");
                    return;
                }

                if (!CheckManager.IsNotNullOrWhiteSpace(tbPriceOperationName.Text) || !CheckManager.IsDouble(tbPriceOperationCost.Text))
                {
                    NotificationManager.ShowErrorMessageBox("Пожалуйста, введите корректные данные для новой операции.");
                    return;
                }

                pl.AddOperation(tbPriceOperationName.Text, double.Parse(tbPriceOperationCost.Text));
                NotificationManager.ShowSuccessMessageBox($"Операция успешно добавлена!");

            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка при добавлении операции: {ex.Message}");
            }
        }

        private async void btnForCreatePriceList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pl == null)
                {
                    NotificationManager.ShowErrorMessageBox("Прайс-лист не создан.");
                    return;
                }
                if (pl.PriceByOperations.Count == 0)
                {
                    NotificationManager.ShowErrorMessageBox("Прайс-лист не содержит технологических оперций.\nДобавьте хотя бы одну операцию к прайс-листу перед сохранением.");
                    return;
                }

                await priceListRepository.CreateAsync(pl);
                FieldManager.ClearFields(
                 tbPriceProductName,
                 tbPriceOperationName,
                 tbPriceOperationCost
                );
                await ListViewManager.RefreshListView(lvForSelectPriceList, priceListRepository);
                NotificationManager.ShowSuccessMessageBox("Прайс-лист успешно создан!");
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка при создании прайс-листа: {ex.Message}");
            }
        }

        private async void btnForCompletionReportAcc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedComplactionReportRep = lvForSelectCompletionReport.SelectedItem as CompletionReport;
                var selectedPriceList = lvForSelectPriceList.SelectedItem as PriceList;

                if (selectedComplactionReportRep == null)
                {
                    NotificationManager.ShowErrorMessageBox("Выберите акт доработки для подтверждения.");
                    return;
                }

                if (selectedPriceList == null)
                {
                    NotificationManager.ShowErrorMessageBox("Выберите прайс-лист для расчета.");
                    return;
                }

                selectedComplactionReportRep.CalcByPrice(selectedPriceList);
                await completionReportRep.UpdateAsync(selectedComplactionReportRep);
                ListViewManager.RefreshListView(lvForSelectCompletionReport, completionReportRep);
                FieldManager.ClearFields(lvForSelectCompletionReport, lvForSelectPriceList);
                NotificationManager.ShowSuccessMessageBox("Расчет успешно подтвержден!");
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка при подтверждении расчета: {ex.Message}");
            }
        }





        private async void btnForCreateOutputInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
                {
                    DepotItemRepository depotItems = new DepotItemRepository(db);

                    var di = (DepotItem)lvForSelectDepoItem.SelectedItem;


                    if (di == null)
                    {
                        NotificationManager.ShowErrorMessageBox("Выберите продукт на складе.");
                        return;
                    }

                    if (!CheckManager.IsInt(tbOutputInvoiceQuantity.Text) ||
                        !CheckManager.IsNotNullOrWhiteSpace(tbOutputInvoiceNum.Text) ||
                        !CheckManager.IsNotNullOrWhiteSpace(tbOutputVehicleNumber.Text))
                    {
                        NotificationManager.ShowErrorMessageBox("Пожалуйста, заполните все поля корректными данными.");
                        return;
                    }

                    OutputInvoice outputInvoice = new OutputInvoice(
                        tbOutputInvoiceNum.Text,
                        DateTime.Now,
                        tbOutputVehicleNumber.Text,
                        di,
                        cbForChooseTypeOfDepoItem.Text,
                        int.Parse(tbOutputInvoiceQuantity.Text));

                    if (outputInvoice.Shipment(di))
                    {
                        await outputInvoiceRep.CreateAsync(outputInvoice);
                        await depotItems.UpdateAsync(di);

                        ListViewManager.RefreshListView(lvForSelectDepoItem, depotItems);
                        FieldManager.ClearFields(tbOutputInvoiceNum, tbOutputVehicleNumber, cbForChooseTypeOfDepoItem, tbOutputInvoiceQuantity);
                        NotificationManager.ShowSuccessMessageBox("Отгрузка успешно создана.");
                    }

               


                }
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка: {ex.Message}");
            }
        }



        private async void btnForCreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CheckManager.IsNotNullOrWhiteSpace(tbUserInfoFirstName.Text) ||
                    !CheckManager.IsNotNullOrWhiteSpace(tbUserInfoLastName.Text) ||
                    !CheckManager.IsDate(btnShowCalendarNewUser.Content.ToString()) ||
                    !CheckManager.IsNotNullOrWhiteSpace(tbUserInfoEmail.Text) ||
                    !CheckManager.IsNotNullOrWhiteSpace(tbUserInfoPhoneNumber.Text) ||
                    !CheckManager.IsNotNullOrWhiteSpace(tbForSavePass.Text) ||
                    !CheckManager.IsNotNullOrWhiteSpace(tbUserInfoCity.Text) ||
                    !CheckManager.IsNotNullOrWhiteSpace(tbUserInfoCountry.Text) ||
                    !CheckManager.IsNotNullOrWhiteSpace(cmbUserInfoGender.Text) ||
                    !CheckManager.IsNotNullOrWhiteSpace(cmbWithRoles.Text))
                {
                    NotificationManager.ShowErrorMessageBox("Пожалуйста, заполните все поля.");
                    return;
                }

                if (!genderMapping.TryGetValue(cmbUserInfoGender.Text, out string gender))
                {
                    NotificationManager.ShowErrorMessageBox("Некорректное значение пола. Выберите 'Мужской' или 'Женский'.");
                    return;
                }

                if (!rolesMapping.TryGetValue(cmbWithRoles.Text, out Roles role))
                {
                    NotificationManager.ShowErrorMessageBox("Некорректное значение роли.");
                    return;
                }
                if (!Regex.IsMatch(tbUserInfoPhoneNumber.Text, @"^\+380\d{9}$"))
                {
                    NotificationManager.ShowErrorMessageBox("Некорректный формат номера телефона. Введите номер в формате +380XXXXXXXXX.");
                    return;
                }
                if (tbUserInfoFirstName.Text.Any(char.IsDigit) ||
                    tbUserInfoLastName.Text.Any(char.IsDigit) ||
                    tbUserInfoCity.Text.Any(char.IsDigit) ||
                    tbUserInfoCountry.Text.Any(char.IsDigit))
                {
                    NotificationManager.ShowErrorMessageBox("Имя, фамилия, город и страна не должны содержать цифры.");
                    return;
                }
                if (!Regex.IsMatch(tbUserInfoEmail.Text, @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    NotificationManager.ShowErrorMessageBox("Пожалуйста, введите корректный адрес электронной почты.");
                    return;
                }
                if (tbForSavePass.Text.ToLower().Contains("error"))
                {
                    NotificationManager.ShowErrorMessageBox("Пароль сгенерирован не на основе роли.");
                    return;
                }

                User newUser = new User(tbUserInfoFirstName.Text, tbUserInfoLastName.Text, DateTime.Parse(btnShowCalendarNewUser.Content.ToString()), tbUserInfoEmail.Text, tbUserInfoPhoneNumber.Text, tbForSavePass.Text, gender, tbUserInfoCity.Text, tbUserInfoCountry.Text, role);

                await userRepository.CreateAsync(newUser);

                FieldManager.ClearFields(
                    tbUserInfoFirstName,
                    tbUserInfoLastName,
                    btnShowCalendarNewUser,
                    tbUserInfoEmail,
                    tbUserInfoPhoneNumber,
                    tbForSavePass,
                    tbUserInfoCity,
                    tbUserInfoCountry,
                    cmbUserInfoGender,
                    cmbWithRoles
                );

                NotificationManager.ShowSuccessMessageBox("Пользователь успешно создан!");
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Произошла ошибка: {ex.Message}");
            }
        }



        private async void btnFiltrationInCreateRegister_Click(object sender, RoutedEventArgs e)
        {
            if (DateTime.TryParse(btnShowCalendarRegister.Content.ToString(), out var selectedDate))
                await ListViewManager.FilterAndRefreshListView<LaboratoryCard>(lvForSelectLaboratoryCard, supplierDictionary, productDictionary, tbForFilterSupplierInRegister, tbForFilterNameOfProductInRegister, selectedDate);
            else
                NotificationManager.ShowInfoMessageBox("Пожалуйста, выберите дату. В этой фильтрации она неотъемлемый элемент.");
        }

        private void btnClearFiltrationInCreateRegister_Click(object sender, RoutedEventArgs e)
        {
            _ = ListViewManager.ClearListViewAndFields<LaboratoryCard>(lvForSelectLaboratoryCard, tbForFilterSupplierInRegister, tbForFilterNameOfProductInRegister, btnShowCalendarRegister);
        }
        private async void btnFiltrationInCompletionReportProd_Click(object sender, RoutedEventArgs e)
        {
            await ListViewManager.FilterAndRefreshListView(lvForSelectRegister, registerRep, supplierDictionary, productDictionary, tbForFilterSupplierInReportProd, tbForFilterNameOfProductInReportProd);
        }

        private async void btnFiltrationInOutputInvoice_Click(object sender, RoutedEventArgs e)
        {
            await ListViewManager.FilterAndRefreshListView(lvForSelectDepoItem, depotItemRep, supplierDictionary, productDictionary, tbForFilterSupplierInOutputInvoice, tbForFilterNameOfProductInOutputInvoice);
        }


        private async void lvForSelectCompletionReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var db = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
            {
                CompletionReport compRep = lvForSelectCompletionReport.SelectedItem as CompletionReport;

                TechnologicalOperationRepository technologicalOperation = new(db);
                var tocoll = compRep != null ? await technologicalOperation.GetAllAsync() : null;

                dgForAdditionalInfoInCompletionReport.ItemsSource = compRep != null ? tocoll.Where(cp => cp.CompletionReportId == compRep.Id) : null;
            }
        }

        private async void lvForSelectDepoItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var dbs = new Db((DbContextOptions<Db>)OptionsManager.GetOptions()))
            {
                var depoItem = lvForSelectDepoItem.SelectedItem as DepotItem;

                CategoryRepository categoryRepository = new(dbs);
                var categoryRepositories = depoItem != null ? await categoryRepository.GetAllAsync() : null;

                dgForAdditionalInfoInOutputInvoice.ItemsSource = depoItem != null ? categoryRepositories.Where(ct => ct.DepotItemId == depoItem.Id) : null;
            }
        }

        private void QuestionInInputInvoice_Click(object sender, RoutedEventArgs e) => PopupInInputInvoice.IsOpen = true;

        private void QuestionInLaboratoryCard_Click(object sender, RoutedEventArgs e) => PopupInLaboratoryCard.IsOpen = true;

        private void QuestionInRegister_Click(object sender, RoutedEventArgs e) => PopupInRegister.IsOpen = true;

        private void QuestionInCompletionReportProd_Click(object sender, RoutedEventArgs e) => PopupInCompletionReportProd.IsOpen = true;

        private void QuestionInNewPrice_Click(object sender, RoutedEventArgs e) => PopupInNewPrice.IsOpen = true;

        private void QuestionInCompletionReportAcc_Click(object sender, RoutedEventArgs e) => PopupInCompletionReportAcc.IsOpen = true;

        private void QuestionInOutputInvoice_Click(object sender, RoutedEventArgs e) => PopupInOutputInvoice.IsOpen = true;

        private void QuestionInNewUser_Click(object sender, RoutedEventArgs e) => PopupInNewUser.IsOpen = true;

        private void QuestionInViewData_Click(object sender, RoutedEventArgs e) => PopupInViewData.IsOpen = true;
    }
}

