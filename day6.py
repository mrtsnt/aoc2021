def get_data():
    with open("data/day6") as f:
        row = f.readline().strip()
        fish_map = {}
        for f in [int(f) for f in row.split(",")]:
            if f not in fish_map: fish_map[f] = 0
            fish_map[f] += 1
        return fish_map

def solve(fish, days):
    old_gen = fish
    for _ in range(days):
        new_gen = {}
        new_gen.setdefault(6, 0)
        new_gen.setdefault(8, 0)

        for k, v in old_gen.items():
            new_gen[k - 1] = v

        new_gen[6] += new_gen.get(-1, 0)
        new_gen[8] += new_gen.get(-1, 0)
        new_gen[-1] = 0
        old_gen = new_gen

    return sum(old_gen.values())

data = get_data()
print(solve(data,80))
print(solve(data,256))