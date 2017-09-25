using System;
using Microsoft.Extensions.CommandLineUtils;
using Avalonia;

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


            var inputFile = app.Option("-i|--input <inputfile>","Input file",CommandOptionType.SingleValue);

            app.OnExecute(()=>{
                // arrancar gui
                AppBuilder.Configure<App>().UsePlatformDetect().Start<GUI.MainWindow>();

                // procesar entrada de comandos
                try{
                    if(!inputFile.HasValue())
                        throw new Exception("Please, specify an input file");
                    if(!System.IO.File.Exists(inputFile.Value()))
                        throw new Exception("Please, check that the specified file exists");

                    var matrix = new File.Rle(inputFile.Value()).ConwayMatrix;
                
                    Console.WriteLine($"{matrix}");
                    Console.WriteLine("Matriz sin bordes:");
                    Console.WriteLine(matrix.GetFinalResult());    

                }catch(Exception e){
                    Console.WriteLine($"Error: {e.Message}");
                }
                return 0;
            });

            app.Execute(args);
        }
    }
}
