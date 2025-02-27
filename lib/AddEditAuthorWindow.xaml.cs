using System;
using System.Windows;

namespace lib
{
    public partial class AddEditAuthorWindow : Window
    {
        public Author Author { get; private set; }

        public AddEditAuthorWindow(Author author = null)
        {
            InitializeComponent();
            Author = author ?? new Author();
            DataContext = Author;

            if (author != null)
            {
                FirstNameTextBox.Text = author.FirstName;
                LastNameTextBox.Text = author.LastName;
                BirthDatePicker.SelectedDate = author.BirthDate;
                CountryTextBox.Text = author.Country;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Заполните обязательные поля (Last Name).");
                return;
            }

            Author.FirstName = FirstNameTextBox.Text;
            Author.LastName = LastNameTextBox.Text;

            // Преобразуем BirthDate в UTC, если оно указано
            if (BirthDatePicker.SelectedDate.HasValue)
            {
                Author.BirthDate = BirthDatePicker.SelectedDate.Value.ToUniversalTime();
            }
            else
            {
                Author.BirthDate = DateTime.MinValue; // Или другое значение по умолчанию
            }

            Author.Country = CountryTextBox.Text;

            DialogResult = true;
            Close();
        }
    }
}