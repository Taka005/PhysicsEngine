using System.Reflection;
using System.Windows;

namespace PhysicsEngineGUI {
    public partial class AboutWindow : Window {
        public AboutWindow() {
            InitializeComponent();

            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "不明";
            VersionTextBlock.Text = $"バージョン: {version}";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
