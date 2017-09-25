using System;
using Conway.Matrix;

namespace Conway.File {
    public class Vaca : IFileFormat{
        public ConwayMatrix ConwayMatrix {get; set;}

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

        }
    }
}