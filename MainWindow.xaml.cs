using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using PhysicsEngineCore;
using PhysicsEngineCore.Options;
using PhysicsEngineCore.Utils;

namespace PhysicsEngineGUI;

public partial class MainWindow : Window {
    public Engine engine { get; set; }
    public Client client { get; set; }

    public MainWindow() {
        EngineOption engineOption = new EngineOption {
            gravity = 100,
            friction = 0.01,
            pps = 180,
        };

        this.engine = new Engine(engineOption);
        this.client = new Client(this,this.engine);

        InitializeComponent();

        this.engine.render.Width = MyCanvas.ActualWidth;
        this.engine.render.Height = MyCanvas.ActualHeight;

        MyCanvas.Children.Add(this.engine.render);

        CompositionTarget.Rendering += this.engine.OnRendering;

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
    }

    private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
        if(sender is Canvas canvas) {
            Point clickPosition = e.GetPosition(canvas);

            this.client.MouseLeftDown(clickPosition);
        }
    }

    private void Canvas_MouseMove(object sender, MouseEventArgs e){
        this.client.MouseMove(e, e.GetPosition(MyCanvas));
    }

    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e){
       this.client.MouseLeftButtonUp();
    }
    
    private void NewFile_Click(object sender, RoutedEventArgs e) {
        MessageBoxResult result = MessageBox.Show("新規作成しますか？", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);

        if(result == MessageBoxResult.Yes) {
            this.engine.Clear(force: true);
        }
    }

    private void SaveFile_Click(object sender, RoutedEventArgs e) {
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
                System.IO.File.WriteAllText(saveFileDialog.FileName, this.engine.Export());
            } catch(System.IO.IOException ex) {
                MessageBox.Show("ファイルの保存中にエラーが発生しました:\n" + ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
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
            Filter = "JSONファイル(*.json)|*.json|すべてのファイル(*.*)|*.*",
        };

        if(openFileDialog.ShowDialog() == true) {
            try {
                string filePath = openFileDialog.FileName;

                string fileContent = System.IO.File.ReadAllText(filePath);

                this.engine.Import(fileContent);

                gravitySlider.Value = this.engine.gravity;
                frictionSlider.Value = this.engine.friction;
                playBackSpeedSlider.Value = this.engine.playBackSpeed;

                ChangeButton.Background = ParseColor.StringToBrush("#FF00AA00");
                ChangeButton.Content = "開始";
            } catch(System.IO.IOException ex) {
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
        }
    }

    private void Source_Click(object sender, RoutedEventArgs e) {
        try {
            ProcessStartInfo psi = new ProcessStartInfo {
                FileName = "https://github.com/Taka005/PhysicsEngineGUI/",
                UseShellExecute = true
            };

            Process.Start(psi);
        } catch {
            MessageBox.Show("開くことができませんでした", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DebugMode_Click(object sender, RoutedEventArgs e) {
        if(sender is MenuItem debugMenuItem) {
            if(debugMenuItem.IsChecked) {
                this.engine.render.isDebugMode = true;
            } else {
                this.engine.render.isDebugMode = false;
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
            this.engine.gravity = slider.Value;
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

    private void Tool_Change(object sender, SelectionChangedEventArgs e) {
        if(sender is ComboBox combobox) {
            ComboBoxItem selectedContent = (ComboBoxItem)combobox.SelectedItem;

            this.client.setTool(selectedContent.Content.ToString() ?? "閲覧");
        }
    }

    private void ObjectType_Change(object sender, SelectionChangedEventArgs e) {
        if(sender is ComboBox combobox) {
            ComboBoxItem selectedContent = (ComboBoxItem)combobox.SelectedItem;

            this.client.setSpawnType(selectedContent.Content.ToString() ?? "円");
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
        SettingWindow settingWindow = new SettingWindow{
            Owner = this
         };

         settingWindow.Show();
    }
}