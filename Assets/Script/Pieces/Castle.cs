﻿public class Castle : BasePiece
{
    public override void Moving_rule()
        {
            #region Castle Location
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
            #endregion
        }
    public void Castling(Eside side, bool is_left)
    {
        Sound_CTL.Current.PlaySound(Esound.CASTLING);
        _currentCell.SetPieces(null);
        if (side == Eside.AI)
        {
            if (is_left == true)
            {
                _currentCell = ChessBoard.Current.cells[3][7];
                mousePos = _currentCell.transform.position;
                mousePos.z = -1;
            }
            else
            {
                _currentCell = ChessBoard.Current.cells[5][7];
                mousePos = _currentCell.transform.position;
                mousePos.z = -1;
            }
        }
        else
        {
            if (is_left == true)
            {
                _currentCell = ChessBoard.Current.cells[3][0];
                mousePos = _currentCell.transform.position;
                mousePos.z = -1;
            }
            else
            {
                _currentCell = ChessBoard.Current.cells[5][0];
                mousePos = _currentCell.transform.position;
                mousePos.z = -1;
            }
        }
        _currentCell.SetPieces(this);
    }
    protected void Awake()
    {
        value = 525;
        type = Etype.CASTLE;
    }
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        BaseGameCTL.Current.AI_turn();
    }
}