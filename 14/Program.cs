using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

WriteLine("hello - let's go!");

WriteLine(0 | 0x00008000);
WriteLine(1 | 0x00008000);

var f = File.OpenRead(args[0]);
var sr = new StreamReader(f);

part2(sr);

void part2(StreamReader sr) {
    string line;
    string mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
    Dictionary<ulong,ulong> dict = new Dictionary<ulong, ulong>();

    while ((line = sr.ReadLine()) != null) {

        if (line.StartsWith("mask")) {
            mask = line.Split("=")[1].Trim();
            WriteLine($"New Mask = |{mask}|");
        }
        else {
            var eqsplits = line.Split("=");
            foreach(var s in eqsplits) Write($" {s}");
            WriteLine();
            var splits = eqsplits[0].Split(new char[] {'[',']'});
            foreach(var s in splits) Write($" {s}");
            WriteLine();
            ulong m = 0;
            ulong n = 0;
            if (!UInt64.TryParse(splits[1], out m)) {
                throw new Exception("rubbish " + line);
            }
            if (!UInt64.TryParse(eqsplits[1], out n)) {
                throw new Exception("rubbish " + line);
            }
            WriteLine($"Call on {n} and {m} with {mask}");
            updateandmanipulate(35, 0, n, m, mask, dict);

        }
    }

    ulong total = 0;
    foreach(var (a,b) in dict)
        total += b;

    WriteLine("RESULT IS " + total);
}

void updateandmanipulate(int index, ulong acc, ulong m, ulong n, string mask, Dictionary<ulong,ulong> dict) {

    if (index == -1)
    {
        WriteLine($"Updating {acc} with {m}");
        if (dict.ContainsKey(acc)) dict[acc] = m;
        else dict.Add(acc, m);
        return;
    }

    if (mask[index] == '1') {
        //WriteLine($"adding {createmask(index)}:{printbinary(createmask(index))} to acc at index {index}");
        acc += createmask(index);
        updateandmanipulate(index-1, acc, m, n, mask, dict);
    }

    if (mask[index] == '0') {
        //WriteLine($"A 0 at {index} yields adding {createmask(index) & n} to {acc} {printbinary(createmask(index))} vs {printbinary(n)}");
        acc += createmask(index) & n;
        updateandmanipulate(index-1, acc, m, n, mask, dict);
    }

    if (mask[index] == 'X') {
        //WriteLine($"A split to {index-1} with {acc} and {acc + createmask(index)}");
        updateandmanipulate(index-1, acc, m, n, mask, dict);
        updateandmanipulate(index-1, acc + createmask(index), m, n, mask, dict);
    }
}

ulong createmask(int x) {
    ulong m = 1;
    for(int i = 34-x; i > -1; i--)
        m = m << 1;
    return m;
}
void part1(StreamReader sr) {
    string line;
    string mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
    Dictionary<ulong,ulong> dict = new Dictionary<ulong, ulong>();

    while ((line = sr.ReadLine()) != null) {

        if (line.StartsWith("mask")) {
            mask = line.Split("=")[1].Trim();
            WriteLine($"New Mask = |{mask}|");
        }
        else {
            var eqsplits = line.Split("=");
            foreach(var s in eqsplits) Write($" {s}");
            WriteLine();
            var splits = eqsplits[0].Split(new char[] {'[',']'});
            foreach(var s in splits) Write($" {s}");
            WriteLine();
            ulong m = 0;
            ulong n = 0;
            if (!UInt64.TryParse(splits[1], out m)) {
                throw new Exception("rubbish " + line);
            }
            if (!UInt64.TryParse(eqsplits[1], out n)) {
                throw new Exception("rubbish " + line);
            }
            ulong res = manipulate(n, mask);
            WriteLine($"memloc = {m} set to {n}:{printbinary(n)} --> {res}:{printbinary(res)}");
            if (dict.ContainsKey(m)) dict[m] = res;
            else dict.Add(m,res);
        }
    }

    WriteLine();
    WriteLine();
    WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");


    ulong asdf = 0;
    foreach(var (a,b) in dict) {
        WriteLine($"{a} -> {b}");
        asdf += b;
    }

    WriteLine($"Result is {asdf}");
}

static ulong manipulate(ulong m, string mask) {
    WriteLine(mask);
    ulong res = m;
    ulong imask = 0xFFFFFFF000000001;
    ulong invimask = 0xFFFFFFFFFFFFFFFE;
    ulong reset = 0x0000000FFFFFFFFF;
    ulong invreset = 0xFFFFFFF000000000;
    
    for(int i = 35; i > -1; i--) {
        //WriteLine($"{i} {imask} {invimask} {imask & invimask} {mask[i]}");
        if (mask[i] != 'X') {
            if (mask[i] == '1') {
                //Write($"{mask} as {printbinary(imask)} on {printbinary(res)} ");
                res = (res | imask);
                //WriteLine($" to {printbinary(res)}");
            }
            else
                res = (res & invimask);
        }
        imask = (imask << 1) | invreset;
        invimask = (invimask << 1) + 1;
    }
    return res & reset;
}

static string printbinary(ulong x) {
    ulong iqmask = 0x0000000800000000;
    string res = "";
    for(int i = 35; i > -1; i--) {
        ulong t = x & iqmask;
        res += (t > 0 ? "1" : 0);
        iqmask = (iqmask >> 1);
    }
    return res;
}