using Avalonia;
using Avalonia.Controls;
using System;
using Avalonia.Media;

namespace Conway.GUI{
    public class ConwayCanvas : Control{
        public override void Render(DrawingContext context)
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
        public ISolidColorBrush RandomColor(){
            var r = new Random().Next(0,5);
            var color = new ISolidColorBrush []{Brushes.Red,Brushes.Blue,Brushes.Green,Brushes.Yellow,Brushes.Purple};
            return color[r];
        }
    }
}