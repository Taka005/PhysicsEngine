using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PhysicsEngineCore.Objects;
using PhysicsEngineCore.Objects.Interfaces;
using PhysicsEngineCore.Utils;
using Xceed.Wpf.Toolkit;

namespace PhysicsEngine {
    public partial class GroundEditWindow : Window {
        private readonly IGround ground;

        public GroundEditWindow(IGround ground) {
            this.ground = ground;

            InitializeComponent();

            if(ground is Line line) {       
                this.StartXSlider.Value = line.start.X;
                this.StartYSlider.Value = line.start.Y;
                this.EndXSlider.Value = line.end.X;
                this.EndYSlider.Value = line.end.Y;

                this.MiddleXSlider.IsEnabled = false;
                this.MiddleYSlider.IsEnabled = false;
                this.MiddleXText.IsEnabled = false;
                this.MiddleYText.IsEnabled = false;
            }

            if(ground is Curve curve) {
                this.StartXSlider.Value = curve.start.X;
                this.StartYSlider.Value = curve.start.Y;
                this.MiddleXSlider.Value = curve.middle.X;
                this.MiddleYSlider.Value = curve.middle.Y;
                this.EndXSlider.Value = curve.end.X;
                this.EndYSlider.Value = curve.end.Y;
            }

            this.GroundId.Text = this.ground.id;
            this.WidthSlider.Value = this.ground.width;
            this.ObjectColor.SelectedColor = ParseColor.StringToColor(this.ground.color);

            this.StartXSlider.ValueChanged += StartXSlider_Change;
            this.StartYSlider.ValueChanged += StartYSlider_Change;
            this.MiddleXSlider.ValueChanged += MiddleXSlider_Change;
            this.MiddleYSlider.ValueChanged += MiddleYSlider_Change;
            this.EndXSlider.ValueChanged += EndXSlider_Change;
            this.EndYSlider.ValueChanged += EndYSlider_Change;
            this.WidthSlider.ValueChanged += WidthSlider_Change;
            this.ObjectColor.SelectedColorChanged += ColorPicker_Change;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e) {
            if(this.ground is Line line) {       
                this.StartXSlider.Value = line.start.X;
                this.StartYSlider.Value = line.start.Y;
                this.EndXSlider.Value = line.end.X;
                this.EndYSlider.Value = line.end.Y;

                this.MiddleXSlider.IsEnabled = false;
                this.MiddleYSlider.IsEnabled = false;
                this.MiddleXText.IsEnabled = false;
                this.MiddleYText.IsEnabled = false;
            }

            if(this.ground is Curve curve) {
                this.StartXSlider.Value = curve.start.X;
                this.StartYSlider.Value = curve.start.Y;
                this.MiddleXSlider.Value = curve.middle.X;
                this.MiddleYSlider.Value = curve.middle.Y;
                this.EndXSlider.Value = curve.end.X;
                this.EndYSlider.Value = curve.end.Y;
            }

            this.GroundId.Text = this.ground.id;
            this.WidthSlider.Value = this.ground.width;
            this.ObjectColor.SelectedColor = ParseColor.StringToColor(this.ground.color);

        }

        private void StartXSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                if(this.ground is Line line) {
                    line.start = new Vector2(slider.Value, line.start.Y);
                } else if(this.ground is Curve curve) {
                    curve.start = new Vector2(slider.Value, curve.start.Y);
                }
            }
        }

        private void StartYSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                if(this.ground is Line line) {
                    line.start = new Vector2(line.start.X, slider.Value);
                } else if(this.ground is Curve curve) {
                    curve.start = new Vector2(curve.start.X, slider.Value);
                }
            }
        }

        private void MiddleXSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                if(this.ground is Curve curve) {
                    curve.middle = new Vector2(slider.Value, curve.middle.Y);
                }
            }
        }

        private void MiddleYSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                if(this.ground is Curve curve) {
                    curve.middle = new Vector2(curve.middle.X, slider.Value);
                }
            }
        }

        private void EndXSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                if(this.ground is Line line) {
                    line.end = new Vector2(slider.Value, line.end.Y);
                } else if(this.ground is Curve curve) {
                    curve.end = new Vector2(slider.Value, curve.end.Y);
                }
            }
        }

        private void EndYSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                if(this.ground is Line line) {
                    line.end = new Vector2(line.end.X, slider.Value);
                } else if(this.ground is Curve curve) {
                    curve.end = new Vector2(curve.end.X, slider.Value);
                }
            }
        }

        private void WidthSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.ground.width = slider.Value;
            }
        }

        private void ColorPicker_Change(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            if(sender is ColorPicker picker) {
                this.ground.color = picker.SelectedColor.ToString() ?? "#F00000";
            }
        }
    }
}
