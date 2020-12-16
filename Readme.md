# Advent of Code 2020 Solutions.

These are my implementations in C# for the various advent of code
exercises.  Actually the first six days were completed in python.

The code is a bit ragged - most take the input in a file, passing the
file name as an argument.  Some don't.  There are no unit tests, but
there are test inputs, but mostly without the answer.  Hopefully I will
clean this up.

In various cases, the code is only the second part, since I overwrote the
first part.  I got a bit better at this later on.

Some of them have a part1 and part2 method.  Some have several part1 or
part2 implementations (a,b,c etc) - even some have code that works or
doesn't work commented out.  Again, might clean this up.  The worst example
of this is the one with the chinese remainder theorem solution, which I
did three times; once with eight threads, once as a sieve, and when I
eventually understood how to use the theorem, there's a third implementation
which completed in seconds.  Much better.
