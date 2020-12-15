ff = open('input','r')
ports = 0
valid = 0
invalid = 0
blank = False
starts = ['byr','iyr','eyr','hgt','hcl','ecl','pid']
found = []

for line in ff:
  
  if (len(line)==1):
    ports += 1
    print(found)
    print(starts)
    print(' ')
    x = len(found)
    if ('cid' in found):
      x -= 1
    if (x ==  7):
      valid += 1
    else:
      invalid += 1
    blank = True
    found = []
  else:
    blank = False
    splits = line.split(' ')
    for pair in splits:
      subsplit = pair.split(':')
      found.append(subsplit[0])  

if (not blank):
  ports += 1
  print(found)
  print(starts)
  print(' ')
  x = len(found)
  if ('cid' in found):
    x -= 1
  if (x ==  7):
    valid += 1
  else:
    invalid += 1

print(ports)
print(valid)
print(invalid)
