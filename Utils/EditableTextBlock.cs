using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PhysicsEngine.Utils {
    public class EditableTextBlock : ContentControl {
        private readonly Grid grid = new Grid();
        private readonly TextBox textBox = new TextBox();
        private readonly TextBlock textBlock = new TextBlock();
        private string previewText = string.Empty;
        private bool isEditMode = false;

        public string Text {
            get {
                return (string)GetValue(TextProperty);
            }
            set {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(EditableTextBlock), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public EditableTextBlock() {
            initializeControl();

            this.MouseDown += EditableTextBlock_MouseDown;
            this.textBox.KeyDown += TextBox_KeyDown;
            this.textBox.LostFocus += TextBox_LostFocus;
        }

        private void initializeControl() {
            this.textBox.Padding = new Thickness(1.0);
            this.textBox.Visibility = Visibility.Hidden;
            this.textBox.VerticalAlignment = VerticalAlignment.Center;
            this.textBox.VerticalContentAlignment = VerticalAlignment.Center;

            this.textBlock.Visibility = Visibility.Visible;
            this.textBlock.VerticalAlignment = VerticalAlignment.Center;

            this.grid.Children.Add(textBlock);
            this.grid.Children.Add(textBox);
            this.Content = grid;
            this.Focusable = false;
            this.grid.Focusable = false;
            this.textBlock.Focusable = false;
            this.MinWidth = 10;

            Binding binding = new(nameof(Text)) {
                Source = this,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };

            this.textBox.SetBinding(TextBox.TextProperty, binding);
            this.textBlock.SetBinding(TextBlock.TextProperty, binding);
        }

        private void EditableTextBlock_MouseDown(object sender, MouseEventArgs e) {
            if(!isEditMode) {
                this.previewText = textBlock.Text;
                this.isEditMode = true;
                this.OnIsEditModeChanged(isEditMode);
                this.textBox.Focus();
                this.textBox.SelectAll();
                e.Handled = true;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                this.isEditMode = false;
                OnIsEditModeChanged(isEditMode);
                e.Handled = true;
            } else if(e.Key == Key.Escape) {
                this.textBox.Text = previewText;
                this.isEditMode = false;
                this.OnIsEditModeChanged(isEditMode);
                e.Handled = true;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
            this.isEditMode = false;
            this.OnIsEditModeChanged(isEditMode);
            e.Handled = true;
        }

        private void OnIsEditModeChanged(bool value) {
            this.textBlock.Visibility = value ? Visibility.Hidden : Visibility.Visible;
            this.textBox.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }
    }
}