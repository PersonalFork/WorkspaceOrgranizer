using System.Windows;

using Prism.Regions;

using RFM.Common.Constants;

namespace RFM.Common.Extensions
{
    public static class RegionManagerExtensions
    {
        public static void BrowseToPage(this IRegionManager regionManager, string pageName)
        {
            if (Application.Current?.Dispatcher == null)
            {
                regionManager.RequestNavigate(Regions.ContentRegion, pageName);
            }
            else
            {
                Application.Current?.Dispatcher?.Invoke(() =>
                {
                    regionManager.RequestNavigate(Regions.ContentRegion, pageName);
                });
            }
        }

        public static void BrowseToPage(this IRegionManager regionManager, string pageName, NavigationParameters navParams)
        {
            if (Application.Current?.Dispatcher == null)
            {
                regionManager.RequestNavigate(Regions.ContentRegion, pageName, navParams);
            }
            else
            {
                Application.Current?.Dispatcher?.Invoke(() =>
                {
                    regionManager.RequestNavigate(Regions.ContentRegion, pageName, navParams);
                });
            }
        }

        public static void BrowseRegionToPage(this IRegionManager regionManager, string regionName, string pageName)
        {
            if (Application.Current?.Dispatcher == null)
            {
                regionManager.RequestNavigate(regionName, pageName);
            }
            else
            {
                Application.Current?.Dispatcher?.Invoke(() =>
                {
                    regionManager.RequestNavigate(regionName, pageName);
                });
            }
        }

        public static void BrowseRegionToPage(this IRegionManager regionManager, string regionName, string pageName, NavigationParameters navParams)
        {
            if (Application.Current?.Dispatcher == null)
            {
                regionManager.RequestNavigate(regionName, pageName, navParams);
            }
            else
            {
                Application.Current?.Dispatcher?.Invoke(() =>
                {
                    regionManager.RequestNavigate(regionName, pageName, navParams);
                });
            }
        }
    }
}
