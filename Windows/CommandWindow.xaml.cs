using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using PhysicsEngineCore;

namespace PhysicsEngine.Windows{
    /// <summary>  
    /// CommandWindow.xaml の相互作用ロジック  
    /// </summary>  
    public partial class CommandWindow : Window {
        private readonly Engine engine;
        private readonly Dictionary<string, object> localVariables = [];

        public CommandWindow(Engine engine) {
            this.engine = engine;

            InitializeComponent();

            if(this.engine.scriptErrorMessage != string.Empty){
                DisplayUpdateError($"{this.engine.scriptErrorMessage}");
            }
        }

        private void CommandTextBox_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                this.ExecuteCommand();

                e.Handled = true;
            }
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e) {
            this.ExecuteCommand();
        }

        private void ExecuteCommand() {
            string command = CommandTextBox.Text.Trim();
            if(string.IsNullOrEmpty(command)) return;

            try {
                string? result = this.engine.command.Execute(command, this.localVariables);

                DisplayResult(result ?? "");
            } catch(Exception ex) {
                DisplayError(ex.Message);
            }

            CommandTextBox.Text = string.Empty;

            ScrollToEnd();
        }

        private void DisplayResult(string message) {
            TextRange tr = new TextRange(ResultTextBlock.Document.ContentEnd, ResultTextBlock.Document.ContentEnd);
            tr.Text = $"> {CommandTextBox.Text}\n{message}\n";
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);

            ScrollToEnd();
        }

        private void DisplayError(string errorMessage) {
            TextRange tr = new TextRange(ResultTextBlock.Document.ContentEnd, ResultTextBlock.Document.ContentEnd);
            tr.Text = $"> {CommandTextBox.Text}\nエラー: {errorMessage}\n";
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);

            ScrollToEnd();
        }

        private void DisplayUpdateError(string errorMessage){
            TextRange tr = new TextRange(ResultTextBlock.Document.ContentEnd, ResultTextBlock.Document.ContentEnd);
            tr.Text = $"アップデートエラー: {errorMessage}\n";
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);

            ScrollToEnd();
        }

        private void ScrollToEnd() {
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                ResultScrollViewer.ScrollToEnd();
            }));
        }
    }
}
