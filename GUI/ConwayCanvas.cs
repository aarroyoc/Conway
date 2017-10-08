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

        public ConwayCanvas()
        {

        }

        public bool HasMatrix()
        {
            return this.matrix != null;
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
                            if(this.ready[j+this.ready.OffsetX,i+this.ready.OffsetY]){
                                
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

        public void OnClick(object sender, PointerPressedEventArgs e)
        {
            int x = (int)e.GetPosition(this).X;
            int y = (int)e.GetPosition(this).Y;
            lock(this.matrix){
                this.matrix[y/10+this.matrix.OffsetX,x/10+this.matrix.OffsetY] = !this.matrix[y/10+this.matrix.OffsetX,x/10+this.matrix.OffsetY];
            }
            lock(this.ready){
                this.ready[y/10+this.ready.OffsetX,x/10+this.ready.OffsetY] = !this.ready[y/10+this.ready.OffsetX,x/10+this.ready.OffsetY];
            }
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