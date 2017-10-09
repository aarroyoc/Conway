using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
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
        Button nuevo;
        Button up, left, down, right;
        StackPanel panel;
        ConwayCanvas conway;
        Thread thread;

        public bool ThreadAlive = false;
        private bool ClickEnabled = false;
        public MainWindow()
        {
            InitializeComponent();
            conway = new ConwayCanvas();
            conway.PointerPressed += ClickStart;
            conway.PointerMoved += ClickRenderCanvas;
            conway.PointerReleased += ClickEnd;
            panel = this.Find<StackPanel>("panel");
            panel.Children.Add(conway);
            boton = this.Find<Button>("exec");
            boton.Click += OnClick;
            nuevo = this.Find<Button>("new");
            nuevo.Click += NewPattern;
            load = this.Find<Button>("load");
            load.Click += LoadPattern;

            up = this.Find<Button>("up");
            up.Click += GoUp;
            left = this.Find<Button>("left");
            left.Click += GoLeft;
            down = this.Find<Button>("down");
            down.Click += GoDown;
            right = this.Find<Button>("right");
            right.Click += GoRight;

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

        private void ClickStart(object sender, PointerEventArgs e)
        {
            this.ClickEnabled = true;
            ClickRenderCanvas(sender,e);
        }
        private void ClickRenderCanvas(object sender, PointerEventArgs e)
        {
            if(!this.ThreadAlive && this.ClickEnabled){
                this.conway.OnClick(sender,e);
                this.Renderer.AddDirty(conway);
                this.Renderer.Dispose();
            }
        }

        private void ClickEnd(object sender, PointerEventArgs e)
        {
            this.ClickEnabled = false;
        }

        private void GoUp(object sender, RoutedEventArgs e)
        {
            conway.Move(-1,0);
            this.Renderer.AddDirty(conway);
            this.Renderer.Dispose();
        }
        private void GoDown(object sender, RoutedEventArgs e)
        {
            conway.Move(1,0);
            this.Renderer.AddDirty(conway);
            this.Renderer.Dispose();

        }
        private void GoLeft(object sender, RoutedEventArgs e)
        {
            conway.Move(0,-1);
            this.Renderer.AddDirty(conway);
            this.Renderer.Dispose();
        }
        private void GoRight(object sender, RoutedEventArgs e)
        {
            conway.Move(0,1);
            this.Renderer.AddDirty(conway);
            this.Renderer.Dispose();
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
            try{
                var file = files[0];
                conway.LoadFile(file);
                this.Renderer.AddDirty(conway);
                this.Renderer.Dispose();
            }catch(Exception){
                
            }
        }

        private void NewPattern(object sender, RoutedEventArgs e)
        {
            conway.New();
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