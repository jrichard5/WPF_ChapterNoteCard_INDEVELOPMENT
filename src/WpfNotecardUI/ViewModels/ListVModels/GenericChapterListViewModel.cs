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
    public class GenericChapterListViewModel : AbstractListVModel<ChapterItemModel>
    {
        private readonly int _categoryId;

        public GenericChapterListViewModel(int categoryId, NavigationStore navigationStore, IServiceProvider serviceProvider)
            : base(navigationStore, serviceProvider)
        {
            _categoryId = categoryId;
            GetDataForList();
        }

        public void SwitchToGenericSentenceView(ChapterItemModel item)
        {
            _navigationStore.CurrentViewModel = new GenericSentenceListViewModel(item.TopicName, _navigationStore, _serviceProvider);
        }

        public override async void GetDataForList()
        {
            IsLoading = true;
            CurrentList = new List<ChapterItemModel>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var chapterRepo = scopedServiceProvider.GetRequiredService<IChapterNoteCardRepo>();
                var chapterList = await chapterRepo.GetAllChaptersWithinACategory(_categoryId);
                foreach (var chapter in chapterList)
                {
                    CurrentList.Add(new ChapterItemModel(chapter));
                }
            }
            IsLoading = false;
        }
    }
}
