using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interactivity;

using Prism.Commands;

using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;

namespace RFM.Behaviors
{
    public class SelectFileBehavior : Behavior<ButtonBase>
    {
        public bool IsDirectory
        {
            get => (bool)GetValue(IsDirectoryProperty);
            set => SetValue(IsDirectoryProperty, value);
        }

        public static readonly DependencyProperty IsDirectoryProperty =
            DependencyProperty.Register(
                "IsDirectory",
                typeof(bool),
                typeof(SelectFileBehavior),
                new PropertyMetadata(false));

        public string SelectedPath
        {
            get => (string)GetValue(SelectedPathProperty);
            set => SetValue(SelectedPathProperty, value);
        }

        public static readonly DependencyProperty SelectedPathProperty =
            DependencyProperty.Register(
                "SelectedPath",
                typeof(string),
                typeof(SelectFileBehavior),
                new PropertyMetadata(string.Empty));

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(
                "Filter",
                typeof(string),
                typeof(SelectFileBehavior),
                new PropertyMetadata(string.Empty));

        public DelegateCommand<string> OnSelectedCommand
        {
            get => (DelegateCommand<string>)GetValue(OnSelectedCommandProperty);
            set => SetValue(OnSelectedCommandProperty, value);
        }

        public static readonly DependencyProperty OnSelectedCommandProperty =
            DependencyProperty.Register(
                "OnSelectedCommand",
                typeof(DelegateCommand<string>),
                typeof(SelectFileBehavior),
                new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= AssociatedObject_Loaded;
                AssociatedObject.Loaded += AssociatedObject_Loaded;

                AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
                AssociatedObject.Unloaded += AssociatedObject_Unloaded;
            }
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Click -= ShowFileBrowserDialog;
            AssociatedObject.Click -= ShowDirectoryBrowserDialog;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsDirectory == false)
            {
                AssociatedObject.Click -= ShowFileBrowserDialog;
                AssociatedObject.Click += ShowFileBrowserDialog;
            }
            else
            {
                AssociatedObject.Click -= ShowDirectoryBrowserDialog;
                AssociatedObject.Click += ShowDirectoryBrowserDialog;
            }
        }

        private void ShowDirectoryBrowserDialog(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (!string.IsNullOrEmpty(SelectedPath))
                {
                    string directory = Path.GetDirectoryName(SelectedPath);
                    if (Directory.Exists(directory))
                    {
                        dialog.SelectedPath = SelectedPath;
                    }
                }
                DialogResult dialogResult = dialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    SelectedPath = dialog.SelectedPath;
                    OnSelectedCommand?.Execute(SelectedPath);
                }
                else
                {
                    return;
                }
            }
        }

        private void ShowFileBrowserDialog(object sender, RoutedEventArgs e)
        {
            try
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    if (!string.IsNullOrEmpty(Filter))
                    {
                        dialog.Filter = Filter;
                    }
                    else
                    {
                        dialog.Filter = null;
                    }
                    if (!string.IsNullOrEmpty(SelectedPath))
                    {
                        SelectedPath = SelectedPath.Replace("\"", string.Empty);
                        string directory = Path.GetDirectoryName(SelectedPath);
                        if (Directory.Exists(directory))
                        {
                            dialog.InitialDirectory = directory;
                        }
                    }
                    DialogResult dialogResult = dialog.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        SelectedPath = dialog.FileName;
                        OnSelectedCommand?.Execute(SelectedPath);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (System.Exception)
            {
                // TODO: Log Exception.
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= AssociatedObject_Loaded;
                AssociatedObject.Click -= ShowDirectoryBrowserDialog;
                AssociatedObject.Click -= ShowFileBrowserDialog;
            }
        }
    }
}
