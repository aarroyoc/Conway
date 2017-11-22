/* Arroyo Calle, Adrián
Bazaco Velasco, Daniel*/
using System;
using Conway.Matrix;

namespace Conway.File {
    public class Vaca : IFileFormat{
        public ConwayMatrix ConwayMatrix {get; set;}

        public Vaca()
        {
            
        }
        public Vaca(string path)
        {
            Load(path);
        }
        public void Load(string path)
        {
            ConwayMatrix = new ConwayMatrix();
            var lines = System.IO.File.ReadAllLines(path);
            var column = Int32.Parse(lines[1].Trim());
            
            for(var i=2;i<lines.Length;i++){
                var line = lines[i];
                var array = line.ToCharArray();
                for(var j=0;j<array.Length;j++){
                    ConwayMatrix[i-2,j] = (array[j] == 'X');
                }
            }

        }

        public void Save(string path)
        {
            var x = ConwayMatrix.Width;
            var y = ConwayMatrix.Height;
            var n = System.Environment.NewLine;
            var matrix = ConwayMatrix.ToString();
            string text = $"{x}{n}{y}{n}{matrix}";
            try{
                System.IO.File.WriteAllText(path,text);
            }catch(Exception e){
                Console.WriteLine($"ERROR while saving file: {e}");
            }
        }

        public bool[][] GetMatrix()
        {
            return ConwayMatrix.GetMatrix();
        }
    }
}