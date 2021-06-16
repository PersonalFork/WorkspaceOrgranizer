using System;

using Prism.Mvvm;

using RFM.Common.ViewModels;

namespace RFM.Controls.Loader
{
    public interface ILoader
    {
        LoadingPayloadViewModel LoadingPayload { get; }
        void ShowLoader(string message);
        void ShowLoader(string message, string header);
        void HideLoader();
    }

    public class Loader : BindableBase, ILoader
    {
        #region Private Variable Declarations.

        private LoadingPayloadViewModel _loadingPayload;
        private readonly string _defaultTitle;

        #endregion

        #region Public Properties.

        public LoadingPayloadViewModel LoadingPayload
        {
            get => _loadingPayload;
            private set => SetProperty(ref _loadingPayload, value);
        }

        #endregion

        #region Constructors.

        public Loader(string defaultTitle)
        {
            _defaultTitle = defaultTitle;
        }

        #endregion

        #region Public Method Declarations.

        public void HideLoader()
        {
            LoadingPayload = null;
        }

        public void ShowLoader(string message, string header)
        {
            LoadingPayload = new LoadingPayloadViewModel()
            {
                IsActive = true,
                Message = Convert.ToString(message),
                Title = Convert.ToString(header)
            };
        }

        public void ShowLoader(string message)
        {
            ShowLoader(message, _defaultTitle);
        }

        #endregion
    }
}
