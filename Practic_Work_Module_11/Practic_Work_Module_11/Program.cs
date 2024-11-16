using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practic_Work_Module_11
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public bool AvailabilityStatus { get; set; } // True - доступна, False - недоступна

        public Book(string title, string author, string genre, string isbn)
        {
            Title = title;
            Author = author;
            Genre = genre;
            ISBN = isbn;
            AvailabilityStatus = true;
        }

        public void ChangeAvailabilityStatus(bool status)
        {
            AvailabilityStatus = status;
        }

        public string GetBookInfo()
        {
            return $"Title: {Title}, Author: {Author}, Genre: {Genre}, ISBN: {ISBN}, Available: {AvailabilityStatus}";
        }
    }
    public abstract class User
    {
        public string Name { get; set; }
        public string TicketNumber { get; set; }

        public User(string name, string ticketNumber)
        {
            Name = name;
            TicketNumber = ticketNumber;
        }

        public abstract void Register();
    }
    public class Reader : User
    {
        public Reader(string name, string ticketNumber) : base(name, ticketNumber) { }

        public override void Register()
        {
            Console.WriteLine($"{Name} зарегистрирован как читатель.");
        }

        public void BorrowBook(Book book)
        {
            if (book.AvailabilityStatus)
            {
                book.ChangeAvailabilityStatus(false);
                Console.WriteLine($"{Name} взял книгу: {book.Title}");
            }
            else
            {
                Console.WriteLine("Книга недоступна.");
            }
        }
    }

    public class Librarian : User
    {
        public Librarian(string name, string ticketNumber) : base(name, ticketNumber) { }

        public override void Register()
        {
            Console.WriteLine($"{Name} зарегистрирован как библиотекарь.");
        }

        public void ReturnBook(Book book)
        {
            book.ChangeAvailabilityStatus(true);
            Console.WriteLine($"Книга {book.Title} возвращена в библиотеку.");
        }

        public void IssueBook(Book book)
        {
            Console.WriteLine($"Библиотекарь выдал книгу: {book.Title}");
        }
    }
    public class Loan
    {
        public Book Book { get; set; }
        public Reader Reader { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public Loan(Book book, Reader reader)
        {
            Book = book;
            Reader = reader;
            LoanDate = DateTime.Now;
        }

        public void IssueLoan()
        {
            Console.WriteLine($"Выдача книги {Book.Title} читателю {Reader.Name}.");
        }

        public void ReturnBook()
        {
            ReturnDate = DateTime.Now;
            Console.WriteLine($"Книга {Book.Title} возвращена {Reader.Name}.");
        }
    }
    public class Catalog
    {
        public List<Book> Books { get; set; }

        public Catalog()
        {
            Books = new List<Book>();
        }

        public void AddBook(Book book)
        {
            Books.Add(book);
            Console.WriteLine($"Книга {book.Title} добавлена в каталог.");
        }

        public Book SearchBookByTitle(string title)
        {
            return Books.FirstOrDefault(book => book.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public List<Book> SearchBooksByAuthor(string author)
        {
            return Books.Where(book => book.Author.Equals(author, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Book> SearchBooksByGenre(string genre)
        {
            return Books.Where(book => book.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
    public class Report
    {
        public void GenerateBookPopularityReport(Catalog catalog)
        {
            Console.WriteLine("Отчет по популярности книг:");
            foreach (var book in catalog.Books)
            {
                Console.WriteLine(book.GetBookInfo());
            }
        }

        public void GenerateReaderActivityReport(List<Reader> readers)
        {
            Console.WriteLine("Отчет по активности читателей:");
            foreach (var reader in readers)
            {
                Console.WriteLine($"Читатель: {reader.Name}, Номер билета: {reader.TicketNumber}");
            }
        }
    }
    public class Library
    {
        public Catalog Catalog { get; set; }
        public List<Reader> Readers { get; set; }
        public List<Librarian> Librarians { get; set; }

        public Library()
        {
            Catalog = new Catalog();
            Readers = new List<Reader>();
            Librarians = new List<Librarian>();
        }

        public void RegisterReader(Reader reader)
        {
            Readers.Add(reader);
            reader.Register();
        }

        public void RegisterLibrarian(Librarian librarian)
        {
            Librarians.Add(librarian);
            librarian.Register();
        }

        public void IssueBook(Book book, Reader reader)
        {
            if (book.AvailabilityStatus)
            {
                reader.BorrowBook(book);
                Loan loan = new Loan(book, reader);
                loan.IssueLoan();
            }
            else
            {
                Console.WriteLine("Книга недоступна.");
            }
        }

        public void ReturnBook(Book book, Reader reader)
        {
            Librarian librarian = Librarians.FirstOrDefault();
            librarian.ReturnBook(book);
            Loan loan = new Loan(book, reader);
            loan.ReturnBook();
        }

        public void GenerateReports()
        {
            Report report = new Report();
            report.GenerateBookPopularityReport(Catalog);
            report.GenerateReaderActivityReport(Readers);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();

            Book book1 = new Book("The Great Gatsby", "F. Scott Fitzgerald", "Fiction", "123456789");
            Book book2 = new Book("1984", "George Orwell", "Dystopian", "987654321");

            library.Catalog.AddBook(book1);
            library.Catalog.AddBook(book2);

            Reader reader1 = new Reader("John Doe", "R001");
            Librarian librarian = new Librarian("Alice Smith", "L001");

            library.RegisterReader(reader1);
            library.RegisterLibrarian(librarian);

            library.IssueBook(book1, reader1);
            library.ReturnBook(book1, reader1);

            library.GenerateReports();

            Console.ReadKey();
        }
    }


}
