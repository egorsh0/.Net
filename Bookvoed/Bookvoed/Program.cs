using HtmlAgilityPack;

using System;
using System.Linq;
using System.Text.RegularExpressions;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Bookvoed
{
    class Program
    {
        private static HtmlWeb webDocument = new HtmlWeb();
        private static HtmlDocument doc = null;

        private static void ById()
        {
            Console.WriteLine("Введите ID книги для поиска =>  ");
            string ID = Console.ReadLine();
            getInfoById(ID);
        }

        private static void ByKeyword()
        {
            Console.WriteLine("Введите ключевое слово для поиска =>  ");
            string search = Console.ReadLine();
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
            ChromeDriver driver = new ChromeDriver();
            string baseUrl = "http://www.bookvoed.ru/book?id=" + link;
            driver.Navigate().GoToUrl(baseUrl);
            driver.FindElement(By.CssSelector("#aboutTabs_info_name > div.tj.wB")).Click();
            
            var name = driver.FindElement(By.CssSelector("h1")).Text;
            name = Regex.Replace(name, @"\r\n", " ");
            var author = driver.FindElement(By.XPath("//*[@id=\"aboutTabs_info_content\"]/div/table/tbody/tr[1]/td[2]")).Text;
            var series = driver.FindElement(By.XPath("//*[@id=\"aboutTabs_info_content\"]/div/table/tbody/tr[3]/td[2]")).Text; ;
            var subject = driver.FindElement(By.XPath("//*[@id=\"aboutTabs_info_content\"]/div/table/tbody/tr[4]/td[2]")).Text;
            driver.Close();

            Book book = new Book
            {
                Name = name,
                Author = author,
                Subject = subject,
                Series = series
            };

            show(book);
            
            Console.ReadKey();
        }
        private static void Menu()
        {
            Console.Clear();
            Console.WriteLine("1) Поиск по ID");
            Console.WriteLine("2) Поиск по KEYWORD");
            Console.WriteLine("3) - EXIT");
        }

        private static void show(Book book)
        {
            Console.WriteLine();
            Console.WriteLine("{0, 20}: {1}", "Наименование", book.Name);
            Console.WriteLine("{0, 20}: {1}", "Автор", book.Author);
            Console.WriteLine("{0, 20}: {1}", "Серия", book.Series);
            Console.WriteLine("{0, 20}: {1}", "Издательство", book.Subject);
        }

        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Menu();
                int key;
                bool keyAction = Int32.TryParse(Console.ReadLine(), out key);

                if (keyAction)
                {
                    while (key > 3 || key < 1)
                    {
                        Console.Write("\nНеизвестное значение операции!\n\n");
                        Menu();
                        Int32.TryParse(Console.ReadLine(), out key);
                    }
                    switch (key)
                    {
                        case 1:
                            ById();
                            break;

                        case 2:
                            ByKeyword();
                            break;
                        case 3:
                            exit = true;
                            break;
                    }
                }
                else
                    Console.Write("\nНеизвестное значение операции!\n\n");
            }
        }
    }
}
