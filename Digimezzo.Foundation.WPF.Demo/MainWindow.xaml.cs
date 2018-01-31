using Digimezzo.Foundation.WPF.Controls;

namespace Digimezzo.Foundation.WPF.Demo
{
    public partial class MainWindow : Windows10BorderlessWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HamburgerButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MySplitView.IsPaneOpen = !this.MySplitView.IsPaneOpen;
        }
    }
}
