from statistics import median

def get_data():
    with open("data/day10") as f:
        return [x.strip() for x in f.readlines()]

close_map = {'}': '{', ')': '(', ']': '[', '>': '<'}
invalid_score_map = {'}': 1197, ')': 3, ']': 57, '>': 25137}
def validate(s: str):
    validator = []

    for c in s:
        if c in '{(<[':
            validator.append(c)
        else:
            vc = validator.pop()
            if close_map[c] != vc:
                return invalid_score_map[c], []

    return 0, validator

score_map = {'{': 3, '(': 1, '[': 2, '<': 4}
def close_score(to_close: list):
    score = 0 
    while len(to_close) != 0:
        score = score * 5 + score_map[to_close.pop()]

    return score

def part1(data: list[str]):
    return sum([validate(s)[0] for s in data])

def part2(data: list[str]):
    with_validity = [validate(x) for x in data]
    incomplete = [v for s, v in with_validity if s == 0]
    scores = [close_score(v) for v in incomplete]
    return median(scores)

data = get_data()
print(part1(data))
print(part2(data))