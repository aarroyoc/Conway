/* Arroyo Calle, Adrián
Bazaco Velasco, Daniel*/
using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using System;
using Avalonia.Media;
using Conway.Matrix;
using Conway.File;
using Quadtree;


namespace Conway.GUI{
    /*
    ConwayCanvas es la control que muestra la simulación en tiempo real
     */
    public class ConwayCanvas : Control{

        const int TILE_HEIGHT = 10;
        const int TILE_WIDTH = 10;

 


        public Cuadrante newMatrix=null;
        private long iterations=0;
        private Boolean tooManyIterations = false;
        private Boolean tooManyCells = false;
       
        private int last_x = -1;
        private int last_y = -1;

        private long desp_x = 0;

       
        private long desp_y = 0;

        public ConwayCanvas()
        {

        }

        public void SaveFile(string file)
        {
            var vaca = new Vaca();
            var conway = new ConwayMatrix(newMatrix.GetMatrix());
            var final = conway.GetFinalResult();

            vaca.ConwayMatrix = final.LimitedMatrix;
            vaca.Save(file);
        }


       
        public void LoadFileCuadrante(string file)
        {
            bool[][] tempMatrix=null;
            try
            {
              
                tempMatrix = new File.Rle(file).GetMatrix();
            }
            catch (Exception)
            {
                try
                {
                    
                    tempMatrix = new File.Vaca(file).GetMatrix();
                }
                catch (Exception)
                {
                    Console.WriteLine("Ha habido un error al cargar el archivo"); //TODO: Cambiar por otra excepción o mensaje
                } 
            };

            Cuadrante temp = Cuadrante.crear(tempMatrix);

            this.tooManyCells = false;
            this.iterations = 0;
            this.tooManyIterations = false;
            this.newMatrix = temp;
            desp_x = 0;
            desp_y = 0;
      
          
        }
      
        public override void Render(DrawingContext context){




           
            if(this.newMatrix != null)
            {
                //lock(this.ready){
                int blocks_w = (int)this.Width / TILE_WIDTH;
                int blocks_h = (int)this.Height / TILE_HEIGHT;

              
                   

              
             
               
                dibujaCuadrante(context, 0,0, this.newMatrix,this.desp_x,this.desp_x+2*blocks_h,desp_y,desp_y+2*blocks_w,-desp_x,-desp_y);
               
                for (int i = 0; i <blocks_w; i++)
                {
                    var rect = new Rect(i*TILE_WIDTH, 0, 1, this.Height);
                    context.FillRectangle(Brushes.Black, rect);
                }
                for (int i = 0; i < blocks_h; i++)
                {
                    var rect = new Rect(0, i*TILE_HEIGHT, this.Width, 1);
                    context.FillRectangle(Brushes.Black, rect);
                }
               
            }
            else{
                var title = new FormattedText{
                    Typeface = new Typeface(null,20),   
                    Text = "¡Bienvenido a Conway!"
                };
                var width = title.Measure().Width;
                var height = title.Measure().Height;
                var point = new Point(this.Width/2 - width/2,this.Height/3);
                context.DrawText(Brushes.Black,point,title);

                var text = new FormattedText{
                    Typeface = new Typeface(null,14),
                    Text = "Selecciona un patrón para dar comienzo al juego de la vida"
                };
                var w = text.Measure().Width;
                var h = text.Measure().Height;
                var p = new Point(this.Width/2 - w/2,this.Height/2 + 40);
                context.DrawText(Brushes.Black,p,text);

            }
        }
       
        internal void IterateEtapa4()
        {

           
            //newMatrix.print();
                while (!(newMatrix.isCentrado() && newMatrix.getCuadranteCentral().isCentrado()))
            {
                Console.WriteLine("SE esta expandiendo la matriz");
                newMatrix = newMatrix.expandir();
                desp_x += (long)Math.Pow(2, newMatrix.nivel - 2);
                desp_y += (long)Math.Pow(2, newMatrix.nivel - 2);
                Console.WriteLine($"El nuevo desplazamiento es: {desp_x}. Tamaño del cuadrante: {newMatrix.nivel}");
               // newMatrix.print();
            }
            long pow = (long)Math.Pow(2, newMatrix.nivel - 2);
            desp_x -= pow; //Baja un nivel al generar, el desplazamiento disminuye
            desp_y -= pow;

            this.iterations += pow;
            if (this.iterations < 0)
            {
                this.tooManyIterations = true;
            }
            if (this.newMatrix.celdasVivas < 0)
            {
                this.tooManyCells = true;
            }
            newMatrix = newMatrix.generacionEtapa4();
           
        }
        internal void Iterate()
        {
            while (!(newMatrix.isCentrado() && newMatrix.getCuadranteCentral().isCentrado()))
            {
              
                newMatrix = newMatrix.expandir();
                desp_x += (long)Math.Pow(2, newMatrix.nivel - 2);
                desp_y += (long)Math.Pow(2, newMatrix.nivel - 2);
              
                // newMatrix.print();
            }
            long pow = (long)Math.Pow(2, newMatrix.nivel - 2);
            desp_x -= pow; //Baja un nivel al generar, el desplazamiento disminuye
            desp_y -= pow;

            this.iterations += 1;
            if (this.iterations < 0)
            {
                this.tooManyIterations = true;
            }
            newMatrix = newMatrix.generacion();
           
        }

        internal bool HasMatrix()
        {
            return newMatrix != null;
        }

        private void dibujaCuadrante(DrawingContext context,long x,long y,Cuadrante cuadrante,long filaIni,long filaEnd, long columIni, long columEnd,long origenX,long origenY)
        {
            /** X e Y se inicializan siempre a 0. Se usan solo durante la recursión
             * 
             * filaIni,filaEnd,columIni y columEnd sirven para delimitar la parte del cuadrante que se imprime. No es muy preciso
             * pero funciona.
             * 
             * **/
            if (cuadrante.celdasVivas == 0)
            {
                return;
            }
            long tamanoCuadrante =(long) Math.Pow(2, cuadrante.nivel-1); //Al hacer -1 cojo el tamaño del cuadrante de nivel inferior
            if (cuadrante.nivel == 0)
            {
                if (cuadrante.celdasVivas ==1 )
                {
                    var rect = new Rect((origenX + x) * TILE_WIDTH + 1, (origenY + y) * TILE_HEIGHT + 1, TILE_WIDTH - 1, TILE_HEIGHT - 1);
                    context.FillRectangle(Brushes.Black, rect);

                }
               
            }
            else
            {
                
                if (!(x + 2*tamanoCuadrante < filaIni) && (x<=filaEnd+tamanoCuadrante) && !(y + 2*tamanoCuadrante < columIni) && (y <= columEnd+tamanoCuadrante))
                { //El 2* tamanoCuadrante y el  x<=filaEnd+ "tamanoCuadrante" es para dejar una cota de error.
                    dibujaCuadrante(context, x, y, cuadrante.nw, filaIni, filaEnd, columIni, columEnd,origenX,origenY);
                    dibujaCuadrante(context, x + tamanoCuadrante, y, cuadrante.ne, filaIni, filaEnd, columIni, columEnd, origenX, origenY);
                    dibujaCuadrante(context, x, y + tamanoCuadrante, cuadrante.sw, filaIni, filaEnd, columIni, columEnd, origenX, origenY);
                    dibujaCuadrante(context, x + tamanoCuadrante, y + tamanoCuadrante, cuadrante.se, filaIni, filaEnd, columIni, columEnd, origenX, origenY);
                }
        
            }
            


        }

        internal void NewPattern()
        {
            this.newMatrix = Cuadrante.crearVacio(6);
            this.iterations = 0;
            this.tooManyIterations = false;
            this.tooManyCells = false;
        }

        public void OnClick(object sender, PointerEventArgs e)
        {
            if (this.newMatrix == null)
                return;
            int x = (int)e.GetPosition(this).X / TILE_WIDTH;
            int y = (int)e.GetPosition(this).Y / TILE_HEIGHT;
            
            if(x == last_x && y == last_y)
                return;

            long tamanoCuadrante = (long)Math.Pow(2, this.newMatrix.nivel);
            Console.WriteLine($"Coordenadas de donde se ha pulsado: x={this.desp_x + x},y={this.desp_y + y}");
            
            while (this.desp_x+x<0 || this.desp_y < 0 ||this.desp_x+x>tamanoCuadrante || this.desp_y+y>tamanoCuadrante)
            {
                this.newMatrix = this.newMatrix.expandir();
                desp_x += (long)Math.Pow(2, newMatrix.nivel - 2);
                desp_y += (long)Math.Pow(2, newMatrix.nivel - 2);
                tamanoCuadrante *= 2;
                Console.WriteLine($"Esta expandiendo muuucho. {this.desp_x+x} {this.desp_y+y}. TamanoCuadrante: {tamanoCuadrante}");

            }

            this.newMatrix = this.newMatrix.setPixelInverso(this.desp_x+x, this.desp_y+y);
           
            last_x = x;
            last_y = y;
        }

        public void Move(int x, int y)
        {
            desp_x += x;
            desp_y += y;
        }

        public long Iterations { get{
                if (tooManyIterations == true)
                {
                    return -1;
                }
            return this.iterations; 
        }
        }

        public long LiveCells {
            get{
              if (tooManyCells == true)
                {
                    return -1;
                }
                return newMatrix.celdasVivas;
            }
        }

        public ISolidColorBrush RandomColor(){
            var r = new Random().Next(0,5);
            var color = new ISolidColorBrush []{Brushes.Red,Brushes.Blue,Brushes.Green,Brushes.Yellow,Brushes.Purple};
            return color[r];
        }
    }
}