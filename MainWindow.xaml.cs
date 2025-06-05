using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using PhysicsEngineCore;
using PhysicsEngineCore.Options;

namespace PhysicsEngineGUI;

public partial class MainWindow : Window{
    public Engine engine { get; set; }

    public MainWindow(){
        InitializeComponent();

        EngineOption engineOption = new EngineOption {
            gravity = 100
        };

        this.engine = new Engine(engineOption);

        this.engine.render.isDebugMode = true;

        this.engine.render.Width = MyCanvas.ActualWidth;
        this.engine.render.Height = MyCanvas.ActualHeight;

        MyCanvas.Children.Add(this.engine.render);

        CompositionTarget.Rendering += this.engine.OnRendering;

        engine.Start();

        CircleOption circleOption1 = new CircleOption{
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

        RopeOption circleOption3 = new RopeOption {
            startX = 100,
            startY = 150,
            endX = 300,
            endY = 150,
            width = 10,
            stiffness = 0.8,
            mass = 10
        };

        LineOption circleOption2 = new LineOption {
            startX = 10,
            startY = 240,
            endX = 500,
            endY = 250,
            width = 10
        };

        engine.SpawnObject(circleOption1);
        engine.SpawnObject(circleOption4);
        engine.SpawnGround(circleOption2);
        engine.SpawnObject(circleOption3);
    }

    private void NewFile_Click(object sender, RoutedEventArgs e){
        MessageBox.Show("新規ファイルを作成します");
    }

    private void SaveFile_Click(object sender, RoutedEventArgs e){
        SaveFileDialog saveFileDialog = new SaveFileDialog{
            FileName = $"SaveData_{DateTime.Now}.json",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads",
            Filter = "すべてのファイル(*.*)|*.*|JSONファイル(*.json)|*.json",
            FilterIndex = 2,
            Title = "保存先のファイルを選択してください",
            RestoreDirectory = true
        };

        if(saveFileDialog.ShowDialog() == true){
            System.IO.File.WriteAllText(saveFileDialog.FileName,this.engine.Export());
        }
    }

    private void Exit_Click(object sender, RoutedEventArgs e){
        this.Close();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
        MessageBoxResult result = MessageBox.Show("本当に終了しますか？", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);

        if(result == MessageBoxResult.No){
            e.Cancel = true;
        }
    }
}