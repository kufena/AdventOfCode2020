ff = open('input','r')

ok = 0
bad = 0

for line in ff:
    splits = line.split(' ')
    rangesplits = splits[0].split('-')
    posone = int(rangesplits[0]) - 1
    postwo = int(rangesplits[1]) - 1
    pchar = splits[1][0]
    count = 0
    posoneok = (splits[2][posone]==pchar)
    postwook = (splits[2][postwo]==pchar)
    if (posoneok and postwook):
        bad = bad + 1
        print("BAD " + line)
    else:
        if (posoneok or postwook):
            ok = ok + 1
            print("OK " + line)
        else:
            bad = bad + 1
            print("BAD " + line)

print(ok)
print(bad)

