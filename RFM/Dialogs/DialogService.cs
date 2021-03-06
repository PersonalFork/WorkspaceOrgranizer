using System;
using System.Windows;
using RFM.Dialogs.Base;

namespace RFM.Dialogs
{
    public interface IDialogService
    {
        T ShowDialog<T>(IDialogViewModel<T> dialog);
        /// <summary>
        /// Displays a dialog and auto-closes the dialog after the specified Auto-Hide-Interval-Seconds./>
        /// </summary>
        /// <typeparam name="T">The T return type.</typeparam>
        /// <param name="viewModel">The <see cref="IDialogViewModel{T}"/> dialog.</param>
        /// <param name="autoHideIntervalSecs">The number of seconds after which the dialog should close. 
        /// Minimum value is 2.</param>
        /// <returns></returns>
        T ShowDialog<T>(IDialogViewModel<T> viewModel, int? autoHideIntervalSecs);
    }

    public class DialogService : IDialogService
    {
        public T ShowDialog<T>(IDialogViewModel<T> dialog)
        {
            IDialogView dialogView = new Dialog
            {
                DataContext = dialog
            };
            dialog.Dialog = dialogView;
            dialogView.ShowDialog();

            return dialog.DialogResult;
        }

        public T ShowDialog<T>(IDialogViewModel<T> viewModel, int? autoHideIntervalSecs)
        {
            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                IDialogView dialog = new Dialog
                {
                    DataContext = viewModel
                };
                viewModel.Dialog = dialog;
                if (autoHideIntervalSecs != null)
                {
                    if (autoHideIntervalSecs < 2)
                    {
                        autoHideIntervalSecs = 2;
                    }

                    viewModel.AutoHideIntervalSecs = autoHideIntervalSecs;
                    viewModel.StartTimer();
                }

                dialog.ShowDialog();
            }));

            return viewModel.DialogResult;
        }
    }
}
