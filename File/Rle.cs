using System;
using System.IO;
using System.Text.RegularExpressions;
using Conway.Matrix;

/*
Conway.File contiene clases para cargar/guardar patrones de el juego de la vida
 */
namespace Conway.File {

    /*
    RLE es el formato "por defecto" de el juego de la vida. Esta clase permite cargar/guardar en este formato
    Documentación: http://www.conwaylife.com/w/index.php?title=Run_Length_Encoded
     */
    public class Rle : FileFormat{

        // constructor sin carga
        public Rle()
        {
            ConwayMatrix = new ConwayMatrix();
        }
        // constructor cargando un archivo existente
        public Rle(string file)
        {
            ConwayMatrix = new ConwayMatrix();
            Load(file);
        }
        public ConwayMatrix ConwayMatrix {get; set;}

        public void Load(string path)
        {
            string patterName = string.Empty;
            string author = string.Empty;
            int x = 0;
            int y = 0;
            // TODO: falta rule name
            Regex regex = new Regex(@"^x = ([0-9]*), y = ([0-9]*)",RegexOptions.IgnoreCase);

            string[] lines = System.IO.File.ReadAllLines(path);
            string rule = string.Empty;
            foreach(string line in lines){
                if(line.StartsWith("#")){
                    char c = line.ToCharArray()[1];
                    switch(c){
                        case 'C': // comentarios
                        case 'c': break; // comentarios
                        case 'N': patterName = line.Substring(2); break;
                        case 'O': author = line.Substring(2); break;
                        case 'P': // sin implementar
                        case 'R': // sin implementar
                        case 'r': break; // sin implementar
                    }
                }else if(regex.IsMatch(line)){
                    var matches = regex.Matches(line);
                    var g = matches[0].Groups;
                    try{
                        x = Int32.Parse(g[1].Value);
                        y = Int32.Parse(g[2].Value);
                    }catch(Exception ex) when(ex is FormatException || ex is OverflowException){
                        x = -1;
                        y = -1;
                        throw new Exception("Error while reading RLE headers");
                    }
                }else{
                    rule += line;
                }
            }
            // TODO: parsear rule, añadir muertas al final de las líneas, mejorar código
            string[] ruleLines = rule.Split('$');
            var r = new Regex(@"([0-9]*)[bo]"); // contar cuantos grupos hay en cada match
            foreach(var line in ruleLines){
                foreach(Match m in r.Matches(line)){
                    if(String.IsNullOrWhiteSpace(m.Groups[1].Value)){
                        switch(m.Groups[0].Value){
                            case "o": Console.WriteLine("Una viva");break;
                            case "b": Console.WriteLine("Una muerta");break;
                            default: throw new Exception("Corrupt file. Illegal character found");
                        }
                    }else{
                        var array = m.Groups[0].Value.ToCharArray();
                        int c = Int32.Parse(m.Groups[1].Value);
                        char cell = array[array.Length - 1];
                        switch(cell){
                           case 'o': Console.WriteLine($"{c} vivas");break;
                           case 'b': Console.WriteLine($"{c} muertas");break;
                           default: throw new Exception("Corrupt file. Illegal character found"); 
                        }
                    }
                }
            }

            // TODO: generar un ConwayMatrix correcto
            Console.WriteLine("Finishing reading the RLE file");
            if(x < 1 || y < 1)
                throw new Exception("Corrupt file");
            Console.WriteLine($"X-> {x}\tY-> {y}");
        }
        public void Save(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.Write("SIN IMPLEMENTAR");
            }
        }
    }
}