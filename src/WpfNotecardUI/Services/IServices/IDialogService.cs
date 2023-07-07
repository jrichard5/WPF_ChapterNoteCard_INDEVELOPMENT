using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Services.IServices
{
    public interface IDialogService
    {
        void ShowDialog(Action<string> callback);
    }
}
