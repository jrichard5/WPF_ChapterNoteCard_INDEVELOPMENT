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


### steps I wanted to document (Learning aid would be better for review)
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


### Steps taken to get double click
Options
1. Use code behind?
2. Use Attached Command Behaviors?
3. ~~Interaction triggers? (interactivity library)~~
4. Style EventSetter Event and give handler?
5. Dependency Properties? https://www.youtube.com/watch?v=Cx6YE86XzYE
  * in the main window, bind command to code-behind dependency property.
6. ~~<Grid.InputBindings><MouseBinding MouseAction="LeftDoubleClick"   Command="{Binding Something}"></MouseBinding> </Grid.InputBindings>~~
7.  See if I can update number in code-behind and use that to trigger a command
  * (branch: DifferentWaysToClickAList002)
8. Items Control.  Iterates over view models.  use input binding in the view model (this is number 6, but instead of having it bound to multiple models, it is bound to multiple view models)
9.  Event Trigger to call command when specific event.  https://stackoverflow.com/questions/64996809/is-there-any-way-easier-to-bind-to-a-click-event-in-a-view-model
  * Branch:  DifferentWaysToClickAList003
  * xmlns:Behaviors="http://schemas.microsoft.com/xaml/behaviors"
  * Get Nuget Package "Microsoft.xaml.Behaviors.wpf"

I've decided to be lazy and use code behind to call the viewmodel from datacontext.  I've heard this may break the MVVM model.  Attached Behavior seems to be the close second though https://stackoverflow.com/questions/4497825/wpf-mvvm-how-to-handle-double-click-on-treeviewitems-in-the-viewmodel 


### Random Notes
Give XAML element a x:Name to use in code behind

When trying to Bind Double Mouse Click to a mousehandler delegate, I get 

+		InnerException	
{"Unable to cast object of type 'System.Reflection.RuntimeEventInfo' to type 'System.Reflection.MethodInfo'."}	
System.Exception {System.InvalidCastException}


With Command classes (not relay command which is what I'm doing)
  1. Logic in CanExecute to determine if true or false
  2. Command has view model.  Subscribe to Property Changed. calls method


		Message	"'Add value to collection of type 'Microsoft.Xaml.Behaviors.TriggerCollection' threw an exception.' Line number '37' and line position '23'."	string
+		InnerException	{"Cannot find an event named \"MouseDoubleClick\" on type \"String\"."}	System.Exception {System.ArgumentException}
