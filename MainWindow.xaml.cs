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

        this.engine = new Engine(null);

        MyCanvas.Children.Add(this.engine.render);
        CompositionTarget.Rendering += this.engine.OnRendering;

        engine.Start();

        CircleOption circleOption = new CircleOption{
            posX = 100,
            posY = 100,
            mass = 0,
            diameter = 20,
            stiffness = 1
        };

        engine.SpawnObject(circleOption);
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