using HtmlAgilityPack;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bookvoed
{
    class Program
    {
        private static HtmlWeb webDocument = new HtmlWeb();
        private static HtmlDocument doc = null;

        private static void getInfoForBook(int Id, string link)
        {
            var nodesNamed = doc.DocumentNode.SelectNodes("//*[@id=\"books\"]/div[3]/div/div["+ Id +"]/div[3]/div/h5[1]");
            var nodesAuthor = doc.DocumentNode.SelectNodes("//*[@id=\"books\"]/div[3]/div/div["+ Id +"]/div[3]/div/h5[2]");
            var nodesSeries = doc.DocumentNode.SelectNodes("//*[@id=\"books\"]/div[3]/div/div[" + Id + "]/div[3]/div/div[2]/a[1]");
            var nodesSubjects = doc.DocumentNode.SelectNodes("//*[@id=\"books\"]/div[3]/div/div["+ Id + "]/div[3]/div/div[2]/a[2]");

            var innerTextNamed = nodesNamed.Select(node => node.InnerText);
            var innerTextAuthor = nodesAuthor.Select(node => node.InnerText);
            var innerTextSeries = nodesSeries.Select(node => node.InnerText);
            var innerTextSubject = nodesSubjects.Select(node => node.InnerText);

            Console.WriteLine();
            Console.WriteLine("Named: " + innerTextNamed.ToList()[0]);
            Console.WriteLine("Author: " + innerTextAuthor.ToList()[0]);
            Console.WriteLine("Author: " + innerTextSeries.ToList()[0]);
            Console.WriteLine("Subject: " + innerTextSubject.ToList()[0]);
            Console.WriteLine("Book Id: " + link);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Input value for search >> ");
                string search = Console.ReadLine();
                Console.WriteLine("//////////////////////////////////////////////////////////////////////////////");
                Console.WriteLine();
                string htmlBookvoed = "http://www.bookvoed.ru/books?q=" + search;
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
                    Console.WriteLine();
                    Console.WriteLine("Input number books >> ");

                    int numberBooks = Convert.ToInt32(Console.ReadLine());
                    int index = 0;
                    foreach (HtmlNode link in nodesNamed)
                    {
                        if (index == (numberBooks - 1))
                        {
                            HtmlAttribute att = link.Attributes["href"];
                            getInfoForBook(numberBooks, new Regex(@"(?<=[\?&]id=)\d+(?=\&|\#|$)").Match(att.Value).Value);
                            break;
                        }
                        else index++;
                    }
                }
                catch (System.Net.WebException ex)
                {
                    Console.WriteLine("Error load page >> " + ex.Message);
                }
                Console.ReadKey();
            }
        }
    }
}
