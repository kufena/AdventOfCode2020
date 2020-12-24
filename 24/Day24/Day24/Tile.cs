using System;
using System.Collections.Generic;
using System.Text;

namespace Day24
{
    public class Tile
    {
        public int _x;
        public int _y;
        public bool _colour;

        public Tile(int x, int y, bool colour )
        {
            // Let's not allow illegal states.
            if (x % 2 == 0 && y % 2 == 1)
                throw new Exception($"violates tile protocol {x} {y}");
            if (x % 2 == 1 && y % 2 == 0)
                throw new Exception($"violates tile protocol {x} {y}");
            _x = x;
            _y = y;
            _colour = colour;
            //Console.WriteLine($"Created tile {_x} {_y} of colour {colour}");
        }

        public void flip()
        {
            bool tmp = _colour;
            _colour = !_colour;
            //Console.WriteLine($"Flipped {_x} {_y} from {tmp} to {_colour}");
        }

        public Tile Copy()
        {
            return new Tile(_x, _y, _colour);
        }
    }
}
