﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace SCM2020___Client.ViewModel
{
    public class SubItem
    {
        public SubItem(string name, Uri source = null)
        {
            Name = name;
            Source = source;
        }

        public SubItem(SubItem subItem)
        {
            this.Name = subItem.Name;
            this.Source = subItem.Source;
        }

        public string Name { get; private set; }
        public Uri Source { get; private set; }
    }
}
