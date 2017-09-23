using System;
using System.Collections.Generic;

namespace Conway.Matrix {

    public class ConwayMatrix{
        private List<List<bool>> matrix;
        public ConwayMatrix()
        {
            matrix = new List<List<bool>>();
        }
        public List<List<bool>> GetMatrix(){
            return this.matrix;

        }

        public void Iterate(){
            //Para que las casillas no afecten al resto, dejo los cambios en un buffer.
            //Problemas: Las casillas fuera del tablero no se verifican, es decir si  [0,0].[0,1][0,2] están activas, [1,-1] debería activarse.

            List<List<int>> Buffer=new List<List<int>>(); //Temporal
            for(int x=0;x<this.matrix.Count;x++){
                for (int y=0;y<this.matrix.Count;y++){
                    bool alive=this[x,y];

                    int aliveNear=0;
                for (int i=-1;i<2;i++){
                    for (int z=-1;z<2;z++){
                        if (!(z==0 & i==0)){
                            aliveNear+=Convert.ToInt32(this[x+i,y+z]); //Posible optimización pequeña: En el momento en el que 
                                                                            //Alivenear>3, salir del bucle
                        if (x==0 & y==1){
                            Console.WriteLine("0,1 PRUEBA "+i+" "+ z + "-->"+ Convert.ToInt32(this[x+i,y+z]));
                        }
                        }

                    }
                }    
                if (x==0 & y==1){
                    Console.WriteLine("0,1"+aliveNear+"¿Alive?"+alive);
                }
                if (alive==true){
                    if (aliveNear!=3 & aliveNear!=2){
                        List<int> temp=new List<int>{
                            x,y,0 //(x,y, estado al que va a pasar)
                        };
                      Buffer.Add(temp);
                    }
                }
                else{
                      if (x==0 & y==1){
                         Console.WriteLine("dasdsadasdfñlasfñljasdfñls"+aliveNear+"¿Alive?"+alive);
                      }
                      if (aliveNear==3){
                        List<int> temp=new List<int>{
                            x,y,1 //(x,y, estado al que va a pasar)
                        };
                        Buffer.Add(temp);
                        Console.WriteLine("UNA CASILLA HA PASADO A LA VIDAAAAAAAAAA");
               

                }  

                }

            }
        }

        foreach(List<int> casilla in Buffer){
            Console.WriteLine("CAMBIOOOOOOOO"+casilla[0]+" "+casilla[1] + "  ---> " + casilla[2]);
            this.matrix[casilla[0]][casilla[1]]=Convert.ToBoolean(casilla[2]);
        
        }
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
                    sb += e ? "X" : "."; //¿?
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