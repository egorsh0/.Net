using HtmlAgilityPack;

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace Bookvoed
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected class OutBook
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }

        List<OutBook> OutputBooks;
        List<Book> Books;

        private static HtmlWeb webDocument;
        private static HtmlDocument doc;

        public MainWindow()
        {
            doc = null;

            webDocument = new HtmlWeb();

            Books = new List<Book>
            {
                new Book()
            };

            OutputBooks = new List<OutBook>
            {
                new OutBook()
            };
            InitializeComponent();
            createContextForAuthors();
        }

        private void createContextForAuthors()
        {
            BookContext context = new BookContext();
            var AllAuthor = context.Authors.Select(a => a.nameAuthor).ToList();
            AuthorsComboBox.ItemsSource = AllAuthor;
        }

        private void keywordSearch()
        {

        }

        private void IDSearch()
        {

        }

        private void keywordSearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IDSearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private static void getInfoByKeyword(string search)
        {
            Console.WriteLine();
            string htmlBookvoed = Resource.searchUrl + search;
            try
            {
                doc = webDocument.Load(htmlBookvoed);
                var nodesNamed = doc.DocumentNode.SelectNodes("//*[@id=\"books\"]//div//div//div//div//div//h5//a");
                var innerTextNamed = nodesNamed.Select(node => node.InnerText);

                for (int i = 0; i < nodesNamed.Count(); i++)
                {
                    Console.WriteLine(Convert.ToString(i + 1) + ") " + innerTextNamed.ToList()[i]);
                }

                Console.WriteLine();
                Console.WriteLine("Введите номер книги для рассмотрения => ");

                int numberBooks = Convert.ToInt32(Console.ReadLine());
                int index = 0;
                foreach (HtmlNode link in nodesNamed)
                {
                    if (index == (numberBooks - 1))
                    {
                        HtmlAttribute att = link.Attributes["href"];
                        getInfoById(new Regex(@"(?<=[\?&]id=)\d+(?=\&|\#|$)").Match(att.Value).Value);
                        break;
                    }
                    else index++;
                }
            }
            catch (System.Net.WebException ex)
            {
                Console.WriteLine("Error load page >> " + ex.Message);
            }
        }

        private static void getInfoById(string link)
        {
            bool isDb = false;
            using (var Db = new LiteDatabase(@"data.db"))
            {
                var Books = Db.GetCollection<dbBook>("books");
                var BookOfDb = Books.FindOne(x => x.BookId.Equals(link));
                if (BookOfDb != null)
                {
                    isDb = true;
                    Console.WriteLine("Работа с БД");
                    showInfo(BookOfDb);
                }
            }

            if (!isDb)
            {
                //Использование Selenium Web Driver для доступа к ресурсу
                ChromeDriver driver = new ChromeDriver();
                string baseUrl = Resource.idUrl + link;
                driver.Navigate().GoToUrl(baseUrl);
                driver.FindElement(By.CssSelector("#aboutTabs_info_name > div.tj.wB")).Click();

                var name = driver.FindElement(By.CssSelector("h1")).Text;
                var author = driver.FindElement(By.XPath("//*[@id=\"aboutTabs_info_content\"]/div/table/tbody/tr[1]/td[2]")).Text;
                var series = driver.FindElement(By.XPath("//*[@id=\"aboutTabs_info_content\"]/div/table/tbody/tr[3]/td[2]")).Text; ;
                var subject = driver.FindElement(By.XPath("//*[@id=\"aboutTabs_info_content\"]/div/table/tbody/tr[4]/td[2]")).Text;

                driver.Close();

                var BookForDB = new dbBook()
                {
                    BookId = link,
                    Name = name,
                    Author = author,
                    Series = series,
                    Subject = subject
                };
                showInfo(BookForDB);

                using (var Db = new LiteDatabase(@"data.db"))
                {
                    var Books = Db.GetCollection<dbBook>("books");
                    Books.Insert(BookForDB);
                }

                using (var Db = new BookContext())
                {
                    var authorNew = Db.Authors.Find(BookForDB.Author);
                    if (authorNew == null)
                    {
                        authorNew = new Author()
                        {
                            nameAuthor = BookForDB.Author
                        };
                    }

                    var bookNew = new Book()
                    {
                        BookId = BookForDB.BookId,
                        Name = BookForDB.Name,
                        Author = authorNew,
                        Series = BookForDB.Series,
                        Subject = BookForDB.Subject
                    };

                    Db.Books.Add(bookNew);
                    Db.SaveChanges();
                }
            }
            Console.WriteLine();
            Console.ReadKey();
        }

        private static void getBooksByAuthor(string author)
        {
            using (var Db = new BookContext())
            {
                var Author = Db.Authors.Find(author);
                var books = Db.Books.Where(b => b.Author.nameAuthor == author).Select(c => c);
                Console.WriteLine("Списо книг автора  {0}:", author);
                foreach (var Book in books)
                {
                    Console.WriteLine("\t {0}", Book.Name);
                }
                Console.ReadLine();
                Console.WriteLine();
            }
        }

        private static void getAuthors()
        {
            using (var Db = new BookContext())
            {
                var AllAuthors = Db.Authors.Select(a => a.nameAuthor).ToList();
                if (AllAuthors.Any())
                {
                    Console.WriteLine("Список всех авторов: ");
                    for (int i = 0; i < AllAuthors.Count; i++)
                    {
                        Console.WriteLine("{0}) {1}", i + 1, AllAuthors[i]);
                    }

                    {
                        Console.WriteLine("\t Книги от автора => ");
                        int numAuthor;
                        if (Int32.TryParse(Console.ReadLine(), out numAuthor))
                        {
                            if ((numAuthor + 1 > 0) && (numAuthor - 1 < AllAuthors.Count))
                            {
                                getBooksByAuthor(AllAuthors[numAuthor - 1]);
                            }
                            else
                            {
                                Console.WriteLine("Номер автора не верен");
                            }
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Было введено не число");
                        }
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Список пуст");
                }
            }
        }
    }
}
