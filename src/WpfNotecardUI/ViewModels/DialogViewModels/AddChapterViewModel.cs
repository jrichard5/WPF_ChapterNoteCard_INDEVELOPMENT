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
    public class AddChapterViewModel : AbstractClosableValidationViewModel
    {
        
        private readonly IServiceProvider _serviceProvider;
        private string _parentName;
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
                AddCommand.NotifyCanExecuteChanged();
            }
        }
        private string topicDefinition = string.Empty;
        public string TopicDefinition
        {
            get { return this.topicDefinition;}
            set
            {
                IsPropertyValid(value);
                this.topicDefinition = value;
                OnPropertyChanged(nameof(TopicDefinition));
                AddCommand.NotifyCanExecuteChanged();
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
                AddCommand.NotifyCanExecuteChanged();
            }
        }

        public RelayCommand AddCommand { get; }

        public AddChapterViewModel(string parentName, IServiceProvider serviceProvider)
        {
            _parentName = parentName;
            _serviceProvider = serviceProvider;
            AddCommand = new RelayCommand(AddChapterFunction, CanAdd);
            HelpNotify += NotifyCanAddChange;
            ValidationRules.Add(nameof(TopicName), new List<ValidationRule> { new MaxCharacterValidationRule() });
            ValidationRules.Add(nameof(TopicDefinition), new List<ValidationRule> { new MaxCharacterValidationRule() });
            ValidationRules.Add(nameof(GradeLevel), new List<ValidationRule> { new IntValidationRule(0, 32) });
        }

        private void NotifyCanAddChange() 
        {
            AddCommand.NotifyCanExecuteChanged();
        }

        private async void AddChapterFunction()
        {
            if(!Int32.TryParse(_parentName, out int parentId))
            {
                throw new Exception();
            }

            var notecard = new ChapterNoteCard
            {
                CategoryId = parentId,
                TopicDefinition = topicDefinition,
                GradeLevel = gradeLevel ?? 0,
                LastTimeAccess = LastTimeAccessed,
                TopicName = topicName,

            };
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopeServiceProvider = scope.ServiceProvider;
                var genericRepo = scopeServiceProvider.GetRequiredService<IGenericRepo<ChapterNoteCard>>();
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
            if (TopicName == string.Empty
                || TopicDefinition == string.Empty
                || GradeLevel == null)
            {
                return false;
            }
            return !HasErrors;
        }
    }
}
