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
        Thread thread;

        public bool ThreadAlive = false;
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
            this.Closed += OnClosed;
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

            this.ThreadAlive = true;
            var iterate = new IterateThread(conway,this);
            thread = new Thread(iterate.Iterate);
            thread.Start();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            this.ThreadAlive = false;
            thread.Join();
        }
    }
}