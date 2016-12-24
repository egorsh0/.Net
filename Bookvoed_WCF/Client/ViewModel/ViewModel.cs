using System.Collections.Generic;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;

namespace Bookvoed.ViewModel
{
    class ViewModel : ViewModelBase
    {
        //Команды
        private ICommand _searchID;
        private ICommand _searchKeyword;
        private ICommand _searchAuthor;
        //Запросы
        public string queryId { get; set; }
        public string queryKeyword { get; set; }
        public string queryAuthor { get; set; }

        private dbBook _showIDValue;
        private List<Book> _showBooksValue;
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

        public List<Book> ShowAuthorsInfo
        {
            get { return _showBooksValue; }
            set
            {
                _showBooksValue = value;
                RaisePropertyChanged(() => ShowAuthorsInfo);
            }
        }

        public ICommand SearchID
        {
            get
            {
                return _searchID ?? (_searchID = new RelayCommand(() =>
                {
                    ShowIDInfo = Parser.getBookById(queryId);
                }));
            }
        }

        public ICommand SearchKeyword
        {
            get
            {
                return _searchKeyword ?? (_searchKeyword = new RelayCommand(() =>
                {
                    ShowKeywordInfo = Parser.getBookByKeyword(queryKeyword);
                }));
            }
        }

        public ICommand SearchAuthor
        {
            get
            {
                return _searchAuthor ?? (_searchAuthor = new RelayCommand(() =>
                {
                    ShowAuthorsInfo = Parser.getBooksByAuthor(queryAuthor);
                }));
            }
        }

    }
}
