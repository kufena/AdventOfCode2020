starts = ['byr','iyr','eyr','hgt','hcl','ecl','pid']

def checkline(found):
    print(found)
    print(starts)
    print(' ')
    x = len(found)
    if ('cid' in found):
      x -= 1
    if (x ==  7):
      return 1
    else:
      return 0

def intbtw(val, x, y):
  if (len(val) != 4):
    return False
  try:
    n = int(val)
    if (n < x or n > y):
      return False;
  except exp:
    return False
  return True;

def checksplit(key,val):
  if (key == 'byr'):
    return intbtw(val,1920,2002)
  if (key == 'iyr'):
    return intbtw(val,2010,2020)
  if (key == 'eyr'):
    return intbtw(val,2020,2030)
  if (key == 'hgt'):
    try:
      unit = val[-2:]
      size = val[0:-2]
      print(val + ' ' + unit + ' ' + size)
      n = int(size)
      if (unit == 'cm'):
        return not (n < 150 or n > 193)
      if (unit == 'in'):
        return not (n < 59 or n > 76)
      return False
    except:
      return False
  if (key == 'hcl'):
    bret = val.startswith('#')
    bret = bret and (len(val)==7)
    for c in val[1:]:
      bret = bret and (c in ['a','b','c','d','e','f','0','1','2','3','4','5','6','7','8','9'])
    return bret
  if (key == 'ecl'):
    return (val in ['amb','blu','brn','gry','grn','hzl','oth'])
  if (key == 'pid'):
    if (len(val)==9):
      try:
        n = int(val)
        return True
      except:
        return False
    return False
  if (key == 'cid'):
    return True
  return False

ff = open('input','r')
ports = 0
valid = 0
invalid = 0
blank = False
found = []

for line in ff:
  line = line[0:-1]
  if (len(line)==0):
    valid += checkline(found)
    blank = True
    found = []
  else:
    blank = False
    splits = line.split(' ')
    for pair in splits:
      subsplit = pair.split(':')
      if (checksplit(subsplit[0],subsplit[1])):
        found.append(subsplit[0])  

if (not blank):
  valid += checkline(found)

print(ports)
print(valid)
print(invalid)
