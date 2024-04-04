using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ElevatorControlSystem
{
    public class SettingsManager
    {
        public bool Is24HourFormat { get; set; }
        public bool IsMusicEnabled { get; set; }
        public double MusicVolume { get; set; }

        private readonly string _filePath;

        public SettingsManager(string filePath)
        {
            _filePath = filePath;
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                SettingsManager settings = JsonConvert.DeserializeObject<SettingsManager>(json);
                if (settings != null)
                {
                    Is24HourFormat = settings.Is24HourFormat;
                    IsMusicEnabled = settings.IsMusicEnabled;
                    MusicVolume = settings.MusicVolume;
                }
            }
        }

        public void SaveSettings()
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(_filePath, json);
        }
    }

    internal class AppSettings
    {
        public static SettingsManager Settings { get; set; } = new SettingsManager(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "appsettings.json"));

    }

    public partial class SettingWindow : Window
    {
        private bool is24HourFormat;
        private bool isModified;
        private bool isMusicEnabled;
        private bool hasChanges;


        public SettingWindow()
        {
            InitializeComponent();

            is24HourFormat = LoadTimeFormatPreference();
            isMusicEnabled = LoadMusicPreference();
            volumeSlider.Value = LoadMusicVolumePreference();

            checkBox24Hour.IsChecked = is24HourFormat;
            checkBoxPlayMusic.IsChecked = isMusicEnabled;


            if (isMusicEnabled)
            {
                volumeSlider.Value = AppSettings.Settings.MusicVolume;
                StackForVolumeChanges.Visibility = Visibility.Visible;
            }
            else
            {
                StackForVolumeChanges.Visibility = Visibility.Collapsed;
            }
        }

        private bool LoadTimeFormatPreference()
        {
            return AppSettings.Settings.Is24HourFormat;
        }

        public bool LoadMusicPreference()
        {
            return AppSettings.Settings.IsMusicEnabled;
        }
        public double LoadMusicVolumePreference()
        {
            return AppSettings.Settings.MusicVolume;
        }
        private void btnExitFromSettings_Click(object sender, RoutedEventArgs e)
        {
            if (isModified)
            {
                checkBox24Hour.IsChecked = AppSettings.Settings.Is24HourFormat;
                checkBoxPlayMusic.IsChecked = AppSettings.Settings.IsMusicEnabled;
                volumeSlider.Value = AppSettings.Settings.MusicVolume;

                StackForVolumeChanges.Visibility = checkBoxPlayMusic.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }

            DoubleAnimation closeAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            closeAnimation.Completed += (s, _) => Close();

            BeginAnimation(OpacityProperty, closeAnimation);
        }

        private void SaveTimeFormatPreference()
        {
            AppSettings.Settings.Is24HourFormat = is24HourFormat;
            AppSettings.Settings.SaveSettings();
        }

        private void SaveMusicPreference()
        {
            AppSettings.Settings.IsMusicEnabled = isMusicEnabled;
            AppSettings.Settings.SaveSettings();
        }

        private void checkBoxPlayMusic_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                if (checkBoxPlayMusic.IsChecked == true)
                {
                    StackForVolumeChanges.Visibility = Visibility.Visible; 
               
                    if (Owner is MainWindow mainWindow)
                    {
                        mainWindow.PlayMusic();
                        mainWindow.SetMusicVolume(AppSettings.Settings.MusicVolume); 
                    }
                }
                else
                {
                    StackForVolumeChanges.Visibility = Visibility.Collapsed; 
                    if (Owner is MainWindow mainWindow)
                    {
                        mainWindow.StopMusic();
                    }
                }
                isModified = true;
            }
        }

        private void checkBoxPlayMusic_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                checkBoxPlayMusic_Checked(sender, e);
            }
        }

        private void bntSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            bool newFormat = checkBox24Hour.IsChecked ?? false;
            bool newMusicEnabled = checkBoxPlayMusic.IsChecked ?? false;
            double newMusicVolume = volumeSlider.Value;

            if (newFormat != is24HourFormat)
            {
                is24HourFormat = newFormat;
                SaveTimeFormatPreference();

                if (Owner is MainWindow mainWindow)
                {
                    mainWindow.UpdateTimeFormat(is24HourFormat);
                }
            }

            if (newMusicEnabled != isMusicEnabled)
            {
                isMusicEnabled = newMusicEnabled;
                SaveMusicPreference();

                if (Owner is MainWindow mainWindow)
                {
                    mainWindow.PlayOrStopMusic(isMusicEnabled);
                }
            }

            if (newMusicVolume != AppSettings.Settings.MusicVolume)
            {
                AppSettings.Settings.MusicVolume = newMusicVolume;
                AppSettings.Settings.SaveSettings();

                if (Owner is MainWindow mainWindow)
                {
                    mainWindow.SetMusicVolume(newMusicVolume);
                }
            }

        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (IsLoaded && checkBoxPlayMusic.IsChecked == true)
            {
                if (Owner is MainWindow mainWindow)
                {
                    mainWindow.SetMusicVolume(e.NewValue);
                }
            }

        }

    }
}
