using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataLayer.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNotecardUI.Models;

namespace WpfNotecardUI.ViewModels.DialogViewModels
{
    public class AddJapanWordViewModel : ObservableObject, INotifyDataErrorInfo
    {
        private readonly string _topicName;
        public string TopicName { get { return _topicName; } }

        public readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();
        private string _itemQuestion = string.Empty;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public string ItemQuestion {
            get { return _itemQuestion; }
            set
            {
                _itemQuestion = value;
                ClearErrors(nameof(ItemQuestion));
                if (!_itemQuestion.Contains(_topicName))
                {
                    AddError(nameof(ItemQuestion), $"Needs to have {_topicName} inside the question");
                }
            }
        }
        public string ItemAnswer { get; set; } = string.Empty;
        public string Hint { get; set; } = string.Empty;
        public int MemorizationLevel { get; set; } = 0;
        public DateTime LastTimeAccessed { get; set; } = DateTime.Now;
        public int JlptLevel { get; set; } = 0;
        public bool IsFocusOn { get; set; } = false;
        public bool IsCommonWord { get; set; } = false;
        public int PageNumber { get; set; } = 0;
        public int Order { get; set; } = 0;
        


        public RelayCommand AddCommand { get; }
        

        public AddJapanWordViewModel(string topicName)
        {
            _topicName = topicName;
            AddCommand = new RelayCommand(AddWordFunction, CanAdd);
        }

        private void AddWordFunction()
        {
            throw new NotImplementedException();
        }

        public bool CanAdd()
        {
            if (ItemQuestion == string.Empty) { return false; }
            return !HasErrors;
        }

        private JapaneseWordNoteCard ResetNotecard()
        {
            var newJapanNoteCard = new JapaneseWordNoteCard();
            newJapanNoteCard.SentenceNoteCard = new SentenceNoteCard();
            newJapanNoteCard.SentenceNoteCard.LastTimeAccess = DateTime.Now;
            newJapanNoteCard.SentenceNoteCard.Chapters = new List<ChapterNoteCard>
            {
                new ChapterNoteCard
                {
                    TopicName = _topicName
                }
            };
            newJapanNoteCard.SentenceNoteCard.ChapterSentences = new List<ChapterNoteCardSentenceNoteCard>
            {
                new ChapterNoteCardSentenceNoteCard
                {
                    ChapterNoteCardTopicName = _topicName,
                    ExtraJishoInfo = new ExtraJishoInfoOnBridge
                    {
                        PageNumber = 17861623,
                        Order = 721346
                    }
                }
            };
            return newJapanNoteCard;
        }


        public bool HasErrors => _propertyErrors.Any();
        public IEnumerable GetErrors(string? propertyName)
        {
            return _propertyErrors.GetValueOrDefault(propertyName, null);
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Add(propertyName, new List<string>());
            }
            _propertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }
        public void ClearErrors(string propertyName)
        {
            _propertyErrors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            AddCommand.NotifyCanExecuteChanged();
        }
    }
}
