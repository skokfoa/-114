using System;
using System.Data.SqlTypes;

class Program
{
    static void Main(string[] args)
    {
        Random rand = new Random();
        List<(int, int)> p1, p2, p3, p4;
        p1 = new List<(int, int)>();
        p2 = new List<(int, int)>();
        p3 = new List<(int, int)>();
        p4 = new List<(int, int)>();

        HashSet<int> first = new HashSet<int>();
        while (first.Count < 4)
        {
            int n = rand.Next(1, 5);
            first.Add(n);
        }

        p1.Add((first.ElementAt(0), 7));
        p2.Add((first.ElementAt(1), 7));
        p3.Add((first.ElementAt(2), 7));
        p4.Add((first.ElementAt(3), 7));

        while(p1.Count < 13)
        {
            int f = rand.Next(1, 5);
            int num = rand.Next(1, 14);

            if (p1.Contains((f, num))) continue;
            if (p2.Contains((f, num))) continue;
            if (p3.Contains((f, num))) continue;
            if (p4.Contains((f, num))) continue;

            p1.Add((f, num));
        }

        while (p2.Count < 13)
        {
            int f = rand.Next(1, 5);
            int num = rand.Next(1, 14);

            if (p1.Contains((f, num))) continue;
            if (p2.Contains((f, num))) continue;
            if (p3.Contains((f, num))) continue;
            if (p4.Contains((f, num))) continue;

            p2.Add((f, num));
        }

        while (p3.Count < 13)
        {
            int f = rand.Next(1, 5);
            int num = rand.Next(1, 14);

            if (p1.Contains((f, num))) continue;
            if (p2.Contains((f, num))) continue;
            if (p3.Contains((f, num))) continue;
            if (p4.Contains((f, num))) continue;

            p3.Add((f, num));
        }

        while (p4.Count < 13)
        {
            int f = rand.Next(1, 5);
            int num = rand.Next(1, 14);

            if (p1.Contains((f, num))) continue;
            if (p2.Contains((f, num))) continue;
            if (p3.Contains((f, num))) continue;
            if (p4.Contains((f, num))) continue;

            p4.Add((f, num));
        }

        Console.WriteLine("一號    二號    三號    四號");
        for(int i = 0; i < 13; i++)
        {

            Console.WriteLine($"{change(p1.ElementAt(i).Item1)},{p1.ElementAt(i).Item2,-2} " +
                $"{change(p2.ElementAt(i).Item1)},{p2.ElementAt(i).Item2,-2} " +
                $"{change(p3.ElementAt(i).Item1)},{p3.ElementAt(i).Item2,-2} " +
                $"{change(p4.ElementAt(i).Item1)},{p4.ElementAt(i).Item2,-2} ");
        }
        Console.ReadKey();
    }

    private static string change(int num)
    {
        switch (num)
        {
            case 1:
                return "黑梅";
                break;
            case 2:
                return "紅磚";
                break;
            case 3:
                return "紅心";
                break;
            case 4:
                return "黑桃";
                break;
        }
        return "";
    }
}