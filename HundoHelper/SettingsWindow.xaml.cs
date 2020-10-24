using HundoHelper.Models;
using System;
using System.ComponentModel;
using System.Windows;


namespace HundoHelper
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        private bool saveChangesEnabled;
        public bool SaveChangesEnabled { get => saveChangesEnabled; 
            set {
                if (saveChangesEnabled != value)
                {
                    saveChangesEnabled = value;
                    OnPropertyChanged(nameof(saveChangesEnabled));
                }
            }

        }
        public SettingsWindow()
        {
            InitializeComponent();
            btnSaveChanges.DataContext = this;
            DataContext = CheckList.settings;
            CheckList.settings.PropertyChanged += Settings_PropertyChanged;
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUnsaved();
        }

        private void SetUnsaved()
        {
            SaveChangesEnabled = true;
        }


        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            CheckList.SaveSettings();
            SaveChangesEnabled = false;
            MessageBox.Show("Changes succesfully saved!");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
