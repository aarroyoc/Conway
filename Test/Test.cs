/* Arroyo Calle, Adrián
Bazaco Velasco, Daniel*/
using Xunit;
using Conway.File;
using Conway.Matrix;

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
    }
}