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
using WpfNotecardUI.Models.DataObjects;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;
using WpfNotecardUI.ViewModels.DialogViewModels;
using WpfNotecardUI.Views.Dialogs;

namespace WpfNotecardUI.ViewModels.ListVModels
{
    class JapaneseWordListViewModel : AbstractListVMExtra<JapaneseWordListItemModel>
    {
        private readonly KanjiListItemModel _item;

        public KanjiListItemModel Item { get { return _item; } }
        //used to figure out which list items have changed because Primary Id is the TopicName.

        public JapaneseWordListViewModel(KanjiListItemModel item, NavigationStore navigationStore, IServiceProvider serviceProvider)
            : base(navigationStore, serviceProvider)
        {
            _item = item;
            GetDataForList();
            GetCountFunction();
        }



        public override void ExecuteShowDialog()
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

        public override async void SaveDataFunction(JapaneseWordListItemModel? hi)
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

        protected override async void GetCountFunction()
        {
            IsPageLoading = true;
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var genericRepo = scopedServiceProvider.GetRequiredService<IJapaneseWordNoteCardRepo>();
                MaxPageCount = await genericRepo.CountFromOneChapter(_item.TopicName);

            }
            LastPageNumber = GetCountHelperFunction((int)MaxPageCount);
            IsPageLoading = false;
        }

        public override async void GetDataForList()
        {
            IsLoading = true;
            var itemAnswerList = new List<string>();
            CurrentList = new List<JapaneseWordListItemModel>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var japanRepo = scopedServiceProvider.GetRequiredService<IJapaneseWordNoteCardRepo>();
                //var wordList = await japanRepo.GetAllFromOneCategory(_item.TopicName);
                var wordList = await japanRepo.GetPerPageFromOneCategory(_item.TopicName, pageNumber, NUMBER_PER_PAGE);
                foreach (var word in wordList)
                {
                    CurrentList.Add(new JapaneseWordListItemModel(word));
                    itemAnswerList.Add(word.ItemQuestion);
                }
                var dictionary = await japanRepo.GetCharacterExistFromList(itemAnswerList);
                foreach(var key in dictionary)
                {
                    //I wished I put the CharacterExist class in the data layer.  Feels super wrong to do for loop on string and still call the boolean with string's index
                    var currentlistItem = CurrentList.Where(item => item.ItemQuestion == key.Key).First();
                    for(var i = 0; i < key.Key.Length; i++)
                    {
                        // if string is 3 characters long.  Max index would be 2.  2 > 3-1 is still good.  2>4-1 should be array out of bounds.
                        //if (i > key.Value.Length - 1) { break; }
                        currentlistItem.CharExistList.Add(new CharacterExist { Character = key.Key[i].ToString(), IsExist = key.Value[i] });
                    }
                }
                OnPropertyChanged(nameof(CurrentList));
            }
            IsLoading = false;
        }

        public override void DeleteSelectedFunction()
        {
            var pkList = new List<SentenceNoteCard>();
            var itemsSelected = CurrentList.Where(item => item.IsSelectedForDeletion == true).ToList();
            foreach(var item in itemsSelected)
            {
                pkList.Add(ModelToEntityMapper.ToSentenceNoteCardPrimaryKey(item));
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var genericRepo = scopedServiceProvider.GetRequiredService<IGenericRepo<SentenceNoteCard>>();
                genericRepo.DeleteByList(pkList);
            }
            Debug.WriteLine("hi");
            GetDataForList();
        }

        ~JapaneseWordListViewModel()
        {
            Debug.WriteLine("disposed of japanesewordModel");
        }
    }
}
