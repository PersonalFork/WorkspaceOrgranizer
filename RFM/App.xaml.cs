using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

using Prism.Ioc;
using Prism.Unity;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Controls.Loader;
using RFM.Dialogs;
using RFM.Services;
using RFM.Views;

using Unity;

namespace RFM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private UnityContainer _container;
        private Mutex _appMutex;
        private const string _appGuid = "607afds34s34231394-g42961-5f7a-8deb-rfm";

        [DllImport("user32.dll")]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool turnon);

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            if (IsSingleInstanceRunning() == false)
            {
                Process process = Process.GetCurrentProcess();
                process.Kill();
                return;
            }

            if (containerRegistry.GetContainer() is UnityContainer)
            {
                _container = containerRegistry.GetContainer() as UnityContainer;
            }

            containerRegistry.RegisterInstance<IWorkflow>(new Workflow());
            _container?.RegisterType<IDialogService, DialogService>();
            _container?.RegisterInstance<ILoader>(new Loader("Please Wait"));

            string settingsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            IPersistenceService persistanceService = new PersistenceService(settingsDirectory);
            _container.RegisterInstance(persistanceService);

            // Headers.
            containerRegistry.RegisterForNavigation<Header>(Pages.HomeHeader);

            // Pages.
            containerRegistry.RegisterForNavigation<DashboardPage>(Pages.Dashboard);
            containerRegistry.RegisterForNavigation<CreateSectionPage>(Pages.CreateSection);
            containerRegistry.RegisterForNavigation<ViewSectionPage>(Pages.ViewSection);
            containerRegistry.RegisterForNavigation<EditSectionPage>(Pages.EditSection);
            containerRegistry.RegisterForNavigation<AddApplicationPage>(Pages.AddApplication);
            containerRegistry.RegisterForNavigation<EditApplicationPage>(Pages.EditApplication);
            containerRegistry.RegisterForNavigation<LoadingPage>(Pages.Loading);
            containerRegistry.RegisterForNavigation<AddNotePage>(Pages.AddNote);
        }

        private bool IsSingleInstanceRunning()
        {
            _appMutex = new Mutex(true, _appGuid, out bool isnewInstance);
            if (!isnewInstance)
            {
                Process process = Process.GetCurrentProcess();
                foreach (Process proc in Process.GetProcessesByName(process.ProcessName))
                {
                    //switch to process by name
                    SwitchToThisWindow(proc.MainWindowHandle, true);
                }
                return false;
            }
            return true;
        }
    }
}
