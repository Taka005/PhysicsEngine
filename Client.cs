using System.Windows;
using System.Windows.Media;
using PhysicsEngineCore;
using PhysicsEngineCore.Options;

namespace PhysicsEngineGUI {
    public class Client(Engine engine){
        private readonly Engine engine = engine;
        private Point? prePoint = null;
        public ToolType toolType = ToolType.View;
        public ObjectType spawnType = ObjectType.Circle;
        public double size = 10;
        public double mass = 1;
        public double stiffness = 0.8;
        public double vecX = 0;
        public double vecY = 0;
        public Color? color;

        private List<string> history = [];

        public void AddHistory() {
            this.history.Add(this.engine.Export());
        }

        public void RestoreHistory() {
            if(this.history.Count > 0) {
                this.engine.Import(this.history.Last());
                this.history.RemoveAt(this.history.Count - 1);
            }
        }

        public void MouseLeftDown(Point point) {
            this.AddHistory();

            if(this.toolType == ToolType.Spawn) {
                if(this.spawnType == ObjectType.Circle) {
                    CircleOption circleOption = new CircleOption {
                        posX = point.X,
                        posY = point.Y,
                        mass = this.mass,
                        diameter = this.size,
                        stiffness = this.stiffness,
                        velocityX = this.vecX,
                        velocityY = this.vecY,
                        color = this.color.ToString() ?? "#F00000"
                    };

                    this.engine.SpawnObject(circleOption);
                } else if(this.spawnType == ObjectType.Square) {
                    SquareOption squareOption = new SquareOption {
                        posX = point.X,
                        posY = point.Y,
                        mass = this.mass,
                        size = this.size,
                        stiffness = this.stiffness,
                        velocityX = this.vecX,
                        velocityY = this.vecY,
                        color = this.color.ToString() ?? "#F00000"
                    };

                    this.engine.SpawnObject(squareOption);
                } else if(this.spawnType == ObjectType.Triangle) {
                    TriangleOption triangleOption = new TriangleOption {
                        posX = point.X,
                        posY = point.Y,
                        mass = this.mass,
                        size = this.size,
                        stiffness = this.stiffness,
                        velocityX = this.vecX,
                        velocityY = this.vecY,
                        color = this.color.ToString() ?? "#F00000"
                    };

                    this.engine.SpawnObject(triangleOption);
                } else {
                    if(this.prePoint == null) {
                        this.prePoint = point;
                    } else {
                        if(this.spawnType == ObjectType.Rope) {
                            RopeOption ropeOption = new RopeOption {
                                startX = this.prePoint.Value.X,
                                startY = this.prePoint.Value.Y,
                                endX = point.X,
                                endY = point.Y,
                                width = this.size,
                                mass = this.mass,
                                stiffness = this.stiffness,
                                velocityX = this.vecX,
                                velocityY = this.vecY,
                                color = this.color.ToString() ?? "#F00000"
                            };

                            this.engine.SpawnObject(ropeOption);
                        } else if(this.spawnType == ObjectType.Line) {
                            LineOption lineOption = new LineOption {
                                startX = this.prePoint.Value.X,
                                startY = this.prePoint.Value.Y,
                                endX = point.X,
                                endY = point.Y,
                                width = this.size,
                                color = this.color.ToString() ?? "#F00000"
                            };

                            this.engine.SpawnGround(lineOption);
                        }

                        this.prePoint = null;
                    }
                }
            }
        }

        public void setTool(string toolType) {
            if(toolType == "閲覧") {
                this.toolType = ToolType.View;
            } else if(toolType == "生成") {
                this.toolType = ToolType.Spawn;
            } else if(toolType == "削除") {
                this.toolType = ToolType.Delete;
            } else if(toolType == "移動") {
                this.toolType = ToolType.Move;
            } else if(toolType == "画面移動") {
                this.toolType = ToolType.ScreenMove;
            } else if(toolType == "接続") {
                this.toolType = ToolType.Connection;
            }
        }

        public void setSpawnType(string objectType){
            if(objectType == "円") {
                this.spawnType = ObjectType.Circle;
            }else if(objectType == "四角") {
                this.spawnType = ObjectType.Square;
            }else if(objectType == "三角") {
                this.spawnType = ObjectType.Triangle;
            }else if(objectType == "ロープ") {
                this.spawnType = ObjectType.Rope;
            }else if(objectType == "直線") {
                this.spawnType = ObjectType.Line;
            }else if(objectType == "曲線") {
                this.spawnType = ObjectType.Curve;
            }
        }
    }

    public enum ToolType {
        View,
        Spawn,
        Delete,
        Move,
        ScreenMove,
        Connection
    };

    public enum ObjectType {
        Circle,
        Square,
        Triangle,
        Rope,
        Line,
        Curve
    }
}
