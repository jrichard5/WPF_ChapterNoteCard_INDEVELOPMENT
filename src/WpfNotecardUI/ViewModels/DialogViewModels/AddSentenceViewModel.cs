using CommunityToolkit.Mvvm.Input;
using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfNotecardUI.ValidationRules;
using WpfNotecardUI.ViewModels.AbstractViewModels;

namespace WpfNotecardUI.ViewModels.DialogViewModels
{
    public class AddSentenceViewModel : AbstractClosableValidationViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        public DateTime LastTimeAccessed { get; } = DateTime.Now;
        private string _topicName;
        public string TopicName
        {
            get { return _topicName; }
        }

        private string _itemQuestion = string.Empty;
        public string ItemQuestion
        {
            get { return _itemQuestion; }
            set
            {
                //Was going to have a helper function to do all my set functions, but i think IsPropertyValid(value) may cause issues
                IsPropertyValid(value);
                _itemQuestion = value;
                OnPropertyChanged(nameof(ItemQuestion));
                AddCommand.NotifyCanExecuteChanged();
            }
        }
        private string _itemAnswer = string.Empty;
        public string ItemAnswer
        {
            get { return _itemAnswer; }
            set
            {
                IsPropertyValid(value);
                _itemAnswer = value;
                OnPropertyChanged(nameof(ItemAnswer));
                AddCommand.NotifyCanExecuteChanged();
            }
        }
        private string _hint = string.Empty;
        public string Hint
        {
            get { return _hint; }
            set
            {
                IsPropertyValid(value);
                _hint = value;
                OnPropertyChanged(nameof(Hint));
                AddCommand.NotifyCanExecuteChanged();
            }
        }
        private string _memorizationLevel = "0";
        public string MemorizationLevel
        {
            get { return _memorizationLevel; }
            set
            {
                IsPropertyValid(value);
                _memorizationLevel = value;
                OnPropertyChanged(nameof(MemorizationLevel));
                AddCommand.NotifyCanExecuteChanged();
            }
        }
        public bool IsFocusOn { get; set; } = false;

        public RelayCommand AddCommand { get; }

        public AddSentenceViewModel(string parentName, IServiceProvider serviceProvider)
        {
            _topicName = parentName;
            _serviceProvider = serviceProvider;

            AddCommand = new RelayCommand(AddSentenceFunction, CanAdd);
            HelpNotify += NotifyCanAddChange;
            ValidationRules.Add(nameof(ItemQuestion), new List<ValidationRule> { new MaxCharacterValidationRule() });
        }

        private void NotifyCanAddChange()
        {
            AddCommand.NotifyCanExecuteChanged();
        }

        public async void AddSentenceFunction()
        {
            if (!Int32.TryParse(_memorizationLevel, out int result))
            {
                result = 0;
            }

            var notecard = new SentenceNoteCard
            {
                ChapterSentences = new List<ChapterNoteCardSentenceNoteCard> {
                    new ChapterNoteCardSentenceNoteCard { ChapterNoteCardTopicName = _topicName, SentenceNoteCardItemQuestion = _itemQuestion }
                },
                Hint = _hint,
                ItemQuestion = _itemQuestion,
                ItemAnswer = _itemAnswer,
                LastTimeAccess = LastTimeAccessed,
                IsUserWantsToFocusOn = IsFocusOn,
                MemorizationLevel = result
            };

            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var genericRepo = scopedServiceProvider.GetRequiredService<IGenericRepo<SentenceNoteCard>>();
                try
                {
                    await genericRepo.AddAsync(notecard);
                    OnRequestClose();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null)
                    {
                        MessageBox.Show(ex.InnerException.Message);
                    }
                }
            }
        }

        public bool CanAdd()
        {
            if (_itemQuestion == string.Empty
                || _itemAnswer == string.Empty
                || _memorizationLevel == string.Empty)
            {
                return false;
            }
            return !HasErrors;
        }
    }
}
