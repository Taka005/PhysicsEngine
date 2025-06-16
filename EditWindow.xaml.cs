using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PhysicsEngineCore.Objects;

namespace PhysicsEngineGUI {
    /// <summary>
    /// EditWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class EditWindow : Window {
        private readonly IObject obj;

        public EditWindow(IObject obj) {
            InitializeComponent();

            this.obj = obj;
        }
    }
}
