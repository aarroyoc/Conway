using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading;

namespace Conway
{
    public class MainWindow : Window
    {
        Button boton;
        StackPanel panel;
        ConwayCanvas conway;
        Timer timer;
        public MainWindow()
        {
            InitializeComponent();
            conway = new ConwayCanvas();
            conway.Width = 500;
            conway.Height = 500;
            panel = this.Find<StackPanel>("panel");
            panel.Children.Add(conway);
            boton = this.Find<Button>("boton");
            boton.Click += OnClick;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static ISolidColorBrush RandomColor(){
            var r = new Random().Next(0,5);
            var color = new ISolidColorBrush []{Brushes.Red,Brushes.Blue,Brushes.Green,Brushes.Yellow,Brushes.Purple};
            return color[r];
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Clicado!");
            this.Renderer.DrawFps = true;

            timer = new Timer((a)=>{
                this.Renderer.Dispose();
                this.Renderer.AddDirty(conway);
            },null,0,1000/60);
        }
    }
    public class ConwayCanvas : Control{
        int x = 0;
        int y = 0;
        public override void Render(DrawingContext context)
        {
            x = 0;
            y = 0;
            for(int i=0;i<50*50;i++){
                var rect = new Rect(x*10,y*10,10,10);
                context.FillRectangle(MainWindow.RandomColor(),rect);
                x++;
                if(x==50){
                    y++;
                    x = 0;
                }
            }
        }
    }
}