using System.Windows.Controls;

namespace RFM.Views
{
    /// <summary>
    /// Interaction logic for AddApplication.xaml
    /// </summary>
    public partial class AddApplicationPage : UserControl
    {
        public AddApplicationPage()
        {
            InitializeComponent();
            Loaded -= AddApplicationPage_Loaded;
            Loaded += AddApplicationPage_Loaded;
        }

        private void AddApplicationPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            txtAppName.Focus();
        }
    }
}
