using System;
using System.IO;
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
                    if(!File.Exists(inputFile.Value()))
                        throw new Exception("Please, check that the specified file exists");
                }catch(Exception e){
                    Console.WriteLine($"Error: {e.Message}");
                }
                return 0;
            });

            app.Execute(args);
        }
    }
}
