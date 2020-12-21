using System;
using System.Collections.Generic;
using System.Text;

namespace Day20Go2
{
    public partial class TileState
    {

        public string top;
        public string bottom;
        public string left;
        public string right;

        public string[] rows;

        public int id;

        public TileState(int id, string[] rows)
        {
            this.id = id;
            this.rows = rows;
            CalculateSides(rows[0].Length);
        }

        // We assume rows is fixed now - immutable.
        // And in their final state, so we find the sides ok.
        private void CalculateSides(int len)
        {
            top = rows[0];
            bottom = rows[len - 1];
            left = findEdge(rows, x => x[0]);
            right = findEdge(rows, x => x[x.Length - 1]);
        }

        private string findEdge(string[] image, Func<string, char> fn)
        {
            string result = "";
            for (int i = 0; i < image.Length; i++)
            {
                result += $"{fn(image[i])}";
            }
            return result;
        }

        public List<(FlipState, FlipState)> matchState(TileState state)
        {
            var result = new List<(FlipState, FlipState)>();

            //if (state.bottom == bottom) result.Add( (FlipState.Bottom, FlipState.Bottom));
            if (state.bottom == top)
            {
                //Console.WriteLine($"{state.bottom} == {top}");
                result.Add((FlipState.Top, FlipState.Bottom));
            }
            //if (state.bottom == left) result.Add((FlipState.Bottom, FlipState.Left));
            //if (state.bottom == right) result.Add((FlipState.Bottom, FlipState.Right));

            if (state.top == bottom)
            {
                result.Add((FlipState.Bottom, FlipState.Top));
                //Console.WriteLine($"{state.top} == {bottom}");
            }
            //if (state.top == top) result.Add((FlipState.Top, FlipState.Top));
            //if (state.top == left) result.Add((FlipState.Top, FlipState.Left));
            //if (state.top == right) result.Add((FlipState.Top, FlipState.Right));

            //if (state.left == bottom) result.Add((FlipState.Left, FlipState.Bottom));
            //if (state.left == top) result.Add((FlipState.Left, FlipState.Top));
            //if (state.left == left) result.Add((FlipState.Left, FlipState.Left));
            if (state.left == right)
            {
                result.Add((FlipState.Right, FlipState.Left));
                //Console.WriteLine($"{state.left} == {right}");
            }
            //if (state.right == bottom) result.Add((FlipState.Right, FlipState.Bottom));
            //if (state.right == top) result.Add((FlipState.Right, FlipState.Top));
            if (state.right == left)
            {
                result.Add((FlipState.Left, FlipState.Right));
                //Console.WriteLine($"{state.right} == {left}");
            }
            //if (state.right == right) result.Add((FlipState.Right, FlipState.Right));

            return result;
        }

        public static string MatchStr((FlipState, FlipState) t)
        {
            var (s1, s2) = t;
            return s1.ToString() + "-" + s2.ToString();
        }

        public string[] stripEdges()
        {
            int len = rows[0].Length;
            string[] result = new string[len - 2];
            for (int i = 1; i < len - 1; i++)
            {
                result[i - 1] = rows[i].Substring(1, len - 2);
            }
            return result;
        }

        public static string[] concat(string[] rows, string[] newrows)
        {
            if (newrows.Length != rows.Length)
            {
                throw new Exception("mismatched row lengths");
            }
            int len = newrows.Length;
            string[] result = new string[len];
            for(int i = 0; i < len; i++)
                result[i] = newrows[i] + rows[i];
            return result;
        }

        public static string[] vertstack(string[] one, string[] two)
        {
            string[] result = new string[one.Length + two.Length];
            for(int i = 0; i < one.Length; i++)
            {
                result[i] = one[i];
            }
            for(int j = 0; j < two.Length; j++)
            {
                result[one.Length + j] = two[j];
            }
            return result;
        }
    }
}
