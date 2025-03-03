using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace lib
{
    public partial class AddEditGenreWindow : Window
    {
        public Genre Genre { get; private set; }
        private LibraryContext _context;

        public AddEditGenreWindow(Genre genre = null)
        {
            InitializeComponent();
            _context = new LibraryContext();
            Genre = genre ?? new Genre();
            DataContext = Genre;

            if (genre != null)
            {
                NameTextBox.Text = genre.Name;
                DescriptionTextBox.Text = genre.Description;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Заполните обязательное поле (Name).");
                return;
            }

            Genre.Name = NameTextBox.Text;
            Genre.Description = DescriptionTextBox.Text;

            try
            {
                // Сохранение изменений в контексте
                if (Genre.Id == 0) // Это новый жанр
                {
                    _context.Genres.Add(Genre);
                }
                else // Это редактирование существующего жанра
                {
                    var existingGenre = _context.Genres.Find(Genre.Id);
                    if (existingGenre != null)
                    {
                        existingGenre.Name = Genre.Name;
                        existingGenre.Description = Genre.Description;
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