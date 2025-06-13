using System.Windows;

namespace PhysicsEngineGUI.Utils {
    public class ClickPoint {
        private readonly List<Point> points = [];

        public void Add(Point point) {
            this.points.Add(point);
        }

        public Point? Get(int index) {
            if(index < 0 || index >= this.points.Count) return null;

            return this.points[index];
        }

        public Point? First() {
            return this.Get(0);
        }

        public Point? Second() {
            return this.Get(1);
        }

        public void Clear() {
            this.points.Clear();
        }
    }
}
