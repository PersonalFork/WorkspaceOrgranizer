using System;
using System.Windows;

using Prism.Ioc;
using Prism.Unity;

using RFM.Common;
using RFM.Common.Constants;
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

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            if (containerRegistry.GetContainer() is UnityContainer)
            {
                _container = containerRegistry.GetContainer() as UnityContainer;
            }

            containerRegistry.RegisterInstance<IWorkflow>(new Workflow());
            _container?.RegisterType<IDialogService, DialogService>();

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
    }
}
