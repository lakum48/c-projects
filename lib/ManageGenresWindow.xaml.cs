using System.Linq;
using System.Windows;

namespace lib
{
    public partial class ManageGenresWindow : Window
    {
        private LibraryContext _context;

        public ManageGenresWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadGenres();
        }

        private void LoadGenres()
        {
            GenresGrid.ItemsSource = _context.Genres.ToList();
        }

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            var addGenreWindow = new AddEditGenreWindow();
            if (addGenreWindow.ShowDialog() == true)
            {
                _context.Genres.Add(addGenreWindow.Genre);
                _context.SaveChanges();
                LoadGenres();
            }
        }

        private void EditGenre_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenre = GenresGrid.SelectedItem as Genre;
            if (selectedGenre != null)
            {
                var editGenreWindow = new AddEditGenreWindow(selectedGenre);
                if (editGenreWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    LoadGenres();
                }
            }
        }

        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            var selectedGenre = GenresGrid.SelectedItem as Genre;
            if (selectedGenre != null)
            {
                _context.Genres.Remove(selectedGenre);
                _context.SaveChanges();
                LoadGenres();
            }
        }
    }
}