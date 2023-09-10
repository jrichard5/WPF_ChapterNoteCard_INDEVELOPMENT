using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Models.TreeNodes
{
    public class CategoryTreeModel : ObservableObject
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ObservableCollection<ChapterTreeModel> Children { get; set; }

        private bool isFocused;
        public bool IsFocused
        {
            get { return isFocused; }
            set
            {
                isFocused = value;
                OnPropertyChanged(nameof(IsFocused));
            }
        }

        public CategoryTreeModel()
        {
            Children = new ObservableCollection<ChapterTreeModel>();
        }
    }
}
