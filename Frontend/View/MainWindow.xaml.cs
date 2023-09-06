using Frontend.Model;
using Frontend.ViewModel;
using System.Windows;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.viewModel = (MainViewModel)DataContext;

        }

        /// <summary>
        /// Login button
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        /// <returns></returns>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Login();
            if (u != null)
            {
                BoardControllerView boardView = new BoardControllerView(u);
                boardView.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Register button
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        /// <returns></returns>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Register();
        }
    }
}
