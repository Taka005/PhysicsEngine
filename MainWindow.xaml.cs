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

namespace PhysicsEngineGUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void NewFile_Click(object sender, RoutedEventArgs e){
        MessageBox.Show("新規ファイルを作成します");
    }

    private void SaveFile_Click(object sender, RoutedEventArgs e){
        MessageBox.Show("ファイルを保存します");
    }

    private void Exit_Click(object sender, RoutedEventArgs e){
        Application.Current.Shutdown();
    }
}