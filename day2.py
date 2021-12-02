def get_data():
    moves = []
    with open("data/day2") as f:
        for ln in f.readlines():
            split = ln.split(' ')
            moves.append((split[0], int(split[1].strip())))

    return moves

def part1(moves):
    x, y = 0, 0

    for dir, units in moves:
        if dir == "up":
            y -= units
        elif dir == "down":
            y += units
        elif dir == "forward":
            x += units
        else:
            raise Exception('unknown dir')

    return x * y

def part2(moves):
    x, y, aim = 0, 0, 0

    for dir, units in moves:
        if dir == "up":
            aim -= units
        elif dir == "down":
            aim += units
        elif dir == "forward":
            x, y = x + units, y + units * aim
        else:
            raise Exception('unknown dir')

    return x * y

data = get_data()
print(part1(data))
print(part2(data))
