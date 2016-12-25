using HtmlAgilityPack;

using System;
using System.Linq;
using System.Text.RegularExpressions;

using LiteDB;
using System.Collections.Generic;

namespace Bookvoed_REST.Model
{
    class Parser
    {
        private static HtmlWeb webDocument = new HtmlWeb();
        private static HtmlDocument doc = null;

        private static string dbName = @"C:\Users\egors\OneDrive\MyShowsParserRest\MyShowsParserRest\MyShowsParserRest\data.db";
        private static string collectionName = "books";

        public static bool addInDB(dbBook book)
        {
            using (var Db = new LiteDatabase(dbName))
            {
                var Books = Db.GetCollection<dbBook>(collectionName);
                var result = Books.FindOne(x => x.BookId.Equals(book.BookId));
                if (result == null)
                {
                    Books.Insert(book);
                    return true;
                }
                return false;
            }
        }

        public static void deleteFromDb(string link)
        {
            using (var Db = new LiteDatabase(dbName))
            {
                var Books = Db.GetCollection<dbBook>(collectionName);
                Books.Delete(x => x.BookId.Equals(link));
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
            dbBook book = new dbBook();
            var searchLink = Resource.idUrl + link + "#info";
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
                string link = Resource.searchUrl + keyword;
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

        public static IEnumerable<dbBook> getBooks()
        {
            using (var Db = new LiteDatabase(dbName))
            {
                var Books = Db.GetCollection<dbBook>(collectionName);
                var result = Books.FindAll();
                return result;
            }
        }
    }
}