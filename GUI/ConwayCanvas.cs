using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using System;
using Avalonia.Media;
using Conway.Matrix;

namespace Conway.GUI{
    /*
    ConwayCanvas es la control que muestra la simulación en tiempo real
     */
    public class ConwayCanvas : Control{

        const int TILE_HEIGHT = 10;
        const int TILE_WIDTH = 10;

        private ConwayMatrix matrix=null;
        private ConwayMatrix ready = null;
        
        private int last_x = -1;
        private int last_y = -1;

        private int desp_x = 0;
        private int desp_y = 0;

        public ConwayCanvas()
        {

        }

        public bool HasMatrix()
        {
            return this.matrix != null;
        }

        public void New()
        {
            this.matrix = new ConwayMatrix();
            this.ready = new ConwayMatrix();
        }

        public void LoadFile(string file)
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
        }

        // zoom, deslizador, velocidad, mostrar iteraciones
        public override void Render(DrawingContext context){
            if(this.ready != null)
            {
                lock(this.ready){
                    int blocks_w = (int)this.Width / TILE_WIDTH;
                    int blocks_h = (int)this.Height / TILE_HEIGHT;
                    
                    context.FillRectangle(Brushes.Black,new Rect(0,0,this.Width,this.Height));

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
                    context.FillRectangle(Brushes.Black,new Rect(0,this.Height-1,this.Width,1));
                }
            }else{
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

        public void OnClick(object sender, PointerEventArgs e)
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
        }

        public void Move(int x, int y)
        {
            desp_x += x;
            desp_y += y;
        }

        public void Iterate(){
            lock(this.matrix){
                this.matrix.Iterate();
                lock(this.ready){
                    this.ready = (ConwayMatrix)this.matrix.Clone();
                }
            }
        }
        public ISolidColorBrush RandomColor(){
            var r = new Random().Next(0,5);
            var color = new ISolidColorBrush []{Brushes.Red,Brushes.Blue,Brushes.Green,Brushes.Yellow,Brushes.Purple};
            return color[r];
        }
    }
}