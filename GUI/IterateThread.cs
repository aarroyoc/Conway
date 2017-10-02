
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
                //Thread.Sleep(100);
            }
        }
    }
}