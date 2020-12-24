using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int goes = Int32.Parse(args[1]);

            int x_current = 0;
            int y_current = 0;
            Dictionary<(int, int), Tile> visited = new Dictionary<(int, int), Tile>();

            var f = File.OpenRead(args[0]);
            var sr = new StreamReader(f);

            string line;

            while ((line = sr.ReadLine()) != null)
            {
                // OK, so each LINE identifies ONE tile.
                x_current = 0;
                y_current = 0;

                int i = 0;
                string sub = line;
                while (i < line.Length)
                {
                    sub = line.Substring(i);
                    if (sub.StartsWith("se"))
                    {
                        y_current -= 1;
                        x_current += 1;
                        i += 2;
                    }
                    if (sub.StartsWith("sw"))
                    {
                        y_current -= 1;
                        x_current -= 1;
                        i += 2;
                    }
                    if (sub.StartsWith("ne"))
                    {
                        y_current += 1;
                        x_current += 1;
                        i += 2;
                    }
                    if (sub.StartsWith("nw"))
                    {
                        y_current += 1;
                        x_current -= 1;
                        i += 2;
                    }
                    if (sub.StartsWith("w"))
                    {
                        x_current -= 2;
                        i += 1;
                    }
                    if (sub.StartsWith("e"))
                    {
                        x_current += 2;
                        i += 1;
                    }
                }

                if (visited.ContainsKey((x_current, y_current)))
                {
                    visited[(x_current, y_current)].flip();
                }
                else
                {
                    Tile t = new Tile(x_current, y_current, true);
                    visited.Add((x_current, y_current), t);
                    var toAdd = new Dictionary<(int, int), Tile>();
                    AddAdjacent(x_current, y_current, visited, toAdd);
                    foreach (var (p, tt) in toAdd) visited.Add(p, tt);

                }
            }

            // Part 1 Result.
            int counter = CountBlack(visited);
            WriteLine($"{counter} black tiles remain.");

            // Part 2 Calculation.
            for (int i = 0; i < goes; i++)
            {

                // Add adjacents where necessary to expand for next pass.
                var toAdd = new Dictionary<(int, int), Tile>();
                foreach (var t in visited)
                    AddAdjacent(t.Key.Item1, t.Key.Item2, visited, toAdd);
                foreach (var (p, t) in toAdd) visited.Add(p, t);

                // Fix all.
                // Assume neighbours already in visited list.  We've just fixed that!
                var newDictionary = new Dictionary<(int, int), Tile>();
                foreach (var (p, t) in visited)
                {
                    int x = CountNeighbours(p, t, visited);
                    if (t._colour && (x == 0 || x > 2))
                    {
                        var cp = t.Copy();
                        cp.flip();
                        newDictionary.Add(p, cp);
                    }
                    else if (!t._colour && x == 2)
                    {
                        var cp = t.Copy();
                        cp.flip();
                        newDictionary.Add(p, cp);
                    }
                    else
                        newDictionary.Add(p, t);
                }
                visited = newDictionary;

                counter = CountBlack(visited);
                WriteLine($"{counter} black tiles remain.");

            }
        }

        private static int CountBlack(Dictionary<(int, int), Tile> visited)
        {
            int counter = 0;
            foreach (var (coords, tile) in visited)
            {
                if (tile._colour)
                {
                    counter++;
                    //WriteLine($"({coords.Item1},{coords.Item2}) is black.");
                }
            }

            return counter;
        }

        private static void AddAdjacent(int x_current, int y_current, Dictionary<(int, int), Tile> visited, Dictionary<(int,int),Tile> toAdd)
        {
            // add adjacent tiles if not already there.
            if (!visited.ContainsKey((x_current - 1, y_current - 1))) // -1,-1
                toAdd.TryAdd((x_current - 1, y_current - 1), new Tile(x_current - 1, y_current - 1, false));
            if (!visited.ContainsKey((x_current - 1, y_current + 1))) // -1,1
                toAdd.TryAdd((x_current - 1, y_current + 1), new Tile(x_current - 1, y_current + 1, false));
            if (!visited.ContainsKey((x_current - 2, y_current))) // -2,0
                toAdd.TryAdd((x_current - 2, y_current), new Tile(x_current - 2, y_current, false));
            if (!visited.ContainsKey((x_current + 2, y_current))) // +2,0
                toAdd.TryAdd((x_current + 2, y_current), new Tile(x_current + 2, y_current, false));
            if (!visited.ContainsKey((x_current + 1, y_current - 1))) // 1, -1
                toAdd.TryAdd((x_current + 1, y_current - 1), new Tile(x_current + 1, y_current - 1, false));
            if (!visited.ContainsKey((x_current + 1, y_current + 1))) // 1, 1
                toAdd.TryAdd((x_current + 1, y_current + 1), new Tile(x_current + 1, y_current + 1, false));
        }

        private static int CountNeighbours((int,int) coord, Tile t, Dictionary<(int,int),Tile> visited)
        {
            int result = 0;

            result += CheckCoord((coord.Item1-1, coord.Item2-1), visited);
            result += CheckCoord((coord.Item1-1, coord.Item2+1), visited);
            result += CheckCoord((coord.Item1-2, coord.Item2), visited);
            result += CheckCoord((coord.Item1+2, coord.Item2), visited);
            result += CheckCoord((coord.Item1+1, coord.Item2-1), visited);
            result += CheckCoord((coord.Item1+1, coord.Item2+1),visited);

            return result;
        }

        private static int CheckCoord((int,int) coord, Dictionary<(int,int),Tile> visited)
        {
            if (visited.ContainsKey(coord))
                return visited[coord]._colour ? 1 : 0;
            return 0;
        }
    }
}
