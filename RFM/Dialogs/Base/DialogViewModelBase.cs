using System;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

using Prism.Commands;
using Prism.Mvvm;

namespace RFM.Dialogs.Base
{
    public abstract class DialogViewModelBase<T> : BindableBase, IDialogViewModel<T>
    {
        #region Protected Variable Declarations.

        protected Timer _timer;

        #endregion

        #region IDialogViewModel Interface Implementation.

        public int? AutoHideIntervalSecs { get; set; }

        private T _dialogResult;
        public T DialogResult
        {
            get => _dialogResult;
            set => SetProperty(ref _dialogResult, value);
        }

        private bool _isLightDismissible;
        public bool IsLightDismissible
        {
            get => _isLightDismissible;
            set => SetProperty(ref _isLightDismissible, value);
        }

        public DelegateCommand LightDismissCommand { get; private set; }
        public IDialogView Dialog { get; set; }
        public virtual void CloseDialog(T result)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
            DialogResult = result;
            if (Dialog != null)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    Dialog.Close();
                }));
            }
        }
        public virtual void StartTimer()
        {
            throw new NotImplementedException("Override this method if a Timer is associated.");
        }
        public virtual void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }

        #endregion

        #region Constructors.

        protected DialogViewModelBase()
        {
            LightDismissCommand = new DelegateCommand(DoLightDismiss, CanLightDismiss)
                .ObservesProperty(() => IsLightDismissible);
        }

        #endregion

        #region Protected Method Declarations.

        protected virtual void DoLightDismiss()
        {
        }

        protected virtual bool CanLightDismiss()
        {
            return IsLightDismissible;
        }

        #endregion
    }
}
