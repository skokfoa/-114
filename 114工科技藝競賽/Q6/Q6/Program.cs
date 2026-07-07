using System;

class Program
{
    public const float Pi = (float)3.1415926;
    static void Main(string[] args)
    {
        List<obj> objec = new List<obj>();
        List<obj> result = new List<obj>();
        string[] input;
        string path = @"./2025contest_obj.txt";
        
        using (StreamReader sr = new StreamReader(path))
        {
            input = sr.ReadToEnd()
                .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        foreach (string line in input)
        {
            string[] each = line
                .Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            int id = int.Parse(each.First());
            int objt = obj.objT(each[1]);
            int mat = obj.maT(each[2]);
            float p1 = float.Parse(each[3]);
            float p2 = float.Parse(each[4]);
            int error = 0;

            if (objt == -1)
            {
                error = 1;
            }
            else if (mat == -1)
            {
                error = 2;
            }
            else if (p1 <= 0) error = 3;
            else if (p2 <= 0 && (objt != 1 && objt != 2)) error = 4;
            else if (objt == 1 || objt == 2)
            {
                if (p2 != 0) error = 4;
            }

            objec.Add(new obj(id, objt, mat, p1, p2, 0, 0, error));
        }

        //Console.WriteLine(string.Join("\r\n", objec
        //    .Select(x => $"{x.ID,2}\t{x.objType,8}\t{x.maType,8}\t{x.para1:F4}\t{x.para2:F4}\t{x.Volume:F4}\t{x.Weight:F4}\t{x.error,3}")
        //    ));

        foreach (obj ob in objec)
        {
            if(ob.error == 3 || ob.error == 4)
            {
                result.Add(ob);
                continue;
            }
            float volume = vol(ob.objType, ob);
            float weight = wei(ob.maType, ob, volume);
            result.Add(new obj(ob.ID, ob.objType, ob.maType, ob.para1, ob.para2, volume, weight, ob.error));
        }
        Console.WriteLine("********************************Display Data***************************");
        Console.WriteLine($"{"ID",2}  {"ObjType",8}  {"matType",8}  {"PARA1",6}  {"PARA2",6}  {"Volume",7}  {"Weight",7}  {"Error",12}");
        Console.WriteLine(string.Join("\r\n", result
            .Select(x => $"{x.ID,2}  {obj.objtS(x.objType),8}  {obj.matS(x.maType),8}  {x.para1,6:F3}  {x.para2,6:F3}  {x.Volume,7:F3}  {x.Weight,8:F3}  {obj.errorS(x.error),12}")
            ));
        Console.WriteLine("***********************************************************************");

        List<obj> lead = result.Where(x => x.maType == 3).ToList();
        List<obj> alum = result.Where(x => x.maType == 2).ToList();
        List<obj> iron = result.Where(x => x.maType == 1).ToList();
        List<obj> last = result.Where(x => !(x.maType == 1 || x.maType == 2 || x.maType == 3)).ToList();

        Console.WriteLine("********************************Display Data***************************");
        Console.WriteLine($"{"ID",2}  {"ObjType",8}  {"matType",8}  {"PARA1",6}  {"PARA2",6}  {"Volume",7}  {"Weight",7}  {"Error",12}");
        Console.WriteLine(string.Join("\r\n", lead
            .Select(x => $"{x.ID,2}  {obj.objtS(x.objType),8}  {obj.matS(x.maType),8}  {x.para1,6:F3}  {x.para2,6:F3}  {x.Volume,7:F3}  {x.Weight,8:F3}  {obj.errorS(x.error),12}")
            ));
        Console.WriteLine(string.Join("\r\n", alum
            .Select(x => $"{x.ID,2}  {obj.objtS(x.objType),8}  {obj.matS(x.maType),8}  {x.para1,6:F3}  {x.para2,6:F3}  {x.Volume,7:F3}  {x.Weight,8:F3}  {obj.errorS(x.error),12}")
            ));
        Console.WriteLine(string.Join("\r\n", iron
            .Select(x => $"{x.ID,2}  {obj.objtS(x.objType),8}  {obj.matS(x.maType),8}  {x.para1,6:F3}  {x.para2,6:F3}  {x.Volume,7:F3}  {x.Weight,8:F3}  {obj.errorS(x.error),12}")
            ));
        Console.WriteLine(string.Join("\r\n", last
            .Select(x => $"{x.ID,2}  {obj.objtS(x.objType),8}  {obj.matS(x.maType),8}  {x.para1,6:F3}  {x.para2,6:F3}  {x.Volume,7:F3}  {x.Weight,8:F3}  {obj.errorS(x.error),12}")
            ));
        Console.WriteLine("***********************************************************************");

        Console.ReadKey();
    }

    public static float vol (int objt,obj o)
    {
        float volume = new float();
        switch (objt)
        {
            case 1:
                volume = (float)(4.0 / 3.0 * Pi * Math.Pow(o.para1, 3)); break;
            case 2:
                volume = (float)(Math.Pow(o.para1, 3)); break;
            case 3:
                volume = (float)(1.0 / 3.0 * Math.Pow(o.para1, 2) * o.para2); break;
            case 4:
                volume = (float)(Pi * Math.Pow(o.para1, 2) * o.para2); break;
        }
        return volume;
    }

    public static float wei (int mat, obj o , float vol)
    {
        float weight = new float();
        switch (mat)
        {
            case 1:
                weight = (float)(vol * 7.87); break;
            case 2:
                weight = (float)(vol * 2.70); break;
            case 3:
                weight = (float)(vol * 11.3); break;
        }
        return weight;
    }
}

public class obj
{
    public int ID { get; set; }
    public int objType { get; set; }
    public int maType { get; set; }
    public float para1 { get; set; }
    public float para2 { get; set; }
    public float Volume { get; set; }
    public float Weight { get; set; }
    public int error { get; set; }


    public obj (int ID, int objType, int maType, float para1, float para2,float Volume = 0, float Weight = 0, int error = 0)
    {
        this.ID = ID;
        this.objType = objType;
        this.maType = maType;
        this.para1 = para1;
        this.para2 = para2;
        this.Volume = Volume;
        this.Weight = Weight;
        this.error = error;
    }

    public static int objT (string objType)
    {
        switch (objType)
        {
            case "Ball":
                return 1;
            case "Cube":
                return 2;
            case "Pyramid":
                return 3;
            case "Cylinder":
                return 4;
            default:
                return -1;
        }
    }

    public static int maT (string maType)
    {
        switch (maType)
        {
            case "Iron":
                return 1;
            case "Aluminum":
                return 2;
            case "Lead":
                return 3;
            default:
                return -1;
        }
    }

    public static string objtS(int n)
    {
        switch (n)
        {
            case 1:
                return "Ball";
            case 2:
                return "Cube";
            case 3:
                return "Pyramid";
            case 4:
                return "Cylinder";
            default:
                return "Unknown";
        }
    }

    public static string matS(int n)
    {
        switch (n)
        {
            case 1:
                return "Iron";
            case 2:
                return "Aluminum";
            case 3:
                return "Lead";
            default:
                return "Unknown";
        }
    }

    public static string errorS(int n)
    {
        switch (n)
        {
            case 0:
                return "Normal";
            case 1:
                return "Err_object";
            case 2:
                return "Err_material";
            case 3:
                return "Err_para1";
            case 4:
                return "Err_para2";
            default:
                return string.Empty;
        }
    }
}