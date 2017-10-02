using Avalonia;
using Avalonia.Controls;
using System;
using Avalonia.Media;
using Conway.Matrix;
namespace Conway.GUI{
    /*
    ConwayCanvas es la control que muestra la simulación en tiempo real
     */
    public class ConwayCanvas : Control{

        private  ConwayMatrix matrix=null;

        public ConwayCanvas()
        {
            this.matrix = new File.Rle(@"Patterns\biloaf3.rle").ConwayMatrix;
        }

        const int TILE_HEIGHT = 10;
        const int TILE_WIDTH = 10;
        // zoom, deslizador, velocidad
        public override void Render(DrawingContext context){
            int blocks_w = (int)this.Width / TILE_WIDTH;
            int blocks_h = (int)this.Height / TILE_HEIGHT;
            
            context.FillRectangle(Brushes.Black,new Rect(0,0,this.Width,this.Height));

            for(var i=0;i<blocks_w;i++){
                for(var j=0;j<blocks_h;j++){
                    if(this.matrix[j+this.matrix.OffsetX-(blocks_w/3),i+this.matrix.OffsetY-(blocks_h/3)]){
                        
                    }else{
                        var rect = new Rect(i*TILE_WIDTH+1,j*TILE_HEIGHT+1,TILE_WIDTH-1,TILE_HEIGHT-1);
                        context.FillRectangle(Brushes.White,rect);
                    }
                }
            }

        }
    
        public void Iterate(){
            this.matrix.Iterate();
        }
        public ISolidColorBrush RandomColor(){
            var r = new Random().Next(0,5);
            var color = new ISolidColorBrush []{Brushes.Red,Brushes.Blue,Brushes.Green,Brushes.Yellow,Brushes.Purple};
            return color[r];
        }
    }
}