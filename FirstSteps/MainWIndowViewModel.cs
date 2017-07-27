using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        

        public MainWIndowViewModel()
        {
            this.ExampleOfTextBinding = $"This is from the view model constructor from threadId={System.Threading.Thread.CurrentThread.ManagedThreadId}";
            Task.Run(ChangeText);
        }

        private async Task ChangeText()
        {
            await Task.Delay(3000);
            
            this.ExampleOfTextBinding = $"And this is after its been changed from an async task from threadId={System.Threading.Thread.CurrentThread.ManagedThreadId}";

        }

    }
}
