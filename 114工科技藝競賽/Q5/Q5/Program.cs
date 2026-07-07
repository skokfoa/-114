using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

class Box
{
    public double x1, y1, x2, y2, score, weight;

    public Box(double x1, double y1, double x2, double y2, double score, double weight)
    {
        this.x1 = x1; this.y1 = y1; this.x2 = x2; this.y2 = y2;
        this.score = score; this.weight = weight;
    }

    public override string ToString()
    {
        return $"x1={x1:F2}, y1={y1:F2}, x2={x2:F2}, y2={y2:F2}, score={score:F2}, weight={weight:F2}";
    }
}

class Program
{
    static void Main()
    {
        string inputPath = @".\boxes.txt";
        string outputPath = @".\fused_boxes.txt";

        // 1) 讀取 boxes
        List<Box> boxes = ReadBoxes(inputPath);

        // 2) 讀 IoU 閾值
        double iouThr = ReadIoUThreshold();

        // 3) 做 WBF 融合
        List<Box> fused = WeightedBoxesFusion(boxes, iouThr);

        // 4) 螢幕輸出
        Console.WriteLine();
        Console.WriteLine($"IoU 閾值 : {iouThr:F2}");
        Console.WriteLine($"融合後的框數量 : {fused.Count}");
        for (int i = 0; i < fused.Count; i++)
        {
            Console.WriteLine($"框 {i + 1}: {fused[i]}");
        }

        // 5) 寫檔（失敗只顯示錯誤，不影響螢幕輸出）
        try
        {
            WriteFusedBoxes(outputPath, fused, iouThr);
            Console.WriteLine();
            Console.WriteLine($"輸出檔案 {Path.GetFileName(outputPath)} 產生完成。");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"[寫檔失敗] {ex.Message}");
        }
    }

    static List<Box> ReadBoxes(string path)
    {
        var list = new List<Box>();

        try
        {
            using var sr = new StreamReader(path);
            string? line;
            int lineNo = 0;

            while ((line = sr.ReadLine()) != null)
            {
                lineNo++;
                line = line.Trim();
                if (line.Length == 0) continue;

                // 允許多個空白
                string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 6)
                {
                    Console.WriteLine($"[略過] 第 {lineNo} 行欄位不足：{line}");
                    continue;
                }

                // 用 InvariantCulture，避免小數點逗號問題
                if (!TryParseDouble(parts[0], out double x1) ||
                    !TryParseDouble(parts[1], out double y1) ||
                    !TryParseDouble(parts[2], out double x2) ||
                    !TryParseDouble(parts[3], out double y2) ||
                    !TryParseDouble(parts[4], out double score) ||
                    !TryParseDouble(parts[5], out double weight))
                {
                    Console.WriteLine($"[略過] 第 {lineNo} 行有無法解析的數字：{line}");
                    continue;
                }

                // 基本合理性檢查（題目說 x1 < x2, y1 < y2）
                if (!(x1 < x2 && y1 < y2))
                {
                    Console.WriteLine($"[略過] 第 {lineNo} 行座標不合法(x1<x2,y1<y2)：{line}");
                    continue;
                }

                list.Add(new Box(x1, y1, x2, y2, score, weight));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[讀檔失敗] {ex.Message}");
        }

        return list;
    }

    static bool TryParseDouble(string s, out double v)
        => double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v);

    static double ReadIoUThreshold()
    {
        Console.Write("請輸入 IoU 閾值 (0, 1]，例如 0.5 或 0.8 (輸入無效值將使用預設值 0.5) : ");
        string? input = Console.ReadLine();

        double iouThr = 0.5;
        if (input != null &&
            double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsed) &&
            parsed > 0 && parsed <= 1)
        {
            iouThr = parsed;
        }
        else
        {
            Console.WriteLine("無效輸入，使用預設值 IoU = 0.5");
        }

        return iouThr;
    }

    // ---- IoU 計算 ----
    static double IoU(Box a, Box b)
    {
        double interX1 = Math.Max(a.x1, b.x1);
        double interY1 = Math.Max(a.y1, b.y1);
        double interX2 = Math.Min(a.x2, b.x2);
        double interY2 = Math.Min(a.y2, b.y2);

        double interW = interX2 - interX1;
        double interH = interY2 - interY1;

        double interArea = 0.0;
        if (interW > 0 && interH > 0)
            interArea = interW * interH;

        double areaA = (a.x2 - a.x1) * (a.y2 - a.y1);
        double areaB = (b.x2 - b.x1) * (b.y2 - b.y1);
        double union = areaA + areaB - interArea;

        if (union <= 0) return 0.0;
        return interArea / union;
    }

    // ---- WBF (greedy clustering) ----
    static List<Box> WeightedBoxesFusion(List<Box> input, double iouThr)
    {
        // 題目：未被融合的框保留為獨立框
        // 實作：每次取 score 最大當種子，把 IoU>=thr 的都拉進同一群
        var remaining = input
            .OrderByDescending(b => b.score)
            .ToList();

        var fused = new List<Box>();

        while (remaining.Count > 0)
        {
            Box seed = remaining[0];
            remaining.RemoveAt(0);

            var cluster = new List<Box> { seed };

            // 收集與 seed 的 IoU >= 閾值者
            for (int i = remaining.Count - 1; i >= 0; i--)
            {
                if (IoU(seed, remaining[i]) >= iouThr)
                {
                    cluster.Add(remaining[i]);
                    remaining.RemoveAt(i);
                }
            }

            fused.Add(FuseCluster(cluster));
        }

        // 為了輸出穩定，也可以依 score 由大到小排
        fused = fused.OrderByDescending(b => b.score).ToList();
        return fused;
    }

    static Box FuseCluster(List<Box> cluster)
    {
        // 題目：融合框的座標與 score 為加權平均，權重為 weight；融合後 weight = 權重總和
        double wSum = cluster.Sum(b => b.weight);
        if (wSum <= 0) wSum = 1.0;

        double x1 = cluster.Sum(b => b.x1 * b.weight) / wSum;
        double y1 = cluster.Sum(b => b.y1 * b.weight) / wSum;
        double x2 = cluster.Sum(b => b.x2 * b.weight) / wSum;
        double y2 = cluster.Sum(b => b.y2 * b.weight) / wSum;
        double score = cluster.Sum(b => b.score * b.weight) / wSum;

        return new Box(x1, y1, x2, y2, score, wSum);
    }

    static void WriteFusedBoxes(string outPath, List<Box> fused, double iouThr)
    {
        using var sw = new StreamWriter(outPath);

        sw.WriteLine($"IoU Threshold: {iouThr:F2}");
        sw.WriteLine($"Number of Fused Boxes: {fused.Count}");
        sw.WriteLine("x1 y1 x2 y2 score weight");

        foreach (var b in fused)
        {
            sw.WriteLine($"{b.x1:F2} {b.y1:F2} {b.x2:F2} {b.y2:F2} {b.score:F2} {b.weight:F2}");
        }
    }
}
