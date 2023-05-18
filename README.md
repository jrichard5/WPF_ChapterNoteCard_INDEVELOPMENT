# ChapterNoteCard

First step - Get Navigation
Everyone says use MVVM , so going to do that.

Using SingletonSean as a guide to do navigation with MVVM (https://www.youtube.com/watch?v=fZxZswmC_BY)

Steps I think I need to note down for navigation
1. Make views with buttons that will be used for navigation (they do nothing atm).
2. Make a ViewModelBase 
    * Implements INotifyPropertyChanged (I wonder if community toolkit == Observableobject Annotation.)
        * ObservableObject has the following main features:  It provides a base implementation for INotifyPropertyChanged (https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/observableobject)
    * Make a MainViewModel
        * Property for a viewModel.  Constructor sets it to first view Model
    * In the app.xaml.cs file.  Make an protected override void OnStartup(s.e.args e). Make a new window with {datacontext = new mainviewmodel};  MainWindow.show()
    * Remove StartupUri from app.xaml (because above line initializes it).
    * So now to change the main view.
        1. MainWindows.xaml -> change the view in the grid.
        2. MainViewModel -> change the currentviewModel to new view model.

    * On StartPageViewModel
        * made an ICommand variable.  And a function.  ICommand variable = new RelayComamnd(function)
    * On StartPageView
        * Added Command="{Binding IcommandVarible}" to button


3. Need to Make single source of truth (the NavigationStore)
    * Make the class
    * add it to the app.xaml.cs
    * give navigation store to mainviewmodel
    * in App, pass in navi store to mainviewmodel


4. Make Grid.Resource data templte on mainwindow
