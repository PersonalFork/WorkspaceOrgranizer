using System.Windows.Controls;

namespace RFM.Views
{
    /// <summary>
    /// Interaction logic for AddNotePage.xaml
    /// </summary>
    public partial class AddNotePage : UserControl
    {
        public AddNotePage()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Focus();
            }
            else
            {
                txtContent.Focus();
            }
        }
    }
}
