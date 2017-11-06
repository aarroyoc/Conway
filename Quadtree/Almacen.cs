 using System.Collections.Generic;
 using Quadtree;
  class Almacen{
    private Dictionary<Cuadrante,Cuadrante> almacen;


    public Almacen(){
        almacen=new  Dictionary<Cuadrante,Cuadrante>();

    }

    public void add(Cuadrante origen,Cuadrante destino){
        almacen.Add(origen,destino);

    }
    public bool checkIfExist(Cuadrante a){
        return almacen.ContainsKey(a);
    }
    public Cuadrante get(Cuadrante key){

        if (almacen.TryGetValue(key,out Cuadrante destino)){
            return destino;
        }else{
            Cuadrante nuevo=key.generacion();
            this.add(key,nuevo);

            return  nuevo;
        }
        
     
    }


}