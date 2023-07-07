using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.ViewModels.AbstractViewModels
{
    public abstract class AbstractClosableValidationViewModel : AbstractValidationViewModel
    {
        public event EventHandler RequestClose;

        protected virtual void OnRequestClose()
        {
            RequestClose?.Invoke(this, new EventArgs());
        }
    }
}
