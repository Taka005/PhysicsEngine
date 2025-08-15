using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using PhysicsEngineCore;
using PhysicsEngineCore.Options;
using PhysicsEngineCore.Utils;
using PhysicsEngine.Utils;
using PhysicsEngine.Windows;
using System.IO;

namespace PhysicsEngine {
    public partial class MainWindow : Window {
        public Engine engine { get; set; }
        public Client client { get; set; }
        public KeyInput keyInput { get; set; }

        public MainWindow() {
            EngineOption engineOption = new EngineOption {
                gravity = 100,
                friction = 0,
                pps = 180,
            };

            RenderOption renderOption = new RenderOption {
                offsetX = 0,
                offsetY = 0,
                scale = 1
            };

            this.engine = new Engine(engineOption,renderOption);
            this.client = new Client(this, this.engine);
            this.keyInput = new KeyInput(this);

            InitializeComponent();

            this.engine.render.Width = MyCanvas.ActualWidth;
            this.engine.render.Height = MyCanvas.ActualHeight;

            MyCanvas.Children.Add(this.engine.render);

            CompositionTarget.Rendering += this.engine.OnRendering;
            CompositionTarget.Rendering += this.Update;

            this.KeyDown += this.Window_KeyDown;

            engine.Start();

            CircleOption circleOption1 = new CircleOption {
                posX = 150,
                posY = 100,
                mass = 1,
                diameter = 10,
                stiffness = 0.8
            };

            SquareOption circleOption4 = new SquareOption {
                posX = 200,
                posY = 100,
                mass = 1,
                size = 10,
                stiffness = 0.8
            };

            TriangleOption circleOption8 = new TriangleOption {
                posX = 300,
                posY = 100,
                mass = 1,
                size = 10,
                stiffness = 0.8
            };

            LineOption circleOption2 = new LineOption {
                startX = 10,
                startY = 240,
                endX = 500,
                endY = 250,
                width = 10
            };

            LineOption circleOption6 = new LineOption {
                startX = 10,
                startY = 240,
                endX = 10,
                endY = 0,
                width = 10
            };


            LineOption circleOption7 = new LineOption {
                startX = 500,
                startY = 240,
                endX = 500,
                endY = 0,
                width = 10
            };

            CurveOption circleOption5 = new CurveOption {
                startX = 500,
                startY = 250,
                middleX = 600,
                middleY = 300,
                endX = 500,
                endY = 500,
                width = 10
            };

            engine.SpawnObject(circleOption1);
            engine.SpawnObject(circleOption4);
            engine.SpawnObject(circleOption8);
            engine.SpawnGround(circleOption2);
            engine.SpawnGround(circleOption6);
            engine.SpawnGround(circleOption7);
            engine.SpawnGround(circleOption5);

            ObjectTypeSelect.SelectionChanged += ObjectType_Change;
        }

        private void Update(object? sender, EventArgs e) {
            if(this.keyInput.IsKeyDown(Key.W)) {
                this.engine.render.offsetY += this.client.moveSpeed;
            }

            if(this.keyInput.IsKeyDown(Key.S)) {
                this.engine.render.offsetY -= this.client.moveSpeed;
            }

            if(this.keyInput.IsKeyDown(Key.A)) {
                this.engine.render.offsetX += this.client.moveSpeed;
            }

            if(this.keyInput.IsKeyDown(Key.D)) {
                this.engine.render.offsetX -= this.client.moveSpeed;
            }
        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Space) {
                if(this.engine.isStarted) {
                    ChangeButton.Background = ParseColor.StringToBrush("#FF00AA00");
                    ChangeButton.Content = "開始";


                    this.engine.Stop();
                } else {
                    ChangeButton.Background = ParseColor.StringToBrush("#FFFF0000");
                    ChangeButton.Content = "停止";

                    this.engine.Start();
                }
            }else if(e.Key == Key.F) {
                if(this.engine.render.isDisplayVector) {
                    DisplayVector.IsChecked = false;

                    this.engine.render.isDisplayVector = false;
                } else {
                    DisplayVector.IsChecked = true;

                    this.engine.render.isDisplayVector = true;
                }
            }else if(e.Key == Key.G) {
                if(this.engine.render.isDisplayGrid) {
                    DisplayGrid.IsChecked = false;

                    this.engine.render.isDisplayGrid = false;
                } else {
                    DisplayGrid.IsChecked = true;

                    this.engine.render.isDisplayGrid = true;
                }
            }else if (e.Key == Key.T) {
                this.engine.Step();
            }else if(e.Key == Key.D1) {
                toolComboBox.SelectedIndex = 0;
            } else if(e.Key == Key.D2) {
                toolComboBox.SelectedIndex = 1;
            } else if(e.Key == Key.D3) {
                toolComboBox.SelectedIndex = 2;
            } else if(e.Key == Key.D4) {
                toolComboBox.SelectedIndex = 3;
            } else if(e.Key == Key.D5) {
                toolComboBox.SelectedIndex = 4;
            } else if(e.Key == Key.D6) {
                toolComboBox.SelectedIndex = 5;
            } else if(e.Key == Key.D7) {
                toolComboBox.SelectedIndex = 6;
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if(sender is Canvas canvas) {
                Point clickPosition = e.GetPosition(canvas);

                this.client.MouseLeftDown(clickPosition);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e) {
            this.client.MouseMove(e, e.GetPosition(MyCanvas));
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            this.client.MouseLeftButtonUp();
        }

        private void Canvas_MouseScroll(object sender, MouseWheelEventArgs e) {
            if((this.engine.render.scale <= 0.1&&e.Delta < 0) || (this.engine.render.scale >= 10&&e.Delta > 0)) return;

            if(sender is Canvas canvas) {
                this.engine.render.scale += Math.Sign(e.Delta)*0.1;

                scaleSlider.Value = this.engine.render.scale;
            }
        }

        private void NewFile_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show("新規作成しますか？", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes) {
                this.engine.Clear(true);
            }
        }

        private void JsonSaveFile_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                FileName = "PE_SaveData.json",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads",
                Filter = "すべてのファイル(*.*)|*.*|JSONファイル(*.json)|*.json",
                FilterIndex = 2,
                Title = "保存先のファイルを選択してください",
                RestoreDirectory = true
            };

            if(saveFileDialog.ShowDialog() == true) {
                try {
                    File.WriteAllText(saveFileDialog.FileName, this.engine.Export());
                } catch(IOException ex) {
                    MessageBox.Show("ファイルの保存中にエラーが発生しました:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                } catch(System.Security.SecurityException ex) {
                    MessageBox.Show("ファイルへのアクセス許可がありません:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                } catch(Exception ex) {
                    MessageBox.Show("予期せぬエラーが発生しました:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                FileName = "MapData.pe",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads",
                Filter = "すべてのファイル(*.*)|*.*|マップファイル(*.pe)|*.pe",
                FilterIndex = 2,
                Title = "保存先のファイルを選択してください",
                RestoreDirectory = true
            };

            if(saveFileDialog.ShowDialog() == true) {
                try {
                    using(Stream zipStream = this.engine.ExportMap()) {
                        using(FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write)) {
                            zipStream.CopyTo(fileStream);
                        }
                    }
                } catch(IOException ex) {
                    MessageBox.Show("ファイルの保存中にエラーが発生しました:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                } catch(System.Security.SecurityException ex) {
                    MessageBox.Show("ファイルへのアクセス許可がありません:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                } catch(Exception ex) {
                    MessageBox.Show("予期せぬエラーが発生しました:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void JsonOpenFile_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads",
                Title = "開くファイルを選択してください",
                Filter = "JSONファイル(*.json)|*.json|すべてのファイル(*.*)|*.*",
            };

            if(openFileDialog.ShowDialog() == true) {
                try {
                    string filePath = openFileDialog.FileName;

                    string fileContent = File.ReadAllText(filePath);

                    this.engine.Import(fileContent);

                    gravitySlider.Value = this.engine.gravity;
                    frictionSlider.Value = this.engine.friction;
                    playBackSpeedSlider.Value = this.engine.playBackSpeed;

                    scaleSlider.Value = this.engine.render.scale;
                } catch(IOException ex) {
                    MessageBox.Show("ファイルの読み込み中にエラーが発生しました:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                } catch(System.Security.SecurityException ex) {
                    MessageBox.Show("ファイルへのアクセス許可がありません:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                } catch(Exception ex) {
                    MessageBox.Show("予期せぬエラーが発生しました:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads",
                Title = "開くファイルを選択してください",
                Filter = "マップファイル(*.pe)|*.pe|すべてのファイル(*.*)|*.*",
            };

            if(openFileDialog.ShowDialog() == true) {
                try {
                    using(FileStream zipFs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        this.engine.ImportMap(zipFs);

                        gravitySlider.Value = this.engine.gravity;
                        frictionSlider.Value = this.engine.friction;
                        playBackSpeedSlider.Value = this.engine.playBackSpeed;

                        scaleSlider.Value = this.engine.render.scale;

                        foreach(PhysicsEngineCore.Utils.Image image in this.engine.assets.images) {
                            ImageSelect.Items.Add(image.filename);
                        }
                    }
                } catch(IOException ex) {
                    MessageBox.Show("ファイルの読み込み中にエラーが発生しました:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                } catch(System.Security.SecurityException ex) {
                    MessageBox.Show("ファイルへのアクセス許可がありません:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                } catch(Exception ex) {
                    MessageBox.Show("予期せぬエラーが発生しました:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UpdateScript_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads",
                Title = "開くファイルを選択してください",
                Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*",
            };

            if(openFileDialog.ShowDialog() == true) {
                try {
                    string filePath = openFileDialog.FileName;

                    string fileContent = File.ReadAllText(filePath);

                    this.engine.updateScript = fileContent;
                } catch(IOException ex) {
                    MessageBox.Show("ファイルの読み込み中にエラーが発生しました:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                } catch(System.Security.SecurityException ex) {
                    MessageBox.Show("ファイルへのアクセス許可がありません:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                } catch(Exception ex) {
                    MessageBox.Show("予期せぬエラーが発生しました:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            MessageBoxResult result = MessageBox.Show("本当に終了しますか？", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(result == MessageBoxResult.No) {
                e.Cancel = true;
            } else {
                this.keyInput.Dispose();
            }
        }

        private void AppSource_Click(object sender, RoutedEventArgs e) {
            try {
                ProcessStartInfo psi = new ProcessStartInfo {
                    FileName = "https://github.com/Taka005/PhysicsEngine/",
                    UseShellExecute = true
                };

                Process.Start(psi);
            } catch {
                MessageBox.Show("開くことができませんでした", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CoreSource_Click(object sender, RoutedEventArgs e) {
            try {
                ProcessStartInfo psi = new ProcessStartInfo {
                    FileName = "https://github.com/Taka005/PhysicsEngineCore/",
                    UseShellExecute = true
                };

                Process.Start(psi);
            } catch {
                MessageBox.Show("開くことができませんでした", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Site_Click(object sender, RoutedEventArgs e) {
            try {
                ProcessStartInfo psi = new ProcessStartInfo {
                    FileName = "https://engine.takadev.jp",
                    UseShellExecute = true
                };

                Process.Start(psi);
            } catch {
                MessageBox.Show("開くことができませんでした", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayVector_Click(object sender, RoutedEventArgs e) {
            if(sender is MenuItem debugMenuItem) {
                if(debugMenuItem.IsChecked) {
                    this.engine.render.isDisplayVector = true;
                } else {
                    this.engine.render.isDisplayVector = false;
                }
            }
        }

        private void DisplayGrid_Click(object sender, RoutedEventArgs e) {
            if(sender is MenuItem debugMenuItem) {
                if(debugMenuItem.IsChecked) {
                    this.engine.render.isDisplayGrid = true;
                } else {
                    this.engine.render.isDisplayGrid = false;
                }
            }
        }


        private void About_Click(object sender, RoutedEventArgs e) {
            AboutWindow aboutWindow = new AboutWindow {
                Owner = this
            };

            aboutWindow.ShowDialog();
        }

        private void StartAndStop_Click(object sender, RoutedEventArgs e) {
            if(sender is Button button) {
                if(this.engine.isStarted) {
                    button.Background = ParseColor.StringToBrush("#FF00AA00");
                    button.Content = "開始";


                    this.engine.Stop();
                } else {
                    button.Background = ParseColor.StringToBrush("#FFFF0000");
                    button.Content = "停止";

                    this.engine.Start();
                }
            }
        }

        private void Step_Click(object sender, RoutedEventArgs e) {
            if(this.engine.isStarted) {
                MessageBox.Show("システムが動いている間は操作できません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            this.engine.Step();
        }

        private void Reset_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show("リセットしますか？", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes) {
                this.engine.Clear();
            }
        }

        private void Restore_Click(object sender, RoutedEventArgs e) {
            this.client.RestoreHistory();
        }

        private void Gravity_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.engine.SetGravity(slider.Value);
            }
        }

        private void Friction_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.engine.friction = slider.Value;
            }
        }

        private void PlayBackSpeed_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.engine.SetPlayBackSpeed((float)slider.Value);
            }
        }

        private void Scale_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.engine.render.scale = slider.Value;
            }
        }

        private void Tool_Change(object sender, SelectionChangedEventArgs e) {
            if(sender is ComboBox combobox) {
                ComboBoxItem selectedContent = (ComboBoxItem)combobox.SelectedItem;

                this.client.setTool(selectedContent.Content.ToString() ?? "閲覧");
            }
        }

        private void Image_Change(object sender, SelectionChangedEventArgs e) {
            if(sender is ComboBox combobox) {
                string? selectedContent = combobox.SelectedItem as string;

                this.client.SetImageName(selectedContent ?? "なし");
            }
        }

        private void ObjectType_Change(object sender, SelectionChangedEventArgs e) {
            if(sender is ComboBox combobox) {
                ComboBoxItem selectedContent = (ComboBoxItem)combobox.SelectedItem;

                this.client.setSpawnType(selectedContent.Content.ToString() ?? "円");

                if(this.client.spawnType == ObjectType.Booster) {
                    SizeSlider.IsEnabled = false;
                    MassSlider.IsEnabled = false;
                    StiffnessSlider.IsEnabled = false;
                    SizeText.IsEnabled = false;
                    MassText.IsEnabled = false;
                    StiffnessText.IsEnabled = false;
                } else {
                    SizeSlider.IsEnabled = true;
                    MassSlider.IsEnabled = true;
                    StiffnessSlider.IsEnabled = true;
                    SizeText.IsEnabled = true;
                    MassText.IsEnabled = true;
                    StiffnessText.IsEnabled = true;
                }
            }
        }

        private void Size_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.client.size = slider.Value;
            }
        }

        private void Mass_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.client.mass = slider.Value;
            }
        }

        private void Stiffness_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.client.stiffness = slider.Value;
            }
        }

        private void VecX_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.client.vecX = slider.Value;
            }
        }

        private void VecY_Change(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(sender is Slider slider) {
                this.client.vecY = slider.Value;
            }
        }

        private void Color_Change(object sender, RoutedPropertyChangedEventArgs<Color?> e) {
            if(sender is Xceed.Wpf.Toolkit.ColorPicker picker) {
                this.client.color = picker.SelectedColor;
            }
        }

        private void ConnectionMode_Click(Object sender, RoutedEventArgs e) {
            if(sender is MenuItem connectionModeMenuItem) {
                if(connectionModeMenuItem.IsChecked) {
                    this.client.connectionType = connectionType.Dynamic;
                } else {
                    this.client.connectionType = connectionType.Minimum;
                }
            }
        }

        private void DynamicTrackingMode_Click(Object sender, RoutedEventArgs e) {
            if(sender is MenuItem dynamicTrackingMenuItem) {
                if(dynamicTrackingMenuItem.IsChecked) {
                    this.engine.isDynamicTrackingMode = true;
                } else {
                    this.engine.isDynamicTrackingMode = false;
                }
            }
        }

        private void TrackingMode_Click(Object sender, RoutedEventArgs e) {
            if(sender is MenuItem trackingModeMenuItem) {
                if(trackingModeMenuItem.IsChecked) {
                    this.engine.isTrackingMode = true;
                } else {
                    this.engine.isTrackingMode = false;
                }
            }
        }

        private void TrackingDelete_Click(Object sender, RoutedEventArgs e) {
            this.engine.ClearTrack();
        }

        private void Setting_Click(object sender, RoutedEventArgs e) {
            SettingWindow settingWindow = new SettingWindow(this.engine, this.client) {
                Owner = this
            };

            settingWindow.Show();
        }

        private void AssetsManage_Click(object sender, RoutedEventArgs e) {
            AssetsManageWindow assetsManagerWindow = new AssetsManageWindow(this.engine,ImageSelect) {
                Owner = this
            };

            assetsManagerWindow.Show();
        }

        private void RunCommand_Click(object sender, RoutedEventArgs e) {
            CommandWindow commandWindow = new CommandWindow(this.engine) {
                Owner = this
            };

            commandWindow.Show();
        }

        private void DebugMode_Click(object sender, RoutedEventArgs e) {
            if(sender is MenuItem devModeMenuItem) {
                if(devModeMenuItem.IsChecked) {
                    this.engine.render.isDebugMode = true;
                } else {
                    this.engine.render.isDebugMode = false;
                }
            }
        }

        private void ResetScaleOffset_Click(Object sender, RoutedEventArgs e) {
            this.engine.render.ResetTransform();

            scaleSlider.Value = 1;
        }

        private void GridCrossMode_Click(object sender, RoutedEventArgs e) {
            if(sender is MenuItem gridCrossModeMenuItem) {
                if(gridCrossModeMenuItem.IsChecked) {
                    this.client.isGridCrossMode = true;
                } else {
                    this.client.isGridCrossMode = false;
                }
            }
        }

        private void IdSet_Click(object sender, RoutedEventArgs e) {
            IdSetWindow idSetWindow = new IdSetWindow(this.client) {
                Owner = this
            };

            idSetWindow.ShowDialog();
        }
    }
}