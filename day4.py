class Board:
    def __init__(self, txt):
        self._nums = [ln.split() for ln in txt.split('\n')]
        self._marks = [[False for x in range(5)] for x in range(5)]

    def _calc_score(self, num):
        unmarked_total = 0
        for rp, r in enumerate(self._marks):
            for cp, c in enumerate(r):
                if not c:
                    unmarked_total += int(self._nums[rp][cp])

        return unmarked_total * int(num)

    def mark(self, num):
        for rp, r in enumerate(self._nums):
            for cp, c in enumerate(r):
                if c == num:
                    self._marks[rp][cp] = True
                    if all(self._marks[rp]) or all([x[cp] for x in self._marks]):
                        return self._calc_score(num)

        return None

    def reset(self):
        self._marks = [[False for x in range(5)] for x in range(5)]
        return self

def get_data():
    with open("data/day4") as f:
        objs = f.read().split('\n\n')
        nums = objs[0].split(',')
        boards = [Board(x) for x in objs[1:]]
        
        return nums, boards

def part1(nums: list[str], boards: list[Board]):
    for n in nums:
        for b in boards:
            score = b.mark(n)
            if score: return score

    return None

def part2(nums: list[str], boards: list[Board]):
    for n in nums:
        boards = [b for b in boards if not b.mark(n)]
        if len(boards) == 1:
            break
    
    final_board = boards[0].reset()
    for n in nums:
        score = final_board.mark(n)
        if score:
            return score

    return None

nums, boards = get_data()
print(part1(nums, boards))
nums, boards = get_data()
print(part2(nums, boards))