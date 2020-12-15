ff = open('input','r')

count = 0;
pos = {'a':0,
        'q':1,
        'r':2,
        's':3,
        't':4,
        'u':5,
        'v':6,
        'w':7,
        'x':8,
        'y':9,
        'z':10,
        'b':11,
        'c':12,
        'd':13,
        'e':14,
        'f':15,
        'g':16,
        'h':17,
        'i':18,
        'j':19,
        'k':20,
        'l':21,
        'm':22,
        'n':23,
        'o':24,
        'p':25}


localc = 0;
linec = 0;
arr = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
endl = False

for line in ff:
  line = line[:-1]
  print(line)
  endl = False
  if (len(line)!=0):
    linec += 1
    for c in line:
      n = pos[c]
      arr[n] += 1
  else:
    endl = True
    for i in range(0,26):
      if arr[i] == linec:
        localc += 1
    print(line + ' ' + str(localc))
    count += localc
    localc = 0
    linec = 0
    arr = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
    
if (not endl):
  for i in range(0,26):
    if arr[i] == linec:
      localc += 1
  print(line + ' ' + str(localc))
  count += localc

print(count)
