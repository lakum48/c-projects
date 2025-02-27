using System.Linq;
using System.Windows;

namespace lib
{
    public partial class ManageAuthorsWindow : Window
    {
        private LibraryContext _context;

        public ManageAuthorsWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            AuthorsGrid.ItemsSource = _context.Authors.ToList();
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var addAuthorWindow = new AddEditAuthorWindow();
            if (addAuthorWindow.ShowDialog() == true)
            {
                _context.Authors.Add(addAuthorWindow.Author);
                _context.SaveChanges();
                LoadAuthors();
            }
        }
        

        private void EditAuthor_Click(object sender, RoutedEventArgs e)
        {
            var selectedAuthor = AuthorsGrid.SelectedItem as Author;
            if (selectedAuthor != null)
            {
                var editAuthorWindow = new AddEditAuthorWindow(selectedAuthor);
                if (editAuthorWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadAuthors();
                }
            }
        }

        private void DeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            var selectedAuthor = AuthorsGrid.SelectedItem as Author;
            if (selectedAuthor != null)
            {
                _context.Authors.Remove(selectedAuthor);
                _context.SaveChanges();
                LoadAuthors();
            }
        }
    }
}