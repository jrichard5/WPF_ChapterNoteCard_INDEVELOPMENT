using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNotecardUI.Models;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;

namespace WpfNotecardUI.ViewModels.ListVModels
{
    class JapaneseWordListViewModel : AbstractListVModel<JapaneseWordListItemModel>
    {
        private readonly KanjiListItemModel _item;
        public JapaneseWordListViewModel(KanjiListItemModel item, NavigationStore navigationStore, IServiceProvider serviceProvider)
            : base(navigationStore, serviceProvider)
        {
            _item = item;
            GetDataForList();
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
    }
}
