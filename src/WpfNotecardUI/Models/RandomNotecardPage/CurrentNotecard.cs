using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNotecardUI.Models.DataObjects;

namespace WpfNotecardUI.Models.RandomNotecardPage
{
    public class CurrentNotecard : ObservableObject
    {
        public bool IsChapter { get; set; }
        public int CurrentIndex { get; set; }

        private bool _isFront;
        public bool IsFront
        {
            get { return _isFront; }
            set { _isFront = value; }
        }

        private string _hint;
        public string Hint
        {
            get { return _hint; }
            set
            {
                _hint = value;
                OnPropertyChanged(nameof(Hint));
            }
        }

        private string _question;
        public string Question
        {
            get { return _question; }
            set
            {
                _question = value;
                OnPropertyChanged(nameof(Question));
            }
        }

        public bool[]? CharExistList { get; set; }
    }
}
