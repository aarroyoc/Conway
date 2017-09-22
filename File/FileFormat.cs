using Conway.Matrix;

namespace Conway.File {
    interface FileFormat{
        ConwayMatrix ConwayMatrix {get; set;}
        void Load(string path);
        void Save(string path);
    }
}