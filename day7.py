from statistics import median

def get_data():
    with open("data/day7") as f:
        return [int(x) for x in f.readline().strip().split(",")]

def part1(data: list[int]):
    m = median(data)
    return int(sum([abs(m - x) for x in data]))

def part2(data: list[int]):
    distances = {}
    for x in range(min(data), max(data) + 1, 1):
        distances[x] = 0

    for k in distances:
        for x in data:
            abs_distance = abs(k - x)
            distances[k] += int((1 + abs_distance) / 2 * abs_distance)

    return min([x for x in distances.values()])

data = get_data()
print(part1(data))
print(part2(data))
