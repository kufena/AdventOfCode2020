using System;
using System.IO;
using System.Collections.Generic;

using static System.Console;

WriteLine("Hello All! Let's go!");

var file = File.OpenRead(args[0]);
StreamReader sr = new StreamReader(file);

List<long> nums = new List<long>();
string line;
while ((line = sr.ReadLine()) != null) {
    long x = Int64.Parse(line);
    nums.Add(x);
}

nums.Sort();
var dict = new Dictionary<long, long>();
for(int i = nums.Count - 1; i>=0; i--) {
  long myn = nums[i];
  if (i + 1 == nums.Count)
    dict.Add(myn, 1);
  else {
      long ways = 0;
      var stk = new Stack<long>();
      for(int j = i + 1; j < nums.Count; j++) {
          if (nums[j] - myn <= 3) {
              stk.Push(nums[j]);
          }
          else {
              break;
          }
      }
      long m = 0;
      while (stk.TryPop(out m)) {
          ways += dict[m];
      }
      if (ways == 0)
      {
          WriteLine("Nothing reachable so setting to 1!");
           ways = 1;
      }
      dict.Add(myn, ways);
  }
}

long result = 0;
for(int i = 0; i < nums.Count; i++) {
  if (nums[i] <= 3) {
    WriteLine($"Num {nums[i]} has {dict[nums[i]]} options.");
    result += dict[nums[i]];
  }
  else
  {
    break;
  }
}

WriteLine("Ways is " + result);
