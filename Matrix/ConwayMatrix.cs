using System;
using System.Collections.Generic;

namespace Conway.Matrix {

    public class ConwayMatrix{
        private List<List<bool>> matrix;
        public ConwayMatrix()
        {
            matrix = new List<List<bool>>();
        }

        public int Width{ 
            get{
                return this.matrix[0].Count;
            }
        }
        public int Height{
            get{
                return this.matrix.Count;
            }
        }

        public void Iterate(){
         
            List<List<int>> Buffer=new List<List<int>>(); //Temporal
            for(int x=-1;x<this.matrix.Count+1;x++){ //El x=-1 y el x<this... +1 es para que incluya la fila anterior a la primera y la fila posterior.
                for (int y=-1;y<this.matrix.Count+1;y++){
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
        int filaExtra=0;
        int columnaExtra=0;
        foreach(List<int> casilla in Buffer){
            
            Console.WriteLine("CAMBIOOOOOOOO"+casilla[0]+" "+casilla[1] + "  ---> " + casilla[2]);
            this[casilla[0]+filaExtra,casilla[1]+columnaExtra]=Convert.ToBoolean(casilla[2]);

            if (casilla[0]==-1){
                filaExtra++;
            } 
            if (casilla[1]==-1){
                columnaExtra+=1;
            }


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