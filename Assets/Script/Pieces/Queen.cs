using Assets.Script.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void Awake()
    {
        value = 900;
        type = Etype.QUEEN;
    }
}
