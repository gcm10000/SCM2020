using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SCM2020___Client.ViewModel
{
    public class ItemMenu
    {
        public ItemMenu(string header, List<SubItem> subItems, PackIconKind icon)
        {
            Header = header;
            SubItems = subItems;
            Icon = icon;
        }
        public ItemMenu(string header, UserControl screen, PackIconKind icon, Uri source)
        {
            Header = header;
            Screen = screen;
            Icon = icon;
            Source = source;
        }

        public ItemMenu(ItemMenu ItemMenu)
        {
            this.Header = ItemMenu.Header;
            this.Screen = ItemMenu.Screen;
            this.Icon = ItemMenu.Icon;
            this.Source = ItemMenu.Source;
        }

        public string Header { get; private set; }
        public UserControl Screen { get; private set; }
        public List<SubItem> SubItems { get; private set; }
        public PackIconKind Icon { get; private set; }
        public Uri Source { get; private set; }
    }
}
