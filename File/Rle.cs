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
    public class Rle : IFileFormat{

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

            // comprobar si la cabecera ha sido leída correctamente
            if(x < 1 || y < 1)
                throw new Exception("Corrupt file");

            ConwayMatrix = new ConwayMatrix();
            ConwayMatrix.SetSize(y,x);

            // lee y almacena en un ConwayMatrix los datos codificados en RLE
            string[] ruleLines = rule.Split('$'); // cada línea en la matriz se separa por $
            var r = new Regex(@"([0-9]*)[bo]"); // contar cuantos grupos hay en cada match
            int a = 0, b = 0;
            // por cada línea aplicamos un regex de vivas/muertas y su números. Se procesan en orden
            // al final se añaden celdas muertas hasta acabar la longitud de la matriz
            foreach(var line in ruleLines){ 
                foreach(Match m in r.Matches(line)){

                    bool alive;
                    int c = 1;

                    if(String.IsNullOrWhiteSpace(m.Groups[1].Value)){
                        switch(m.Groups[0].Value){
                            case "o": alive = true;break;
                            case "b": alive = false;break;
                            default: throw new Exception("Corrupt file. Illegal character found");
                        }
                    }else{
                        var array = m.Groups[0].Value.ToCharArray();
                        c = Int32.Parse(m.Groups[1].Value);
                        char cell = array[array.Length - 1];
                        switch(cell){
                           case 'o': alive=true;break;
                           case 'b': alive=false;break;
                           default: throw new Exception("Corrupt file. Illegal character found"); 
                        }
                    }

                    for(int i=0;i<c;i++){
                        ConwayMatrix[b,a] = alive;
                        a++;
                    }
                }
                while(a<x){
                    ConwayMatrix[b,a]=false;
                    a++;
                }
                b++;
                a = 0;
            }
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