using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading;

/*
Conway.GUI contiene los elementos visuales
 */
namespace Conway.GUI
{
    /*
    Ventana principal
     */
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


        private void OnClick(object sender, RoutedEventArgs e)
        {
            
            this.Renderer.Dispose();
            this.Renderer.AddDirty(conway);
            
            this.Renderer.DrawFps = true;

            timer = new Timer((a)=>{
                conway.Iterate();
                this.Renderer.Dispose();
                this.Renderer.AddDirty(conway);
            },null,0,1000/10);
        }
    }
}