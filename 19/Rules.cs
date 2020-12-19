using System;
using System.Collections.Generic;
using System.IO;

namespace _19
{
    public class Rules {
        public enum Kind { Char, Choice, Seq };

        public int num;
        public char c;

        public List<int> tomatch;

        public Kind kind;
    }
}
