using CommunityToolkit.Mvvm.Input;
using DataLayer.Entities;
using DataLayer.IRepos;
using DataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using WpfNotecardUI.Mappers;
using WpfNotecardUI.Models;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;

namespace WpfNotecardUI.ViewModels.ListVModels
{
    class JapaneseWordListViewModel : AbstractListVModel<JapaneseWordListItemModel>
    {
        private readonly KanjiListItemModel _item;
        public RelayCommand<JapaneseWordListItemModel> SaveData { get; }
        public ICommand ListItemChanged { get; }

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

        public override async void GetDataForList()
        {
            IsLoading = true;
            CurrentList = new List<JapaneseWordListItemModel>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var japanRepo = scopedServiceProvider.GetRequiredService<IJapaneseWordNoteCardRepo>();
                var wordList = await japanRepo.GetAllFromOneCategory(_item.TopicName);
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
