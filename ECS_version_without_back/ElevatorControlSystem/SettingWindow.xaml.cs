using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ElevatorControlSystem
{
    internal class AppSettings : ApplicationSettingsBase
    {
        public bool Is24HourFormat { get; set; }
        public bool IsMusicEnabled { get; set; } = false;
        public double MusicVolume { get; set; } = 0.1;

        private static AppSettings _default;
        public static AppSettings Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new AppSettings();
                }
                return _default;
            }
        }
   
    }

    public partial class SettingWindow : Window
    {
        private bool is24HourFormat;
        private bool isModified;
        private bool isMusicEnabled;


       
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
                volumeSlider.Value = AppSettings.Default.MusicVolume;
                StackForVolumeChanges.Visibility = Visibility.Visible;
            }
            else
            {
                StackForVolumeChanges.Visibility= Visibility.Collapsed;
            }
        }

        private bool LoadTimeFormatPreference()
        {
            return AppSettings.Default.Is24HourFormat;
        }

        public bool LoadMusicPreference()
        {
            return AppSettings.Default.IsMusicEnabled;
        }
        public double LoadMusicVolumePreference() 
        {
           return AppSettings.Default.MusicVolume;
        }
        private void btnExitFromSettings_Click(object sender, RoutedEventArgs e)
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

        private void SaveTimeFormatPreference()
        {
            AppSettings.Default.Is24HourFormat = is24HourFormat;
            AppSettings.Default.Save();
        }

        private void SaveMusicPreference()
        {
            AppSettings.Default.IsMusicEnabled = isMusicEnabled;
            AppSettings.Default.Save();
        }

        private void checkBoxPlayMusic_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                if (checkBoxPlayMusic.IsChecked == true)
                {
                    StackForVolumeChanges.Visibility = Visibility.Visible; // Включаем слайдер при включенной музыке
                    // Включаем воспроизведение музыки
                    if (Owner is MainWindow mainWindow)
                    {
                        mainWindow.PlayMusic();
                        mainWindow.SetMusicVolume(AppSettings.Default.MusicVolume); // Установка громкости музыки
                    }
                }
                else
                {
                    StackForVolumeChanges.Visibility = Visibility.Collapsed; // Выключаем слайдер при выключенной музыке
                    // Останавливаем воспроизведение музыки
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

            if (newMusicVolume != AppSettings.Default.MusicVolume)
            {
                AppSettings.Default.MusicVolume = newMusicVolume;
                AppSettings.Default.Save();

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
