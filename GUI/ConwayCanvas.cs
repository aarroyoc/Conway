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
        public override void Render(DrawingContext context){

            if (matrix==null){ //TEMPORAL
                this.matrix= new ConwayMatrix();
                this.matrix.SetSize(8,8);
                this.matrix[1,0]=true;
                this.matrix[1,1]=true;
                this.matrix[1,2]=true;
                this.Width=this.matrix.GetMatrix()[0].Count*10;
                this.Height=this.matrix.GetMatrix().Count*10;



                Console.WriteLine(this.matrix.ToString());


                this.matrix.Iterate();
                
                Console.WriteLine(this.matrix.ToString());
            }
           

              for (int y=0;y<this.matrix.GetMatrix().Count;y++){
                ;
                    for (int x=0;x<this.matrix.GetMatrix()[0].Count;x++){
                        var rect=new Rect(x*10,y*10,10,10);
                     
                     
                        if (this.matrix.GetMatrix()[y][x]==true){
                                context.FillRectangle(Brushes.Black,rect);
                            
                        }
                        else{

                            context.FillRectangle(Brushes.Red,rect);
                        }
                       
    
            
                 }
          }
        }
        /* 
        {
            int x = 0;
            int y = 0;
            for(int i=0;i<50*50;i++){
                var rect = new Rect(x*10,y*10,10,10);
                context.FillRectangle(RandomColor(),rect);
                x++;
                if(x==50){
                    y++;
                    x = 0;
                }
            }
        }
 */
    
        public ISolidColorBrush RandomColor(){
            var r = new Random().Next(0,5);
            var color = new ISolidColorBrush []{Brushes.Red,Brushes.Blue,Brushes.Green,Brushes.Yellow,Brushes.Purple};
            return color[r];
        }
    }
}