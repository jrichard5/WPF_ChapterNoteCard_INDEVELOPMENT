using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfNotecardUI.Services.IServices;
using WpfNotecardUI.Stores;
using WpfNotecardUI.ViewModels.AbstractViewModels;
using WpfNotecardUI.ViewModels.DialogViewModels;
using WpfNotecardUI.ViewModels.ListVModels;
using WpfNotecardUI.Views.Dialogs;

namespace WpfNotecardUI.Services.RealServices
{
    public class DialogServices<TViewModel> : IDialogService 
        where TViewModel: AbstractClosableValidationViewModel
    {

        TViewModel viewModel;
        private readonly string _parentName;
        private readonly IServiceProvider _serviceProvider;
        Window dialog;

        public DialogServices(TViewModel newViewModel)
        {
            viewModel = newViewModel;
            SimpleViewModelFactory();
            
        }


        private ObservableObject SimpleViewModelFactory()
        {
            ObservableObject dialogViewModel = null;
            dialog = viewModel switch
            {
                AddJapanWordViewModel ajwvm => new JapanWordDialog(),
                AddKanjiWordViewModel ak => new KanjiWordDialog(),
                _ => throw new NotImplementedException()
            };

            return dialogViewModel;
        }


        public void ShowDialog(Action<string> callback)
        {
            dialog.DataContext = viewModel;
            EventHandler closeEventHandler = null;
            closeEventHandler = (s, e) =>
            {
                callback(dialog.DialogResult.ToString());
                dialog.Closed -= closeEventHandler;
                viewModel.RequestClose -= CloseDialog;
            };
            dialog.Closed += closeEventHandler;
            viewModel.RequestClose += CloseDialog;
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        private void CloseDialog(object? sender, EventArgs e)
        {
            if (dialog != null)
            {
                dialog.Close();
            }
        }
    }
}
