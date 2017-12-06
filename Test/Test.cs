using Xunit;
using Conway.File;
using Conway.Matrix;
using Quadtree;
using System;
using Conway.GUI;
using System.Diagnostics;
using System.Numerics;
namespace Conway.Test {
    public class Test{
                    string glider=@"........................X...........
......................X.X...........
............XX......XX............XX
...........X...X....XX............XX
XX........X.....X...XX..............
XX........X...X.XX....X.X...........
..........X.....X.......X...........
...........X...X....................
............XX......................
";
        [Fact]
        public void ReadHeader()
        {
            var rle = new Rle();
            rle.Load(@"Patterns/glider.rle");
        }

        [Fact]
        public void GliderVaca()
        {
            var vaca = new Vaca("Patterns/Glider.vaca");
            Assert.Equal(glider,vaca.ConwayMatrix.GetFinalResult().LimitedMatrix.ToString());
        }

        [Fact]
        public void SaveVaca()
        {
            string test = @"3
3
X..
.X.
..X
";
            var vaca = new Vaca();
            vaca.ConwayMatrix = new ConwayMatrix();
            vaca.ConwayMatrix[0,0] = true;
            vaca.ConwayMatrix[1,1] = true;
            vaca.ConwayMatrix[2,2] = true;
            var file = System.IO.Path.GetTempFileName();
            vaca.Save(file);

            Assert.Equal(System.IO.File.ReadAllText(file),test);

        }

        [Fact]
        public void GliderMatrix()
        {
            var rle = new Rle();
            rle.Load(@"Patterns/glider.rle");
            Assert.Equal(glider,rle.ConwayMatrix.GetFinalResult().LimitedMatrix.ToString());
        }

        [Fact]
        public void SixBitsMatrix()
        {
            string sixbits=@".....................X..................
.....................X..................
....................X.X.................
.....................X..................
.....................X..................
.....................X..................
.....................X..................
....................X.X.................
.....................X..................
.....................X..................
........................................
........................................
........................................
........................................
..X..X....X..X..........................
XXX..XXXXXX..XXX........................
..X..X....X..X..........................
......................XX................
.....................XX.................
.......................X................
................................X....X..
..............................XX.XXXX.XX
................................X....X..
";
            var rle = new Rle();
            rle.Load("Patterns/6bits.rle");
            Assert.Equal(sixbits,rle.ConwayMatrix.GetFinalResult().LimitedMatrix.ToString());
        }
        [Fact]
        public void testSetPixelQuadtree(){
            Cuadrante test=Cuadrante.crearVacio(3);
            Cuadrante verif=test.setPixel(2,2,1);
            Assert.Equal(1,verif.getPixel(2,2));
            Assert.Equal(test.nivel,verif.nivel);
            
        }
        [Fact]
        public void testEqualQuadtree(){
            Cuadrante unoNW=Cuadrante.crear(1);
            Cuadrante unoNE=Cuadrante.crear(0);
            Cuadrante unoSW=Cuadrante.crear(1);
            Cuadrante unoSE=Cuadrante.crear(0);

            Cuadrante dosNW=Cuadrante.crear(1);
            Cuadrante dosNE=Cuadrante.crear(0);
            Cuadrante dosSW=Cuadrante.crear(1);
            Cuadrante dosSE=Cuadrante.crear(0);

         
            Cuadrante uno=Cuadrante.crear(unoNW,unoNE,unoSW,unoSE);
             Cuadrante dos=Cuadrante.crear(dosNW,dosNE,dosSW,dosSE);
            Assert.Equal(uno,dos);
            
            Almacen almacen=new Almacen(16,0.6);
            almacen.add(uno,dos);
            Assert.Equal(almacen.get(uno),dos);

            Cuadrante tresNW=Cuadrante.crear(1);
            Cuadrante tresNE=Cuadrante.crear(0);
            Cuadrante tresSW=Cuadrante.crear(1);
            Cuadrante tresSE=Cuadrante.crear(0);

            Cuadrante tres=Cuadrante.crear(tresNW,tresNE,tresSW,tresSE);
            Assert.Equal(almacen.get(tres),dos);

        }
    [Fact] 
    public void segundoTestGeneracionEtapa2(){
        ConwayCanvas conway=new ConwayCanvas();
       

          File.Rle testmatrix = new File.Rle("Patterns/3enginecordership.rle");
        bool[][] matrix=testmatrix.GetMatrix();
        Console.WriteLine("Iniciando segundo test Generacion etapa 2");
        Cuadrante test=Cuadrante.crear(matrix);
        Console.WriteLine("Antes de expandir");
        test.print(); 
     
         
          test=test.expandir();
        for (int i=0;i<50;i++){
            
        
               
                test=test.expandir();
            
            
            //Console.WriteLine("ITERACION " + (i+1));
            
            test=test.generacion();
        }    
        
        
        

        
        Console.WriteLine("EXPANDIDO ********************************+++++++++++");
        test.print();
         Console.WriteLine("Acabando test etapa 2");
        Console.WriteLine("Nivel final:" +test.nivel);
        Console.WriteLine("Finalizando segundo test Generacion etapa 4");

    }
    
       [Fact] 
    public static void segundoTestGeneracionEtapa4(){
        ConwayCanvas conway=new ConwayCanvas();
            
          
         File.Vaca testmatrix = new File.Vaca("../../../edna.txt");
        //File.Rle testmatrix = new File.Rle("../../../gosperglidergun.rle");
        bool[][] matrix=testmatrix.GetMatrix();
        Cuadrante test=Cuadrante.crear(matrix);
       
       test.print();
            double iteraciones = 0;
            Cuadrante centro;
        Cuadrante temp;
        int expandiendo=0;
        bool flag=false;
        var sw = Stopwatch.StartNew();
        for (int i=0;i<3;i++){
            Console.WriteLine(!(test.getCuadranteCentral().isCentrado() && test.isCentrado()));
            while(!(test.getCuadranteCentral().isCentrado() && test.isCentrado())){
               
                Console.WriteLine("El bucle a empezado a ejecutarse");
                 

                   
                        test=test.expandir();
                     test.print();
                   

                Console.WriteLine("El buce ha acabado de ejecutarse. Estado " + test.getCuadranteCentral().isCentrado());
            }

                
                //Console.WriteLine("APUNTO DE ITERAR");
                // temp=test.getCuadranteCentral();
                //Console.WriteLine("Numero de celulas totales: " + test.celdasVivas);
                //Console.WriteLine("Celulas en el central: " + temp.celdasVivas);
                //centro=temp.getCuadranteCentral();
                //Console.WriteLine("Celulas en el central del central" + centro.celdasVivas);

                //Console.WriteLine("Estado:" +test.getCuadranteCentral().isCentrado());
                //Console.WriteLine("estado del centor: " +temp.isCentrado());

                iteraciones +=(long) Math.Pow(2,test.nivel-2);
          test=test.generacionEtapa4();
                test.print();
                Console.WriteLine($"Acaba de iterar  nivel= {test.nivel+1}  iteraciones {iteraciones} celulasVivas={test.celdasVivas} tiempo={sw.Elapsed.TotalMilliseconds}");
          Console.WriteLine("Celulas vivas despues de iterar:" +test.celdasVivas);
        }
          sw.Stop();
            
  
           Console.WriteLine("Nivel:" +test.nivel);
      
        
       // test.print();
        Console.WriteLine("Nivel de iteraciones totales:  " + iteraciones);
         Console.WriteLine("Acabando test etapa 4");
        Console.WriteLine("Nivel final:" +test.nivel);
        Console.WriteLine("Finalizando segundo test Generacion etapa 4");
        Console.WriteLine($"Ha tardado {sw.Elapsed.TotalMilliseconds} milisegundos en iterar");

    }
    
    [Fact]
    public void testGeneracionEtapa2Basico(){
       var matrix = new bool[][]{
                new bool[]{false,false,true,true,true,false,false,false},
                new bool[]{false,false,false,true,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                 new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,true,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false}
            };

        
        Cuadrante test=Cuadrante.crear(matrix);
        Console.WriteLine("Antes de expandir");
        test.print(); 
        Console.WriteLine("Nivel:" +test.nivel);
        Console.WriteLine("Empezando test etapa 2 básico");
        
        test=test.expandir();
        test=test.expandir();
        Console.WriteLine("Antes de empezar a generar");
        test.print();
        test=test.generacion();
        
        test.print();
     Console.WriteLine("Acabando test etapa  2 básico");
     Console.WriteLine("Nivel final:" +test.nivel);
    }

    [Fact]
    public void testGeneracionEtapa4(){
       var matrix = new bool[][]{
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                 new bool[]{false,false,true,true,true,false,false,false},
                new bool[]{false,false,false,true,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false}
            };

        
        Cuadrante test=Cuadrante.crear(matrix);
        Console.WriteLine("Antes de expandir");
        test.print(); 
        Console.WriteLine("Nivel:" +test.nivel);
        Console.WriteLine("Empezando test etapa 4");
        
        test=test.expandir();
        test=test.generacionEtapa4();
        
        test.print();
     Console.WriteLine("Acabando test etapa 4");
     Console.WriteLine("Nivel final:" +test.nivel);
    }

    [Fact]
    public void TestGeneracionNivel2(){
          var matrix2 = new bool[][]{
               new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,true,false,false,false},
                 new bool[]{false,false,false,false,true,false,false,false},
                new bool[]{false,false,false,false,false,true,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false}
               
            };
            var matrix= new bool[][]{
               new bool[]{false,true,false,false},
                new bool[]{false,true,false,false},
                new bool[]{false,false,true,false},
                new bool[]{false,false,false,false}
               
            };
        
        Cuadrante test=Cuadrante.crear(matrix);
        //test=test.expandir();
        Cuadrante gen=test.generacion2();
        test.print();

        gen.print();
        Assert.Equal(1,gen.getPixel(0,0));
        Assert.Equal(1,gen.getPixel(0,1));
        Assert.Equal(1,gen.getPixel(1,0));
        Assert.Equal(1,gen.getPixel(1,1));
        Assert.Equal(1,gen.nivel);

    }
    [Fact]
    public void TestExpandir(){
        Cuadrante test=Cuadrante.crearVacio(1);
        test=test.setPixel(0,0,1);
        test=test.setPixel(0,1,1);
        test=test.setPixel(1,0,1);
        test=test.setPixel(1,1,1);
    
        Cuadrante expandido=test.expandir();


        int nivel=(int)Math.Pow(2,expandido.nivel);

        Assert.Equal(test.nivel+1,expandido.nivel);
        Assert.Equal(0,expandido.getPixel(0,0));
        Assert.Equal(0,expandido.getPixel(0,nivel-1));
        Assert.Equal(0,expandido.getPixel(nivel-1,0));
        Assert.Equal(0,expandido.getPixel(nivel-1,nivel-1));

        
    }
    [Fact]
    public void TestdivideEn9Cuadrados(){
        Cuadrante uno=Cuadrante.crearVacio(2);
        
        uno=uno.setPixel(1,1,1);
        uno=uno.setPixel(2,2,1); 
        uno=uno.setPixel(2,1,1);
        uno=uno.setPixel(1,2,1);
       
        Cuadrante vacio=Cuadrante.crearVacio(2);
        Cuadrante total=Cuadrante.crear(uno,uno,uno,uno);
         
        Cuadrante primero= total.divideEn9Cuadrados()[0];
        Cuadrante tercero=total.divideEn9Cuadrados()[2];
        Cuadrante septimo=total.divideEn9Cuadrados()[6];
        Cuadrante noveno=total.divideEn9Cuadrados()[8]; 
        Assert.Equal(1,tercero.getPixel(0,0));
        Assert.Equal(1,tercero.getPixel(0,1));
        Assert.Equal(1,tercero.getPixel(1,0));
        Assert.Equal(1,tercero.getPixel(1,1));
        
        Assert.Equal(1,primero.getPixel(0,0));
        Assert.Equal(1,primero.getPixel(0,1));
        Assert.Equal(1,primero.getPixel(1,0));
        Assert.Equal(1,primero.getPixel(1,1));

        Assert.Equal(1,septimo.getPixel(0,0));
        Assert.Equal(1,septimo.getPixel(0,1));
        Assert.Equal(1,septimo.getPixel(1,0));
        Assert.Equal(1,septimo.getPixel(1,1));
        

        Assert.Equal(1,noveno.getPixel(0,0));
        Assert.Equal(1,noveno.getPixel(0,1));
        Assert.Equal(1,noveno.getPixel(1,0));
        Assert.Equal(1,noveno.getPixel(1,1));
       
        
        uno=Cuadrante.crearVacio(2);
        Cuadrante dos=Cuadrante.crearVacio(2);
        uno=uno.setPixel(3,1,1);
        uno=uno.setPixel(3,2,1);
        uno=uno.setPixel(1,3,1);
        uno=uno.setPixel(2,3,1);

        dos=dos.setPixel(0,1,1);
        dos=dos.setPixel(0,2,1);
        dos=dos.setPixel(1,3,1);
        dos=dos.setPixel(2,3,1);
        
        Cuadrante tres=Cuadrante.crearVacio(2);
        tres=tres.setPixel(1,0,1);
        tres=tres.setPixel(2,0,1);
        tres=tres.setPixel(3,1,1);
        tres=tres.setPixel(3,2,1);

        Cuadrante cuatro=Cuadrante.crearVacio(2);
        cuatro=cuatro.setPixel(1,0,1);
        cuatro=cuatro.setPixel(2,0,1);
        cuatro=cuatro.setPixel(0,1,1);
        cuatro=cuatro.setPixel(0,2,1);

        total=Cuadrante.crear(uno,dos,tres,cuatro);
        Cuadrante segundo= total.divideEn9Cuadrados()[1];
        Cuadrante cuarto= total.divideEn9Cuadrados()[3];
        Cuadrante sexto=total.divideEn9Cuadrados()[5];
        Cuadrante octavo=total.divideEn9Cuadrados()[7]; 
        
        
        Assert.Equal(1,segundo.getPixel(0,0));
        Assert.Equal(1,segundo.getPixel(0,1));
        Assert.Equal(1,segundo.getPixel(1,0));
        Assert.Equal(1,segundo.getPixel(1,1));

        Assert.Equal(1,octavo.getPixel(0,0));
        Assert.Equal(1,octavo.getPixel(0,1));
        Assert.Equal(1,octavo.getPixel(1,0));
        Assert.Equal(1,octavo.getPixel(1,1));

        Assert.Equal(1,sexto.getPixel(0,0));
        Assert.Equal(1,sexto.getPixel(0,1));
        Assert.Equal(1,sexto.getPixel(1,0));
        Assert.Equal(1,sexto.getPixel(1,1));

        Assert.Equal(1,cuarto.getPixel(0,0));
        Assert.Equal(1,cuarto.getPixel(0,1));
        Assert.Equal(1,cuarto.getPixel(1,0));
        Assert.Equal(1,cuarto.getPixel(1,1));

        uno=Cuadrante.crearVacio(2);
        dos=Cuadrante.crearVacio(2);
        tres=Cuadrante.crearVacio(2);
        cuatro=Cuadrante.crearVacio(2);

        uno=uno.setPixel(3,3,1);
        dos=dos.setPixel(0,3,1);
        tres=tres.setPixel(3,0,1);
        cuatro=cuatro.setPixel(0,0,1);

        total=Cuadrante.crear(uno,dos,tres,cuatro);
  
        Cuadrante quinto= total.divideEn9Cuadrados()[4];

        Assert.Equal(1,quinto.getPixel(0,0));
        Assert.Equal(1,quinto.getPixel(0,1));
        Assert.Equal(1,quinto.getPixel(1,0));
        Assert.Equal(1,quinto.getPixel(1,1));

    }

    [Fact]
       public void TestCuadranteEquals(){
              Cuadrante unoNW=Cuadrante.crear(1);
            Cuadrante unoNE=Cuadrante.crear(0);
            Cuadrante unoSW=Cuadrante.crear(0);
            Cuadrante unoSE=Cuadrante.crear(0);
            Cuadrante uno=Cuadrante.crear(unoNW,unoNE,unoSW,unoSE);

            Cuadrante vacio= Cuadrante.crearVacio(1);

            Cuadrante verificar=Cuadrante.crear(uno,vacio,vacio,vacio);

            Assert.Equal(1,verificar.getPixel(0,0));
            Assert.Equal(0,verificar.getPixel(0,1));
            Assert.Equal(0,verificar.getPixel(0,2));

            Cuadrante dosNW=Cuadrante.crear(0);
            Cuadrante dosNE=Cuadrante.crear(0);
            Cuadrante dosSW=Cuadrante.crear(0);
            Cuadrante dosSE=Cuadrante.crear(1);
            Cuadrante dos=Cuadrante.crear(dosNW,dosNE,dosSW,dosSE);

            Cuadrante verificar2=Cuadrante.crear(vacio,vacio,vacio,dos);
            Assert.Equal(1,verificar2.getPixel(3,3));
            for (int i=0;i<4;i++){
                for (int j=0;j<4;j++){
                    if (i==j & j==3){
                          Assert.Equal(1,verificar2.getPixel(i,j));
                    }else{
                        Assert.Equal(0,verificar2.getPixel(i,j));
                    }
                }
            }

       }


        [Fact]
        public void TestGeneracion(){
            Cuadrante unoNW=Cuadrante.crear(1);
            Cuadrante unoNE=Cuadrante.crear(1);
            Cuadrante unoSW=Cuadrante.crear(0);
            Cuadrante unoSE=Cuadrante.crear(0);

            Cuadrante uno=Cuadrante.crear(unoNW,unoNE,unoSW,unoSE);

            Cuadrante primeraExpansion=uno.expandir();
              Cuadrante segundaExpansion=primeraExpansion.expandir();
            uno.print();
            
            segundaExpansion=segundaExpansion.setPixel(5,3,1);
            Cuadrante generado=segundaExpansion.generacion();
            primeraExpansion.print();
            segundaExpansion.print();
           
            Assert.True(segundaExpansion.isCentrado());
            generado.print();
            //uno.expandir().generacion().print();




        }

        [Fact]
        public void TestCrearMatrix()
        {
            string test = @"00000000
00100000
00100000
00100000
00000000
00000000
00000000
00000000
";
            var matrix = new bool[][]{
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,true,false,false,false,false,false},
                new bool[]{false,false,true,false,false,false,false,false},
                new bool[]{false,false,true,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false},
                new bool[]{false,false,false,false,false,false,false,false}
            };

            string test2 = @"0000
0010
0010
0010
";
            var matrix2 = new bool[][]{
                new bool[]{false,false,false,false},
                new bool[]{false,false,true,false},
                new bool[]{false,false,true,false},
                new bool[]{false,false,true,false}
            };
            Cuadrante cuadrante = Cuadrante.crear(matrix);
            Assert.Equal(test,cuadrante.ToString());

            Cuadrante cuadrante2 = Cuadrante.crear(matrix2);
            Assert.Equal(test2,cuadrante2.ToString());
            
        }

        [Fact]
        public void ConwayMatrixToBoolMatrix()
        {
            string test=@"0000000000000000000000001000000000000000000000000000000000000000
0000000000000000000000101000000000000000000000000000000000000000
0000000000001100000011000000000000110000000000000000000000000000
0000000000010001000011000000000000110000000000000000000000000000
1100000000100000100011000000000000000000000000000000000000000000
1100000000100010110000101000000000000000000000000000000000000000
0000000000100000100000001000000000000000000000000000000000000000
0000000000010001000000000000000000000000000000000000000000000000
0000000000001100000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
0000000000000000000000000000000000000000000000000000000000000000
";
            var rle = new Rle();
            rle.Load(@"Patterns/glider.rle");
            var matrix = rle.GetMatrix();
            var cuadrante = Cuadrante.crear(matrix);
            Assert.Equal(test,cuadrante.ToString());
        }
        [Fact]
        public void TestMatrix()
        {
            string test1 = @"....
....
....
....
";
            string test2 = @".....
.....
.....
.....
....X
";
            string test3 = @"X.....
......
......
......
......
.....X
";
            string test4 = @"X......
.......
.......
.......
......X
.....X.
";
            string test5 = @"X......
.......
.......
.......
......X
.....X.
....X..
";
            var matrix = new ConwayMatrix();
            matrix.SetSize(4,4);
            Assert.Equal(test1,matrix.ToString());
            matrix[4,4] = true;
            Assert.Equal(test2,matrix.ToString());
            matrix[-1,-1] = true;
            Assert.Equal(test3,matrix.GetFinalResult().LimitedMatrix.ToString());
            matrix[4,6] = true;
            Assert.Equal(test4,matrix.GetFinalResult().LimitedMatrix.ToString());
            matrix[6,4] = true;
            Assert.Equal(test5,matrix.GetFinalResult().LimitedMatrix.ToString());
        }
        
        [Fact]
        public void InsertarCuadrantesEnAlmacen(){
            var almacen = new Almacen(16,0.6);

            // es un still, no evoluciona más
            var m = new bool[][]{
                new bool[]{false,false,false,false},
                new bool[]{false,true,true,false},
                new bool[]{false,true,true,false},
                new bool[]{false,false,false,false}
            };
            var c = Cuadrante.crear(m);
            almacen.add(c,c);

            var f = almacen.get(c);
            Assert.Equal(f,c);

        }

        [Fact]
        public void IterateGlider(){
            //var rle = new Rle();
            //rle.Load(@"Patterns/glider.rle");
            //var matrix = rle.GetMatrix();
            var matrix = new bool[][]{
                new bool[]{false,false,false,false},
                new bool[]{false,true,true,true},
                new bool[]{false,false,false,false},
                new bool[]{false,false,false,false}
            };
            var cuadrante = Cuadrante.crear(matrix);
            Console.WriteLine($"Nivel: {cuadrante.nivel}");
            while(!(cuadrante.isCentrado() && cuadrante.getCuadranteCentral().isCentrado())){
                cuadrante = cuadrante.expandir();
            }
            cuadrante.print();
            Console.WriteLine($"Nivel: {cuadrante.nivel}");
            cuadrante = cuadrante.generacionEtapa4();
            cuadrante.print();
        }

        [Fact]
        public void BoolMatrixToConwayMatrix()
        {
            string test = @"...X
.X.X
....
....
";
            var matrix = new bool[][]{
                new bool[]{false,false,false,true},
                new bool[]{false,true,false,true},
                new bool[]{false,false,false,false},
                new bool[]{false,false,false,false}
            };
            var conway = new ConwayMatrix(matrix);
            Assert.Equal(test,conway.ToString());
        }

        [Fact]
        public void CuadranteToMatrix()
        {
            var matrix = new bool[][]{
                new bool[]{false,false,true,true},
                new bool[]{true,true,false,false},
                new bool[]{true,false,true,false},
                new bool[]{false,true,false,true}
            };
            var c = Cuadrante.crear(matrix);
            Assert.Equal(matrix,c.GetMatrix());
        }
    }


}