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
            LoadBooks();
        }

        private void LoadBooks()
        {
            BooksGrid.ItemsSource = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .ToList();
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
            LoadBooks();
        }
        private void ManageGenres_Click(object sender, RoutedEventArgs e)
        {
            var manageGenresWindow = new ManageGenresWindow();
            manageGenresWindow.ShowDialog();
            
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