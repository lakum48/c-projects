using System;
using System.Collections.Generic;

namespace lib
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();

        // Вычисляемое свойство для отображения полного имени
        public string FullName => $"{FirstName} {LastName}";
    }
}