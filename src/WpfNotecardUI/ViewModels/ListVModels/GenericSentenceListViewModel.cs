using DataLayer.IRepos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNotecardUI.Models;
using WpfNotecardUI.Services.RealServices;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;
using WpfNotecardUI.ViewModels.DialogViewModels;

namespace WpfNotecardUI.ViewModels.ListVModels
{
    public class GenericSentenceListViewModel : AbstractListVMExtra<SentenceNoteCardModel>
    {
        private readonly string _topicName;

        public GenericSentenceListViewModel(string topicName, NavigationStore navigationStore, IServiceProvider serviceProvider)
            : base(navigationStore, serviceProvider)
        {
            _topicName = topicName;
            GetDataForList();
            GetCountFunction();
        }

        public override void ExecuteShowDialog()
        {
            AddSentenceViewModel sentenceVM = new AddSentenceViewModel(_topicName, _serviceProvider);
            _dialogService = new DialogServices<AddSentenceViewModel>(sentenceVM);
            //TODO: When adding, I don't re get the count for pages
            _dialogService.ShowDialog(result =>
            {
                GetDataForList();
                GetCountFunction();
            });
        }

        public override async void GetDataForList()
        {
            IsLoading = true;
            CurrentList = new List<SentenceNoteCardModel>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var sentenceRepo = scopedServiceProvider.GetRequiredService<ISentenceNoteCardRepo>();
                var sentenceList = await sentenceRepo.GetAllWithAChapter(_topicName);
                foreach (var sentence in sentenceList)
                {
                    CurrentList.Add(new SentenceNoteCardModel(sentence));
                }
            }
            IsLoading = false;
            OnPropertyChanged(nameof(CurrentList));
        }

        public override void SaveDataFunction(SentenceNoteCardModel? hi)
        {
            throw new NotImplementedException();
        }

        protected override async void GetCountFunction()
        {
            IsPageLoading = true;
            using(var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var sentRepo = scopedServiceProvider.GetRequiredService<ISentenceNoteCardRepo>();
                MaxPageCount = await sentRepo.CountFromOneChapter(_topicName);
            }
            LastPageNumber = GetCountHelperFunction((int)MaxPageCount);
            IsPageLoading=false;
        }
    }
}
