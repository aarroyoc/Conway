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

 


        private Cuadrante newMatrix=null;

        private int last_x = -1;
        private int last_y = -1;

        private long desp_x = 0;
        private long desp_y = 0;

        public ConwayCanvas()
        {

        }

   

        public void New()
        {
           // this.matrix = new ConwayMatrix();
            //this.ready = new ConwayMatrix();
        }
        public void LoadFileCuadrante(string file)
        {
            bool[][] tempMatrix=null;
            //try
            {
                tempMatrix = new File.Vaca(file).GetMatrix();
                //tempMatrix = new File.Rle(file).GetMatrix();
            }
            //catch (Exception)
            {
                /*try
                {
                    tempMatrix = new File.Vaca(file).GetMatrix();
                }
                catch (Exception)
                {
                    Console.WriteLine("Adivina, si, algo ha ido mal");
                } */
            };

            Cuadrante temp = Cuadrante.crear(tempMatrix);

             if(temp == null)
            {
                Console.WriteLine("Ha habido un error al cargar el fichero");
            }
            else
            {
                Console.WriteLine("Todo ha ido bien, new Matrix deberia tener la nueva referencia");
            }
            this.newMatrix = temp;
            temp.print();
          
        }
       /* public void LoadFile(string file)
        {
            ConwayMatrix matrix = null;
            try{
                matrix = new File.Rle(file).ConwayMatrix;
            }catch(Exception){
                try{
                    matrix = new File.Vaca(file).ConwayMatrix;
                }catch(Exception){

                }
            };
            this.matrix = matrix;
            this.ready = (ConwayMatrix)this.matrix.Clone();
        } */

        /*public void SaveFile(string file)
        {
            var vaca = new Vaca();
            vaca.ConwayMatrix = matrix;
            vaca.Save(file);
        }
        */
        // zoom, deslizador, velocidad, mostrar iteraciones
        public override void Render(DrawingContext context){




            Console.WriteLine("Ha empezado a dibujar el cuadrado");
            if(this.newMatrix != null)
            {
                //lock(this.ready){
                int blocks_w = (int)this.Width / TILE_WIDTH;
                int blocks_h = (int)this.Height / TILE_HEIGHT;

              
                   

              
                Console.WriteLine("Ha empezado a dibujar el cuadrado");
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
                Console.WriteLine("Ha TERMINADO  DE dibujar el cuadrado");

                //context.FillRectangle(Brushes.Black, new Rect(this.Width - 1, 0, 1, this.Height));
                //.FillRectangle(Brushes.Black, new Rect(0, this.Height - 1, this.Width, 1)); 
                /*
                for(var i=0;i<blocks_w;i++){
                    for(var j=0;j<blocks_h;j++){
                        if(this.ready[j+this.ready.OffsetX+desp_x,i+this.ready.OffsetY+desp_y]){

                        }else{
                            var rect = new Rect(i*TILE_WIDTH+1,j*TILE_HEIGHT+1,TILE_WIDTH-1,TILE_HEIGHT-1);
                            context.FillRectangle(Brushes.White,rect);
                        }
                    }
                }
                context.FillRectangle(Brushes.Black,new Rect(this.Width-1,0,1,this.Height));
                context.FillRectangle(Brushes.Black,new Rect(0,this.Height-1,this.Width,1));*/
                //}
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
        internal void expandir()
        {
            newMatrix.print();
            newMatrix = newMatrix.expandir();
            desp_x += (long)Math.Pow(2, newMatrix.nivel - 2);
            desp_y += (long)Math.Pow(2, newMatrix.nivel - 2);
            newMatrix.print();
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
            desp_x -= (long)Math.Pow(2, newMatrix.nivel - 2); //Baja un nivel al generar, el desplazamiento disminuye
            desp_y -= (long)Math.Pow(2, newMatrix.nivel - 2);
            
            newMatrix = newMatrix.generacionEtapa4();
            Console.WriteLine($"Se ha genrado, el nuevo desplazamiento es: {desp_x}. Tamaño del cuadrante {newMatrix.nivel}");
        }

        internal bool HasMatrix()
        {
            return newMatrix != null;
        }

        private void dibujaCuadrante(DrawingContext context,long x,long y,Cuadrante cuadrante,long filaIni,long filaEnd, long columIni, long columEnd,long origenX,long origenY)
        {
            /** X e Y se inicializan siempre a 0. Se usan solo durante la recursión **/
            long tamanoCuadrante =(long) Math.Pow(2, cuadrante.nivel-1); //Al hacer -1 cojo el tamaño del cuadrante de nivel inferior
            if (cuadrante.nivel == 0)
            {
                if (cuadrante.celdasVivas == 0)
                {
                    
                  
                }
                else
                {
                    var rect = new Rect((origenX + x) * TILE_WIDTH + 1, (origenY + y) * TILE_HEIGHT + 1, TILE_WIDTH - 1, TILE_HEIGHT - 1);
                    context.FillRectangle(Brushes.Black, rect);
         
                }
            }
            else
            {
                if (cuadrante.nivel == newMatrix.nivel)
                {

               
                Console.WriteLine($"filaIni: {filaIni}, filaEnd:{filaEnd}, columIni:{columIni}, columEnd{columEnd}");
                Console.WriteLine($"Tamaño del cuadrante inferior: {tamanoCuadrante}. X={x} Y={y}");
                    Console.WriteLine($"Tamaño de ESTE CUADRANTE: {Math.Pow(2, cuadrante.nivel)}"); 
                Console.WriteLine($"y + tamanoCuadrante > filaEnd: {y + tamanoCuadrante > filaEnd}");
                Console.WriteLine($"y + tamanoCuadrante > filaIni) {y + tamanoCuadrante > filaIni}");
                Console.WriteLine($"x + tamanoCuadrante > columEnd: {x + tamanoCuadrante > columEnd}");
                Console.WriteLine($"x + tamanoCuadrante > columIni) {x + tamanoCuadrante > columIni}");
                }
                if (!(x + 2*tamanoCuadrante < filaIni) && (x<=filaEnd+tamanoCuadrante) && !(y + 2*tamanoCuadrante < columIni) && (y <= columEnd+tamanoCuadrante))
                { //El 2* tamanoCuadrante y el  x<=filaEnd+ "tamanoCuadrante" es para dejar una cota de error.
                    dibujaCuadrante(context, x, y, cuadrante.nw, filaIni, filaEnd, columIni, columEnd,origenX,origenY);
                    dibujaCuadrante(context, x + tamanoCuadrante, y, cuadrante.ne, filaIni, filaEnd, columIni, columEnd, origenX, origenY);
                    dibujaCuadrante(context, x, y + tamanoCuadrante, cuadrante.sw, filaIni, filaEnd, columIni, columEnd, origenX, origenY);
                    dibujaCuadrante(context, x + tamanoCuadrante, y + tamanoCuadrante, cuadrante.se, filaIni, filaEnd, columIni, columEnd, origenX, origenY);
                }
               
                //PROBLEMA: !(x + tamanoCuadrante > columEnd). Los cuadrantes se escriben en nivel descendiente, por tanto al descartar
                // el nivel superior, perdemos los inferiores, y algunos de los inferiores SI deberían imprimirse
                if (!(x  > columEnd) && x + tamanoCuadrante >= columIni)
                {
                    
                }
                if (!(y  > filaEnd) && y + tamanoCuadrante>= filaIni)
                {
                   
                }
                if(!(x  > columEnd) && x + tamanoCuadrante > columIni && !(y > filaEnd) && y + tamanoCuadrante >= filaIni) { 
                  
                }
            }
            


        }
        /*public void OnClick(object sender, PointerEventArgs e)
        {
            if(this.matrix == null || this.ready == null)
                return;
            int x = (int)e.GetPosition(this).X / TILE_WIDTH;
            int y = (int)e.GetPosition(this).Y / TILE_HEIGHT;
            
            if(x == last_x && y == last_y)
                return;
            lock(this.matrix){
                this.matrix[y+this.matrix.OffsetX+desp_x,x+this.matrix.OffsetY+desp_y] = !this.matrix[y+this.matrix.OffsetX+desp_x,x+this.matrix.OffsetY+desp_y];
            }
            lock(this.ready){
                this.ready[y+this.ready.OffsetX+desp_x,x+this.ready.OffsetY+desp_y] = !this.ready[y+this.ready.OffsetX+desp_x,x+this.ready.OffsetY+desp_y];
            }
            last_x = x;
            last_y = y;
        }*/

        public void Move(int x, int y)
        {
            desp_x += x;
            desp_y += y;
        }

        public int Iterations { get{
            return 4; //TODO
        }
        }

        public long LiveCells {
            get{
              
                return newMatrix.celdasVivas;
            }
        }

        /*public void Iterate(){
            lock(this.matrix){
                this.matrix.Iterate();
                lock(this.ready){
                    this.ready = (ConwayMatrix)this.matrix.Clone();
                }
            }
        }*/
        public ISolidColorBrush RandomColor(){
            var r = new Random().Next(0,5);
            var color = new ISolidColorBrush []{Brushes.Red,Brushes.Blue,Brushes.Green,Brushes.Yellow,Brushes.Purple};
            return color[r];
        }
    }
}