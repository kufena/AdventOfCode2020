using System;
using System.Collections.Generic;
using System.Text;

namespace Day20Go2
{
    class TileStateMatchList
    {
        Tile tile;
        TileState tilestate;

        public Dictionary<(TileState.FlipState, TileState.FlipState), (Tile, TileState)> matches;
        public bool illegal;

        public TileStateMatchList(Tile t, TileState s)
        {
            illegal = false;
            matches = new Dictionary<(TileState.FlipState, TileState.FlipState), (Tile, TileState)>();
            tile = t;
            tilestate = s;
        }

        public void AddTarget(Tile t, TileState s, (TileState.FlipState, TileState.FlipState) match)
        {
            if (matches.ContainsKey(match))
            {
                throw new Exception($"{TileState.MatchStr(match)} already found");
            }
            else
            {
                matches.Add(match,(t, s));
            }
        }
    }
}
