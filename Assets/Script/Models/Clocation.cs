public class Clocation
{
    public int X;
    public int Y;

    public Clocation(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool Check_Location()
    {
        if (X < 8 && X >= 0 && Y < 8 && Y >= 0)
            return true;
        else
            return false;
    }
}
