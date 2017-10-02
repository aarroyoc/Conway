using System;
using System.Diagnostics;
using Microsoft.Extensions.CommandLineUtils;
using Avalonia;
using Conway.Matrix;

namespace Conway
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Conway - Game of Life");
            var app = new CommandLineApplication();

            
            app.Name = "Conway";
            app.HelpOption("-?|-h|--help");
            app.VersionOption("-v|--version","Conway 1.0.0");


            var inputFile = app.Option("-i <inputfile>|--input <inputfile>","Input file",CommandOptionType.SingleValue);
            var iterations = app.Option("-n <n>|--iter <n>","Number of iterations",CommandOptionType.SingleValue);
  
            app.OnExecute(()=>{
                // procesar entrada de comandos
                try{
                    if(!inputFile.HasValue())
                        throw new Exception("Please, specify an input file");
                    if(!System.IO.File.Exists(inputFile.Value()))
                        throw new Exception("Please, check that the specified file exists");
                    if(!iterations.HasValue())
                        throw new Exception("Please, specify a number of iterations");
                    var iter = Int32.Parse(iterations.Value());

                    // TODO: ser capaz de detectar el formato correcto
                    
                    ConwayMatrix matrix = null;
                    try{
                        matrix = new File.Rle(inputFile.Value()).ConwayMatrix;
                    }catch(Exception){
                        matrix = new File.Vaca(inputFile.Value()).ConwayMatrix;
                    };
                    if(matrix == null)
                        throw new Exception("File format error");
                    Console.WriteLine($"{matrix}");
                    var sw = Stopwatch.StartNew();
                    for(var i=0;i<iter;i++){
                        matrix.Iterate();
                    }
                    sw.Stop();

                    var final = matrix.GetFinalResult();
                    // report final
                    Console.WriteLine($"{iter} iteraciones");
                    Console.WriteLine($"{final.CeldasVivas} celdas vivas");
                    Console.WriteLine($"Dimensiones: {final.LimitedMatrix.Width} x {final.LimitedMatrix.Height}");
                    Console.WriteLine($"Tiempo: {sw.Elapsed.TotalMilliseconds} ms");

                    // guardar
                    Console.Write("Fichero de salida: ");
                    var filename = Console.ReadLine().Trim();
                    if(filename == string.Empty){
                        Console.WriteLine(final.LimitedMatrix);
                    }else{
                        var file = new File.Vaca();
                        file.ConwayMatrix = final.LimitedMatrix;
                        file.Save(filename);
                        Console.WriteLine($"Matriz final guardada en {filename}");
                    }

               }catch(Exception e){
                    Console.WriteLine($"WARNING: {e.Message}");
                    Console.WriteLine("Starting UI");

                    // arrancar gui
                    AppBuilder.Configure<App>().UsePlatformDetect().Start<GUI.MainWindow>();
               } 
               return 0;
            });

            app.Execute(args);
        }
    }
}
