using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bookvoed
{
    class Program
    {

        private static void getInfoForBook(string Id)
        {
            string htmlBookvoed = "http://www.bookvoed.ru/book?id=" + Id;
            HtmlDocument doc = null;

            try
            {
                HtmlWeb webDocument = new HtmlWeb();
                doc = webDocument.Load(htmlBookvoed);
                var nodesNamed = doc.DocumentNode.SelectNodes("/html/body/div[1]/div[1]/div/h1/text()[1]");
                var nodesPrice = doc.DocumentNode.SelectNodes("//*[@id=\"book_buttons\"]/div[3]/div[2]/div[2]");
                //var nodesDescription = doc.DocumentNode.SelectNodes("//*[@id=\"aboutTabs_descr_content\"]/div/div/text()[1]");
                var nodesDescription = doc.DocumentNode.SelectNodes("//*[@id=\"aboutTabs_descr_content\"]/div/div");

                var innerTextNamed = nodesNamed.Select(node => node.InnerText);
                var innerTextPrice = nodesPrice.Select(node => node.InnerText);
                var innerTextDescription = nodesDescription.Select(node => node.InnerText);

                Console.WriteLine();
                Console.WriteLine("Named: \t\t" + innerTextNamed.ToList()[0]);
                Console.WriteLine("Price: " + innerTextPrice.ToList()[0]);
                Console.WriteLine("Description: \n" + innerTextDescription.ToList()[0]);
            }
            catch (System.Net.WebException ex)
            {
                Console.WriteLine("Error load page >> " + ex.Message);
            }
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
                HtmlDocument doc = null;

                try
                {
                    HtmlWeb webDocument = new HtmlWeb();
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
                            getInfoForBook(new Regex(@"(?<=[\?&]id=)\d+(?=\&|\#|$)").Match(att.Value).Value);
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
