using CarService.Data.Local;
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

namespace CarService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _currentPage = 1;
        private int _itemPerPage = new CarServiceModel().Clients.Count();
        private List<Client> _filteredList = new CarServiceModel().Clients.ToList();

        public List<Client> ClientList { get => new CarServiceModel().Clients.ToList(); }
        public int TotalPages
        {
            get => (int)Math.Ceiling((double)_filteredList.Count / _itemPerPage);
        }

        private string _searchParameter = "";
        private int _sortParameter = 0;
        private int _filterParameter = 0;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (value >= 1 && value <= TotalPages)
                {
                    _currentPage = value;
                    DisplayItemPerPage();
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            SetSearchParameters();
        }

        private void ResetPage()
        {
            _currentPage = 1;
        }

        private void UpdateNoteCount()
        {
            noteCountTextBlock.Text = _filteredList.Count().ToString();
        }

        private void SetSearchParameters()
        {
            var searchParameters = ClientList
                .Where(client => $"{client.FirstName} {client.LastName} {client.Patronymic} {client.Email} {client.Phone}".Contains(_searchParameter))
                .Where(client => client.GenderCode == _filterParameter.ToString() || _filterParameter == 0);

            if (_sortParameter == 0)
                searchParameters = searchParameters.OrderBy(client => client.LastName);

            if (_sortParameter == 1)
                searchParameters = searchParameters.OrderByDescending(client => client.LastVisit);

            if (_sortParameter == 2)
                searchParameters = searchParameters.OrderByDescending(client => client.CountVisit);

            _filteredList = searchParameters.ToList();

            ResetPage();
            UpdateNoteCount();
            DisplayItemPerPage();
        }

        private void DisplayItemPerPage()
        {
            pageNumberTextBlock.Text = _currentPage.ToString();
            clientListView.ItemsSource = _filteredList.Skip((CurrentPage - 1) * _itemPerPage).Take(_itemPerPage).ToList();
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (itemPerPageComboBox.SelectedIndex == 0)
                pageSelectorStackPanel.Visibility = Visibility.Hidden;

            else
                pageSelectorStackPanel.Visibility = Visibility.Visible;

            switch (itemPerPageComboBox.SelectedIndex)
            {
                case 0: _itemPerPage = new CarServiceModel().Clients.Count(); break;
                case 1: _itemPerPage = 10; break;
                case 2: _itemPerPage = 50; break;
                case 3: _itemPerPage = 200; break;
            }

            ResetPage();
            DisplayItemPerPage();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            itemPerPageComboBox.SelectedIndex = 0;
        }

        private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            _searchParameter = searchTextBox.Text;
            SetSearchParameters();
        }

        private void SortComboBoxDropDownClosed(object sender, EventArgs e)
        {
            _sortParameter = sortComboBox.SelectedIndex;
            SetSearchParameters();
        }

        private void FilterComboBoxDropDownClosed(object sender, EventArgs e)
        {
            _filterParameter = filterComboBox.SelectedIndex;
            SetSearchParameters();
        }

        private void PreviousButtonPageClick(object sender, RoutedEventArgs e)
        {
            CurrentPage -= 1;
        }

        private void NextButtonPageClick(object sender, RoutedEventArgs e)
        {
            CurrentPage += 1;
        }
    }
}
