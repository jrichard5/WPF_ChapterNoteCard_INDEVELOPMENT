using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Models.TreeNodes
{
    public class ChapterTreeModel : ObservableObject
    {
        public string ChapterName { get; set; }
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
    }
}
