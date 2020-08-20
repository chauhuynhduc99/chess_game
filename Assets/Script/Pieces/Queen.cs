public class Queen : BasePiece
{
    public override void Moving_rule()
    {
        #region Qeen Location
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x, (int)Location.y + i);
            if (c.Check_Location())
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x + i, (int)Location.y);
            if (c.Check_Location())
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x - i, (int)Location.y);
            if (c.Check_Location())
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x, (int)Location.y - i);
            if (c.Check_Location())
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x + i, (int)Location.y + i);
            if (c.Check_Location())
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x - i, (int)Location.y + i);
            if (c.Check_Location())
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x + i, (int)Location.y - i);
            if (c.Check_Location())
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        for (int i = 1; i < 8; i++)
        {
            c = new Clocation((int)Location.x - i, (int)Location.y - i);
            if (c.Check_Location())
            {
                if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                    list.Add(c);
                else
                {
                    list.Add(c);
                    break;
                }
            }
        }
        #endregion
    }
    protected void Awake()
    {
        value = 900;
        type = Etype.QUEEN;
    }
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        BaseGameCTL.Current.AI_turn();
    }
}