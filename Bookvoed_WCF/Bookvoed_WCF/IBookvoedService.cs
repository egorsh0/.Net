using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Bookvoed_WCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IBookvoedService" в коде и файле конфигурации.
    [ServiceContract]
    public interface IBookvoedService
    {
        [OperationContract]
        dbBook getBookById(string ID);

        [OperationContract]
        dbBook getBookByKeyword(string keyword);

        [OperationContract]
        List<Book> getBookByAuthor(string author);
    }
}
