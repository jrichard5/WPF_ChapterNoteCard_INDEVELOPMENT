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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfNotecardUI.Mappers;
using WpfNotecardUI.ValidationRules;
using WpfNotecardUI.ViewModels.AbstractViewModels;

namespace WpfNotecardUI.ViewModels.DialogViewModels
{
    public class AddKanjiWordViewModel : AbstractValidationViewModel
    {
        private string _parentName;
        private readonly IServiceProvider _serviceProvider;

        public string ParentName { get { return _parentName; } }
        public DateTime LastTimeAccessed { get; set; } = DateTime.Now;
        private string topicName = string.Empty;
        public string TopicName
        {
            get { return topicName; }
            set
            {
                IsPropertyValid(value);
                this.topicName = value;
                OnPropertyChanged(nameof(TopicName));
                NotifyCanAddChange();
            }
        }
        private string topicDefinition = string.Empty;
        public string TopicDefinition
        {
            get { return topicDefinition; }
            set
            {
                IsPropertyValid(value);
                this.topicDefinition = value;
                OnPropertyChanged(nameof(TopicDefinition));
                NotifyCanAddChange();
            }
        }

        private int? gradeLevel = null;
        public int? GradeLevel
        {
            get { return gradeLevel; }
            set
            {
                IsPropertyValid(value);
                this.gradeLevel = value;
                OnPropertyChanged(nameof(GradeLevel));
                NotifyCanAddChange();
            }
        }
        private int newsPaperRank = 0;
        public int NewsPaperRank
        {
            get { return newsPaperRank; }
            set
            {
                IsPropertyValid(value);
                this.newsPaperRank = value;
                OnPropertyChanged(nameof(NewsPaperRank));
                NotifyCanAddChange();
            }
        }
        private int jLPTLevel = 0;
        public int JLPTLevel
        {
            get { return jLPTLevel; }
            set
            {
                IsPropertyValid(value);
                this.jLPTLevel = value;
                OnPropertyChanged(nameof(JLPTLevel));
                NotifyCanAddChange();
            }
        }
        private string kunReadings = string.Empty;
        public string KunReadings
        {
            get { return kunReadings; }
            set
            {
                IsPropertyValid(value);
                this.kunReadings = value;
                OnPropertyChanged(nameof(KunReadings));
                NotifyCanAddChange();
            }
        }

        private string onReadings = string.Empty;
        public string OnReadings
        {
            get { return onReadings; }
            set
            {
                IsPropertyValid(value);
                this.onReadings = value;
                OnPropertyChanged(nameof(OnReadings));
                NotifyCanAddChange();
            }
        }

        public RelayCommand AddKanjiCommand { get; }

        public AddKanjiWordViewModel(string parentName, IServiceProvider serviceLocator)
        {
            _parentName = parentName;
            _serviceProvider = serviceLocator;
            AddKanjiCommand = new RelayCommand(AddKanjiFunction, CanAdd);
            HelpNotify += NotifyCanAddChange;
            //Register the ValidationRules
            ValidationRules.Add(nameof(TopicName), new List<ValidationRule>{ new OneCharacterValdiationRule() });
            ValidationRules.Add(nameof(TopicDefinition), new List<ValidationRule> { new MaxCharacterValidationRule() });
            ValidationRules.Add(nameof(GradeLevel), new List<ValidationRule> { new IntValidationRule(0, 32) });
            ValidationRules.Add(nameof(NewsPaperRank), new List<ValidationRule> { new IntValidationRule(0, 100000) });
            ValidationRules.Add(nameof(JLPTLevel), new List<ValidationRule> { new IntValidationRule(0, 5) });
            ValidationRules.Add(nameof(onReadings), new List<ValidationRule> { new MaxCharacterValidationRule() });
            ValidationRules.Add(nameof(kunReadings), new List<ValidationRule> { new MaxCharacterValidationRule() });
        }

        public void NotifyCanAddChange()
        {
            AddKanjiCommand.NotifyCanExecuteChanged();
        }

        private async void AddKanjiFunction()
        {
            var notecard = await AddKanjiToKanjiNoteCard.MapToNotecard(_parentName, TopicName, TopicDefinition, LastTimeAccessed, (int)GradeLevel, NewsPaperRank, JLPTLevel, KunReadings, OnReadings);
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopeServiceProvider = scope.ServiceProvider;
                var kanjiRepo = scopeServiceProvider.GetRequiredService<IKanjiNoteCardRepo>();
                try
                {
                    await kanjiRepo.AddAndSearchWords(notecard);
                    ResetForm();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is not null)
                    {
                        MessageBox.Show(ex.InnerException.Message);
                    }
                }
            }

        }
        public bool CanAdd()
        {
            if(TopicName == string.Empty
                || TopicDefinition == string.Empty
                || GradeLevel == null)
            {
                return false;
            }
            return !HasErrors;
        }

        private void ResetForm()
        {
            topicName = string.Empty;
            topicDefinition = string.Empty;
            gradeLevel = null;
            newsPaperRank = 0;
            jLPTLevel = 0;
            kunReadings = string.Empty;
            onReadings = string.Empty;
            OnPropertyChanged(nameof(TopicName));
            OnPropertyChanged(nameof(TopicDefinition));
            OnPropertyChanged(nameof(GradeLevel));
            OnPropertyChanged(nameof(JLPTLevel));
            OnPropertyChanged(nameof(NewsPaperRank));
            OnPropertyChanged(nameof(OnReadings));
            OnPropertyChanged(nameof(KunReadings));
        }
    }
}
