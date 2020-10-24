using HundoHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HundoHelper
{
    /// <summary>
    /// Interaction logic for EditCollectiblesWindow.xaml
    /// </summary>
    public partial class EditCollectiblesWindow : Window, INotifyPropertyChanged
    {
        private ICollectible _selectedCollectible;
        private bool _saveChangesEnabled;

        public bool SaveChangesEnabled {
            get => _saveChangesEnabled;
            set {
                _saveChangesEnabled = value;
                OnPropertyChanged(nameof(SaveChangesEnabled));
            }
        }

        public ICollectible SelectedCollectible {
            get => _selectedCollectible;
            set {
                _selectedCollectible = value;
                OnPropertyChanged(nameof(SelectedCollectible));
                SelectedCollectible.OnNameChanged += SetUnsaved;
            }
        }
        private IList<HiddenPackage> _hiddenPackages = new ObservableCollection<HiddenPackage>();
        private IList<UniqueStuntJump> _usjsPackages = new ObservableCollection<UniqueStuntJump>();
        private IList<Robbery> _robberiesPackages = new ObservableCollection<Robbery>();
        public EditCollectiblesWindow()
        {
            InitializeComponent();
            DataContext = this;

            foreach(var p in CheckList.collectibles.OfType<HiddenPackage>().OrderBy(p => p.OrderIndex))
                _hiddenPackages.Add(p);
            foreach (var p in CheckList.collectibles.OfType<UniqueStuntJump>().OrderBy(p => p.OrderIndex))
                _usjsPackages.Add(p);
            foreach (var p in CheckList.collectibles.OfType<Robbery>().OrderBy(p => p.OrderIndex))
                _robberiesPackages.Add(p);

            packagesListBox.DataContext = _hiddenPackages;
            packagesListBox.SelectionChanged += PackagesListBox_SelectionChanged;
            packagesListBox.OnItemMoved += SetUnsaved;

            usjsListBox.DataContext = _usjsPackages;
            usjsListBox.SelectionChanged += PackagesListBox_SelectionChanged;
            usjsListBox.OnItemMoved += SetUnsaved;

            robberiesListBox.DataContext = _robberiesPackages;
            robberiesListBox.SelectionChanged += PackagesListBox_SelectionChanged;
            robberiesListBox.OnItemMoved += SetUnsaved;
        }

        private void SetUnsaved()
        {
            SaveChangesEnabled = true;
        }


        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            CheckList.SaveCollectibles();
            SaveChangesEnabled = false;
            MessageBox.Show("Changes succesfully saved!");
        }

        private void PackagesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is PackagesDragAndDropListBox)
            {
                SelectedCollectible = (ICollectible)packagesListBox.SelectedItem;
            }
            else if (e.Source is UsjsDragAndDropListBox)
            {
                SelectedCollectible = (ICollectible)usjsListBox.SelectedItem;
            }
            else if (e.Source is RobberiesDragAndDropListBox)
            {
                SelectedCollectible = (ICollectible)robberiesListBox.SelectedItem;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
