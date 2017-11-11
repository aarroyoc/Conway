using  System;

namespace Quadtree{
class Cuadrante{
    public static Almacen almacen;
    public readonly int nivel;
    public readonly long celdasVivas;

    public readonly Cuadrante nw,ne, sw,se;

    public readonly Cuadrante res;
    protected Cuadrante(int valor){
        //Crea un cuadrante de nivel 0 
        if (Cuadrante.almacen==null){
            Cuadrante.almacen=new Almacen();
        }



        if (valor==1 || valor==0){
             this.nivel=0;
            this.celdasVivas=valor;
        }  
        else{
            //TODO: Poner excepción
        }

    }

    protected Cuadrante(Cuadrante nw, Cuadrante ne, Cuadrante sw, Cuadrante se){
        //TODO: Verificar que nw, ne, sw, se tienen el mismo nivel 
        if (Cuadrante.almacen==null){
            Cuadrante.almacen=new Almacen();
        }
        if (nw.nivel==ne.nivel && ne.nivel==sw.nivel && sw.nivel==se.nivel){
        this.nw=nw;
        this.ne=ne;
        this.sw=sw;
        this.se=se;

        this.nivel=nw.nivel+1;
        this.celdasVivas=nw.celdasVivas+ne.celdasVivas+sw.celdasVivas+se.celdasVivas;

        }else{
             throw new System.ArgumentException("Los cuadrantes no tienen el mismo nivel." );
        }
        
       
    }

    
    public Boolean isCentrado(){
        //Indica si hay alguna celda viva fuera del centro

        Cuadrante centro=this.getCuadranteCentral();

        return (centro.celdasVivas==this.celdasVivas);

    }
     public Cuadrante expandir(){
      
        Cuadrante vacio=crearVacio(this.nivel-1);
        Cuadrante nw=Cuadrante.crear(vacio,vacio,vacio,this.nw);
        Cuadrante ne=Cuadrante.crear(vacio,vacio,this.ne,vacio);
        Cuadrante sw=Cuadrante.crear(vacio,this.sw,vacio,vacio);
        Cuadrante se=Cuadrante.crear(this.se,vacio,vacio,vacio);
        
        Cuadrante total=Cuadrante.crear(nw,ne,sw,se);

         return total;
    }
    public Cuadrante setPixel(long x,long y, int estado){
        if (this.nivel==0){
            return Cuadrante.crear(estado);
        }
        long numCasillas = (int)Math.Pow(2,nivel); //no es el número de casillas sino la posició máxima: Para 2 es 3: 00, 01, 02, 03 
        if (x>numCasillas-1 || y>numCasillas-1 ||x<0 || y<0){
                throw new System.ArgumentException($"Coordenadas x,y no nulas: ({x},{y}) " );
            }
        else{

            //TODO: verificar si el pixel es válido (está dentro del nivel)
            
             if (x<numCasillas/2){
                 if (y<numCasillas/2){
                     
                     return Cuadrante.crear(this.nw.setPixel(x,y,estado),ne,sw,se);
                    //return this.nw.setPixel(x,y,estado);
                 }
                 else{
                    return Cuadrante.crear(this.nw,this.ne,this.sw.setPixel(x,y-(numCasillas/2),estado),se);
                     //return this.sw.setPixel(x,y-(numCasillas/2),estado);
                 }

             }else{
                 if (y<numCasillas/2){
                      return Cuadrante.crear(this.nw,this.ne.setPixel(x-(numCasillas/2),y,estado),sw,se);
                     //return this.ne.setPixel(x-(numCasillas/2),y,estado);
                 }else{
                    return Cuadrante.crear(this.nw,this.ne,this.sw,this.se.setPixel(x-(numCasillas/2),y-(numCasillas/2),estado));
                     //return this.se.setPixel(x-(numCasillas/2),y-(numCasillas/2),estado);

                

                 }


             }


        }




    }
    public long getPixel(long x, long y){
        /* X= Columnas Y=Filas */
        if (this.nivel==0){
            return this.celdasVivas;
        }
        else{

          
              long numCasillas = (int)Math.Pow(2,nivel);  //no es el número de casillas sino la posició máxima: Para 2 es 3: 00, 01, 02, 03 
            if (x>numCasillas-1 || y>numCasillas-1 ||x<0 || y<0){
                throw new System.ArgumentException($"Coordenadas x,y no nulas: ({x},{y}) " );
            }


             if (x<numCasillas/2){
                 if (y<numCasillas/2){
                     return this.nw.getPixel(x,y);
                 }
                 else{
                     return this.sw.getPixel(x,y-(numCasillas/2));
                 }

             }else{
                 if (y<numCasillas/2){
                     return this.ne.getPixel(x-(numCasillas/2),y);
                 }else{
                     return this.se.getPixel(x-(numCasillas/2),y-(numCasillas/2));

                

                 }


             }
        }
   

    }
    //Se puede mantener la referencia de todos las casillas vivas y todas las muertas en solo dos Cuadrantes?   
    public static Cuadrante crear(int valor){
        //TODO
        //if valor==unico
        return new Cuadrante(valor);

        //else
        //return copia


    }

    public static Cuadrante crear(Cuadrante nw, Cuadrante ne, Cuadrante sw, Cuadrante se){
        //TODO
        //if valor==unico
        return new Cuadrante( nw, ne,  sw, se);

        //else
        //return copia


    }
    public Cuadrante generacion2() { 
        // ¿ Se pierde información? TODO
        // Si solo necesitamos los 4 puntos centrales, se puede reducir para que solo calcule
        //esos puntos=
        if (this.res!=null){
            return this.res;
        }
        if (nivel>2){
            return this.generacion();
        }

        Cuadrante buffer=new Cuadrante(this.nw,this.ne,this.sw,this.se);
        int celdasVivasCerca=0;
        bool alive=false;
        for (int i=0;i<4;i++){
            for (int j=0;j<4;j++){


                if (this.getPixel(i,j)==1){
                    alive=true;
                  
                }   
                  
                for (int z=-1;z<2;z++){
                    for (int w=-1;w<2;w++){
                        if (i+z>=0 && w+j>=0 && i+z<=3 && w+j<=3){
                            if (this.getPixel(i+z,w+j)==1 && !(z==0 & w==0)){
                            celdasVivasCerca++;
                         }
                        }
                       
                    }
                }
                 /*if (alive){
                      Console.WriteLine($"casilla {i},{j} celdas vivas: {celdasVivasCerca}");
                 }
                Console.WriteLine($"Analizando la casilla ({i},{j}. CeldasVivasCerca: {celdasVivasCerca})"); */
                if (alive & !(celdasVivasCerca==2 | celdasVivasCerca==3)){
                    buffer=buffer.setPixel(i,j,0);
                  
                }
                if (!alive &  celdasVivasCerca==3){ 
                    buffer=buffer.setPixel(i,j,1);
                 
                }

                alive=false;
                celdasVivasCerca=0;
            }

        }

    
        Cuadrante central=Cuadrante.crear(buffer.nw.se,buffer.ne.sw,buffer.sw.ne,buffer.se.nw);
        return central; //TODO

     }
    //Metodo temporal, borrarlo antes de entregar la práctica TODO
    public void print(){
        
        long numCasillas = (int)Math.Pow(2,nivel);
        for (int i=0;i<numCasillas;i++){
            for (int j=0;j<numCasillas;j++){
                Console.Write(this.getPixel(j,i));
            }
            Console.WriteLine();

        }
    }
    public Cuadrante generacion(){
        if (nivel==2){
            return this.generacion2();
        }
        if (this.res!=null){
            return this.res;
        }

   
        
        Cuadrante[] lista= this.divideEn9Cuadrados();

        Cuadrante uno= Cuadrante.crear(lista[0],lista[1],lista[3],lista[4]).generacion();
        Cuadrante dos= Cuadrante.crear(lista[1],lista[2],lista[4],lista[5]).generacion();
        Cuadrante tres= Cuadrante.crear(lista[3],lista[4],lista[6],lista[7]).generacion();
        Cuadrante cuatro=Cuadrante.crear(lista[4],lista[5],lista[7],lista[8]).generacion();

        Cuadrante generado=Cuadrante.crear(uno,dos,tres,cuatro);
        //this.res=generado; //TODO

        Cuadrante NWAlmacen=Cuadrante.almacen.get(this.nw);
        Cuadrante NEAlmacen=Cuadrante.almacen.get(this.ne);
        Cuadrante SWAlmacen=Cuadrante.almacen.get(this.sw);
        Cuadrante SEAlmacen=Cuadrante.almacen.get(this.se);

        

        
        return generado; //TODO
    }
    public  Cuadrante[] divideEn9Cuadrados(){ //TODO: Poner a privado cuando acabe los test
        Cuadrante[] lista=new Cuadrante[9];
        lista[0]=this.nw.getCuadranteCentral();
        Cuadrante temp=Cuadrante.crear(this.nw.ne,this.ne.nw,this.nw.se,this.ne.sw);
        lista[1]=temp.getCuadranteCentral();
        lista[2]=this.ne.getCuadranteCentral();
        temp=Cuadrante.crear(this.nw.sw,this.nw.se,this.sw.nw,this.sw.ne);
        lista[3]=temp.getCuadranteCentral();
        temp=Cuadrante.crear(this.nw.se,this.ne.sw,this.sw.ne,this.se.nw);
        lista[4]=temp.getCuadranteCentral();
        temp=Cuadrante.crear(this.ne.sw,this.ne.se,this.se.nw,this.se.ne);
        lista[5]=temp.getCuadranteCentral();
        lista[6]=this.sw.getCuadranteCentral();
        temp=Cuadrante.crear(this.sw.ne,this.se.nw,this.sw.se,this.se.ne);
        lista[7]=temp.getCuadranteCentral();
        lista[8]=this.se.getCuadranteCentral();

        return lista;
    }

    private Cuadrante getCuadranteCentral(){
        if (this.nivel<2){ 
            throw new System.ArgumentException($"Los cuadrantes de nivel 0 y 1 no tienen Cuadrante Central" );
        }
    
            
           Cuadrante central=Cuadrante.crear(this.nw.se,this.ne.sw,this.sw.ne,this.se.nw);
            return central;    


    }
    public static Cuadrante crearVacio(int nivel){
        if (nivel==0){
             return Cuadrante.crear(0);

        }
         return Cuadrante.crear(Cuadrante.crearVacio(nivel-1),Cuadrante.crearVacio(nivel-1),Cuadrante.crearVacio(nivel-1),Cuadrante.crearVacio(nivel-1));


    }

   
    public override bool Equals(object obj)
    {
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
            return this.GetHashCode()==obj.GetHashCode();
    
    }
    
    public override int GetHashCode()
    {
        if (this.nw==null){
            return (int)this.celdasVivas;
        }
        return nw.GetHashCode()+11*this.ne.GetHashCode()+101*this.sw.GetHashCode()+1007*this.se.GetHashCode();
    }

    
}
}