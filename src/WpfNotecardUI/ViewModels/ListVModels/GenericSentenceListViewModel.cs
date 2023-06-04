using DataLayer.IRepos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNotecardUI.Models;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;

namespace WpfNotecardUI.ViewModels.ListVModels
{
    public class GenericSentenceListViewModel : AbstractListVModel<SentenceNoteCardModel>
    {
        private readonly string _topicName;

        public GenericSentenceListViewModel(string topicName, NavigationStore navigationStore, IServiceProvider serviceProvider)
            : base(navigationStore, serviceProvider)
        {
            _topicName = topicName;
            GetDataForList();
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
        }
    }
}
