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

namespace PhysicsEngineGUI;

public partial class MainWindow : Window{
    public Engine engine { get; set; }

    public MainWindow(){
        InitializeComponent();

        this.engine = new Engine(null);

        engine.Start();
    }
    private void NewFile_Click(object sender, RoutedEventArgs e){
        MessageBox.Show("新規ファイルを作成します");
    }

    private void SaveFile_Click(object sender, RoutedEventArgs e){
        SaveFileDialog saveFileDialog = new SaveFileDialog{
            FileName = "map.json",
            InitialDirectory = @"C:\",
            Filter = "JSONファイル(*.json)|*.json|すべてのファイル(*.*)|*.*",
            FilterIndex = 2,
            Title = "保存先のファイルを選択してください",
            RestoreDirectory = true
        };

        if(saveFileDialog.ShowDialog() == true){
            System.IO.File.WriteAllText(saveFileDialog.FileName,this.engine.Export());
        }
    }

    private void Exit_Click(object sender, RoutedEventArgs e){
        Application.Current.Shutdown();
    }
}