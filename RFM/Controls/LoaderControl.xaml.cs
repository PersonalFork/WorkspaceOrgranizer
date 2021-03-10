using System.Windows;
using System.Windows.Controls;

using RFM.Common.ViewModels;

namespace RFM.Controls
{
    /// <summary>
    /// Interaction logic for LoaderControl.xaml
    /// </summary>
    public partial class LoaderControl : UserControl
    {
        public LoadingPayloadViewModel LoadingContext
        {
            get => (LoadingPayloadViewModel)GetValue(LoadingContextProperty);
            set => SetValue(LoadingContextProperty, value);
        }

        public static readonly DependencyProperty LoadingContextProperty =
            DependencyProperty.Register(
                "LoadingContext",
                typeof(LoadingPayloadViewModel),
                typeof(LoaderControl),
                new PropertyMetadata(null));

        public LoaderControl()
        {
            InitializeComponent();
        }
    }
}
