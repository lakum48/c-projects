using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace lib
{
    public partial class AddEditAuthorWindow : Window
    {
        public Author Author { get; private set; }
        private LibraryContext _context;

        public AddEditAuthorWindow(Author author = null)
        {
            InitializeComponent();
            _context = new LibraryContext();
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
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Заполните обязательные поля (First Name и Last Name).");
                return;
            }

            Author.FirstName = FirstNameTextBox.Text;
            Author.LastName = LastNameTextBox.Text;
            Author.BirthDate = BirthDatePicker.SelectedDate?.ToUniversalTime() ?? DateTime.MinValue;
            Author.Country = CountryTextBox.Text;

            try
            {
                // Сохранение изменений в контексте
                if (Author.Id == 0) // Это новый автор
                {
                    _context.Authors.Add(Author);
                }
                else // Это редактирование существующего автора
                {
                    var existingAuthor = _context.Authors.Find(Author.Id);
                    if (existingAuthor != null)
                    {
                        existingAuthor.FirstName = Author.FirstName;
                        existingAuthor.LastName = Author.LastName;
                        existingAuthor.BirthDate = Author.BirthDate;
                        existingAuthor.Country = Author.Country;
                    }
                }

                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.InnerException?.Message}");
            }
        }

    }
}
