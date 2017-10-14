using Avalonia.Threading;
using System;
using System.Threading;
using System.Diagnostics;

namespace Conway.GUI {
    class IterateThread {
        ConwayCanvas canvas;
        MainWindow window;
        int speed;
        int n;
        public IterateThread(ConwayCanvas canvas,MainWindow window,int speed,int n = -1)
        {
            this.canvas = canvas;
            this.window = window;
            this.speed=speed;
            this.n = n;
            
        }
        public void Iterate()
        {
            var sw = Stopwatch.StartNew();
            while(window.ThreadAlive){
                canvas.Iterate();
                window.Renderer.Dispose();
                window.Renderer.AddDirty(canvas);
                Dispatcher.UIThread.InvokeAsync(new Action(
                    delegate()
                    {
                        window.iterations.Text =$"Iteraciones: {canvas.Iterations}";
                        window.alive.Text = $"Celdas vivas: {canvas.LiveCells}";
                    }
                ));
                if(this.speed != 0)
                    Thread.Sleep(this.speed*50);
                if(n>0)
                    n--;
                if(n==0)
                    break;
            }
            sw.Stop();
            Dispatcher.UIThread.InvokeAsync(new Action(
                delegate()
                {
                    window.iterBlock.Text = $"Tiempo: {sw.Elapsed}";
                }
            ));
            window.ThreadAlive = false;
        }
    }
}