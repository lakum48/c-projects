using System.Windows;

namespace lib
{
    public partial class AddEditGenreWindow : Window
    {
        public Genre Genre { get; private set; }

        public AddEditGenreWindow(Genre genre = null)
        {
            InitializeComponent();
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

            DialogResult = true;
            Close();
        }
    }
}