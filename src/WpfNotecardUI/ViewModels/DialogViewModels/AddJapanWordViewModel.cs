using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfNotecardUI.Models;

namespace WpfNotecardUI.ViewModels.DialogViewModels
{
    public class AddJapanWordViewModel : ObservableObject, INotifyDataErrorInfo
    {
        private readonly string _topicName;
        public string TopicName { get { return _topicName; } }

        public readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();
        private readonly IServiceProvider _serviceProvider;
        private string _itemQuestion = string.Empty;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public string ItemQuestion
        {
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
        private int _memorizationLevel = 0;
        public int MemorizationLevel
        {
            get { return _memorizationLevel; }
            set
            {
                _memorizationLevel = value;
                ClearErrors(nameof(MemorizationLevel));
                if (_memorizationLevel < 0)
                {
                    AddError(nameof(MemorizationLevel), $"Must be above 0");
                }
                if (_memorizationLevel > 100000)
                {
                    AddError(nameof(MemorizationLevel), $"Must be less than 100000");
                }
            }
        }
        public DateTime LastTimeAccessed { get; set; } = DateTime.Now;
        private int _jlptLevel = 1;

        public int JlptLevel
        {
            get { return _jlptLevel; }
            set
            {
                _jlptLevel = value;
                ClearErrors(nameof(JlptLevel));
                if (_jlptLevel < 0 || _jlptLevel > 6)
                {
                    AddError(nameof(JlptLevel), $"Number must be between 1 and 5");
                }
            }
        }
        public bool IsFocusOn { get; set; } = false;
        public bool IsCommonWord { get; set; } = false;
        public int PageNumber { get; set; } = 0;
        public int Order { get; set; } = 0;



        public RelayCommand AddCommand { get; }


        public AddJapanWordViewModel(string topicName, IServiceProvider serviceProvider)
        {

            _topicName = topicName;
            AddCommand = new RelayCommand(AddWordFunction, CanAdd);
            _serviceProvider = serviceProvider;
        }

        private async void AddWordFunction()
        {
            var notecard = NewNotecard();
            notecard.ItemQuestion = ItemQuestion;
            notecard.JLPTLevel = JlptLevel;
            notecard.IsCommonWord = IsCommonWord;
            notecard.SentenceNoteCard.ItemQuestion = ItemQuestion;
            notecard.SentenceNoteCard.MemorizationLevel = MemorizationLevel;
            notecard.SentenceNoteCard.Hint = Hint;
            notecard.SentenceNoteCard.ItemAnswer = ItemAnswer;
            notecard.SentenceNoteCard.IsUserWantsToFocusOn = IsFocusOn;
            notecard.SentenceNoteCard.LastTimeAccess = LastTimeAccessed;
            notecard.SentenceNoteCard.ChapterSentences.First().SentenceNoteCardItemQuestion = ItemQuestion;
            notecard.SentenceNoteCard.ChapterSentences.First().ExtraJishoInfo.PageNumber = PageNumber;
            notecard.SentenceNoteCard.ChapterSentences.First().ExtraJishoInfo.Order = Order;

            using (var scope = _serviceProvider.CreateScope())
            {
                var scopeServiceProvider = scope.ServiceProvider;
                var genericRepo = scopeServiceProvider.GetRequiredService<IJapaneseWordNoteCardRepo>();
                try
                {
                    await genericRepo.AddAndAddBridgeTableInfo(notecard);
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is not null)
                    {
                        MessageBox.Show(ex.InnerException.Message);
                    }
                }
            }
            ResetForm();
        }

        public bool CanAdd()
        {
            if (ItemQuestion == string.Empty) { return false; }
            return !HasErrors;
        }

        private void ResetForm()
        {
            _itemQuestion = string.Empty;
            OnPropertyChanged(nameof(ItemQuestion));
            ItemAnswer = string.Empty;
            OnPropertyChanged(nameof(ItemAnswer));
            Hint = string.Empty;
            OnPropertyChanged(nameof(Hint));
            MemorizationLevel = 0;
            OnPropertyChanged(nameof(MemorizationLevel));
            LastTimeAccessed = DateTime.Now;
            OnPropertyChanged(nameof(LastTimeAccessed));
            JlptLevel = 1;
            OnPropertyChanged(nameof(JlptLevel));
            IsFocusOn = false;
            OnPropertyChanged(nameof(IsFocusOn));
            IsCommonWord = false;
            OnPropertyChanged(nameof(IsCommonWord));
            PageNumber = 0;
            OnPropertyChanged(nameof(PageNumber));
            Order = 0;
            OnPropertyChanged(nameof(Order));


        }
        private JapaneseWordNoteCard NewNotecard()
        {
            var newJapanNoteCard = new JapaneseWordNoteCard();
            newJapanNoteCard.SentenceNoteCard = new SentenceNoteCard();
            newJapanNoteCard.SentenceNoteCard.LastTimeAccess = DateTime.Now;
            //newJapanNoteCard.SentenceNoteCard.Chapters = new List<ChapterNoteCard>
            //{
            //    new ChapterNoteCard
            //    {
            //        TopicName = _topicName
            //    }
            //};
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
