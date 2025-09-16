using PhysicsEngineCore;
using PhysicsEngineCore.Objects;
using PhysicsEngineCore.Objects.Interfaces;
using PhysicsEngineCore.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace PhysicsEngine{
    public partial class EffectEditWindow : Window{
        private readonly Engine engine;
        private readonly IEffect effect;

        public EffectEditWindow(Engine engine, IEffect effect){
            this.engine = engine;
            this.effect = effect;

            InitializeComponent();

            foreach (PhysicsEngineCore.Utils.Image image in this.engine.assets.images){
                this.ImageSelect.Items.Add(image.filename);
            }

            if (effect is Booster booster){
                this.StartXSlider.Value = booster.start.X;
                this.StartYSlider.Value = booster.start.Y;
                this.EndXSlider.Value = booster.end.X;
                this.EndYSlider.Value = booster.end.Y;
                this.VelXSlider.Value = booster.velocity.X;
                this.VelYSlider.Value = booster.velocity.Y;
            }

            this.EffectId.Text = this.effect.id;
            this.EffectColor.SelectedColor = ParseColor.StringToColor(this.effect.color);
            this.ImageSelect.SelectedItem = this.effect.image?.filename ?? "なし";

            this.StartXSlider.ValueChanged += StartXSlider_Change;
            this.StartYSlider.ValueChanged += StartYSlider_Change;
            this.EndXSlider.ValueChanged += EndXSlider_Change;
            this.EndYSlider.ValueChanged += EndYSlider_Change;
            this.VelXSlider.ValueChanged += VelXSlider_Change;
            this.VelYSlider.ValueChanged += VelYSlider_Change;
            this.EffectColor.SelectedColorChanged += ColorPicker_Change;
            this.ImageSelect.SelectionChanged += Image_Change;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e){
            if (this.effect is Booster booster){
                this.StartXSlider.Value = booster.start.X;
                this.StartYSlider.Value = booster.start.Y;
                this.EndXSlider.Value = booster.end.X;
                this.EndYSlider.Value = booster.end.Y;
                this.VelXSlider.Value = booster.velocity.X;
                this.VelYSlider.Value = booster.velocity.Y;
            }

            this.EffectId.Text = this.effect.id;
            this.EffectColor.SelectedColor = ParseColor.StringToColor(this.effect.color);
        }

        private void StartXSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e){
            if (sender is Slider slider){
                if (this.effect is Booster booster){
                    booster.start = new Vector2(slider.Value, booster.start.Y);
                }
            }
        }

        private void StartYSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e){
            if (sender is Slider slider){
                if (this.effect is Booster booster){
                    booster.start = new Vector2(booster.start.X, slider.Value);
                }
            }
        }

        private void EndXSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e){
            if (sender is Slider slider){
                if (this.effect is Booster booster){
                    booster.end = new Vector2(slider.Value, booster.end.Y);
                }
            }
        }

        private void EndYSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e){
            if (sender is Slider slider){
                if (this.effect is Booster booster){
                    booster.end = new Vector2(booster.end.X, slider.Value);
                }
            }
        }

        private void VelXSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e){
            if (sender is Slider slider){
                if (this.effect is Booster booster){
                    booster.velocity = new Vector2(slider.Value, booster.velocity.Y);
                }
            }
        }

        private void VelYSlider_Change(object sender, RoutedPropertyChangedEventArgs<double> e){
            if (sender is Slider slider){
                if (this.effect is Booster booster){
                    booster.velocity = new Vector2(booster.velocity.X, slider.Value);
                }
            }
        }


        private void ColorPicker_Change(object sender, RoutedPropertyChangedEventArgs<Color?> e){
            if (sender is ColorPicker picker)
            {
                this.effect.color = picker.SelectedColor.ToString() ?? "#F00000";
            }
        }

        private void Image_Change(object sender, SelectionChangedEventArgs e){
            if (sender is ComboBox combobox){
                string? selectedContent = combobox.SelectedItem as string;

                if (selectedContent == "なし"){
                    this.effect.image = null;
                }else{
                    PhysicsEngineCore.Utils.Image? image = this.engine.assets.Get(selectedContent ?? "");

                    if (image == null){
                        this.effect.image = null;
                    }else{
                        this.effect.image = image;
                    }
                }
            }
        }
    }
}
