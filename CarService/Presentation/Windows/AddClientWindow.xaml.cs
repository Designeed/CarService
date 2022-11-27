using CarService.Data.Local;
using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CarService.Presentation.Windows
{
    /// <summary>
    /// Interaction logic for AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        private string _photoPath = "";

        public AddClientWindow()
        {
            InitializeComponent();

            tagListView.ItemsSource = new CarServiceModel().Tags.ToList();
        }

        private bool IsFieldValid()
        {
            if (
                String.IsNullOrEmpty(firstNameTextBox.Text)
                || String.IsNullOrEmpty(lastNameTextBox.Text)
                || String.IsNullOrEmpty(patronymicNameTextBox.Text)
            )
            {
                MessageBox.Show("Проверьте фамилию, имя или отчество");
                return false;
            }

            if (!IsEmailValid(emailTextBox.Text))
            {
                MessageBox.Show("Проверьте почту");
                return false;
            }

            if (!IsPhoneValid(phoneTextBox.Text))
            {
                MessageBox.Show("Проверьте телефон");
                return false;
            }

            if (!DateTime.TryParse(birthdayTextBox.Text, out DateTime result))
            {
                MessageBox.Show("Проверьте доту рождения");
                return false;
            }

            if (String.IsNullOrEmpty(_photoPath))
            {
                MessageBox.Show("Выберите изображение");
                return false;
            }

            return true;
        }

        private bool IsEmailValid(string email) => new Regex(@"^\w+@\w+.\w+$").IsMatch(email);

        private bool IsPhoneValid(string phoneNumber) => new Regex(@"^\d+").IsMatch(phoneNumber);

        private void selectClientImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Image Files(*.jpg)|*.jpg;";

            if (fileDialog.ShowDialog() == true)
            {
                _photoPath = fileDialog.FileName;
                clientLogoImage.Source = new BitmapImage(new Uri(_photoPath));
            }
        }

        private void addClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFieldValid())
                return;

            try
            {
                var dbContext = new CarServiceModel();
                var client = dbContext.Clients.Add(
                    new Client
                    {
                        FirstName = firstNameTextBox.Text,
                        LastName = lastNameTextBox.Text,
                        Patronymic = patronymicNameTextBox.Text,
                        Birthday = Convert.ToDateTime(birthdayTextBox.Text),
                        RegistrationDate = DateTime.Now,
                        Email = emailTextBox.Text,
                        Phone = phoneTextBox.Text,
                        GenderCode = (genderComboBox.SelectedIndex + 1).ToString(),
                        PhotoPath = _photoPath,
                    }
                );

                var tags = tagListView?.SelectedItems.OfType<Tag>().ToList();
                foreach (var item in tags)
                {
                    var findedTag = dbContext.Tags.Find(item.ID);
                    if (findedTag == null)
                        continue;

                    client.Tags.Add(findedTag);
                }

                dbContext.SaveChanges();

                MessageBox.Show("Клиент успешно добавлен");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FullNameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(
                symbol => Char.IsLetter(symbol) || symbol == ' ' || symbol == '-'
            );
        }

        private void phoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(symbol => Char.IsDigit(symbol));
        }
    }
}
