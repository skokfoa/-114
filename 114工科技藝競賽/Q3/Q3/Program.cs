using System;
using System.ComponentModel.DataAnnotations;

class Program
{
    static void Main(string[] args)
    {
        int n = 0;
        Console.WriteLine("請輸入節點數 N:");
        n = int.Parse(Console.ReadLine().Trim());
        Console.WriteLine("請輸入 N-1 行血緣關係 (每行兩個整數，以空格分隔):");
        List<(int, int)> path = new List<(int, int)>();
        int max = int.MinValue;
        int min = int.MaxValue;

        List<List<Node>> allPath = new List<List<Node>>();

        for(int i = 0; i < n - 1; i++)
        {
            int[] a = Console.ReadLine()
                .Trim()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            max = Math.Max(max, a[0]);
            max = Math.Max(max, a[1]);

            min = Math.Min(min, a[0]);
            min = Math.Min(min, a[1]);

            path.Add((a[0], a[1]));
            path.Add((a[1], a[0]));
        }

        int maxcount = int.MinValue;
        for(int i = min; i <= max; i++)
        {
            for(int j = i; j <= max; j++)
            {
                maxcount = Math.Max(maxcount, count(new Node(i), j, path));
            }
        }

        Console.WriteLine("===============");
        Console.WriteLine($"輸出：最遠血緣距離為{maxcount}");
        Console.ReadKey();
    }
    public static int count (Node start, int end, List<(int, int)> path)
    {
        Queue<Node> q = new Queue<Node>();
        HashSet<int> visit = new HashSet<int>();
        Node result = null;
        q.Enqueue(start);
        visit.Add(start.num);
        while(q.Count > 0)
        {
            Node now = q.Dequeue();
            if (now.num == end)
            {
                result = now;
                break;
            }

            List<int> next = new List<int>();
            for(int i = 0; i < path.Count; i++)
            {
                if (path.ElementAt(i).Item1 == now.num) next.Add(path.ElementAt(i).Item2);
            }

            foreach(var a in next)
            {
                if (!visit.Contains(a))
                {
                    visit.Add(a);
                    q.Enqueue(new Node(a, now));
                }
            }
        }

        int count = 0;
        if (result == null) return 0;

        while (result.father != null)
        {
            count++;
            result = result.father;
        }

        return count;
    }
}

class Node
{
    public int num { get; set; }
    public Node father { get; set; }

    public Node(int num, Node father = null)
    {
        this.num = num;
        this.father = father;
    }
}