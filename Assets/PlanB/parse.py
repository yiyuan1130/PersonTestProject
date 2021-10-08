from os import write
import sys
import json

base_path = sys.path[0]
file_path = base_path + '\\base.pdb'
pdb_file = open(file_path, mode='r')
lines = pdb_file.readlines()
splited_lines = []
for line in lines:
    if line.startswith('ATOM'):
        items = line.split()
        splited_lines.append(items)
        # print(items)
        # break

datas = []
for line in splited_lines:
    x = float(line[5])
    y = float(line[6])
    z = float(line[7])
    n = line[-1]
    data = [x, y, z, n]
    datas.append(data)

# for data in datas:
#     print(data)

json_data = json.dumps(datas)

output_path = base_path + '\\data.json'
output_file = open(output_path, mode="w+")
output_file.write(json_data)
output_file.flush()
output_file.close()
