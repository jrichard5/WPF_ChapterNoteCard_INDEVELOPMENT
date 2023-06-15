using CommunityToolkit.Mvvm.Input;
using DataLayer.Entities;
using DataLayer.IRepos;
using DataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using WpfNotecardUI.Mappers;
using WpfNotecardUI.Models;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;
using WpfNotecardUI.ViewModels.DialogViewModels;
using WpfNotecardUI.Views.Dialogs;

namespace WpfNotecardUI.ViewModels.ListVModels
{
    class JapaneseWordListViewModel : AbstractListVModel<JapaneseWordListItemModel>
    {
        private readonly KanjiListItemModel _item;
        private int pageNumber = 1;
        private int NUMBER_PER_PAGE = 10 ;
        private bool _isPageLoading;
        public bool IsPageLoading 
        {
            get { return _isPageLoading; }
            set
            {
                _isPageLoading = value;
                OnPropertyChanged(nameof(IsPageLoading));
            } 
        }
        public KanjiListItemModel Item { get { return _item; } }
        public RelayCommand<JapaneseWordListItemModel> SaveData { get; }
        public ICommand ListItemChanged { get; }
        public ICommand AddWordCommand { get; }
        public RelayCommand PreviousPageCommand { get; }
        public RelayCommand NextPageCommand { get; }

        public int PageNumber
        {
            get { return pageNumber; }
            set 
            { 
                pageNumber = value;
                OnPropertyChanged(nameof(PageNumber));
                NextPageCommand.NotifyCanExecuteChanged();
                PreviousPageCommand.NotifyCanExecuteChanged();
            }
        }

        private int? maxPageCount;
        public int? MaxPageCount
        {
            get { return maxPageCount; }
            set 
            { 
                maxPageCount = value;
                NextPageCommand.NotifyCanExecuteChanged();
            }
        }

        private string? lastPageNumber = "?";
        public string? LastPageNumber 
        { 
            get { return  lastPageNumber; } 
            set
            {
                lastPageNumber = value;
                OnPropertyChanged(nameof(LastPageNumber));
            }
        }
        //used to figure out which list items have changed because Primary Id is the TopicName.
        public List<string> ItemQuestionsThatHaveChanged {get; set;}

        public JapaneseWordListViewModel(KanjiListItemModel item, NavigationStore navigationStore, IServiceProvider serviceProvider)
            : base(navigationStore, serviceProvider)
        {
            ItemQuestionsThatHaveChanged = new List<string>();
            _item = item;
            GetDataForList();
            SaveData = new RelayCommand<JapaneseWordListItemModel>(SaveDataFunction, CanSaveData);
            ListItemChanged = new RelayCommand<string>(AddToHaveChangedList);
            AddWordCommand = new RelayCommand(AddWordFunction);
            PreviousPageCommand = new RelayCommand(PreviousPageFunction, CanPreviousPage);
            NextPageCommand = new RelayCommand(NextPageFunction, CanNextPage);
            GetCountFunction();
        }

        private void PreviousPageFunction()
        {
            PageNumber -= 1;
            GetDataForList();
            OnPropertyChanged(nameof(CurrentList));
        }

        private bool CanPreviousPage()
        {
            return PageNumber > 1;
        }

        private void NextPageFunction()
        {
            PageNumber += 1;
            GetDataForList();
            OnPropertyChanged(nameof(CurrentList));
        }
        private bool CanNextPage()
        {
            if (MaxPageCount is null)
            {
                return false;
            }
            if (MaxPageCount % NUMBER_PER_PAGE == 0)
            {
                // 100 / 20 = 5      1:1-20 2:21-40 3:41-60 4:61:80, 5:81-100  so can't have page 6
                return MaxPageCount / NUMBER_PER_PAGE > PageNumber;
            }
            return (MaxPageCount / NUMBER_PER_PAGE) + 1 > PageNumber;

        }

        private void AddWordFunction()
        {
            AddJapanWordViewModel vm = new AddJapanWordViewModel(_item.TopicName, _serviceProvider);
            JapanWordDialog dialog = new JapanWordDialog
            {
                DataContext = vm
            };
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();

            GetDataForList();
        }
        public void AddToHaveChangedList(string? topicName)
        {
            if (topicName == null)
            {
                return;
            }
            if (!ItemQuestionsThatHaveChanged.Contains(topicName))
            {
                ItemQuestionsThatHaveChanged.Add(topicName);
                SaveData.NotifyCanExecuteChanged();


                //OnPropertyChanged(nameof(TopicNamesThatHaveChanged));
                //CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool CanSaveData(JapaneseWordListItemModel? obj)
        {
            var hasAny = ItemQuestionsThatHaveChanged.Any();
            return hasAny;
        }

        public async void SaveDataFunction(JapaneseWordListItemModel? hi)
        {
            if (CurrentList == null)
            {
                return;
            }
            List<JapaneseWordListItemModel> many = CurrentList.Where(item => ItemQuestionsThatHaveChanged.Contains(item.ItemQuestion)).ToList();
            List<JapaneseWordNoteCard> backToDb = new List<JapaneseWordNoteCard>();
            foreach (var item in many)
            {
                backToDb.Add(JapanWordListItemToDataLayer.ConvertToNoteCard(item, _item.TopicName));
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var genericRepo = scopedServiceProvider.GetRequiredService<IJapaneseWordNoteCardRepo>();
                await genericRepo.BulkUpdate(backToDb);
            }
            ItemQuestionsThatHaveChanged.Clear();
            SaveData.NotifyCanExecuteChanged();
        }

        private async void GetCountFunction()
        {
            IsPageLoading = true;
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var genericRepo = scopedServiceProvider.GetRequiredService<IJapaneseWordNoteCardRepo>();
                MaxPageCount = await genericRepo.CountFromOneChapter(_item.TopicName);
                
            }
            var tempLastPage = (MaxPageCount / NUMBER_PER_PAGE);
            if (MaxPageCount % NUMBER_PER_PAGE == 0)
            {
                LastPageNumber = tempLastPage.ToString();
            }
            else
            {
                LastPageNumber = (tempLastPage + 1).ToString();
            }
            IsPageLoading = false;
        }

        public override async void GetDataForList()
        {
            IsLoading = true;
            CurrentList = new List<JapaneseWordListItemModel>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var japanRepo = scopedServiceProvider.GetRequiredService<IJapaneseWordNoteCardRepo>();
                //var wordList = await japanRepo.GetAllFromOneCategory(_item.TopicName);
                var wordList = await japanRepo.GetPerPageFromOneCategory(_item.TopicName, pageNumber, NUMBER_PER_PAGE);
                foreach( var word in wordList )
                {
                    CurrentList.Add(new JapaneseWordListItemModel(word));
                }
                OnPropertyChanged(nameof(CurrentList));
            }
            IsLoading = false;
        }

        public override void GoToPreviousHandler()
        {
            MessageBoxIfDetectChanges();
            base.GoToPreviousHandler();
        }

        public override void GoToStartHandler()
        {
            MessageBoxIfDetectChanges();
            base.GoToStartHandler();
        }

        public void MessageBoxIfDetectChanges()
        {
            if (ItemQuestionsThatHaveChanged.Any())
            {
                var result = MessageBox.Show("An item was editted atleast once, do you want to save before you exit?",
                    "Save Items",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                //Could use an if statement, but wanted this to be here to remember
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        SaveData.Execute(null);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }
        ~JapaneseWordListViewModel()
        {
            Debug.WriteLine("disposed of japanesewordModel");
        }
    }
}
