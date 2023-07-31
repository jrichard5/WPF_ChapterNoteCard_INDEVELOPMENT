using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfNotecardUI.Models;
using WpfNotecardUI.Services.IServices;
using WpfNotecardUI.Stores;

namespace WpfNotecardUI.ViewModels.AbstractViewModels
{
    public abstract class AbstractListVMExtra<T> : AbstractListVModel<T>
    {
        public AbstractListVMExtra(NavigationStore navigationStore, IServiceProvider serviceProvider) :  base(navigationStore, serviceProvider)
        {
            #region PageConstructor
            PreviousPageCommand = new RelayCommand(PreviousPageFunction, CanPreviousPage);
            NextPageCommand = new RelayCommand(NextPageFunction, CanNextPage);

            #endregion

            #region UpdateCosntructor
            ItemQuestionsThatHaveChanged = new List<string>();
            SaveData = new RelayCommand<T>(SaveDataFunction, CanSaveData);
            ListItemChanged = new RelayCommand<string>(AddToHaveChangedList);

            #endregion

            #region AddConstructor
            AddCommand = new RelayCommand(ExecuteShowDialog);
            #endregion
        }

        #region Pages
        protected int pageNumber = 1;
        protected int NUMBER_PER_PAGE = Properties.Settings.Default.NumberPerPage;
        protected bool _isPageLoading;
        public bool IsPageLoading
        {
            get { return _isPageLoading; }
            set
            {
                _isPageLoading = value;
                OnPropertyChanged(nameof(IsPageLoading));
            }
        }

        public RelayCommand PreviousPageCommand { get; }
        public RelayCommand NextPageCommand { get; }

        public int PageNumber
        {
            get { return pageNumber; }
            set
            {
                pageNumber = value;
                OnPropertyChanged(nameof(PageNumber));
                NextPageCommand.NotifyCanExecuteChanged();
                PreviousPageCommand.NotifyCanExecuteChanged();
            }
        }

        private int? maxPageCount;
        public int? MaxPageCount
        {
            get { return maxPageCount; }
            set
            {
                maxPageCount = value;
                NextPageCommand.NotifyCanExecuteChanged();
            }
        }

        private string? lastPageNumber = "?";
        public string? LastPageNumber
        {
            get { return lastPageNumber; }
            set
            {
                lastPageNumber = value;
                OnPropertyChanged(nameof(LastPageNumber));
            }
        }

        private void PreviousPageFunction()
        {
            PageNumber -= 1;
            GetDataForList();
            OnPropertyChanged(nameof(CurrentList));
        }

        private bool CanPreviousPage()
        {
            return PageNumber > 1;
        }

        private void NextPageFunction()
        {
            PageNumber += 1;
            GetDataForList();
            OnPropertyChanged(nameof(CurrentList));
        }
        private bool CanNextPage()
        {
            if (MaxPageCount is null)
            {
                return false;
            }
            if (MaxPageCount % NUMBER_PER_PAGE == 0)
            {
                // 100 / 20 = 5      1:1-20 2:21-40 3:41-60 4:61:80, 5:81-100  so can't have page 6
                return MaxPageCount / NUMBER_PER_PAGE > PageNumber;
            }
            return (MaxPageCount / NUMBER_PER_PAGE) + 1 > PageNumber;

        }

        protected string GetCountHelperFunction(int maxPageCount)
        {
            var tempLastPage = (maxPageCount / NUMBER_PER_PAGE);
            if (MaxPageCount % NUMBER_PER_PAGE == 0)
            {
                return tempLastPage.ToString();
            }
            else
            {
                return (tempLastPage + 1).ToString();
            }
        }

        protected abstract void GetCountFunction();

        #endregion

        #region UpdatesMembers

        public ICommand ListItemChanged { get; }

        public List<string> ItemQuestionsThatHaveChanged { get; set; }

        public RelayCommand<T> SaveData { get; }

        public void AddToHaveChangedList(string? parentName)
        {
            if (parentName == null)
            {
                return;
            }
            if (!ItemQuestionsThatHaveChanged.Contains(parentName))
            {
                ItemQuestionsThatHaveChanged.Add(parentName);
                SaveData.NotifyCanExecuteChanged();
            }
        }

        protected bool CanSaveData(T? obj)
        {
            var hasAny = ItemQuestionsThatHaveChanged.Any();
            return hasAny;
        }

        public abstract void SaveDataFunction(T? hi);

        public override void GoToPreviousHandler()
        {
            MessageBoxIfDetectChanges();
            base.GoToPreviousHandler();
        }

        public override void GoToStartHandler()
        {
            MessageBoxIfDetectChanges();
            base.GoToStartHandler();
        }

        public void MessageBoxIfDetectChanges()
        {
            if (ItemQuestionsThatHaveChanged.Any())
            {
                var result = MessageBox.Show("An item was editted atleast once, do you want to save before you exit?",
                    "Save Items",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                //Could use an if statement, but wanted this to be here to remember
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        SaveData.Execute(null);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        #endregion

        #region AddCommandMembers

        protected IDialogService _dialogService;
        public ICommand AddCommand { get; }

        public abstract void ExecuteShowDialog();

        #endregion
    }
}
