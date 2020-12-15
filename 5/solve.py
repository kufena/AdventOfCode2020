ff = open('input','r')

highest = 0;

for line in ff:
  x = 0;
  for c in line:
    if (c == 'B' or c == 'R'):
      x = (x << 1) + 1  
    else:
      x = (x << 1) + 0
  x = x / 2
  print(str(line[:-1]) + ' ' + str(x))
  if (x > highest):
    highest = x

print(highest)
