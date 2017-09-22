using System;
using System.Collections.Generic;

namespace Conway.Matrix {

    public class ConwayMatrix{
        private List<List<bool>> matrix;
        public ConwayMatrix()
        {
            matrix = new List<List<bool>>();
        }
        public void SetSize(int x, int y)
        {
            for(int i=matrix.Count;i<x;i++){
                matrix.Add(new List<bool>());
                for(int j=matrix[i].Count;j<y;j++){
                    matrix[i].Add(false);
                }
            }
        }
        public override string ToString()
        {
            var sb = string.Empty;
            foreach(var c in matrix){
                foreach(var e in c){
                    sb += e ? "X" : ".";
                }
                sb += System.Environment.NewLine;
            }
            return sb;
        }
        public bool this[int x, int y]
        {
            // fuera de la matriz siempre están muertas las celdas
            get{
                try{
                    return matrix[x][y];
                }catch(ArgumentOutOfRangeException){
                    return false;
                }
            }

            set{
                try{
                    matrix[x][y] = value;
                }catch(ArgumentOutOfRangeException){
                    // la coordenada especificada no está implementada
                    // hacer crecer la matriz
                    int xplus = 0;
                    int yplus = 0;
                    Console.WriteLine($"X: {x}\tCount: {matrix.Count}");
                    if(x<0){
                        // insert
                        matrix.Insert(0,new List<bool>());
                        // crear Y
                        for(int j=0;j<matrix[1].Count;j++){
                            matrix[0].Add(false);
                        }
                        xplus = 1;
                    }else if(x==matrix.Count){
                        Console.WriteLine("Ampliando X");
                        // add
                        matrix.Add(new List<bool>());
                        // crear y
                        for(int j =0;j<matrix[0].Count;j++){
                            matrix[x].Add(false);
                        }
                    }
                    if(y<0){
                        foreach(var m in matrix){
                            m.Insert(0,false);
                        }
                        yplus = 1;
                    }else if(y == matrix[0].Count){
                        Console.WriteLine("Ampliando Y");
                        foreach(var m in matrix){
                            m.Add(false);
                        }
                    }
                    matrix[x+xplus][y+yplus] = value;
                }
            }
        }
    }
}