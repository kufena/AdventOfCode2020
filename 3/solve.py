ff = open('input','r')
print(9 % 8)
x = 0;
l = 0;
trees = 0
miss = False

for line in ff:
    if (not miss):
      print(line)
      if (line[x] == '#'):
          print('tree')
          trees = trees + 1
      x = x + 1;
      print(len(line))
      x = x % (len(line)-1)
      l = l + 1
      print(x)
    miss = not miss

print('lines ')
print(l)
print('trees ')
print(trees)

