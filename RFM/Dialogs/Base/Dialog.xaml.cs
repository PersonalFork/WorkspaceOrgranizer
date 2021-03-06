using System.Windows;

namespace RFM.Dialogs.Base
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window, IDialogView
    {
        public Dialog()
        {
            InitializeComponent();

            try
            {
                if (Application.Current?.MainWindow?.IsLoaded == true)
                {
                    Owner = Application.Current.MainWindow;
                }
            }
            catch (System.Exception)
            {

            }
        }
    }
}
