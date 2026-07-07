using System;
using System.IO;

class Program
{
    const int LR = 18, LC = 24;
    const int SR = 5, SC = 13;

    static void Main()
    {
        char[,] L = new char[LR, LC];
        char[,] S = new char[SR, SC];

        Console.Write("Enter 1st filename：");
        string path1 = "./" + Console.ReadLine();
        Console.Write("Enter 2en filename：");
        string path2 = "./" + Console.ReadLine();

        ReadGrid(path1, LR, LC, L);
        ReadGrid(path2, SR, SC, S);

        int mid = LC / 2;

        // ---------- 1) 算 h1（上平台高度差） ----------
        int leftTop = int.MaxValue;   // 左半部最上面的 H
        int rightTop = int.MaxValue;  // 右半部最上面的 H

        for (int r = 0; r < LR; r++)
        {
            for (int c = 0; c < LC; c++)
            {
                if (L[r, c] != 'H') continue;

                if (c < mid) leftTop = Math.Min(leftTop, r);
                else rightTop = Math.Min(rightTop, r);
            }
        }

        int h1 = Math.Abs(leftTop - rightTop);

        // ---------- 2) 上平台右端角：平台水平線結尾（H 且右邊是 .） ----------
        int platformRightH = -1;
        bool first = true;
        for (int c = 0; c < LC - 1; c++)
        {
            if (L[leftTop, c] == 'H' && L[leftTop, c + 1] == '.' && first)
            {
                platformRightH = c;
                first = false;
            }
        }

        // ---------- 3) 小圖：算 hT、找 O 的最底列 maxOR ----------
        int minOR = int.MaxValue, maxOR = -1;
        int minOC = int.MaxValue, maxOC = -1;

        for (int r = 0; r < SR; r++)
        {
            for (int c = 0; c < SC; c++)
            {
                if (S[r, c] == 'O')
                {
                    minOR = Math.Min(minOR, r);
                    maxOR = Math.Max(maxOR, r);
                    minOC = Math.Min(minOC, c);
                    maxOC = Math.Max(maxOC, c);
                }
            }
        }

        int hT = (maxOR - minOR + 1);

        // ✅ 用「最底那排 O」的最右欄對齊右端角（更符合範例）
        int bottomRowMaxOC = -1;
        for (int c = 0; c < SC; c++)
        {
            if (S[maxOR, c] == 'O')
                bottomRowMaxOC = Math.Max(bottomRowMaxOC, c);
        }

        // ---------- 4) 下平底：找「內側右牆」與「下平台水平線」 ----------
        int innerRightWallCol = FindInnerRightWallCol(L);
        int lowerPlatformRow = FindLowerPlatformRow(L, innerRightWallCol);

        // ---------- 5) 決定貼的位置 ----------
        int targetBottomRow;
        int targetRightCol;

        if (hT <= h1)
        {
            // 放上平台右端角：底部O放在平台上面一格；右端貼牆內側一格
            targetBottomRow = leftTop - 1;
            targetRightCol = platformRightH - 1;
        }
        else
        {
            // 放下平底右端角：底部O放在下平台上面一格；右端貼內側右牆內側一格
            targetBottomRow = lowerPlatformRow - 1;
            targetRightCol = innerRightWallCol - 1;
        }

        // 平移量：底對齊用 maxOR；右對齊用 bottomRowMaxOC（不是 maxOC）
        int dr = targetBottomRow - maxOR;
        int dc = targetRightCol - bottomRowMaxOC;

        // ---------- 6) 合成 ----------
        char[,] R = new char[LR, LC];
        for (int r = 0; r < LR; r++)
            for (int c = 0; c < LC; c++)
                R[r, c] = L[r, c];

        // 把 O 貼上去：只覆蓋 '.'，不壓掉 'H'
        for (int r = 0; r < SR; r++)
        {
            for (int c = 0; c < SC; c++)
            {
                if (S[r, c] != 'O') continue;

                int rr = r + dr;
                int cc = c + dc;

                if (rr < 0 || rr >= LR || cc < 0 || cc >= LC) continue;

                if (R[rr, cc] == '.') R[rr, cc] = 'O';
            }
        }

        // ---------- 7) 輸出 ----------
        Console.WriteLine();
        Console.WriteLine("組合後圖形：");
        PrintGrid(R, LR, LC);
    }

    // 內側右牆：找「H數量最多」的那一欄（U型框內側直牆最長）
    static int FindInnerRightWallCol(char[,] L)
    {
        int bestCol = -1;
        int bestCount = -1;

        for (int c = 0; c < LC; c++)
        {
            int count = 0;
            for (int r = 0; r < LR; r++)
                if (L[r, c] == 'H') count++;

            if (count > bestCount)
            {
                bestCount = count;
                bestCol = c;
            }
        }

        return bestCol;
    }

    // 下平台水平線：在內側右牆左邊，H 數量最多的那一排
    static int FindLowerPlatformRow(char[,] L, int innerRightWallCol)
    {
        int bestRow = -1;
        int bestCount = -1;

        for (int r = 0; r < LR; r++)
        {
            int count = 0;
            for (int c = 0; c < innerRightWallCol; c++)
                if (L[r, c] == 'H') count++;

            if (count > bestCount)
            {
                bestCount = count;
                bestRow = r;
            }
        }

        return bestRow;
    }

    static void ReadGrid(string path, int rows, int cols, char[,] grid)
    {
        string[] lines = File.ReadAllLines(path);
        int idx = 0;

        for (int r = 0; r < rows; r++)
        {
            while (idx < lines.Length && lines[idx].Length == 0) idx++;
            string line = lines[idx++];

            for (int c = 0; c < cols; c++)
                grid[r, c] = line[c];
        }
    }

    static void PrintGrid(char[,] g, int rows, int cols)
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
                Console.Write(g[r, c]);
            Console.WriteLine();
        }
    }
}
