using ActReport.ViewModel;
using System.Windows;

namespace ActReport.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
         Window window = Window.GetWindow(this);
         window.Close();
		}
	}
}
