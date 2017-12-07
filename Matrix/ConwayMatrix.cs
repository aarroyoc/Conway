/* Arroyo Calle, Adrián
Bazaco Velasco, Daniel*/
using System;
using System.Collections.Generic;

namespace Conway.Matrix
{

    public class ConwayMatrix : ICloneable
    {
        private List<List<bool>> matrix;
        public ConwayMatrix()
        {
            matrix = new List<List<bool>>();
            this.OffsetX = 0;
            this.OffsetY = 0;
        }

        public ConwayMatrix(bool[][] m)
        {
            matrix = new List<List<bool>>();
            this.OffsetX = 0;
            this.OffsetY = 0;
            for (var i = 0; i < m.Length; i++)
            {
                for (var j = 0; j < m[i].Length; j++)
                {
                    this[i, j] = m[i][j];
                }
            }

        }

        public int Iterations = 0;

        public int OffsetX = 0;

        public int OffsetY = 0;

        public int Width
        {
            get
            {
                return this.matrix[0].Count;
            }
        }
        public int Height
        {
            get
            {
                return this.matrix.Count;
            }
        }

        public object Clone()
        {
            var c = new ConwayMatrix();
            var m = new List<List<bool>>();
            foreach (var l in this.matrix)
            {
                m.Add(new List<bool>(l));
            }
            c.matrix = m;
            c.OffsetX = this.OffsetX;
            c.OffsetY = this.OffsetY;
            return c;
        }

        public void Iterate()
        {
            this.Iterations++;
            var buffer = (ConwayMatrix)Clone();
            var x_off = buffer.OffsetX;
            var y_off = buffer.OffsetY;
            for (var x = -1; x < matrix.Count + 1; x++)
            {
                for (var y = -1; y < matrix[0].Count + 1; y++)
                {
                    var s = Convert.ToInt32(this[x - 1, y - 1]);
                    s += Convert.ToInt32(this[x - 1, y]);
                    s += Convert.ToInt32(this[x - 1, y + 1]);
                    s += Convert.ToInt32(this[x, y - 1]);
                    s += Convert.ToInt32(this[x, y + 1]);
                    s += Convert.ToInt32(this[x + 1, y - 1]);
                    s += Convert.ToInt32(this[x + 1, y]);
                    s += Convert.ToInt32(this[x + 1, y + 1]);

                    if (this[x, y])
                    {
                        if (!(s == 2 || s == 3))
                        {
                            buffer[x + buffer.OffsetX - this.OffsetX, y + buffer.OffsetY - this.OffsetY] = false;
                        }
                    }
                    if (!this[x, y] && (s == 3))
                    {
                        buffer[x + buffer.OffsetX - this.OffsetY, y + buffer.OffsetY - this.OffsetY] = true;
                    }
                }
            }
            this.matrix = buffer.matrix;
            this.OffsetX = buffer.OffsetX;
            this.OffsetY = buffer.OffsetY;

        }


        public void SetSize(int x, int y)
        {
            for (int i = matrix.Count; i < x; i++)
            {
                matrix.Add(new List<bool>());
                for (int j = matrix[i].Count; j < y; j++)
                {
                    matrix[i].Add(false);
                }
            }
        }
        public override string ToString()
        {
            var sb = string.Empty;
            foreach (var c in matrix)
            {
                foreach (var e in c)
                {
                    sb += e ? "X" : ".";
                }
                sb += System.Environment.NewLine;
            }
            return sb;
        }
        public bool this[int x, int y]
        {
            // fuera de la matriz siempre están muertas las celdas
            get
            {
                if (x >= 0 && y >= 0 && x < matrix.Count && y < matrix[x].Count)
                {
                    return matrix[x][y];
                }
                else
                {
                    return false;
                }
            }

            set
            {
                try
                {
                    matrix[x][y] = value;
                }
                catch (ArgumentOutOfRangeException)
                {
                    // la coordenada especificada no está implementada
                    // hacer crecer la matriz
                    int xplus = 0;
                    int yplus = 0;
                    if (x < 0 || y < 0)
                    {
                        // insert
                        matrix.Insert(0, new List<bool>());
                        this.OffsetX++;
                        this.OffsetY++;
                        // crear Y
                        for (int j = 0; j < matrix[1].Count; j++)
                        {
                            matrix[0].Add(false);
                        }
                        foreach (var m in matrix)
                        {
                            m.Insert(0, false);
                        }
                        xplus = 1;
                        yplus = 1;
                    }
                    else
                    {
                        // add
                        matrix.Add(new List<bool>());
                        // crear y
                        for (int j = 0; j < matrix[0].Count; j++)
                        {
                            matrix[matrix.Count - 1].Add(false);
                        }
                        foreach (var m in matrix)
                        {
                            m.Add(false);
                        }
                    }
                    this[x + xplus, y + yplus] = value;
                }
            }
        }
        public bool[][] GetMatrix()
        {
            var e = 0;
            while (Math.Pow(2, e) < Math.Max(this.matrix.Count, this.matrix[0].Count))
            {
                e++;
            }
            int n = (int)Math.Pow(2, e);
            bool[][] m = new bool[n][];
            for (var i = 0; i < m.Length; i++)
            {
                m[i] = new bool[n];
            }
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    m[i][j] = this[i, j];
                }
            }

            return m;
        }
        public ConwayMatrix LimitMatrix(int x1, int y1, int x2, int y2)
        {
            //Requisitos: x1,y1 menores que x2,y2 respectivamente.
            //Podría generalizarse fácilmente, pero como solo tiene un único uso no es necesario.
            var limitedMatrix = new ConwayMatrix();

            limitedMatrix.SetSize(x2 - x1 + 1, y2 - y1 + 1);

            int x = 0;//Establecen las coordenadas de la nueva matriz
            int y = 0;

            for (int i = x1; i < x2 + 1; i++)
            {
                y = 0;

                for (int j = y1; j < y2 + 1; j++)
                {

                    limitedMatrix[x, y] = this[i, j];
                    y++;

                }
                x++;
            }

            return limitedMatrix;
        }

        public FinalDataStruct GetFinalResult()
        { //Cambiar por Structs
            int celdasVivas = 0;
            int filaMinima = this.Height; //filaminima nunca va a ser mayor que la altura de la matriz.
            int filaMaxima = 0;
            int columnaMinima = this.Width;
            int columnaMaxima = 0;

            FinalDataStruct data = new FinalDataStruct();

            for (int x = 0; x < this.Height; x++)
            {
                //filas


                for (int y = 0; y < this.Width; y++)
                {
                    //Columnas

                    bool alive = this[x, y];
                    if (alive == true)
                    {

                        celdasVivas++;
                        if (y < columnaMinima)
                        {
                            columnaMinima = y;
                        }
                        if (y > columnaMaxima)
                        {
                            columnaMaxima = y;
                        }
                        if (x < filaMinima)
                        {
                            filaMinima = x;
                        }
                        if (x > filaMaxima)
                        {
                            filaMaxima = x;
                        }
                    }
                }
            }


            data.CeldasVivas = celdasVivas;
            data.LimitedMatrix = this.LimitMatrix(filaMinima, columnaMinima, filaMaxima, columnaMaxima);


            return data;



        }
    }
}