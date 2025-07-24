using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PhysicsEngineCore.Objects;
using PhysicsEngineCore.Utils;
using Xceed.Wpf.Toolkit;

namespace PhysicsEngine {
    public partial class EditWindow : Window {
        private readonly IObject obj;

        public EditWindow(IObject obj) {
            this.obj = obj;

            InitializeComponent();

            this.ObjectId.Text = this.obj.id;
            this.PosXSlider.Value = this.obj.position.X;
            this.PosYSlider.Value = this.obj.position.Y;
            this.VelXSlider.Value = this.obj.velocity.X;
            this.VelYSlider.Value = this.obj.velocity.Y;
            this.MassSlider.Value = this.obj.mass;
            this.StiffnessSlider.Value = this.obj.stiffness;
            this.ObjectColor.SelectedColor = ParseColor.StringToColor(this.obj.color);

            this.PosXSlider.ValueChanged += PosXSlider_Change;
            this.PosYSlider.ValueChanged += PosYSlider_Change;
            this.VelXSlider.ValueChanged += VelXSlider_Change;
            this.VelYSlider.ValueChanged += VelYSlider_Change;
            this.MassSlider.ValueChanged += MassSlider_Change;
            this.StiffnessSlider.ValueChanged += StiffnessSlider_Change;
            this.ObjectColor.SelectedColorChanged += ColorPicker_Change;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e) {
            this.ObjectId.Text = this.obj.id;
            this.PosXSlider.Value = this.obj.position.X;
            this.PosYSlider.Value = this.obj.position.Y;
            this.VelXSlider.Value = this.obj.velocity.X;
            this.VelYSlider.Value = this.obj.velocity.Y;
            this.MassSlider.Value = this.obj.mass;
            this.StiffnessSlider.Value = this.obj.stiffness;
            this.ObjectColor.SelectedColor = ParseColor.StringToColor(this.obj.color);
        }

        private void PosXSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.obj.position = new Vector2(slider.Value,this.obj.position.Y);
            }
        }

        private void PosYSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.obj.position = new Vector2(this.obj.position.X, slider.Value);
            }
        }

        private void VelXSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.obj.velocity = new Vector2(slider.Value, this.obj.velocity.Y);
            }
        }

        private void VelYSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.obj.velocity = new Vector2(this.obj.velocity.X, slider.Value);
            }
        }

        private void MassSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.obj.mass = slider.Value;
            }
        }


        private void StiffnessSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.obj.stiffness = slider.Value;
            }
        }

        private void ColorPicker_Change(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            if(sender is ColorPicker picker) {
                this.obj.color = picker.SelectedColor.ToString() ?? "#F00000";
            }
        }
    }
}
