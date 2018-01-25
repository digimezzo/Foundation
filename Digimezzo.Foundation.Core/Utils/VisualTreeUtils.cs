using System;
using System.Windows;
using System.Windows.Media;

namespace Digimezzo.Foundation.Core.Utils
{
    public static class VisualTreeUtils
    {
        /// <summary>
        /// Finds the first ancestor of type T in the Visual Tree of a given DependencyObject
        /// </summary>
        /// <typeparam name="T">The type of the ancestor to find in the Visual Tree</typeparam>
        /// <param name="current">The given DependencyObject</param>
        /// <returns></returns>
        public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }

                current = VisualTreeHelper.GetParent(current);

            } while (current != null);

            return null;
        }

        /// <summary>
        /// Finds the first Visual ancestor of the given type in the Visual Tree of a given Visual
        /// </summary>
        /// <param name="element">The given Visual</param>
        /// <param name="type">The type of the Visual to find in the Visual Tree</param>
        /// <returns></returns>
        public static Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null)
            {
                return null;
            }
            if (element.GetType() == type)
            {
                return element;
            }

            Visual foundElement = null;

            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(element) - 1; i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);

                if (foundElement != null)
                {
                    break;
                }
            }

            return foundElement;
        }
    }
}
