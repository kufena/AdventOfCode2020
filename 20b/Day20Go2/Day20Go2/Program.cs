using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;

namespace Day20Go2
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            string line;
            var f = File.OpenRead(args[0]);
            StreamReader sr = new StreamReader(f);
            int tilesize = 10;
            SortedDictionary<int, Tile> images = new SortedDictionary<int, Tile>();
            line = sr.ReadLine();
            while (line != null)
            {
                // Tile <num>:
                var numstr = line.Trim().Substring(5, line.Trim().Length - 6);
                int num = Int32.Parse(numstr);
                WriteLine($"Getting image {num}");
                // ten lines:
                string[] image = new string[10];
                for (int i = 0; i < 10; i++)
                {
                    line = sr.ReadLine().Trim();
                    image[i] = line;
                }

                Tile t = new Tile(num);
                t.Init(image);
                images.Add(num, t);

                // blank
                line = sr.ReadLine();
                line = sr.ReadLine();
            }
            
            int gridsize = (int) Math.Sqrt(images.Count);

            // So now we have tiles and all their states, we need to find matches i guess.
            Dictionary<(Tile, TileState), TileStateMatchList> scores = new Dictionary<(Tile, TileState), TileStateMatchList>();

            foreach (var (n1, k1) in images)
            {
                foreach (var (v1, state1) in k1.states)
                {
                    var statematch = new TileStateMatchList(k1, state1);
                    scores.Add((k1, state1), statematch);
                    foreach (var (n2, k2) in images)
                    {
                        if (n1 != n2)
                        {
                            foreach (var (v2, state2) in k2.states)
                            {
                                var result = state1.matchState(state2);
                                if (result.Count == 1)
                                    statematch.AddTarget(k2, state2, result[0]);
                            }
                        }
                    }
                }
            }

            List<(Tile, TileState)> corners = new List<(Tile, TileState)>();

            foreach(var ((til,sta),mlist) in scores)
            {
                if (mlist.matches.Count == 2 && !corners.Contains((til, sta)))
                {
                    corners.Add((til, sta));
                }

                WriteLine($"{til.num} - state {sta.id} has matches::");
                foreach(var (match,(ttil, tsta)) in mlist.matches)
                {
                    WriteLine($"  {TileState.MatchStr(match)} == {ttil.num} state {tsta.id}");
                }
            }

            WriteLine();

            List<(Tile, TileState)> topleftcorners = new List<(Tile, TileState)>();
            foreach (var (t, s) in corners)
            {
                var matches = scores[(t, s)].matches;
                if (matches.ContainsKey((TileState.FlipState.Left, TileState.FlipState.Right)) && matches.ContainsKey((TileState.FlipState.Bottom, TileState.FlipState.Top)))
                {
                    topleftcorners.Add((t, s));
                }
            }

            foreach(var (t,s) in topleftcorners)
            { 
                WriteLine($"corner potential -- {t.num} {s.id}");

                Dictionary<Tile, TileState> seen = new Dictionary<Tile, TileState>();
                var store = (t, s);
                var first = (t, s); // scores[(t, s)].matches[(TileState.FlipState.Right, TileState.FlipState.Left)];
                (Tile, TileState) leftmost = (null,null);
                bool illegalrow = false;
                bool illegalcol = false;

                (Tile, TileState)[,] grid = new (Tile, TileState)[gridsize,gridsize];
                for (int i = 0; i < gridsize; i++)
                {
                    for (int j = 0; j < gridsize-1; j++)
                    {
                        grid[j, i] = first;
                        //WriteLine($"We are at {first.t.num} - {first.s.id} for ({i},{j})");
                        var matches = scores[first].matches;
                        if (!matches.ContainsKey((TileState.FlipState.Right, TileState.FlipState.Left)))
                        {
                            if (leftmost != (null, null))
                            {
                                illegalrow = true;
                                break;
                            }
                        } else if (matches[(TileState.FlipState.Right, TileState.FlipState.Left)] != leftmost)
                        {
                            illegalrow = true;
                            break;
                        }
                        if (!matches.ContainsKey((TileState.FlipState.Left, TileState.FlipState.Right)))
                        {
                            illegalrow = true;
                            break;
                        }
                        leftmost = first;
                        first = matches[(TileState.FlipState.Left, TileState.FlipState.Right)];
                    }

                    //WriteLine($"That last line (i, {gridsize-1}) is {first.t.num}-{first.s.id}");
                    grid[gridsize-1, i] = first;

                    // we've done a row.
                    if (illegalrow)
                        break;

                    if (i < gridsize-1 && !scores[store].matches.ContainsKey((TileState.FlipState.Bottom, TileState.FlipState.Top)))
                    {
                        illegalcol = true;
                        break;
                    }

                    if (i < gridsize-1)
                    {
                        var tmp = scores[store].matches[(TileState.FlipState.Bottom, TileState.FlipState.Top)];
                        store = tmp;
                        first = tmp;
                        leftmost = (null, null);
                    }
                }

                if (!illegalcol && !illegalrow)
                {
                    string[] block = new string[0];
                    for(int i = 0; i < gridsize; i++)
                    {
                        var rowblock = new string[tilesize]; // -2]; // remember, we're stripping edges!
                        for (int k = 0; k < gridsize; k++) rowblock[k] = "";
                        for(int j = 0; j < gridsize; j++)
                        {
                            var (n, st) = grid[j,i];
                            var ijblock = images[n.num].states[st.id].rows; // stripEdges();
                            rowblock = TileState.concat(rowblock, ijblock);
                        }
                        block = TileState.vertstack(block, rowblock);
                    }

                    for (int i = 0; i < block.Length; i++)
                        WriteLine("     " + block[i]);
                }

                ReadLine();
            }

        }
    }
}
