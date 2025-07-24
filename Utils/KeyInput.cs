using System.Windows.Input;
using System.Windows;

namespace PhysicsEngine.Utils{
     public class KeyInput : IDisposable{
        private readonly UIElement targetElement;

        private readonly HashSet<Key> pressedKeys = [];

        public KeyInput(UIElement targetElement){
            if (targetElement == null) throw new ArgumentNullException(nameof(targetElement),"対象のUI要素がnullです");

            this.targetElement = targetElement;
            this.targetElement.KeyDown += OnKeyDown;
            this.targetElement.KeyUp += OnKeyUp;
        }

        public bool IsKeyDown(Key key){
            return this.pressedKeys.Contains(key);
        }

        private void OnKeyDown(object sender, KeyEventArgs e){
            if (!e.IsRepeat){
                this.pressedKeys.Add(e.Key);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e){
            this.pressedKeys.Remove(e.Key);
        }

        public void Dispose(){
            if (this.targetElement != null){
                this.targetElement.KeyDown -= OnKeyDown;
                this.targetElement.KeyUp -= OnKeyUp;
            }
        }
    }
}
