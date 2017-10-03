using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading;
using System.Threading.Tasks;

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
        Button load;
        StackPanel panel;
        ConwayCanvas conway;
        Thread thread;

        public bool ThreadAlive = false;
        public MainWindow()
        {
            InitializeComponent();
            conway = new ConwayCanvas();
            panel = this.Find<StackPanel>("panel");
            panel.Children.Add(conway);
            boton = this.Find<Button>("exec");
            boton.Click += OnClick;
            load = this.Find<Button>("load");
            load.Click += LoadPattern;
            this.Closed += OnClosed;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void HandleResized(Size size)
        {
            base.HandleResized(size);
            conway.Width = size.Width * 0.8 - 20;
            conway.Height = size.Height - 20;

        }


        private void OnClick(object sender, RoutedEventArgs e)
        {
            if(this.ThreadAlive || !conway.HasMatrix()){
                this.ThreadAlive = false;
                if(thread!=null)
                    thread.Join();
                boton.Content = "Ejecutar";
            }else{
                this.Renderer.Dispose();
                this.Renderer.AddDirty(conway);
                
                this.Renderer.DrawFps = true;

                this.ThreadAlive = true;
                var iterate = new IterateThread(conway,this);
                thread = new Thread(iterate.Iterate);
                thread.Start();
                boton.Content = "Parar";
            }

        }
        private async void LoadPattern(object sender, RoutedEventArgs e)
        {
            var filter = new FileDialogFilter{
                Extensions = new List<string>() {"rle","vaca"},
                Name = "Patrones",
            };
            var dlg = new OpenFileDialog{
                Title = "Abrir patrón",
                AllowMultiple = false,
                Filters = new List<FileDialogFilter>() {filter},
            };
            var files = await dlg.ShowAsync(this);
            var file = files[0];
            conway.LoadFile(file);
            this.Renderer.AddDirty(conway);
            this.Renderer.Dispose();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            this.ThreadAlive = false;
            if(thread!=null)
                thread.Join();
        }
    }
}