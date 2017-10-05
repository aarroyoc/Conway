using System;
using System.Collections.Generic;

namespace Conway.Matrix {

    public class ConwayMatrix : ICloneable {
        private List<List<bool>> matrix;
        public ConwayMatrix()
        {
            matrix = new List<List<bool>>();
            this.OffsetX = 0;
            this.OffsetY = 0;
        }

        public int OffsetX {get; set;}

        public int OffsetY {get; set;}

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

        public object Clone()
        {
            var c = new ConwayMatrix();
            var m = new List<List<bool>>();
            foreach(var l in this.matrix){
                m.Add(new List<bool>(l));
            }
            c.matrix = m;
            c.OffsetX = this.OffsetX;
            c.OffsetY = this.OffsetY;
            return c;
        }

        public void Iterate(){
         
            List<List<int>> Buffer=new List<List<int>>(); //Temporal
            for(int x=-1;x<this.Height+1;x++){ //El x=-1 y el x<this... +1 es para que incluya la fila anterior a la primera y la fila posterior.
                for (int y=-1;y<this.Width+1;y++){
                    bool alive=this[x,y];
                
                    int aliveNear=0;
                for (int i=-1;i<2;i++){
                    for (int z=-1;z<2;z++){
                        if (!(z==0 & i==0)){
                            aliveNear+=Convert.ToInt32(this[x+i,y+z]); //Posible optimización pequeña: En el momento en el que 
                                                                            //Alivenear>3, salir del bucle
                    
                        }

                    }
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
                      if (aliveNear==3){
                        List<int> temp=new List<int>{
                            x,y,1 //(x,y, estado al que va a pasar)
                        };
                        Buffer.Add(temp);
                       
               

                }  

                }

            }
        }
        int filaExtra=0;
        int columnaExtra=0;
        foreach(List<int> casilla in Buffer){
            
        
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
                if(x >= 0 && y >= 0 && x < matrix.Count && y < matrix[0].Count){
                    return matrix[x][y];
                }else{
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
                        this.OffsetX++;
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
                        this.OffsetY++;
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
        public ConwayMatrix LimitMatrix(int x1,int y1,int x2,int y2){
            //Requisitos: x1,y1 menores que x2,y2 respectivamente.
            //Podría generalizarse fácilmente, pero como solo tiene un único uso no es necesario.
            var limitedMatrix=new ConwayMatrix();

            limitedMatrix.SetSize(x2-x1+1,y2-y1+1);

            int x=0;//Establecen las coordenadas de la nueva matriz
            int y=0; 
            
            for (int i=x1;i<x2+1;i++){
                y=0;
              
                for (int j=y1;j<y2+1;j++){
                 
                    limitedMatrix[x,y]=this[i,j];
                    y++;    

                }
                 x++;
            }

            return limitedMatrix;
        }

         public FinalDataStruct GetFinalResult(){ //Cambiar por Structs
            int celdasVivas=0;
            int filaMinima=this.Height; //filaminima nunca va a ser mayor que la altura de la matriz.
            int filaMaxima=0;
            int columnaMinima=this.Width; 
            int columnaMaxima=0;
          
            FinalDataStruct data = new FinalDataStruct();

            for(int x=0;x<this.Height;x++){ 
            //filas
        
        
                for (int y=0;y<this.Width;y++){
                    //Columnas
                    
                    bool alive=this[x,y];
                    if (alive==true){
                       
                        celdasVivas++;
                        if (y<columnaMinima){
                            columnaMinima=y;
                        }
                        if (y>columnaMaxima){
                            columnaMaxima=y;
                        }
                        if (x<filaMinima){
                            filaMinima=x;
                        }
                        if (x>filaMaxima){
                            filaMaxima=x;
                        }
                    }
                }
            }
             
            
            data.CeldasVivas=celdasVivas;
            data.LimitedMatrix= this.LimitMatrix(filaMinima,columnaMinima,filaMaxima,columnaMaxima);
           
           
            return data; //TODO



        }
    }
}