public class Knight : BasePiece
{
    public override void Moving_rule()
    {
        #region Knight Location
        //+1 +2
        c = new Clocation((int)Location.x + 1, (int)Location.y + 2);
        if (c.Check_Location())
            list.Add(c);
        //+2 +1
        c = new Clocation((int)Location.x + 2, (int)Location.y + 1);
        if (c.Check_Location())
            list.Add(c);

        //+2 -1
        c = new Clocation((int)Location.x + 2, (int)Location.y - 1);
        if (c.Check_Location())
            list.Add(c);
        //+1 -2
        c = new Clocation((int)Location.x + 1, (int)Location.y - 2);
        if (c.Check_Location())
            list.Add(c);
        //-1 -2
        c = new Clocation((int)Location.x - 1, (int)Location.y - 2);
        if (c.Check_Location())
            list.Add(c);
        //-2 -1
        c = new Clocation((int)Location.x - 2, (int)Location.y - 1);
        if (c.Check_Location())
            list.Add(c);
        //-2 +1
        c = new Clocation((int)Location.x - 2, (int)Location.y + 1);
        if (c.Check_Location())
            list.Add(c);
        //-1 +2
        c = new Clocation((int)Location.x - 1, (int)Location.y + 2);
        if (c.Check_Location())
            list.Add(c);
        #endregion
    }
    protected void Awake()
    {
        value = 350;
        type = Etype.KNIGHT;
    }
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        BaseGameCTL.Current.AI_turn();
    }
}
