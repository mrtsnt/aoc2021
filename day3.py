import operator

def get_data():
    with open("data/day3") as f:
        return [int(x, 2) for x in f.readlines()]

def get_freq(nums):
    freq = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]

    for n in nums:
        for p in range(12):
            if (1 << p) & n > 0:
                freq[p] += 1
    
    return freq

def part1(nums):
    freq = get_freq(nums)

    gamma = 0
    for p, f in enumerate(freq):
        if f > 500:
            gamma |= 1 << p

    return gamma * (0b111111111111 ^ gamma)

def filter_nums(nums, cmp):
    cp = nums.copy()

    pos = 11
    while len(cp) != 1:
        freq = get_freq(cp)

        if cmp(2 * freq[pos], len(cp)):
            cp = [n for n in cp if n & (1 << pos) > 0]
        else:
            cp = [n for n in cp if n & (1 << pos) == 0]

        pos -= 1

    return cp[0]

def part2(nums):
    o2 = filter_nums(nums, operator.ge)
    co2 = filter_nums(nums, operator.lt)

    return o2 * co2

data = get_data()
print(part1(data))
print(part2(data))