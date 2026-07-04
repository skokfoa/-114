import sys
import heapq

class UnionFind:
    def __init__(self, size):
        self.parent = list(range(size))

    def find(self, i):
        if self.parent[i] == i:
            return i
        self.parent[i] = self.find(self.parent[i])
        return self.parent[i]

    def union(self, i, j):
        root_i = self.find(i)
        root_j = self.find(j)
        if root_i != root_j:
            self.parent[root_i] = root_j
            return True
        return False

class Path:
    def __init__(self, dot1, dot2, cost):
        self.dot1 = dot1
        self.dot2 = dot2
        self.cost = cost

    def __lt__(self, other):
        return self.cost < other.cost

def kruskal(paths, m):
    paths.sort()
    uf = UnionFind(m)
    mst_total_cost = 0
    edge_count = 0

    for p in paths:
        if uf.union(p.dot1, p.dot2):
            mst_total_cost += p.cost
            edge_count += 1
            if edge_count == m - 1:
                break
    return mst_total_cost

def prim(paths, m):
    adj = [[] for _ in range(m)]
    for p in paths:
        adj[p.dot1].append(p)
        adj[p.dot2].append(p)

    mst_total_cost = 0
    pq = []
    visit = set()

    for start in range(m):
        if start in visit:
            continue
        
        visit.add(start)
        for p in adj[start]:
            heapq.heappush(pq, p)

        while pq and len(visit) < m:
            now = heapq.heappop(pq)
            target_node = now.dot2 if now.dot1 in visit else now.dot1

            if target_node in visit:
                continue

            visit.add(target_node)
            mst_total_cost += now.cost

            for p in adj[target_node]:
                next_target = p.dot2 if p.dot1 == target_node else p.dot1
                if next_target not in visit:
                    heapq.heappush(pq, p)
    return mst_total_cost

def main():
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
            paths.append(Path(d1, d2, cost))
            total_original_cost += cost
            
        print("Choose Solution: 1. Kruskal, 2. Prim")
        solution = input("Your choice: ")
        
        if solution == "1":
            mst_cost = kruskal(paths, m)
        else:
            mst_cost = prim(paths, m)
            
        print(total_original_cost - mst_cost)

if __name__ == "__main__":
    main()