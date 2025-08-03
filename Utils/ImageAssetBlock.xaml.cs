using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PhysicsEngine.Utils {
    /// <summary>
    /// ImageAssetBlock.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageAssetBlock : UserControl {
        public static readonly RoutedEvent DeleteRequestedEvent = EventManager.RegisterRoutedEvent("DeleteRequested", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ImageAssetBlock));

        public event RoutedEventHandler DeleteRequested{
            add {
                AddHandler(DeleteRequestedEvent, value);
            }
            remove {
                RemoveHandler(DeleteRequestedEvent, value);
            }
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(BitmapImage), typeof(ImageAssetBlock));

        public BitmapImage ImageSource{
            get {
                return (BitmapImage)GetValue(ImageSourceProperty);
            }
            set {
                SetValue(ImageSourceProperty, value);
            }
        }

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(ImageAssetBlock));

        public string FileName{
            get {
                return (string)GetValue(FileNameProperty);
            }
            set {
                SetValue(FileNameProperty, value);
            }
        }

        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(int), typeof(ImageAssetBlock));

        public int ImageWidth{
            get {
                return (int)GetValue(ImageWidthProperty);
            }
            set {
                SetValue(ImageWidthProperty, value);
            }
        }

        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(int), typeof(ImageAssetBlock));

        public int ImageHeight{
            get {
                return (int)GetValue(ImageHeightProperty);
            }
            set {
                SetValue(ImageHeightProperty, value);
            }
        }

        public ImageAssetBlock(){
            InitializeComponent();
            this.DataContext = this;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e){
            RaiseEvent(new RoutedEventArgs(DeleteRequestedEvent, this));
        }
    }
}
