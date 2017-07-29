using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; //ObservableCollection
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

//Install-Package Prism.Core
using Prism.Mvvm; //BindableBase

//Install-Package Microsoft.Tpl.Dataflow
using System.Threading.Tasks.Dataflow; // Blocks :)


namespace FirstSteps
{
    class MainWIndowViewModel : BindableBase
    {
        private string _exampleOfTextBinding;
        public string ExampleOfTextBinding
        {
            get { return _exampleOfTextBinding; }
            set
            {
                SetProperty(ref _exampleOfTextBinding, value);
            }
        }

        //TODO: DependencyProperties

        //TODO: Command: both routed and delegate

        //TODO: AttachedProperties
        
        //TODO: ValueConverters (will AttachedProperties work better in converting a ViewModel value to a view element property?)


    
        public class TwoNumbers { public int X { get; set; } public int Y { get; set; } }
        public ObservableCollection<TwoNumbers> RandomTwoNumbers { get; set; }

        public MainWIndowViewModel()
        {
            RandomTwoNumbers = new ObservableCollection<TwoNumbers>();

            this.ExampleOfTextBinding = $"This is from the view model constructor from threadId={System.Threading.Thread.CurrentThread.ManagedThreadId}";
            Task.Run(ChangeText);

            //Can't do this when there are two windows with two different dispatchers.. How can both set the global "Application.Current.Dispatcher" proprty?
            // => They can't.. The one that looses will not be able to update the UI becauses its ui dependant object is now created from a different thread
            //because that thread happened to invoke "System.Windows.Threading.Dispatcher.Run()" - which blocks to let Dispatcher have control
            //Task.Run(GenerateRandomTwoNumbers);
        }

        private async Task ChangeText()
        {
            await Task.Delay(3000);
            
            this.ExampleOfTextBinding = $"And this is after its been changed from an async task from threadId={System.Threading.Thread.CurrentThread.ManagedThreadId}";
        }

        private async Task GenerateRandomTwoNumbers()
        {
            //return now and let the schedular worry about where its going to run...
            await Task.Yield();

            var r = new Random(DateTime.Now.Millisecond);

            while (true)
            {
                //Got to create the TwoNumbers object (because of thread affinity - STA) and add it to the collection on the UI thread..
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var xy = new TwoNumbers { X = r.Next(100), Y = r.Next(100) };
                    RandomTwoNumbers.Add(xy);
                });

                await Task.Delay(100);
            }
        }

    }
}
