# ChapterNoteCard

### Everyone says use MVVM , so going to do that.

### I really need to do the Migrate method in startup to setup database if it is not already there
This is using a database from my web scrapper app, but that doesn't have the migration either.

First step - Get Navigation



## Resources
Using SingletonSean as a guide to do navigation with MVVM (https://www.youtube.com/watch?v=fZxZswmC_BY)
Another good guide for people staring out with Wpf.  
* https://stackoverflow.com/questions/7262137/what-is-datacontext-for
* https://rachel53461.wordpress.com/2012/07/14/what-is-this-datacontext-you-speak-of/

## Problems
* Loading data in constructor.  Function was async, but UI was still freezing.  
  * https://www.youtube.com/watch?v=0SCKUine6tY&list=PLA8ZIAm2I03jSfo18F7Y65XusYzDusYu5&index=4 Create a factory method.  Ok it did work.  It does freeze the first time, but the second time it does instantly change.  I really hope I didn't keep changing it for no reason this whole time.........
  * He did say "probably because loading symbols".  Which map identifiers in source code to identifiers in compile app for debugging


## TODOs:
Find better ways to make extensible
  1. Any time I need to pass something down from app to view models, I'm making a new parameter
  2. All viewModels have a Navigate to Start page command
    * Could make a navi command, but it feels like the app.xaml.cs file gets bloated (singletonSean does this, but I wanted to try without it).
    * Could make a layout (singletonSean)

Made a learning aid for this navigation part (see jpeg in folder Learning aids)


## steps I wanted to document (Learning aid would be better for review)
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
