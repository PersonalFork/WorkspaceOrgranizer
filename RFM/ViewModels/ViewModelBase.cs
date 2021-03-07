using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using RFM.Common;
using RFM.Common.Constants;
using RFM.Common.Extensions;
using RFM.Dialogs;

namespace RFM.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware
    {
        #region Private Variable Declarations.

        private readonly int _pageInactivityIntervalSeconds = 0;
        private readonly object _locker = new object();

        #endregion

        #region Public Properties.

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            private set => SetProperty(ref _isActive, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            protected set => SetProperty(ref _isBusy, value);
        }

        public IRegionManager RegionManager { get; }
        public IWorkflow Workflow { get; }
        public IDialogService DialogService { get; }
        public virtual DelegateCommand HomeCommand { get; protected set; }

        #endregion

        #region Constructors.

        protected ViewModelBase(
            IWorkflow workflow,
            IRegionManager regionManager,
            IDialogService dialogService)
        {
            Workflow = workflow;
            RegionManager = regionManager;
            DialogService = dialogService;

            HomeCommand = new DelegateCommand(GoHome);
        }

        #endregion

        #region Public Overrides.

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Deactivate();
            lock (_locker)
            {
                IsActive = false;
            }
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            lock (_locker)
            {
                IsActive = true;
            }
            Activate();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        #endregion

        #region Protected Method Declarations.

        protected abstract void Activate();
        protected abstract void Deactivate();

        protected void Browse(string page)
        {
            if (IsActive)
            {
                RegionManager?.BrowseToPage(page);
            }
        }

        protected void Browse(string page, NavigationParameters navParams)
        {
            if (IsActive)
            {
                RegionManager?.BrowseToPage(page, navParams);
            }
        }

        protected void GoHome()
        {
            RegionManager?.BrowseRegionToPage(Regions.HeaderRegion, Pages.HomeHeader);
            RegionManager?.BrowseRegionToPage(Regions.ContentRegion, Pages.Dashboard);
        }

        #endregion
    }
}