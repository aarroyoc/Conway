using  System;
using System.Collections.Generic;
using System.Threading;
using System.Collections;

namespace Quadtree{
public class Cuadrante{
    public static Almacen almacen;
    public readonly int nivel;
    public readonly long celdasVivas;
      
     public static ArrayList  cuadrantesVacios;

        
    public readonly Cuadrante nw,ne, sw,se;


    public Cuadrante res,resEtapa2;
    protected Cuadrante(int valor){
        //Crea un cuadrante de nivel 0 
        if (Cuadrante.almacen==null){
            Cuadrante.almacen=new Almacen(1024,0.6);
        }
        if (cuadrantesVacios == null)
            {
                cuadrantesVacios = new ArrayList();
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
       
        if (Cuadrante.almacen==null){
            Cuadrante.almacen=new Almacen(1024,0.6);
        }
            if (cuadrantesVacios == null)
            {
                cuadrantesVacios = new ArrayList();
            }

            if (nw.nivel==ne.nivel && ne.nivel==sw.nivel && sw.nivel==se.nivel){
        this.nw=nw;
        this.ne=ne;
        this.sw=sw;
        this.se=se;

        this.nivel=nw.nivel+1;
        this.celdasVivas=nw.celdasVivas+ne.celdasVivas+sw.celdasVivas+se.celdasVivas;
          
            
            }
            else{
             throw new System.ArgumentException("Los cuadrantes no tienen el mismo nivel." );
        }
        
       
    }

    
    public Boolean isCentrado(){
        //Indica si hay alguna celda viva fuera del centro

        Cuadrante centro=this.getCuadranteCentral();

        return (centro.celdasVivas==this.celdasVivas);

    }

    public Boolean isDobleCentrado(){
        //Devuelve true si está centrado en el cuadrante central del cuadrante central
        
        if (this.nw.celdasVivas==this.nw.se.celdasVivas && this.ne.celdasVivas==this.ne.sw.celdasVivas && this.sw.celdasVivas==this.sw.ne.celdasVivas
        && this.se.celdasVivas==this.se.nw.celdasVivas
        ){
            return true;
        }
            return false;
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
    // buscar o crear cuadrante
    private Cuadrante buscarSiguiente(Cuadrante nw, Cuadrante ne, Cuadrante sw, Cuadrante se){
        var c = Cuadrante.crear(nw,ne,sw,se);
        var next = almacen.get(c);
        if(next!=null){
            return next;
        }
        next = c.generacionEtapa4();
        almacen.add(c,next);
        return next;
    }
    public Cuadrante generacionEtapa4(){
          if (nivel==2){
            return this.generacion2();
        }
        if (this.res!=null){
         
            return this.res;
        }
        if (this.celdasVivas == 0)
            {
                this.res = this.nw;
                return this.nw;
            }
   
          
        Cuadrante[] lista= this.divideEn9CuadradosEtapa4();

        
        Cuadrante uno= Cuadrante.crear(lista[0],lista[1],lista[3],lista[4]).generacionEtapa4();
        Cuadrante dos= Cuadrante.crear(lista[1],lista[2],lista[4],lista[5]).generacionEtapa4();
        Cuadrante tres= Cuadrante.crear(lista[3],lista[4],lista[6],lista[7]).generacionEtapa4();
        Cuadrante cuatro=Cuadrante.crear(lista[4],lista[5],lista[7],lista[8]).generacionEtapa4();

        Cuadrante generado=Cuadrante.crear(uno,dos,tres,cuatro);
        this.res=generado; 

        

        
        return generado; 

    }
    

    public Cuadrante[] divideEn9CuadradosEtapa4(){
        Cuadrante[] lista=new Cuadrante[9];

           
        lista[0]=this.nw.generacionEtapa4();
        Cuadrante temp=Cuadrante.crear(this.nw.ne,this.ne.nw,this.nw.se,this.ne.sw);
        lista[1]=temp.generacionEtapa4();
        lista[2]=this.ne.generacionEtapa4();
        temp=Cuadrante.crear(this.nw.sw,this.nw.se,this.sw.nw,this.sw.ne);
        lista[3]=temp.generacionEtapa4();
        temp=Cuadrante.crear(this.nw.se,this.ne.sw,this.sw.ne,this.se.nw);
        lista[4]=temp.generacionEtapa4();
        temp=Cuadrante.crear(this.ne.sw,this.ne.se,this.se.nw,this.se.ne);
        lista[5]=temp.generacionEtapa4();
        lista[6]=this.sw.generacionEtapa4();
        temp=Cuadrante.crear(this.sw.ne,this.se.nw,this.sw.se,this.se.sw);
        lista[7]=temp.generacionEtapa4();
        lista[8]=this.se.generacionEtapa4();

  

        return lista;

   


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
      
    public static Cuadrante crear(int valor){
        
         Cuadrante temp=new Cuadrante(valor);

         Cuadrante get= almacen.get(temp); 
        if (get==null){
           almacen.add(temp,temp);
            return temp;
          }
        if (get.nw==null && get.ne==null && get.sw==null && get.se==null){
        return get;
        }
        else{
            throw new ArgumentException("Hash table no ha devuelto el cuadrante correcto");
           
        }
      


    }

    public static Cuadrante crear(Cuadrante nw, Cuadrante ne, Cuadrante sw, Cuadrante se){
      
       

        Cuadrante get= almacen.get(nw,ne,sw,se); 
        if (get==null){
                Cuadrante temp = new Cuadrante(nw, ne, sw, se);
                almacen.add(temp,temp);
            return temp;
        }
   
           
            if (get.nw==nw && get.ne==ne&& get.sw==sw && get.se==se){
        return get;
         }else{
       
         throw new ArgumentException("Hash table no ha devuelto el cuadrante correcto en Cuadrante.crear de 4 parámetros");
         }

    }

    public static Cuadrante crear(bool[][] m){
        var matrix = new Cuadrante[m.Length][];
        for(var i=0;i<m.Length;i++){
            matrix[i] = new Cuadrante[m.Length];
            for(var j=0;j<m.Length;j++){
                matrix[i][j]=Cuadrante.crear(Convert.ToInt32(m[i][j]));
            }
        }

        while(matrix.Length != 1){
            var buffer = new Cuadrante[matrix.Length/2][];
            for(var i=0;i<matrix.Length/2;i++){
                buffer[i] = new Cuadrante[matrix.Length/2];
            }

            for(var i=0;i<matrix.Length;i+=2){
                for(var j=0;j<matrix.Length;j+=2){
                    var nw = matrix[j][i];
                    var ne = matrix[j][i+1];
                    var sw = matrix[j+1][i];
                    var se = matrix[j+1][i+1];
                    buffer[j/2][i/2] = Cuadrante.crear(nw,ne,sw,se);
                }
            }
            matrix = buffer;
        }
        return matrix[0][0];
    }
    public Cuadrante generacion2() { 
    
        if (this.res!=null){
            return this.res;
        }
        if (nivel>2){
            return this.generacionEtapa4();
        }
            if (this.celdasVivas == 0)
            {
                this.res = this.nw;
                return this.nw;
            }

            //Console.WriteLine("Se ha ejecutado generación nivel 2");
            Cuadrante nw2=nw.se;
                Cuadrante ne2=ne.sw;
                Cuadrante sw2=sw.ne;
                Cuadrante se2=se.nw;
       
                int celdasVivasCerca=0;
       
                celdasVivasCerca=Convert.ToInt32(this.nw.nw.celdasVivas)+Convert.ToInt32(this.nw.ne.celdasVivas)+Convert.ToInt32(this.nw.sw.celdasVivas);
                celdasVivasCerca+=Convert.ToInt32(ne.nw.celdasVivas)+Convert.ToInt32(ne.sw.celdasVivas);
                celdasVivasCerca+=Convert.ToInt32(se.nw.celdasVivas);
                celdasVivasCerca+=Convert.ToInt32(sw.nw.celdasVivas)+Convert.ToInt32(sw.ne.celdasVivas);
               
                if (nw.se.celdasVivas==1 & !(celdasVivasCerca==2 | celdasVivasCerca==3)){
                   nw2=Cuadrante.crear(0);
                }
                if (!(nw.se.celdasVivas==1) &  celdasVivasCerca==3){ 
                    nw2=Cuadrante.crear(1);
                }

                celdasVivasCerca=Convert.ToInt32(this.ne.nw.celdasVivas)+Convert.ToInt32(this.ne.ne.celdasVivas)+Convert.ToInt32(this.ne.se.celdasVivas);
                // Console.WriteLine("Celdas vivas de ne Fase 1: " + celdasVivasCerca);
                celdasVivasCerca+=Convert.ToInt32(nw.se.celdasVivas)+Convert.ToInt32(nw.ne.celdasVivas);
                //Console.WriteLine("Celdas vivas de ne Fase 2: " + celdasVivasCerca);
                celdasVivasCerca+=Convert.ToInt32(sw.ne.celdasVivas);
                //Console.WriteLine("Celdas vivas de ne Fase 3: " + celdasVivasCerca);
                celdasVivasCerca+=Convert.ToInt32(se.ne.celdasVivas)+Convert.ToInt32(se.nw.celdasVivas);
                //Console.WriteLine("Celdas vivas de ne: " + celdasVivasCerca);
                if (ne.sw.celdasVivas==1 & !(celdasVivasCerca==2 | celdasVivasCerca==3)){
                   ne2=Cuadrante.crear(0);
                }
                if (!(ne.sw.celdasVivas==1) &  celdasVivasCerca==3){ 
                    ne2=Cuadrante.crear(1);
                }


                celdasVivasCerca=Convert.ToInt32(this.sw.nw.celdasVivas)+Convert.ToInt32(this.sw.sw.celdasVivas)+Convert.ToInt32(this.sw.se.celdasVivas);
               
                celdasVivasCerca+=Convert.ToInt32(se.nw.celdasVivas)+Convert.ToInt32(se.sw.celdasVivas);
               
                celdasVivasCerca+=Convert.ToInt32(ne.sw.celdasVivas);
              
                 celdasVivasCerca+=Convert.ToInt32(nw.se.celdasVivas)+Convert.ToInt32(nw.sw.celdasVivas);

                 
                if (sw.ne.celdasVivas==1 & !(celdasVivasCerca==2 | celdasVivasCerca==3)){
                   sw2=Cuadrante.crear(0);
                }
                if (!(sw.ne.celdasVivas==1) &  celdasVivasCerca==3){ 
                    sw2=Cuadrante.crear(1);
                }


                celdasVivasCerca=Convert.ToInt32(this.se.ne.celdasVivas)+Convert.ToInt32(this.se.sw.celdasVivas)+Convert.ToInt32(this.se.se.celdasVivas);
                celdasVivasCerca+=Convert.ToInt32(ne.sw.celdasVivas)+Convert.ToInt32(ne.se.celdasVivas);
                celdasVivasCerca+=Convert.ToInt32(nw.se.celdasVivas);
                celdasVivasCerca+=Convert.ToInt32(sw.se.celdasVivas)+Convert.ToInt32(sw.ne.celdasVivas);

                if (se.nw.celdasVivas==1 & !(celdasVivasCerca==2 | celdasVivasCerca==3)){
                   se2=Cuadrante.crear(0);
                }
                if (!(se.nw.celdasVivas==1) &  celdasVivasCerca==3){ 
                    se2=Cuadrante.crear(1);
                }
                
            
        Cuadrante central=Cuadrante.crear(nw2,ne2,sw2,se2);
        this.res=central;
        return central; 
           
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

    public override String ToString(){
        String str = "";
        long numCasillas = (int)Math.Pow(2,nivel);
        for (int i=0;i<numCasillas;i++){
            for (int j=0;j<numCasillas;j++){
                str+=$"{this.getPixel(j,i)}";
            }
            str += System.Environment.NewLine;

        }
        return str;
    }
    public Cuadrante generacion(){
        if (nivel==2){
            return this.generacion2();
        }
        if (this.celdasVivas == 0)
            {
                this.resEtapa2 = this.nw;
                return this.nw;
            }
        if (this.resEtapa2!=null){
            return this.resEtapa2;
        }

   
        
        Cuadrante[] lista= this.divideEn9Cuadrados();

        Cuadrante uno= Cuadrante.crear(lista[0],lista[1],lista[3],lista[4]).generacion();
 
        Cuadrante dos= Cuadrante.crear(lista[1],lista[2],lista[4],lista[5]).generacion();
        Cuadrante tres= Cuadrante.crear(lista[3],lista[4],lista[6],lista[7]).generacion();
        Cuadrante cuatro=Cuadrante.crear(lista[4],lista[5],lista[7],lista[8]).generacion();

        Cuadrante generado=Cuadrante.crear(uno,dos,tres,cuatro);




            this.resEtapa2 = generado;
        return generado; 
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
        temp=Cuadrante.crear(this.sw.ne,this.se.nw,this.sw.se,this.se.sw); 
        lista[7]=temp.getCuadranteCentral();
        lista[8]=this.se.getCuadranteCentral();

        /* 
        Console.WriteLine("Dividiendo esto en 9 cuadrados:");
        //this.print();
        Console.WriteLine("Se ha dividido en 9 cuadrados, son estos:");
        for (int i=0;i<9;i++){
            Console.WriteLine("imprimiento el cuadrado " + i);
            lista[i].print();
        }*/
        return lista;
    }

    public Cuadrante getCuadranteCentral(){
        if (this.nivel<2){ 
            throw new System.ArgumentException($"Los cuadrantes de nivel 0 y 1 no tienen Cuadrante Central" );
        }
    
            
           Cuadrante central=Cuadrante.crear(this.nw.se,this.ne.sw,this.sw.ne,this.se.nw);
            return central;    


    }
    public static Cuadrante crearVacio(int nivel){
            Cuadrante vacio;
       
            if (cuadrantesVacios == null)
            {
                cuadrantesVacios = new ArrayList();
            }
        if (nivel < cuadrantesVacios.Count)
            {
               vacio =(Cuadrante) cuadrantesVacios[nivel];
                if (vacio.nivel==nivel && vacio.celdasVivas == 0)
                {
                    return vacio;
                }
                else
                {
                    vacio.print();
                    throw  new ArgumentException("Ha habido un error al generar un cuadrante vacio");
                }
            }


         if (nivel==0){
                vacio= Cuadrante.crear(0); ;
                Cuadrante.cuadrantesVacios.Add(vacio);
                return vacio;

        }
            vacio=Cuadrante.crear(Cuadrante.crearVacio(nivel - 1), Cuadrante.crearVacio(nivel - 1), Cuadrante.crearVacio(nivel - 1), Cuadrante.crearVacio(nivel - 1));
            Cuadrante.cuadrantesVacios.Add(vacio);
            return vacio;


    }

   
    public override bool Equals(object obj)
    {
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        Cuadrante obj2=(Cuadrante) obj;
        if (this.nivel==0){
            return obj2.celdasVivas==this.celdasVivas;
        }
        
        return (this.nivel == obj2.nivel && this.nw==obj2.nw && this.ne==obj2.ne && this.sw==obj2.sw && this.se==obj2.se);

            
    
    }
    public static int getHash(Cuadrante nw, Cuadrante ne,Cuadrante sw, Cuadrante se)
        {
            return 2 * nw.GetHashCode() + 11 *ne.GetHashCode() + 101 * sw.GetHashCode() + 1007 * se.GetHashCode()+ 5 * (nw.nivel+1);
        }
    public  int getHash()
    {
        if (this.nw==null){
            return (int)this.celdasVivas;
        }
       //Solucionar los que tienen hashes igual a 0
 
       return 2 * this.nw.GetHashCode() + 11 * this.ne.GetHashCode() + 101 * this.sw.GetHashCode() + 1007 * this.se.GetHashCode() + 5 * this.nivel;
            
    }

    public bool[][] GetMatrix(){
        var n = (int)Math.Pow(2,this.nivel);
        var matrix = new bool[n][];
        for(var i=0;i<n;i++){
            matrix[i] = new bool[n];
            for(var j=0;j<n;j++){
                matrix[i][j] = getPixel(j,i) == 1;
            }
        }
        return matrix;
    }

    
}
}