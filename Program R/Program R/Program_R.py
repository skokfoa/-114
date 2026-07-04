import heapq
import math

class Node:
    def __init__(self, x, y, sum_val, father):
        self.x = x
        self.y = y
        self.sum = sum_val
        self.father = father

    def __lt__(self, other):
        return self.sum < other.sum

def main():
    try:
        input_str = input().split()
        while not input_str:
            input_str = input().split()
            
        n = int(input_str[0])
        m = int(input_str[1])
        
        maze = []
        for i in range(n):
            row_input = input().split()
            maze.append([int(x) for x in row_input])
            
        min_cost = [[math.inf] * m for _ in range(n)]
        
        pq = []
        
        start_node = Node(0, 0, 0, None)
        heapq.heappush(pq, start_node)
        min_cost[0][0] = 0
        
        while pq:
            now = heapq.heappop(pq)
            
            if now.x == n - 1 and now.y == m - 1:
                print(now.sum)
                return
                
            if now.sum > min_cost[now.x][now.y]:
                continue
                
            if now.x + 1 < n:
                next_sum = now.sum + maze[now.x + 1][now.y]
                if next_sum < min_cost[now.x + 1][now.y]:
                    min_cost[now.x + 1][now.y] = next_sum
                    heapq.heappush(pq, Node(now.x + 1, now.y, next_sum, now))
                    
            if now.y + 1 < m:
                next_sum = now.sum + maze[now.x][now.y + 1]
                if next_sum < min_cost[now.x][now.y + 1]:
                    min_cost[now.x][now.y + 1] = next_sum
                    heapq.heappush(pq, Node(now.x, now.y + 1, next_sum, now))
                    
        print("No path found")
        
    except EOFError:
        pass

if __name__ == '__main__':
    main()
