using Avalonia.Threading;
using System;

namespace Conway.GUI {
    class IterateThread {
        ConwayCanvas canvas;
        MainWindow window;
        public IterateThread(ConwayCanvas canvas,MainWindow window)
        {
            this.canvas = canvas;
            this.window = window;
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
                //Thread.Sleep(100);
            }
        }
    }
}