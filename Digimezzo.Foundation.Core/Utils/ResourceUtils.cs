using System.Windows;
using System.Windows.Media;

namespace Digimezzo.Foundation.Core.Utils
{
    public static class ResourceUtils
    {
        /// <summary>
        /// Gets a string resource for the current application
        /// </summary>
        /// <param name="resourceName">The resource's name</param>
        /// <returns>The resources string content</returns>
        public static string GetString(string resourceName)
        {
            object resource = Application.Current.TryFindResource(resourceName);
            return resource == null ? resourceName : resource.ToString();
        }

        /// <summary>
        /// Gets a Geometry resource for the current application
        /// </summary>
        /// <param name="resourceName">The resource's name</param>
        /// <returns>The resources Geometry content</returns>
        /// <returns></returns>
        public static Geometry GetGeometry(string resourceName)
        {
            return (Geometry)Application.Current.TryFindResource(resourceName);
        }
    }
}
