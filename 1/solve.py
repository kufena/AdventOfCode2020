ff = open('input','r')
data = []

for line in ff:
    data.append(int(line))


print(len(data))

data.sort()
print(len(data))

x = 0
y = len(data)-1

while(x < y):
    if (data[x] + data[y] == 2020):
        print(data[x])
        print(data[y])
        break
    if (data[x] + data[y] > 2020):
        y = y - 1
    else:
        x = x + 1

