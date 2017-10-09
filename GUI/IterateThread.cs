using Avalonia.Threading;
using System;
using System.Threading;
namespace Conway.GUI {
    class IterateThread {
        ConwayCanvas canvas;
        MainWindow window;
        int speed;
        public IterateThread(ConwayCanvas canvas,MainWindow window,int speed)
        {
            this.canvas = canvas;
            this.window = window;
            this.speed=speed;
            
        }
        public void Iterate()
        {
            while(window.ThreadAlive){
                canvas.Iterate();
                window.Renderer.Dispose();
                window.Renderer.AddDirty(canvas);
                Dispatcher.UIThread.InvokeAsync(new Action(
                    delegate()
                    {
                        window.iterations.Text =$"Iteraciones: {canvas.Iterations}";
                    }
                ));
                if(this.speed != 0)
                    Thread.Sleep(this.speed*50);
            }
        }
    }
}