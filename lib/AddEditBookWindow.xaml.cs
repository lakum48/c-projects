using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace lib
{
    public partial class AddEditBookWindow : Window
    {
        public Book Book { get; private set; }
        private LibraryContext _context;

        public ObservableCollection<Author> Authors { get; set; }
        public ObservableCollection<Genre> Genres { get; set; }

        public AddEditBookWindow(Book book = null)
        {
            InitializeComponent();
            _context = new LibraryContext();
            Book = book ?? new Book();
            DataContext = Book;

            // Загружаем авторов и жанры
            LoadAuthors();
            LoadGenres();

            if (book != null)
            {
                TitleTextBox.Text = book.Title;
                AuthorComboBox.SelectedItem = book.Author;
                GenreComboBox.SelectedItem = book.Genre;
                PublishYearTextBox.Text = book.PublishYear.ToString();
                ISBNTextBox.Text = book.ISBN;
                QuantityInStockTextBox.Text = book.QuantityInStock.ToString();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                string.IsNullOrWhiteSpace(ISBNTextBox.Text) ||
                AuthorComboBox.SelectedItem == null ||
                GenreComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все обязательные поля.");
                return;
            }

            Book.Title = TitleTextBox.Text;
            Book.Author = (Author)AuthorComboBox.SelectedItem;
            Book.Genre = (Genre)GenreComboBox.SelectedItem;
            Book.PublishYear = int.Parse(PublishYearTextBox.Text);
            Book.ISBN = ISBNTextBox.Text;
            Book.QuantityInStock = int.Parse(QuantityInStockTextBox.Text);

            try
            {
                // Сохранение изменений в контексте
                if (Book.Id == 0) // Это новая книга
                {
                    _context.Books.Add(Book);
                }
                else // Это редактирование существующей книги
                {
                    var existingBook = _context.Books.Include(b => b.Author).Include(b => b.Genre).FirstOrDefault(b => b.Id == Book.Id);
                    if (existingBook != null)
                    {
                        existingBook.Title = Book.Title;
                        existingBook.Author = Book.Author; // Убедитесь, что Author уже отслеживается
                        existingBook.Genre = Book.Genre;   // Убедитесь, что Genre уже отслеживается
                        existingBook.PublishYear = Book.PublishYear;
                        existingBook.ISBN = Book.ISBN;
                        existingBook.QuantityInStock = Book.QuantityInStock;
                    }
                }

                _context.SaveChanges(); // Сохраняем изменения в базе данных
                DialogResult = true;
                Close();
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.InnerException?.Message}");
            }
        }



        private void LoadAuthors()
        {
            Authors = new ObservableCollection<Author>(_context.Authors.ToList());
            AuthorComboBox.ItemsSource = Authors;
        }

        private void LoadGenres()
        {
            Genres = new ObservableCollection<Genre>(_context.Genres.ToList());
            GenreComboBox.ItemsSource = Genres;
        }
    }
}
