using System.Windows;
using System.Windows.Controls;
using PhysicsEngineCore;

namespace PhysicsEngineGUI{
    public partial class SettingWindow : Window{
        private readonly Engine engine;
        private readonly Client client;

        public SettingWindow(Engine engine,Client client){
            this.engine = engine;
            this.client = client;

            InitializeComponent();

            this.TrackingLimitSlider.Value = this.engine.trackingLimit;
            this.TrackingIntervalSlider.Value = this.engine.trackingInterval;
            this.MovementLimitSlider.Value = this.engine.movementLimit;
            this.PpsSlider.Value = this.engine.pps;
            this.MoveSpeedSlider.Value = this.client.moveSpeed;

            this.TrackingLimitSlider.ValueChanged += TrackingLimit_Change;
            this.TrackingIntervalSlider.ValueChanged += TrackingInterval_Change;
            this.MovementLimitSlider.ValueChanged += MovementLimit_Change;
            this.PpsSlider.ValueChanged += Pps_Change;
            this.MoveSpeedSlider.ValueChanged += MoveSpeed_Change;
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

        private void Pps_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.engine.SetPps((int)slider.Value);
            }
        }

        private void MoveSpeed_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.client.moveSpeed = slider.Value;
            }
        }
    }
}
