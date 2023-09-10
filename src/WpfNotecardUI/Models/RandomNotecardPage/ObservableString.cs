using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Models.RandomNotecardPage
{
    public class ObservableString : ObservableObject
    {
        private string _text;
        public string Text
        {
            get { return _text; } 
            set 
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
    }
}
