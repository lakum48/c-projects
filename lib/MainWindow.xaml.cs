using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace lib
{
    public partial class MainWindow : Window
    {
        private LibraryContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadAuthorsAndGenres();
            LoadBooks();
        }

        private void LoadAuthorsAndGenres()
        {
            AuthorFilterComboBox.ItemsSource = _context.Authors.ToList();
            GenreFilterComboBox.ItemsSource = _context.Genres.ToList();
        }

        private void LoadBooks()
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .AsQueryable();

            // Применение поиска по названию
            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                query = query.Where(b => b.Title.Contains(SearchTextBox.Text));
            }

            // Применение фильтра по автору
            if (AuthorFilterComboBox.SelectedItem != null)
            {
                var selectedAuthor = (Author)AuthorFilterComboBox.SelectedItem;
                query = query.Where(b => b.AuthorId == selectedAuthor.Id);
            }

            // Применение фильтра по жанру
            if (GenreFilterComboBox.SelectedItem != null)
            {
                var selectedGenre = (Genre)GenreFilterComboBox.SelectedItem;
                query = query.Where(b => b.GenreId == selectedGenre.Id);
            }

            BooksGrid.ItemsSource = query.ToList();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadBooks();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            var addBookWindow = new AddEditBookWindow();
            if (addBookWindow.ShowDialog() == true)
            {
                _context.Books.Add(addBookWindow.Book);
                _context.SaveChanges();
                LoadBooks();
            }
        }

        private void ManageAuthors_Click(object sender, RoutedEventArgs e)
        {
            var manageAuthorsWindow = new ManageAuthorsWindow();
            manageAuthorsWindow.ShowDialog();

            // После закрытия окна управления авторами обновляем список авторов
            LoadAuthorsAndGenres();
            LoadBooks();
        }

        private void ManageGenres_Click(object sender, RoutedEventArgs e)
        {
            var manageGenresWindow = new ManageGenresWindow();
            manageGenresWindow.ShowDialog();

            // После закрытия окна управления жанрами обновляем список жанров
            LoadAuthorsAndGenres();
            LoadBooks();
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksGrid.SelectedItem as Book;
            if (selectedBook != null)
            {
                var editBookWindow = new AddEditBookWindow(selectedBook);
                if (editBookWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadBooks();
                }
            }
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksGrid.SelectedItem as Book;
            if (selectedBook != null)
            {
                _context.Books.Remove(selectedBook);
                _context.SaveChanges();
                LoadBooks();
            }
        }
    }
}