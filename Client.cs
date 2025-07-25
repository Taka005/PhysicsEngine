﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using PhysicsEngineCore;
using PhysicsEngineCore.Objects;
using PhysicsEngineCore.Options;
using PhysicsEngineCore.Utils;

namespace PhysicsEngine {
    public class Client(Window window, Engine engine) {
        private readonly Window window = window;
        private readonly Engine engine = engine;
        private Point? prePoint = null;
        private Point? prePrePoint = null;
        private Entity? selectedEntity = null;
        public ToolType toolType = ToolType.View;
        public ObjectType spawnType = ObjectType.Circle;
        public connectionType connectionType = connectionType.Minimum;
        public double size = 10;
        public double mass = 1;
        public double stiffness = 0.8;
        public double vecX = 0;
        public double vecY = 0;
        public Color? color;
        public double moveSpeed = 3;

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

        public void MouseMove(MouseEventArgs e, Point point) {
            point.X = this.calcPosX(point.X);
            point.Y = this.calcPosY(point.Y);

            if(this.toolType == ToolType.Move) {
                if(e.LeftButton == MouseButtonState.Pressed && this.selectedEntity != null) {

                    this.selectedEntity.velocity = Vector2.Zero;

                    this.selectedEntity.position.X = point.X;
                    this.selectedEntity.position.Y = point.Y;
                }
            }
        }

        public void MouseLeftButtonUp() {
            if(this.toolType == ToolType.Move) {
                if(this.selectedEntity != null) {
                    this.selectedEntity = null;
                }
            }
        }

        public void MouseLeftDown(Point point) {
            point.X = this.calcPosX(point.X);
            point.Y = this.calcPosY(point.Y);

            if(this.toolType == ToolType.Spawn) {
                if(this.spawnType == ObjectType.Circle) {
                    this.AddHistory();

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
                    this.AddHistory();

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
                    this.AddHistory();

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
                            this.AddHistory();

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

                            this.prePoint = null;
                        } else if(this.spawnType == ObjectType.Line) {
                            this.AddHistory();

                            LineOption lineOption = new LineOption {
                                startX = this.prePoint.Value.X,
                                startY = this.prePoint.Value.Y,
                                endX = point.X,
                                endY = point.Y,
                                width = this.size,
                                color = this.color.ToString() ?? "#F00000"
                            };

                            this.engine.SpawnGround(lineOption);

                            this.prePoint = null;
                        } else if(this.spawnType == ObjectType.Curve) {
                            if(this.prePrePoint == null) {
                                this.prePrePoint = point;
                            } else {
                                this.AddHistory();

                                CurveOption curveOption = new CurveOption {
                                    startX = this.prePoint.Value.X,
                                    startY = this.prePoint.Value.Y,
                                    middleX = this.prePrePoint.Value.X,
                                    middleY = this.prePrePoint.Value.Y,
                                    endX = point.X,
                                    endY = point.Y,
                                    width = this.size,
                                    color = this.color.ToString() ?? "#F00000"
                                };

                                this.engine.SpawnGround(curveOption);

                                this.prePoint = null;
                                this.prePrePoint = null;
                            }
                        }else if(this.spawnType == ObjectType.Booster) {
                            this.AddHistory();

                            BoosterOption boosterOption = new BoosterOption {
                                startX = this.prePoint.Value.X,
                                startY = this.prePoint.Value.Y,
                                endX = point.X,
                                endY = point.Y,
                                velocityX = this.vecX,
                                velocityY = this.vecY,
                                color = this.color.ToString() ?? "#F00000"
                            };

                            this.engine.SpawnEffect(boosterOption);

                            this.prePoint = null;
                        }
                    }
                }
            } else if(this.toolType == ToolType.Delete) {
                List<IObject> objects = this.engine.GetObjectsAt(point.X, point.Y);
                List<IGround> grounds = this.engine.GetGroundsAt(point.X, point.Y);
                List<IEffect> effects = this.engine.GetEffectsAt(point.X, point.Y);

                foreach(IObject obj in objects) {
                    this.engine.DeSpawnObject(obj.id);
                }

                foreach(IGround ground in grounds) {
                    this.engine.DeSpawnGround(ground.id);
                }

                foreach(IEffect effect in effects) {
                    this.engine.DeSpawnEffect(effect.id);
                }
            } else if(this.toolType == ToolType.Move) {
                List<Entity> entities = this.engine.GetEntitiesAt(point.X, point.Y);
                if(entities.Count > 0) {
                    Entity entity = entities[0];

                    entity.velocity = Vector2.Zero;

                    this.selectedEntity = entity;
                }
            } else if(this.toolType == ToolType.Edit) {
                List<IObject> objects = this.engine.GetObjectsAt(point.X, point.Y);
                if(objects.Count > 0) {
                    EditWindow editWindow = new EditWindow(objects[0]) {
                        Owner = this.window
                    };

                    editWindow.Show();
                }
            } else if(this.toolType == ToolType.Connection) {
                List<Entity> entities = this.engine.GetEntitiesAt(point.X, point.Y);

                if(entities.Count > 0) {
                    Entity entity = entities[0];

                    if(this.selectedEntity == null) {
                        this.selectedEntity = entity;
                    } else {
                        if(this.connectionType == connectionType.Dynamic) {
                            double distance = Vector2.Distance(this.selectedEntity.position, entity.position);

                            this.selectedEntity.connection.Add(entity, distance, this.selectedEntity.stiffness);
                            entity.connection.Add(this.selectedEntity, distance, entity.stiffness);
                        } else if(this.connectionType == connectionType.Minimum) {
                            this.selectedEntity.connection.Add(entity, entity.radius + this.selectedEntity.radius, this.selectedEntity.stiffness);
                            entity.connection.Add(this.selectedEntity, entity.radius + this.selectedEntity.radius, entity.stiffness);
                        }

                        this.selectedEntity = null;
                    }
                }
            } else if(this.toolType == ToolType.DisConnection) {
                List<Entity> entities = this.engine.GetEntitiesAt(point.X, point.Y);
                if(entities.Count > 0) {
                    Entity entity = entities[0];

                    if(this.selectedEntity == null) {
                        this.selectedEntity = entity;
                    } else {
                        this.selectedEntity.connection.Remove(entity.id);
                        entity.connection.Remove(this.selectedEntity.id);
                        this.selectedEntity = null;
                    }
                }
            }
        }

        public void setTool(string toolType) {
            this.prePoint = null;
            this.prePrePoint = null;
            this.selectedEntity = null;

            if(toolType == "閲覧") {
                this.toolType = ToolType.View;
            } else if(toolType == "生成") {
                this.toolType = ToolType.Spawn;
            } else if(toolType == "削除") {
                this.toolType = ToolType.Delete;
            } else if(toolType == "編集") {
                this.toolType = ToolType.Edit;
            } else if(toolType == "移動") {
                this.toolType = ToolType.Move;
            } else if(toolType == "画面移動") {
                this.toolType = ToolType.ScreenMove;
            } else if(toolType == "接続") {
                this.toolType = ToolType.Connection;
            } else if(toolType == "接続解除") {
                this.toolType = ToolType.DisConnection;
            }
        }

        public void setSpawnType(string objectType) {
            if(objectType == "円") {
                this.spawnType = ObjectType.Circle;
            } else if(objectType == "四角") {
                this.spawnType = ObjectType.Square;
            } else if(objectType == "三角") {
                this.spawnType = ObjectType.Triangle;
            } else if(objectType == "ロープ") {
                this.spawnType = ObjectType.Rope;
            } else if(objectType == "直線") {
                this.spawnType = ObjectType.Line;
            } else if(objectType == "曲線") {
                this.spawnType = ObjectType.Curve;
            }else if(objectType == "ブースター") {
                this.spawnType = ObjectType.Booster;
            }
        }

        public double calcPosX(double value) {
            value -= this.engine.render.offsetX;
            value /= this.engine.render.scale;

            return value;
        }

        public double calcPosY(double value) {
            value -= this.engine.render.offsetY;
            value /= this.engine.render.scale;

            return value;
        }
    }

    public enum ToolType {
        View,
        Spawn,
        Delete,
        Move,
        Edit,
        ScreenMove,
        Connection,
        DisConnection
    };

    public enum ObjectType {
        Circle,
        Square,
        Triangle,
        Rope,
        Line,
        Curve,
        Booster
    }

    public enum connectionType {
        Dynamic,
        Minimum
    }
}
