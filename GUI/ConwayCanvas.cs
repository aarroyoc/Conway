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
                this.matrix=new File.Rle("Patterns/10cellinfinitegrowth.rle").ConwayMatrix;
                 /* 
                this.matrix= new ConwayMatrix();
                this.matrix.SetSize(3,8);
               
                this.matrix[0,0]=true;
                this.matrix[0,1]=true;
                this.matrix[0,2]=true;
                this.Width=this.matrix.Width*10;
                this.Height=this.matrix.Height*10;



                Console.WriteLine(this.matrix.ToString());


                //this.matrix.Iterate();
                
                Console.WriteLine(this.matrix.ToString());
                this.matrix.GetFinalResult();
                */
            }
           

              for (int y=0;y<this.matrix.Height;y++){
                ;
                    for (int x=0;x<this.matrix.Width;x++){
                        var rect=new Rect(x*10,y*10,10,10);
                     
                     
                        if (this.matrix[y,x]==true){
                                context.FillRectangle(Brushes.Black,rect);
                            
                        }
                        else{

                            context.FillRectangle(Brushes.Red,rect);
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