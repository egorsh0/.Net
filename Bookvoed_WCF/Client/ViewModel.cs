using System.Collections.Generic;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Client.BookvoedService;

namespace Bookvoed
{
    class ViewModel : ViewModelBase
    {
        BookvoedServiceClient client = new BookvoedServiceClient();
        //Команды
        private ICommand _searchID;
        private ICommand _searchKeyword;
        //Запросы
        public string queryId { get; set; }
        public string queryKeyword { get; set; }

        private dbBook _showIDValue;
        private dbBook _showKeywordValue;

        public dbBook ShowIDInfo
        {
            get { return _showIDValue; }
            set
            {
                _showIDValue = value;
                RaisePropertyChanged(() => ShowIDInfo);
            }
        }

        public dbBook ShowKeywordInfo
        {
            get { return _showKeywordValue; }
            set
            {
                _showKeywordValue = value;
                RaisePropertyChanged(() => ShowKeywordInfo);
            }
        }

        public ICommand SearchID
        {
            get
            {
                return _searchID ?? (_searchID = new RelayCommand(() =>
                {
                    ShowIDInfo = client.getBookById(queryId);
                }));
            }
        }

        public ICommand SearchKeyword
        {
            get
            {
                return _searchKeyword ?? (_searchKeyword = new RelayCommand(() =>
                {
                    ShowKeywordInfo = client.getBookByKeyword(queryKeyword);
                }));
            }
        }
    }
}
