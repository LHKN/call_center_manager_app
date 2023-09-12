using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Services
{
    static internal class CalendarViewChildrenHelper
    {
        // get all children of calendarview
        public static List<FrameworkElement> Children(this DependencyObject parent)
        {
            var list = new List<FrameworkElement>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is FrameworkElement)
                {
                    list.Add(child as FrameworkElement);
                }

                list.AddRange(Children(child));
            }

            return list;
        }
    }
}
