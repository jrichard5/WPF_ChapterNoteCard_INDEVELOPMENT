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

namespace WpfNotecardUI.Services.RealServices
{
    public class DialogServices<TView, TViewModel> : IDialogService 
        where TView : Window, new()
        where TViewModel: ObservableObject
    {

        TViewModel viewModel;
        private readonly string _parentName;
        private readonly IServiceProvider _serviceProvider;

        public DialogServices(TViewModel newViewModel)
        {
            viewModel = newViewModel;
            
        }


        //private ObservableObject SimpleViewModelFactory()
        //{
        //    ObservableObject dialogViewModel = null;
        //    dialogViewModel = viewModel switch
        //    {
        //        AddJapanWordViewModel ajwvm =>  new AddJapanWordViewModel(_parentName, _serviceProvider),
        //        AddKanjiWordViewModel ak => new AddKanjiWordViewModel(_parentName, _serviceProvider),
        //        _ => throw new NotImplementedException()
        //    } ;

        //    return dialogViewModel;
        //}


        public void ShowDialog(Action<string> callback)
        {


            var dialog = new TView()
            {
                DataContext = viewModel
            };
            EventHandler closeEventHandler = null;
            closeEventHandler = (s, e) =>
            {
                callback(dialog.DialogResult.ToString());
                dialog.Closed -= closeEventHandler;
            };
            dialog.Closed += closeEventHandler;
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }
    }
}
