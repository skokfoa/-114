import sys

def find_min_cost(maze, min_cost, x, y):
    n = len(maze)
    m = len(maze[0])

    # 超出邊界，回傳無限大
    if x >= n or y >= m:
        return float('inf')

    # 到達終點
    if x == n - 1 and y == m - 1:
        return maze[x][y]

    # 如果這個格子已經計算過了，回傳紀錄值
    if min_cost[x][y] != -1:
        return min_cost[x][y]

    # 遞迴計算往下跟右的最小花費
    cost_go_down = find_min_cost(maze, min_cost, x + 1, y)
    cost_go_right = find_min_cost(maze, min_cost, x, y + 1)

    # 找出兩條路徑中的最小值
    min_next_cost = min(cost_go_down, cost_go_right)

    # 紀錄當前格子的最小花費
    if min_next_cost == float('inf'):
        min_cost[x][y] = float('inf')
    else:
        min_cost[x][y] = maze[x][y] + min_next_cost

    return min_cost[x][y]

def main():
    # 讀取輸入
    input_str = input().split()
    if not input_str:
        return
    
    n = int(input_str[0])
    m = int(input_str[1])

    maze = []

    #讀取迷宮
    for i in range(n):
        row_input = list(map(int, input().split()))
        maze.append(row_input)

    min_cost = [[-1 for _ in range(m)] for _ in range(n)] # 初始化最小花費矩陣，-1 表示尚未計算 

    # 計算從 (0, 0) 到 (n-1, m-1) 的最小花費
    result = find_min_cost(maze, min_cost, 0, 0)
    print(result)

if __name__ == '__main__':
    main()