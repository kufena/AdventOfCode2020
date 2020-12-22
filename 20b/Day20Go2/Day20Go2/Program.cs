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

            // Read the monster file - produce a list of relative
            // points to look for '#' characters.

            var g = File.OpenRead(args[1]);
            StreamReader tr = new StreamReader(g);
            // for the monster.
            List<(int, int)> monster = new List<(int, int)>();
            int qwerty = 0;
            string line;
            int monsterlength = 0;
            while((line = tr.ReadLine()) != null)
            {
                if (line.Length > monsterlength)
                    monsterlength = line.Length;

                for(int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '#')
                        monster.Add((i, qwerty));
                }
                qwerty += 1;
            }
            int monsterlines = qwerty;

            // read and parse input file.

            line = "";
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

            //
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
                                
                                // CONVENIENT ASSUMPTION.
                                // In fact we find the input only ever has one result, which makes life easier.
                                // Otherwise we'd have to dedupe and create new Tile/State pairings.
                                if (result.Count == 1)
                                    statematch.AddTarget(k2, state2, result[0]);
                            }
                        }
                    }
                }
            }

            //
            // We want the corners next = those with two matches.

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

            //
            // Now search for Tile/TileState pairs that have left-right and bottom-top matches, which make
            // them top left hand corners of the grid.

            List<(Tile, TileState)> topleftcorners = new List<(Tile, TileState)>();
            foreach (var (t, s) in corners)
            {
                var matches = scores[(t, s)].matches;
                if (matches.ContainsKey((TileState.FlipState.Left, TileState.FlipState.Right)) && matches.ContainsKey((TileState.FlipState.Bottom, TileState.FlipState.Top)))
                {
                    topleftcorners.Add((t, s));
                }
            }

            //
            // Create a grid for each top left corner then search for monsters.
            foreach(var (t,s) in topleftcorners)
            { 
                WriteLine($"corner potential -- {t.num} {s.id}");

                //
                // We start at the top left and keep going looking for left to right matches,
                // then when we get to the end we go down to the next layer and start on the
                // next line of the grid.

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

                //
                // If we haven't set an illegal flag (we never do!) then build a block of text and
                // search for monsters.

                if (!illegalcol && !illegalrow)
                {
                    //
                    // Build the block of text, stripping edges, concatening and stacking.
                    // We don't need to do the rotating/flipping as each TileState already has the
                    // text of it's block as required.
                    string[] block = new string[0];
                    for(int i = 0; i < gridsize; i++)
                    {
                        var rowblock = new string[tilesize-2]; // remember, we're stripping edges!
                        for (int k = 0; k < tilesize-2; k++) rowblock[k] = "";
                        for(int j = 0; j < gridsize; j++)
                        {
                            var (n, st) = grid[j,i];
                            var ijblock = images[n.num].states[st.id].stripEdges();
                            rowblock = TileState.concat(rowblock, ijblock);
                        }
                        block = TileState.vertstack(block, rowblock);
                    }

                    //
                    // Count hashes in the block of text.  The number of untouch hashes as the end
                    // will be the number of monsters found times the number of points in the monster,
                    // subtracted from the number of hashes we find now.
                    // Note this won't work, potentially for monsters that have vertical symmetry as
                    // two monsters may overlap.  Similarly for vertical symmetry I guess.  Haven't
                    // thought this through but I know what I mean.
                    int hashcount = 0;
                    for (int i = 0; i < block.Length; i++)
                    {
                        for (int j = 0; j < block[i].Length; j++)
                            if (block[i][j] == '#') hashcount += 1;

                        WriteLine("     " + block[i]);
                    }

                    //
                    // we know how many hashes there are, lets count the monsters.
                    int monstercount = 0;
                    for(int i = 0; i < block.Length-monsterlines; i++)
                    {
                        for(int j = 0; j < block[i].Length - monsterlength; j++)
                        {
                            int ppp = 0;
                            foreach(var (x,y) in monster)
                            {
                                if (block[i + y][j+x] == '#')
                                    ppp += 1;
                            }
                            if (ppp == monster.Count)
                                monstercount += 1;
                        }
                    }

                    //
                    // Result for this grid is calculated and output.
                    int rty = monstercount * monster.Count;
                    Console.WriteLine($"Found {monstercount} monsters, so {hashcount} hashes to start, minus {rty} removed, leaving {hashcount - rty}");

                }

                ReadLine();
            }

            // Done!
        }
    }
}
