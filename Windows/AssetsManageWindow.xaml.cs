using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;
using PhysicsEngine.Utils;

namespace PhysicsEngine.Windows {
    /// <summary>
    /// AssetsManageWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AssetsManageWindow : Window {
        public AssetsManageWindow() {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e){
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "画像ファイル|*.jpg;*.jpeg;*.png|すべてのファイル|*.*";

            if (openFileDialog.ShowDialog() == true){
                try{
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    ImageAssetBlock imageBlock = new ImageAssetBlock();
                    imageBlock.ImageSource = bitmap;
                    imageBlock.FileName = Path.GetFileName(openFileDialog.FileName);
                    imageBlock.ImageWidth = bitmap.PixelWidth;
                    imageBlock.ImageHeight = bitmap.PixelHeight;

                    imageBlock.DeleteRequested += ImageBlock_DeleteRequested;

                    ImageListPanel.Children.Add(imageBlock);
                }catch (Exception ex){
                    MessageBox.Show($"画像の読み込み中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ImageBlock_DeleteRequested(object sender, RoutedEventArgs e){
            if(sender is ImageAssetBlock imageBlockToRemove) {
                if (imageBlockToRemove != null){
                    ImageListPanel.Children.Remove(imageBlockToRemove);
                }
            }
        }
    }
}
