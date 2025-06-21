using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PhysicsEngineCore;
using PhysicsEngineCore.Objects;
using PhysicsEngineCore.Utils;

namespace PhysicsEngineGUI {
    /// <summary>
    /// EditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class EditWindow : Window {
        private readonly IObject obj;

        public EditWindow(IObject obj) {
            InitializeComponent();

            this.obj = obj;

            this.ObjectId.Text = obj.id;
            this.PosXSlider.Value = obj.position.X;
            this.PosYSlider.Value = obj.position.Y;
            this.VelXSlider.Value = obj.velocity.X;
            this.VelYSlider.Value = obj.velocity.Y;
            this.MassSlider.Value = obj.mass;
            this.StiffnessSlider.Value = obj.stiffness;
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

        private void VolXSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.obj.velocity = new Vector2(slider.Value, this.obj.velocity.Y);
            }
        }

        private void VolYSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.obj.velocity = new Vector2(this.obj.velocity.X, slider.Value);
            }
        }

        private void SizeSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                
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
    }
}
