using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

WriteLine("Hello day 15, here we go.");

var f = File.OpenRead(args[0]);
var sr = new StreamReader(f);

Dictionary<int,List<int>> goes = new Dictionary<int, List<int>>();

string line = sr.ReadLine();
var splits = line.Split(',');
int[] allnums = new int[30000000];

int counter = 0;
for(int i = 0; i < splits.Length; i++) {
    int starter = Int32.Parse(splits[i]);
    if (goes.ContainsKey(starter)) {
        WriteLine($"{counter}:: {starter}");
        goes[starter].Add(counter);
    }
    else 
    {
        WriteLine($"{counter}:: {starter} is NEW");
        goes.Add(starter,new List<int>{counter});
    }

    allnums[counter] = starter;
    counter++;
}

int guess = 0;
var before = new Dictionary<int,int>();

while(counter < 30000000) {

    //Write($"{counter}::");

    int lastnum = allnums[counter-1];
    List<int> goers = goes[lastnum];
    
    if (goers.Count == 1) {
        guess = 0;
    }
    else {
        int i = goers.Count - 1;
        guess = goers[i] - goers[i-1];
    }

    //Write($"counter:: new guess is {guess}");

    if (goes.ContainsKey(guess)) {
        //WriteLine($" which we've seen before");
        goes[guess].Add(counter);
    }
    else 
    {
        //WriteLine($" and {guess} is NEW");
        goes.Add(guess, new List<int>{counter});
    }

    allnums[counter] = guess;
    counter++;
}

WriteLine($"Final guess is {guess} - last guess was {allnums[2019]} and before that {allnums[counter-2]}");
