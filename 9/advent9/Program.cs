using System;
using System.IO;
using System.Collections.Generic;

var file = File.OpenRead(args[0]);
var stream = new StreamReader(file);

string line = stream.ReadLine();
int preamble = Int32.Parse(line);
line = stream.ReadLine();
int answer = Int32.Parse(line);

Console.WriteLine("Looking for contiguous set of ints adding to " + answer);

/*
int[] block = new int[preamble];
for(int i = 0; i < preamble; i++) {
    block[i] = Int32.Parse(stream.ReadLine());
}

int position = 0;
while((line = stream.ReadLine()) != null) 
{
    int value = Int32.Parse(line);

    // Check sums = we only need to find one.
    bool found = false;
    for(int i = 0; i < preamble; i++)
      for(int j = i+1; j < preamble; j++) {
          if (block[i] + block[j] == value) {
              found = true;
              break;
          }
      }
    

    if (!found) {
        for(int i = 0; i < preamble; i++)
          Console.Write($"{block[i]} ");
        Console.WriteLine($"Value = {value}");
        break;
    }
    // Update position in ring buffer.
    block[position] = value;
    position = (position + 1) % preamble;

}
*/

List<long> allnums = new List<long>();
while((line = stream.ReadLine()) != null) {
    long n = Int64.Parse(line);
    allnums.Add(n);
}

for(int i = 0; i < allnums.Count; i++) {
    int j = i + 1;
    bool found = false;
    long total = allnums[i];
    long greatest = allnums[i];
    long smallest = allnums[i];

    Console.Write("Checking " + allnums[i] + " ");
    while (j < allnums.Count) {
        if (allnums[j] > greatest) greatest = allnums[j];
        if (allnums[j] < smallest) smallest = allnums[j];
        total += allnums[j];
        Console.Write(" " + allnums[j]);

        if (total == answer) {
            found = true;
            Console.WriteLine(" It's true! " + total);
            Console.WriteLine("Greatest is " + greatest + " and smallest is " + smallest);
            Console.Write($"{allnums[i]} + {allnums[j]}");
            break;
        }
        if (total > answer) {
            Console.Write(" It's too big " + total);
            break;
        }
        j += 1;
    }
    Console.WriteLine("");
    if (found)
      break;
}
