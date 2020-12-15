ff = open('input','r')
data = []

for line in ff:
    data.append(int(line))


print(len(data))

data.sort()
print(len(data))

x = 0
y = 0
z = 0

end = len(data)-1

for x in range(0,end):
    for y in range(0,end):
        for z in range(0,end):
           if data[z] + data[y] + data[x] == 2020:
               print(data[x])
               print(data[y])
               print(data[z])
               break;

