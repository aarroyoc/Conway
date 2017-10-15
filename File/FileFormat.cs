/* Arroyo Calle, Adrián
Bazaco Velasco, Daniel*/
using Conway.Matrix;

namespace Conway.File {
    interface IFileFormat{
        ConwayMatrix ConwayMatrix {get; set;}
        void Load(string path);
        void Save(string path);
    }
}