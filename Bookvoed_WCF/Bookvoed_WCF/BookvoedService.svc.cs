using System;
using System.Collections.Generic;

namespace Bookvoed_WCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "BookvoedService" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы BookvoedService.svc или BookvoedService.svc.cs в обозревателе решений и начните отладку.
    public class BookvoedService : IBookvoedService
    {
        public List<Book> getBookByAuthor(string author)
        {
            return Parser.getBooksByAuthor(author);
        }

        public dbBook getBookById(string ID)
        {
            return Parser.getBookById(ID);
        }

        public dbBook getBookByKeyword(string keyword)
        {
            return Parser.getBookByKeyword(keyword);
        }
    }
}
