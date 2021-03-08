using System.Windows;
using System.Windows.Controls;

namespace RFM.Views
{
    /// <summary>
    /// Interaction logic for CreateSectionPage.xaml
    /// </summary>
    public partial class CreateSectionPage : UserControl
    {
        public CreateSectionPage()
        {
            InitializeComponent();
            Loaded += CreateSectionPage_Loaded;
        }

        private void CreateSectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            txtWorkspaceName.Focus();
        }
    }
}
