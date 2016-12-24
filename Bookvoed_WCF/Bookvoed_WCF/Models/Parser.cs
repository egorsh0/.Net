using HtmlAgilityPack;

using System;
using System.Linq;
using System.Text.RegularExpressions;

using LiteDB;
using System.Collections.Generic;

namespace Bookvoed_WCF
{
    class Parser
    {
        private static HtmlWeb webDocument = new HtmlWeb();
        private static HtmlDocument doc = null;

        private static string dbName = @"C:\Users\egors\OneDrive\dotNet\.Net\Bookvoed_WCF\Bookvoed_WCF\data.db";
        private static string collectionName = "books";

        public static void addInDB(dbBook book)
        {
            using (var Db = new LiteDatabase(dbName))
            {
                var Books = Db.GetCollection<dbBook>(collectionName);
                Books.Insert(book);
            }
        }

        public static dbBook searchBookInDB(string link)
        {
            using (var Db = new LiteDatabase(dbName))
            {
                var Books = Db.GetCollection<dbBook>(collectionName);
                var book = Books.FindOne(x => x.BookId.Equals(link));
                return book;
            }
        }

        public static void addInContext(dbBook book)
        {
            using (var Db = new BookContext())
            {
                var author = Db.Authors.Find(book.Author);
                if (author == null)
                {
                    author = new Author()
                    {
                        nameAuthor = book.Author
                    };
                }

                var bookNew = new Book()
                {
                    BookId = book.BookId,
                    Name = book.Name,
                    Author = author,
                    Series = book.Series,
                    Subject = book.Subject
                };

                Db.Books.Add(bookNew);
                Db.SaveChanges();
            }
        }

        public static string getBookID(string searchLink)
        {
            try
            {
                doc = webDocument.Load(searchLink);
                var nodesNamed = doc.DocumentNode.SelectNodes("//*[@id=\"books\"]//div//div//div//div//div//h5//a");
                var innerTextNamed = nodesNamed.Select(node => node.InnerText);

                int index = 0;
                string ID = string.Empty;

                foreach (HtmlNode link in nodesNamed)
                {
                    if (index == 0)
                    {
                        HtmlAttribute att = link.Attributes["href"];
                        ID = new Regex(@"(?<=[\?&]id=)\d+(?=\&|\#|$)").Match(att.Value).Value;
                        return ID;
                    }
                }

                return ID;
            }
            catch (System.Net.WebException ex)
            {
                Console.WriteLine("Error load page >> " + ex.Message);
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static dbBook getBookInfo(string link)
        {
            var searchLink = "http://www.bookvoed.ru/book?id=" + link + "#info";
            doc = webDocument.Load(searchLink);
            try
            {
                var nameBook = doc.DocumentNode.SelectSingleNode("//h1[contains(@itemprop,'name')]").InnerText.Replace(@"\r\n", " ");
                var authorBook = doc.DocumentNode.SelectSingleNode("//*[@id=\"aboutTabs_info_content\"]//div//table//tr[1]//td[2]//a").InnerText;
                var seriesBook = doc.DocumentNode.SelectSingleNode("//*[@id=\"aboutTabs_info_content\"]//div//table//tr[3]//td[2]//a").InnerText;
                var subjectBook = doc.DocumentNode.SelectSingleNode("//*[@id=\"aboutTabs_info_content\"]//div//table//tr[4]//td[2]//a").InnerText;

                var BookForDB = new dbBook()
                {
                    BookId = link,
                    Name = nameBook,
                    Author = authorBook,
                    Series = seriesBook,
                    Subject = subjectBook
                };

                addInDB(BookForDB);
                addInContext(BookForDB);

                return BookForDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return null;
            }
        }

        public static dbBook getBookById(string id)
        {
            try
            {
                var result = searchBookInDB(id);
                if (result == null)
                    return getBookInfo(id);
                else
                {
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static dbBook getBookByKeyword(string keyword)
        {
            try
            {
                string link = "http://www.bookvoed.ru/books?q=" + keyword;
                var id = getBookID(link);

                if (id == String.Empty)
                {
                    return null;
                }

                var result = searchBookInDB(id);

                if (result == null)
                    return getBookInfo(id);
                else
                {
                    return result;
                }
            }

            catch (Exception)
            {
                return null;
            }
        }

        public static List<Book> getBooksByAuthor(string author)
        {
            using (var Db = new BookContext())
            {
                List<Book> Books = Db.Books.Where(b => b.Author.nameAuthor.ToLower() == author.ToLower()).ToList();
                return Books;
            }
        }
    }
}