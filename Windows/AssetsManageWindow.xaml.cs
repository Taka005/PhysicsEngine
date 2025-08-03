using System.Windows;
using System.IO;
using Microsoft.Win32;
using PhysicsEngine.Utils;
using PhysicsEngineCore;
using System.Windows.Controls;

namespace PhysicsEngine.Windows {
    /// <summary>
    /// AssetsManageWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AssetsManageWindow : Window {
        private readonly Engine engine;
        private ComboBox ImageSelect;

        public AssetsManageWindow(Engine engine,ComboBox ImageSelect) {
            this.engine = engine;
            this.ImageSelect = ImageSelect;

            InitializeComponent();

            foreach(PhysicsEngineCore.Utils.Image image in this.engine.assets.images) {
                ImageAssetBlock imageBlock = new ImageAssetBlock {
                    ImageSource = image.source,
                    FileName = image.filename,
                    ImageWidth = image.width,
                    ImageHeight = image.height
                };

                imageBlock.DeleteRequested += ImageBlock_DeleteRequested;

                ImageListPanel.Children.Add(imageBlock);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e){
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "画像ファイル|*.jpg;*.jpeg;*.png|すべてのファイル|*.*";

            if (openFileDialog.ShowDialog() == true){
                try{
                    PhysicsEngineCore.Utils.Image image = this.engine.assets.Add(Path.GetFileName(openFileDialog.FileName), new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

                    ImageAssetBlock imageBlock = new ImageAssetBlock{
                        ImageSource = image.source,
                        FileName = image.filename,
                        ImageWidth = image.width,
                        ImageHeight = image.height
                    };

                    imageBlock.DeleteRequested += ImageBlock_DeleteRequested;

                    ImageListPanel.Children.Add(imageBlock);
                    this.ImageSelect.Items.Add(image.filename);
                }catch (Exception ex){
                    MessageBox.Show($"画像の読み込み中にエラーが発生しました: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ImageBlock_DeleteRequested(object sender, RoutedEventArgs e){
            if(sender is ImageAssetBlock imageBlockToRemove) {
                if (imageBlockToRemove != null){
                    this.engine.assets.Remove(imageBlockToRemove.FileName);
                    ImageListPanel.Children.Remove(imageBlockToRemove);
                    this.ImageSelect.Items.Remove(imageBlockToRemove.FileName);
                }
            }
        }
    }
}
