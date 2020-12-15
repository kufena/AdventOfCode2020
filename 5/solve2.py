ff = open('input','r')
tick = []

for i in range(0,1023):
  tick.append(False)

for line in ff:
  x = 0;
  for c in line:
    if (c == 'B' or c == 'R'):
      x = (x << 1) + 1  
    else:
      x = (x << 1) + 0
  x = int(x / 2)
  tick[x] = True
  print(str(line[:-1]) + ' ' + str(x))

for i in range(0,1024):
  if (not tick[i]):
    if (i > 0 and tick[i-1] and tick[i+1]):
      print(i)
