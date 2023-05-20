using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotecardUI.Stores
{
    public class CategoryChildrenStore
    {
        public Stack<ObservableObject> ChildrenStack { get; set; }


    }
}
