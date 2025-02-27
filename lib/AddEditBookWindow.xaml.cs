using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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
            Authors = new ObservableCollection<Author>(_context.Authors.ToList());
            Genres = new ObservableCollection<Genre>(_context.Genres.ToList());
            
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

            DialogResult = true;
            Close();
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