/* Arroyo Calle, Adrián
Bazaco Velasco, Daniel*/
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
        Button next;
        Button load;
        Button nuevo;

        Button save;
        Button up, left, down, right;
        public TextBlock iterations;
        public TextBlock alive;
        Button iterButton;
        TextBox iterBox;
        public TextBlock iterBlock;
        StackPanel panel;
        ConwayCanvas conway;
        Thread thread;

        DropDown speedSelector;

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
            next = this.Find<Button>("next");
            next.Click += NextStep;
            nuevo = this.Find<Button>("new");
            nuevo.Click += NewPattern;
            load = this.Find<Button>("load");
            load.Click += LoadPattern;
            save = this.Find<Button>("save");
            save.Click += SavePattern;

            this.speedSelector=this.Find<DropDown>("speedSelector");
       
            up = this.Find<Button>("up");
            up.Click += GoUp;
        
            left = this.Find<Button>("left");
            left.Click += GoLeft;
            down = this.Find<Button>("down");
            down.Click += GoDown;
            right = this.Find<Button>("right");
            right.Click += GoRight;

            iterations = this.Find<TextBlock>("iterations");
            alive = this.Find<TextBlock>("alive");

            iterButton = this.Find<Button>("iter-n-do");
            iterButton.Click += IterN;
            iterBox = this.Find<TextBox>("iter-n");
            iterBox.TextInput += TextIter;
            iterBlock = this.Find<TextBlock>("iter-time");

            this.KeyDown += MoveKey;
            this.Closed += OnClosed;

            //conway.LoadFile("Patterns/3enginecordership.rle"); //Temporal, para que pueda ejeutarlo en Fedora
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
            this.Renderer.AddDirty(conway);

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

                this.ThreadAlive = true;
                int selectedSpeed=Int32.Parse((String)this.speedSelector.GetValue(DropDown.SelectionBoxItemProperty));
                var iterate = new IterateThread(conway,this,selectedSpeed);
                thread = new Thread(iterate.Iterate);
                thread.Start();
                boton.Content = "Parar";
            }
        }
      
        private void NextStep(object sender, RoutedEventArgs e)
        {
            if(!this.ThreadAlive && conway.HasMatrix()){
                conway.IterateEtapa4();
                long iterationNum = conway.Iterations;
                if (iterationNum == -1){
                    iterations.Text = $"Iteraciones: Desbordamiento";
                }
                else { 
                iterations.Text = $"Iteraciones: {iterationNum}";
                }
                long celulasVivas = conway.LiveCells;
                if (celulasVivas == -1)
                {
                    alive.Text = $"Celdas vivas: Desbordamiento";
                }
                else {
                 alive.Text = $"Celdas vivas: {conway.LiveCells}";
                }

                this.Renderer.AddDirty(conway);
                this.Renderer.Dispose();
            }
        }
        
        private void IterN(object sender, RoutedEventArgs e)
        {
            if(!this.ThreadAlive && conway.HasMatrix())
            {
                int n = 0;
                if(Int32.TryParse(iterBox.Text, out n)){
                    this.ThreadAlive = true;
                    int selectedSpeed=Int32.Parse((String)this.speedSelector.GetValue(DropDown.SelectionBoxItemProperty));
                    var iterate = new IterateThread(conway,this,selectedSpeed,n);
                    thread = new Thread(iterate.Iterate);
                    thread.Start();
                }

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
                alive.Text = $"Celdas vivas: {conway.LiveCells}";
                this.Renderer.AddDirty(conway);
                this.Renderer.Dispose();
            }
        }

        private void ClickEnd(object sender, PointerEventArgs e)
        {
            this.ClickEnabled = false;
        }
          
        private void MoveKey(object sender, KeyEventArgs e)
        {
            switch(e.Key){
                case Key.Up:
                case Key.W: GoUp(null,null);break;
                case Key.Down:
                case Key.S: GoDown(null,null);break;
                case Key.Left:
                case Key.A: GoLeft(null,null);break;
                case Key.Right:
                case Key.D: GoRight(null,null);break;
            }
        }

        private void GoUp(object sender, RoutedEventArgs e)
        {
            conway.Move(0,-1);
            this.Renderer.AddDirty(conway);
            this.Renderer.Dispose();
        }
        private void GoDown(object sender, RoutedEventArgs e)
        {
            conway.Move(0,1);
            this.Renderer.AddDirty(conway);
            this.Renderer.Dispose();

        }
        private void GoLeft(object sender, RoutedEventArgs e)
        {
            conway.Move(-1,0);
            this.Renderer.AddDirty(conway);
            this.Renderer.Dispose();
        }
        private void GoRight(object sender, RoutedEventArgs e)
        {
            conway.Move(1,0);
            this.Renderer.AddDirty(conway);
            this.Renderer.Dispose();
        }

        private async void LoadPattern(object sender, RoutedEventArgs e)
        {

            if (this.ThreadAlive || !conway.HasMatrix()) //Si le das a cargar patrón y  se estaba ejecutando el oto, se para.
            {
                this.ThreadAlive = false;
                if (thread != null)
                    thread.Join();
                boton.Content = "Ejecutar"; 
            }



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
              conway.LoadFileCuadrante(file);
               iterations.Text =$"Iteraciones: {conway.Iterations}";
                alive.Text = $"Celdas vivas: {conway.LiveCells}";
                Console.WriteLine("LoadPattern ha concluido");
                this.Renderer.AddDirty(conway);
                this.Renderer.Dispose();
            }catch(Exception ){
              
            }
        }

        private async void SavePattern(object sender, RoutedEventArgs e)
        {
            if(!conway.HasMatrix())
                return;
            var filter = new FileDialogFilter{
                Extensions = new List<string>() {"vaca"},
                Name = "Patrones",
            };
            var dlg = new SaveFileDialog{
                Title = "Guardar patrón",
                Filters = new List<FileDialogFilter>(){filter},
            };
            var file = await dlg.ShowAsync(this);
            if(String.IsNullOrEmpty(file))
                return;
            if(!file.EndsWith(".vaca")){
                file += ".vaca";
            }
            conway.SaveFile(file);
            

        } 

        private void NewPattern(object sender, RoutedEventArgs e)
        {
            conway.NewPattern();
            iterations.Text = $"Iteraciones: {conway.Iterations}";
            alive.Text = $"Celdas vivas: {conway.LiveCells}";

        }

        private void OnClosed(object sender, EventArgs e)
        {
            this.ThreadAlive = false;
            if(thread!=null)
                thread.Join();
        }

        private void TextIter(object sender, TextInputEventArgs e)
        {
            try{
                var n = Int32.Parse(iterBox.Text);
                iterButton.Content = $"Iterar {n} veces";
                iterButton.IsEnabled = true;
            }catch(Exception){
                iterButton.Content = "(Inserta un número)";
                iterButton.IsEnabled = false;
            }
        }
    }
}