ff = open('input','r')

ok = 0
bad = 0

for line in ff:
    splits = line.split(' ')
    rangesplits = splits[0].split('-')
    rangelow = int(rangesplits[0])
    rangehig = int(rangesplits[1])
    pchar = splits[1][0]
    count = 0
    for c in splits[2]:
        if (c == pchar):
            count = count + 1
    if (count >= rangelow and count <= rangehig):
        ok = ok + 1
        print("OK " + line)
    else:
        bad = bad + 1
        print("BAD " + line)

print(ok)
print(bad)

