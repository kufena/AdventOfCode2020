using System;
using System.Collections.Generic;
using System.Text;

namespace Day20Go2
{
    public class Tile
    {

        public int num { get; set; }
        public Dictionary<int, TileState> states { get; set; }

        public Tile(int num)
        {
            this.num = num;
        }

        // Initialise a tile with all its sixteen states.
        public void Init(string[] rows)
        {
            states = new Dictionary<int, TileState>();
            int counter = 0;
            // no change
            TileState tsnorm = new TileState(counter, rows);
            states.Add(counter, tsnorm);
            counter += 1;

            // Flip Vert.
            rows = FlipVert(rows);
            TileState tsvert = new TileState(counter, rows);
            states.Add(counter, tsvert);
            counter += 1;

            // Flip vert and horizontal.
            rows = FlipHori(rows);
            TileState tsboth = new TileState(counter, rows);
            states.Add(counter, tsboth);
            counter += 1;

            // Flip back vert so just Horizontal.
            rows = FlipVert(rows);
            TileState tshori = new TileState(counter, rows);
            states.Add(counter, tshori);
            counter += 1;
            // put back to normal.
            rows = FlipHori(rows);

            // normal 90
            rows = Rotate(rows);
            TileState tsnorm90 = new TileState(counter, rows);
            states.Add(counter, tsnorm90);
            counter += 1;

            // + another 180
            rows = Rotate(Rotate(rows));
            TileState tsnorm270 = new TileState(counter, rows);
            states.Add(counter, tsnorm270);
            rows = Rotate(rows); // back to norm.
            counter += 1;

            // flipv + 90
            rows = Rotate(FlipVert(rows));
            TileState v90 = new TileState(counter, rows);
            states.Add(counter, v90);
            counter += 1;
            rows = FlipVert(rows); // undo flip now just 90.

            // fliph + 90
            rows = FlipHori(rows);
            TileState h90 = new TileState(counter, rows);
            states.Add(counter, h90);
        }

        // Some convenient one-off functions.
        public string reverseString(string s)
        {
            string rev = "";
            for (int i = s.Length - 1; i >= 0; i--)
                rev += $"{s[i]}";
            return rev;
        }

        public string[] FlipVert(string[] image)
        {
            int len = image[0].Length;
            string[] newrows = new string[len];
            for (int i = 0; i < len; i++)
            {
                newrows[i] = reverseString(image[i]);
            }
            return newrows;
        }

        public string[] FlipHori(string[] image)
        {
            int len = image[0].Length;
            string[] newimage = new string[len];
            for (int i = 0; i < len; i++)
            {
                newimage[(len - i) - 1] = image[i];
            }
            return newimage;

        }

        public string[] Rotate(string[] image)
        {
            int len = image[0].Length;
            string[] newimg = new string[len]; // assumes square.
            string line = "";
            int counter = 0;
            for (int j = len - 1; j >= 0; j--)
            {
                line = "";
                for (int i = 0; i < len; i++)
                {
                    line += image[i][j];
                }
                newimg[counter] = line;
                counter += 1;
            }
            return newimg;
        }
    }
}
