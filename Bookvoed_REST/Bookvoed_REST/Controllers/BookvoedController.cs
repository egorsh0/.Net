using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Bookvoed_REST.Models;

namespace Bookvoed_REST.Controllers
{
    public class Controller : ApiController
    {
        [HttpPost]
        public void AddInDB([FromBody]dbBook book)
        {
            var Book = Parser.addInDB(book);
            if (Book == false)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Книга {0} существует (id = {1})", book.Name, book.BookId)),
                    ReasonPhrase = "Повторите ввод"
                };
                throw new HttpResponseException(response);
            }
        }

        [HttpDelete]
        public void DeleteBook(string link)
        {
            Parser.deleteFromDb(link);
        }

        public dbBook getBookById(string id)
        {
            var book = Parser.getBookById(id);

            if (book == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Нет книги c ID = {0}", id)),
                    ReasonPhrase = "Книги нет"
                };
                throw new HttpResponseException(response);
            }
            return book;
        }

        public dbBook getBookByKeyword(string keyword)
        {
            var book = Parser.getBookByKeyword(keyword);
            if (book == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Нет книги по ключевому слову '{0}'", keyword)),
                    ReasonPhrase = "Книги нет"
                };
                throw new HttpResponseException(response);
            }
            return book;
        }

        public IEnumerable<dbBook> getBooks()
        {
            return Parser.getBooks();
        }
    }
}