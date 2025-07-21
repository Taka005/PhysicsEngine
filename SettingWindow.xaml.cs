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

namespace PhysicsEngineGUI{
    public partial class SettingWindow : Window{
        private readonly Engine engine;

        public SettingWindow(Engine engine){
            this.engine = engine;

            InitializeComponent();
        }

        private void TrackingLimit_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.engine.SetTrackingLimit((int)slider.Value);
            }
        }

        private void TrackingInterval_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.engine.SetTrackingInterval((int)slider.Value);
            }
        }

        private void MovementLimit_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.engine.SetMovementLimit((int)slider.Value);
            }
        }
    }
}
