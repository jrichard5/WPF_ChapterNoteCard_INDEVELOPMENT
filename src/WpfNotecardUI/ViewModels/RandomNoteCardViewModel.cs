using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataLayer.Entities;
using DataLayer.IRepos;
using DataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNotecardUI.Models.RandomNotecardPage;
using WpfNotecardUI.Models.TreeNodes;
using WpfNotecardUI.Stores;

namespace WpfNotecardUI.ViewModels
{
    public class RandomNoteCardViewModel : ObservableObject
    {
        #region StateProperties
        private static int MAX_PAST_NOTECARDS = 20;

        private bool _onlyFoucsOn = Properties.Settings.Default.OnlyUseOnFocus;
        private bool _chapterSelection = Properties.Settings.Default.OnlyChaptersSelected;
        private List<string> chaptersSelected;
        private Queue<string> chaptersQueued;
        private Queue<Task<ChapterDeck>> decksQueued;
        private ChapterDeck _currentChapterDeck;
        private int _japaneseVocabCategoryId;
        private ObservableCollection<string> _nextChapters = new ObservableCollection<string>();

        #endregion
        public ObservableCollection<string> NextChapters
        {
            get { return _nextChapters; }
            set
            {
                _nextChapters = value;
                OnPropertyChanged(nameof(NextChapters));
            }
        }
        public CurrentNotecard DisplayedNotecard { get; set; }

        public CurrentNotecard[] oldNotecards = new CurrentNotecard[MAX_PAST_NOTECARDS];
        public bool IsPastNotecard { get; set; } = false;
        public int PastIndex { get; set; } = 0;

        private readonly NavigationStore _navigationStore;

        public ICommand GoToStartViewModel { get; }
        public ICommand NextCommand { get; }

        public RelayCommand PreviousCommand { get; }

        private readonly IServiceProvider _serviceProvider;

        public void SwitchToStart()
        {
            _navigationStore.CurrentViewModel = new StartPageViewModel(_navigationStore, _serviceProvider);
        }

        public RandomNoteCardViewModel(NavigationStore navigationStore, IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            GoToStartViewModel = new RelayCommand(SwitchToStart);
            NextCommand = new RelayCommand(NextFunction);
            PreviousCommand = new RelayCommand(PreviousFunction, CanPrevious);
            _serviceProvider = serviceProvider;
            GetStartingData();
        }


        #region PreviousCardStuff

        private void DisplayedToOld(int index)
        {
            DisplayedNotecard.CharExistList = oldNotecards[index].CharExistList;
            DisplayedNotecard.IsChapter = oldNotecards[index].IsChapter;
            DisplayedNotecard.CurrentIndex = oldNotecards[index].CurrentIndex;
            DisplayedNotecard.Hint = oldNotecards[index].Hint;
            DisplayedNotecard.Question = oldNotecards[index].Question;
        }

        public void PreviousFunction()
        {
            if (IsPastNotecard == false)
            {
                IsPastNotecard = true;
                PastIndex++;
                DisplayedToOld(PastIndex);
            }
            else
            {
                PastIndex++;
                //DisplayedNotecard = OldNotecards[PastIndex];
                DisplayedToOld(PastIndex);
            }


            PreviousCommand.NotifyCanExecuteChanged();
        }
        public bool CanPrevious()
        {
            if (PastIndex == (MAX_PAST_NOTECARDS - 1))
            {
                return false;
            }
            if (oldNotecards[PastIndex + 1] == null)
            {
                return false;
            }
            if (String.IsNullOrEmpty(oldNotecards[PastIndex + 1].Question))
            {
                return false;
            }
            return true;
        }

        #endregion

        #region NextpageAndFlip

        private string _nextButtonContent = "Flip";
        public string NextButtonContent
        {
            get { return _nextButtonContent; }
            set
            {
                _nextButtonContent = value;
                OnPropertyChanged(nameof(NextButtonContent));
            }
        }
        private bool _hasHint = false;
        public bool HasHint
        {
            get { return _hasHint; }
            set
            {
                _hasHint = value;
            }
        }

        private void AddToPastArray(CurrentNotecard cardToPlac)
        {
            int firstEmptyIndex = MAX_PAST_NOTECARDS - 1;
            for (int i = 0; i < oldNotecards.Length; i++)
            {
                //if (String.IsNullOrEmpty(OldNotecards[i].Question))
                if (oldNotecards[i] == null)
                {
                    firstEmptyIndex = i;
                    break;
                }
            }


            if (!(firstEmptyIndex == 0))
            {
                for (int i = firstEmptyIndex; i > 0; i--)
                {
                    var oneToLeft = oldNotecards[i - 1];
                    oldNotecards[i] = oneToLeft;

                }
            }

            //TODO: save a new card, not the same reference
            oldNotecards[0] = new CurrentNotecard
            {
                CharExistList = cardToPlac.CharExistList,
                CurrentIndex = cardToPlac.CurrentIndex,
                Hint = cardToPlac.Hint,
                IsChapter = cardToPlac.IsChapter,
                IsFront = cardToPlac.IsFront,
                Question = cardToPlac.Question
            };
            PreviousCommand.NotifyCanExecuteChanged();

        }

        public void NextFunction()
        {
            if (IsPastNotecard)
            {
                if (PastIndex == 0)
                {
                    throw new Exception();
                }
                PastIndex--;
                if (PastIndex == 0)
                {
                    IsPastNotecard = false;
                }
                if (oldNotecards[PastIndex].IsFront)
                {
                    NextButtonContent = "Next";
                    //DisplayedNotecard.CharExistList = OldNotecards[PastIndex].CharExistList;
                    DisplayedToOld(PastIndex);
                }
                else
                {
                    NextButtonContent = "Flip";
                    DisplayedToOld(PastIndex);
                }
                PreviousCommand.NotifyCanExecuteChanged();
                return;
            }

            if (DisplayedNotecard.IsFront && DisplayedNotecard.IsChapter)
            {

                //Must be above DisplayedNotecard.Question because of the way I change font in view.
                DisplayedNotecard.CharExistList = null;
                NextButtonContent = "Next";
                DisplayedNotecard.IsFront = false;
                DisplayedNotecard.Question = _currentChapterDeck.CurrentChapter.TopicDefinition;
                DisplayedNotecard.Hint = "";
                HasHint = false;
                AddToPastArray(DisplayedNotecard);

            }
            else if (!DisplayedNotecard.IsFront && DisplayedNotecard.IsChapter)
            {
                if (_currentChapterDeck.Sentences.Count == 0)
                {
                    GetNewChapterFromQueue();
                    AddToPastArray(DisplayedNotecard);
                }
                else
                {
                    var currentSentence = _currentChapterDeck.Sentences[DisplayedNotecard.CurrentIndex];

                    NextButtonContent = "Flip";
                    //Must be above DisplayedNotecard.Question because of the way I change font in view.
                    if (_currentChapterDeck.CurrentChapter.CategoryId == _japaneseVocabCategoryId)
                    {
                        DisplayedNotecard.CharExistList = currentSentence.CharExistList;
                    }
                    DisplayedNotecard.IsFront = true;
                    DisplayedNotecard.IsChapter = false;
                    DisplayedNotecard.Question = currentSentence.SentenceNoteCard.ItemQuestion;
                    DisplayedNotecard.Hint = currentSentence.SentenceNoteCard.Hint;
                    HasHint = !string.IsNullOrEmpty(DisplayedNotecard.Hint);
                    AddToPastArray(DisplayedNotecard);
                }

            }
            else if (DisplayedNotecard.IsFront && !DisplayedNotecard.IsChapter)
            {

                NextButtonContent = "Next";
                //Must be above DisplayedNotecard.Question because of the way I change font in view.
                DisplayedNotecard.CharExistList = null;
                DisplayedNotecard.IsFront = false;
                DisplayedNotecard.Question = _currentChapterDeck.Sentences[DisplayedNotecard.CurrentIndex].SentenceNoteCard.ItemAnswer;
                DisplayedNotecard.Hint = "";
                HasHint = false;
                AddToPastArray(DisplayedNotecard);

            }
            else if (!DisplayedNotecard.IsFront && !DisplayedNotecard.IsChapter)
            {
                NextButtonContent = "Flip";
                DisplayedNotecard.IsFront = true;

                UpdateTimeOnSentenceNoteCard(_currentChapterDeck.Sentences[DisplayedNotecard.CurrentIndex].SentenceNoteCard);

                DisplayedNotecard.CurrentIndex++;

                if (DisplayedNotecard.CurrentIndex < _currentChapterDeck.Sentences.Count)
                {
                    var currentSentence = _currentChapterDeck.Sentences[DisplayedNotecard.CurrentIndex];
                    //Must be above DisplayedNotecard.Question because of the way I change font in view.
                    if (_currentChapterDeck.CurrentChapter.CategoryId == _japaneseVocabCategoryId)
                    {
                        DisplayedNotecard.CharExistList = currentSentence.CharExistList;
                    }
                    DisplayedNotecard.Question = currentSentence.SentenceNoteCard.ItemQuestion;
                    DisplayedNotecard.Hint = currentSentence.SentenceNoteCard.Hint;
                    HasHint = !string.IsNullOrEmpty(DisplayedNotecard.Hint);


                }
                else
                {
                    //Must be above DisplayedNotecard.Question because of the way I change font in view.
                    GetNewChapterFromQueue();
                }
                AddToPastArray(DisplayedNotecard);


            }
        }

        private async void GetNewChapterFromQueue()
        {
            DisplayedNotecard.CharExistList = null;
            DisplayedNotecard.IsChapter = true;
            DisplayedNotecard.CurrentIndex = 0;
            DisplayedNotecard.IsFront = true;

            _currentChapterDeck = await decksQueued.Dequeue();
            DisplayedNotecard.Question = _currentChapterDeck.CurrentChapter.TopicName;
            DisplayedNotecard.Hint = "New Chapter";


            //Queue next task
            if (!(chaptersQueued.Count > 0))
            {
                RandomizeChaptersAndQueue();
            }
            var nextChapter = chaptersQueued.Dequeue();
            for (var i = 0; i < NextChapters.Count - 1; i++)
            {
                NextChapters[i] = NextChapters[i + 1];
            }
            NextChapters[4] = nextChapter;
            decksQueued.Enqueue(CreateChapterDeck(nextChapter));
        }

        #endregion

        public async void UpdateTimeOnSentenceNoteCard(SentenceNoteCard sentenceNoteCard)
        {
            if (sentenceNoteCard != null)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    sentenceNoteCard.LastTimeAccess = DateTime.Now;
                    var scopedServiceProvider = scope.ServiceProvider;
                    var sentRepo = scopedServiceProvider.GetRequiredService<ISentenceNoteCardRepo>();
                    await sentRepo.UpdateGeneric(sentenceNoteCard);
                }
            }
        }

        #region GetNotecardsFromDatabaseAndSettings
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        private async void GetStartingData()
        {
            if (_onlyFoucsOn)
            {
                List<string> chaptersWithFocus = new List<string>();
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopeServiceProvider = scope.ServiceProvider;
                    var chapterRepo = scopeServiceProvider.GetRequiredService<IChapterNoteCardRepo>();
                    var chapters = await chapterRepo.GetAllThatHasFocus();
                    chaptersWithFocus = chapters.Select(c => c.TopicName).ToList();
                    chaptersSelected = chaptersWithFocus;
                }
                if (_chapterSelection)
                {
                    GetChapterSelected();
                    chaptersSelected = chaptersSelected.Intersect(chaptersWithFocus).ToList();
                }
            }
            else
            {
                if (_chapterSelection)
                {
                    GetChapterSelected();
                }
                else
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var scopeServiceProvider = scope.ServiceProvider;
                        var genericRepo = scopeServiceProvider.GetRequiredService<IGenericRepo<ChapterNoteCard>>();
                        var chapters = await genericRepo.GetAll();
                        chaptersSelected = chapters.Select(c => c.TopicName).ToList();
                    }
                }
            }

            if (chaptersSelected.Count == 0)
            {
                throw new Exception("no chapters match");
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var categoryRepo = scopedServiceProvider.GetRequiredService<ICategoryRepo>();
                var category = await categoryRepo.GetFirstCategoryByName("Japanese Vocab");
                _japaneseVocabCategoryId = category.Id;
            }


            RandomizeChaptersAndQueue();

            var chapterToSearch = chaptersQueued.Dequeue();
            NextChapters.Add(chapterToSearch);

            _currentChapterDeck = await CreateChapterDeck(chapterToSearch);
            DisplayedNotecard = new CurrentNotecard();
            DisplayedNotecard.Hint = "New Chapter!!!";
            DisplayedNotecard.Question = _currentChapterDeck.CurrentChapter.TopicName;
            DisplayedNotecard.CharExistList = null;
            DisplayedNotecard.IsChapter = true;
            DisplayedNotecard.IsFront = true;
            DisplayedNotecard.CurrentIndex = 0;
            AddToPastArray(DisplayedNotecard);

            decksQueued = new Queue<Task<ChapterDeck>>();

            for (var i = 0; i < 4; i++)
            {
                if (!(chaptersQueued.Count > 0))
                {
                    RandomizeChaptersAndQueue();
                }
                chapterToSearch = chaptersQueued.Dequeue();
                NextChapters.Add(chapterToSearch);
                decksQueued.Enqueue(CreateChapterDeck(chapterToSearch));
            }
        }

        private void RandomizeChaptersAndQueue()
        {
            //https://stackoverflow.com/questions/69503717/how-to-use-random-class-to-shuffle-array-in-c-sharp
            var random = new Random();
            for (int i = 0; i < chaptersSelected.Count - 1; i++)
            {
                int r = random.Next(i, chaptersSelected.Count);
                (chaptersSelected[r], chaptersSelected[i]) = (chaptersSelected[i], chaptersSelected[r]);
            }
            chaptersQueued = new Queue<string>(chaptersSelected);
        }

        private void GetChapterSelected()
        {
            ObservableCollection<CategoryTreeModel> categories = null;

            try
            {
                categories = JsonSerializer.Deserialize<ObservableCollection<CategoryTreeModel>>(Properties.Settings.Default.ChaptersJSON);
            }
            catch (JsonException ex)
            {
                Debug.WriteLine(ex);
            }
            if (categories != null)
            {
                chaptersSelected = categories.SelectMany(c => c.Children).Where(c => c.IsFocused).Select(c => c.ChapterName).ToList();
                chaptersSelected.Sort();
            }
        }

        private async Task<ChapterDeck> CreateChapterDeck(string chapterName)
        {
            ChapterDeck chapterDeck = new ChapterDeck();
            chapterDeck.Sentences = new List<SentenceForDeck>();

            using (var scope = _serviceProvider.CreateScope())
            {
                var scopeServiceProvider = scope.ServiceProvider;
                var chapRepo = scope.ServiceProvider.GetRequiredService<IChapterNoteCardRepo>();
                chapterDeck.CurrentChapter = await chapRepo.GetChapterNoteCardByTopicName(chapterName);

                var sentRepo = scope.ServiceProvider.GetRequiredService<ISentenceNoteCardRepo>();
                var sentences = await sentRepo.GetAllWithAChapter(chapterName);
                sentences = sentences.Where(s => s.IsUserWantsToFocusOn).ToList();


                var random = new Random();
                for (int i = 0; i < sentences.Count - 1; i++)
                {
                    int r = random.Next(i, sentences.Count);
                    (sentences[r], sentences[i]) = (sentences[i], sentences[r]);
                }

                if (chapterDeck.CurrentChapter.CategoryId == _japaneseVocabCategoryId)
                {
                    var sentButJustItemQuestion = sentences.Select(s => s.ItemQuestion).ToList();
                    var japanRepo = scope.ServiceProvider.GetRequiredService<IJapaneseWordNoteCardRepo>();
                    var charExistList = await japanRepo.GetCharacterExistFromList(sentButJustItemQuestion);
                    foreach (var sent in sentences)
                    {
                        var sentForDeck = new SentenceForDeck();
                        sentForDeck.SentenceNoteCard = sent;
                        sentForDeck.CharExistList = charExistList[sent.ItemQuestion];
                        chapterDeck.Sentences.Add(sentForDeck);

                    }
                }
                else
                {
                    foreach (var sent in sentences)
                    {
                        var sentForDeck = new SentenceForDeck();
                        sentForDeck.SentenceNoteCard = sent;
                        chapterDeck.Sentences.Add(sentForDeck);
                    }
                }

            }
            return chapterDeck;
        }
        #endregion
    }
}
