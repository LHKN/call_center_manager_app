using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace ManagerApp.Services.MapUiTools
{
    public class MenuButton : Button
    {
        protected MenuButton(string icon)
        {
            Content = new FontIcon { Glyph = icon };
        }

        protected MenuFlyout CreateMenu()
        {
            var menu = new MenuFlyout();
            Flyout = menu;
            return menu;
        }

        protected IEnumerable<ToggleMenuFlyoutItem> GetMenuItems()
        {
            return ((MenuFlyout)Flyout).Items.OfType<ToggleMenuFlyoutItem>();
        }

        protected static ToggleMenuFlyoutItem CreateMenuItem(string text, object item, RoutedEventHandler click)
        {
            var menuItem = new ToggleMenuFlyoutItem { Text = text, Tag = item };
            menuItem.Click += click;
            return menuItem;
        }

        protected static MenuFlyoutSeparator CreateSeparator()
        {
            return new MenuFlyoutSeparator();
        }
    }
}
