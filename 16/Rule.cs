using System;
using System.IO;
using System.Collections.Generic;

using static System.Console;

namespace _16
{

    public record Rule {

        public string name;
        public List<(int,int)> constraints;
    }
}