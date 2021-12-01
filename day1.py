def get_data():
    with open("data/day1") as f:
        return [int(x) for x in f.readlines()]

def part1(nums: list[int]):
    count = 0
    for i in range(1, len(nums)):
        if nums[i - 1] <  nums[i]:
            count += 1

    return count

def part2(nums: list[int]):
    count = 0
    for i in range(1, len(nums) - 2):
        if sum(nums[i - 1:i + 2]) < sum(nums[i:i + 3]):
            count += 1

    return count

data = get_data()
print(part1(data))
print(part2(data))
