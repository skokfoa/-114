import sys
import heapq

# 找尋根節點的函式(輔助 union 函式找尋迴圈)
def find(parent, i):
    if parent[i] == i:
        return i
    parent[i] = find(parent, parent[i])
    return parent[i]

def union(parent, i, j):
    root_i = find(parent, i)
    root_j = find(parent, j)
    if root_i != root_j:
        parent[root_i] = root_j
        return True
    return False

# Kruskal 演算法
def kruskal(paths, m):
    paths.sort()
    parent = list(range(m))
    mst_total_cost = 0
    edge_count = 0

    for cost, dot1, dot2 in paths:
        if union(parent, dot1, dot2):
            mst_total_cost += cost
            edge_count += 1
            if edge_count == m - 1:
                break
    return mst_total_cost

# Prim 演算法
def prim(paths, m):
    adj = [[] for _ in range(m)]
    for cost, dot1, dot2 in paths:
        adj[dot1].append((cost, dot2))
        adj[dot2].append((cost, dot1))

    mst_total_cost = 0
    pq = []
    visit = set()

    for start in range(m):
        if start in visit:
            continue
        
        visit.add(start)
        for edge in adj[start]:
            heapq.heappush(pq, edge)

        while pq and len(visit) < m:
            cost, target_node = heapq.heappop(pq)

            if target_node in visit:
                continue

            visit.add(target_node)
            mst_total_cost += cost

            for next_cost, next_target in adj[target_node]:
                if next_target not in visit:
                    heapq.heappush(pq, (next_cost, next_target))
                    
    return mst_total_cost

def main():
    # 讀取輸入
    line = sys.stdin.readline()
    if not line: return
    t = int(line.strip())
    
    for _ in range(t):
        input_data = sys.stdin.readline().split()
        if not input_data: break
        m, n = map(int, input_data)
        
        paths = []
        total_original_cost = 0
        
        for _ in range(n):
            d1, d2, cost = map(int, sys.stdin.readline().split())
            paths.append((cost, d1, d2))
            total_original_cost += cost
            
        print("Choose Solution: 1. Kruskal, 2. Prim")
        solution = input("Your choice: ")
        
        if solution == "1":
            mst_cost = kruskal(paths, m)
        else:
            mst_cost = prim(paths, m)
            
        # 輸出省下的總成本
        print(total_original_cost - mst_cost)

if __name__ == "__main__":
    main()