using PhysicsEngineCore;

namespace PhysicsEngineGUI {
    public class Client(Engine engine){
        private readonly Engine engine = engine;
        public ToolType toolType = ToolType.View;
        public double size = 10;
        public double mass = 1;
        public double stiffness = 0.8;
        public double vecX = 0;
        public double vecY = 0;

        private List<string> history = [];

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
    }

    public enum ToolType {
        View,
        Spawn,
        Delete,
        Move,
        ScreenMove,
        Connection
    };
}
