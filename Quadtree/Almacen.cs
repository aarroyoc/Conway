 using System.Collections.Generic;
 using System;
 using Quadtree;


  class Almacen{
    //private Dictionary<Cuadrante,Cuadrante> almacen;
    private Item[] almacen;
    private int capacity;
    private int nElements;
    private double maxLoadFactor;

    public Almacen(int initialCapacity, double maxLoadFactor){
        this.capacity = initialCapacity;
        this.maxLoadFactor = maxLoadFactor;
        almacen = new Item[this.capacity];
        for(var i=0;i<almacen.Length;i++){
            almacen[i] = null;
        }
        this.nElements = 0;
    }

    protected int index(Cuadrante c){
        return Math.Abs(c.GetHashCode()) % capacity;
    }

    protected int salto(Cuadrante c){
        int s = Math.Abs(c.GetHashCode()) / capacity;
        return (s % 2 == 0) ? s+1 : s;
    }

    protected void reestructurar(){
        Item[] tmp = almacen;
        this.nElements = 0;
        this.capacity *= 2;
        almacen = new Item[this.capacity];
        for(var i=0;i<almacen.Length;i++){
            almacen[i] = null;
        }
        for(var i=0;i<tmp.Length;i++){
            Item item = tmp[i];
            if(item != null && item.Item1 != null){
                add(item.Item1,item.Item2);
            }
        }
    }
   
        public Cuadrante get(Cuadrante key){
        int i = index(key);
        int d = salto(key);

            while(almacen[i]!=null && (almacen[i].Item1 == null || !almacen[i].Item1.Equals(key) )){
                i = (i+d) % capacity;
            }

         var item = almacen[i];
       
           
        
        return item?.Item2;
    }

    public void add(Cuadrante origen,Cuadrante destino){
        this.nElements++;
        if((1.0*this.nElements)/this.capacity > this.maxLoadFactor){
            reestructurar();
        }

        int i = index(origen);
        int d = salto(origen);

        while(almacen[i] != null && almacen[i].Item1 != null){
            i = (i+d) % capacity;
        }

        almacen[i] = new Item(origen,destino);
    }

}
    enum EstadoCelda {
        CeldaVacia,
        CeldaBorrada,
        CeldaOcupada
    };
  class Item : Tuple<Cuadrante,Cuadrante> {
      public Item(Cuadrante key, Cuadrante value) : base(key,value)
      {

      }
  }