using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Models.TreeNodes
{
    public class CategoryTreeModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ObservableCollection<ChapterTreeModel> Children { get; set; }
        public bool IsFocused { get; set; }

        public CategoryTreeModel() 
        {
            Children = new ObservableCollection<ChapterTreeModel>();
        }
    }
}
