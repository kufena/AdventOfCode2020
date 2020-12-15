using System;
using System.IO;
using System.Collections.Generic;

Console.WriteLine("Hello world");

var f = File.OpenRead(args[0]);
StreamReader streamReader= new StreamReader(f);

string line;
int i = 0;
Dictionary<int,Instruction> dict = new Dictionary<int, Instruction>();

while ((line = streamReader.ReadLine()) != null) {
    var splits = line.Split();
    if (splits.Length > 2)
      throw new Exception("Error too many splits? " + line);
    int n = Int32.Parse(splits[1]);
    var instruction = new Instruction {
        op = splits[0],
        arg = n,
        visited = false,
        position = i
    };
    dict.Add(i, instruction);
    i++;

}

var numops = i - 1;


for(int key = 0; key < numops; key++) {
    foreach(var pair in dict) {
        pair.Value.visited = false;
    }

    // swap jmp to nop or nop to jmp if we need to.
    string oldop = dict[key].op;
    if (dict[key].op == "jmp") {
        dict[key].op = "nop";
    }
    else if (dict[key].op == "nop") {
        dict[key].op = "jmp";
    } else
      continue;

    bool term = false;

    i = 0;
    int acc = 0;
    while(true) {
        if (i > numops) {
            Console.WriteLine($"About to terminate - acc is {acc}");
            term = true;
            break;
        }

        var nextOp = dict[i];
        if (nextOp.visited) {
            Console.WriteLine($"Accumulater at first already visited op is {acc}");
            break;
        }

        switch (nextOp.op) {
            case "jmp":
            i += nextOp.arg;
            break;
            case "nop":
            i += 1;
            break;
            case "acc":
            acc += nextOp.arg;
            i += 1;
            break;
            default:
            throw new Exception($"Unexpected op {nextOp.arg}");
        }

        nextOp.visited = true;
    }

    // put old op back and do next.
    dict[key].op = oldop;

}

public record Instruction {
    public string op;
    public int arg;
    public bool visited;
    public int position;
}