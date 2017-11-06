using  System;

namespace Quadtree{
class Cuadrante{
    public static Almacen almacen;
    public readonly int nivel;
    public readonly long celdasVivas;

    public readonly Cuadrante nw,ne, sw,se;

    
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

        
        this.nw=nw;
        this.ne=ne;
        this.sw=sw;
        this.se=se;

        this.nivel=nw.nivel+1;
        this.celdasVivas=nw.celdasVivas+ne.celdasVivas+sw.celdasVivas+se.celdasVivas;

    }


     public Cuadrante expandir(){
         //TODO: Dependencia setPixel()
        Cuadrante vacio=crearVacio(this.nivel+1);
        //vacio.nw=

         return new Cuadrante(1);
    }
    
    public long getPixel(long x, long y){
        /* X= Columnas Y=Filas */
        if (this.nivel==0){
            return this.celdasVivas;
        }
        else{

            //TODO: verificar si el pixel es válido (está dentro del nivel)
             long numCasillas=2*nivel; //no es el número de casillas sino la posició máxima: Para 2 es 3: 00, 01, 02, 03 
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
    public Cuadrante generacion(){
        
        Cuadrante NWAlmacen=Cuadrante.almacen.get(this.nw);
        Cuadrante NEAlmacen=Cuadrante.almacen.get(this.ne);
        Cuadrante SWAlmacen=Cuadrante.almacen.get(this.sw);
        Cuadrante SEAlmacen=Cuadrante.almacen.get(this.se);



        
        return null; //TODO
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