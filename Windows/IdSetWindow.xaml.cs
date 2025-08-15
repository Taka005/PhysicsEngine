using System.Windows;
using System.Windows.Input;

namespace PhysicsEngine.Windows{
    /// <summary>
    /// IdSetWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class IdSetWindow : Window{
        private Client client;

        public IdSetWindow(Client client){
            this.client = client;

            InitializeComponent();

            this.IdSetTextBox.Text = this.client.id;
        }

        private void IdSetTextBox_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                this.SetId();

                e.Handled = true;
            }
        }

        private void IdSetButton_Click(object sender, RoutedEventArgs e) {
            this.SetId();
        }

        private void SetId() {
            this.client.id = this.IdSetTextBox.Text.Trim();

            this.Close();
        }
    }
}
