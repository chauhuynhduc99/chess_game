using Assets.Script.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Pawn : BasePiece
{
    public override void Moving_rule()
    {
        #region Pawn Location
        if (this.Side == Eside.HUMAN)
        {
            //0 +1
            c = new Clocation((int)Location.x, (int)Location.y + 1);
            if (c.Check_Location() && ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                list.Add(c);
            //0 +2
            if (ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null && Is_it_moved == false)
            {
                c = new Clocation((int)Location.x, (int)Location.y + 2);
                if (c.Check_Location())
                    list.Add(c);
            }
            //+1 +1
            c = new Clocation((int)Location.x + 1, (int)Location.y + 1);
            if (c.Check_Location() && ChessBoard.Current.cells[c.X][c.Y].CurrentPiece != null)
                list.Add(c);
            //-1 +1
            c = new Clocation((int)Location.x - 1, (int)Location.y + 1);
            if (c.Check_Location() && ChessBoard.Current.cells[c.X][c.Y].CurrentPiece != null)
                list.Add(c);
        }
        else
        {
            //0 -1
            c = new Clocation((int)Location.x, (int)Location.y - 1);
            if (c.Check_Location() && ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null)
                list.Add(c);
            //0 -2
            if(ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null && Is_it_moved == false)
            {
                c = new Clocation((int)Location.x, (int)Location.y - 2);
                if (c.Check_Location())
                    list.Add(c);
            }
            //-1 -1
            c = new Clocation((int)Location.x - 1, (int)Location.y - 1);
            if (c.Check_Location() && ChessBoard.Current.cells[c.X][c.Y].CurrentPiece != null)
                list.Add(c);
            //+1 -1
            c = new Clocation((int)Location.x + 1, (int)Location.y - 1);
            if (c.Check_Location() && ChessBoard.Current.cells[c.X][c.Y].CurrentPiece != null)
                list.Add(c);
        }
        #endregion
    }
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if((Location.y == 7 && Side == Eside.HUMAN)|| (Location.y == 0 && Side == Eside.AI))
        {
            if(BaseGameCTL.Current.CheckGameState() == Egame_state.PLAYING)
            {
                pro_P.Current.Promotion(this);
                BaseGameCTL.Current.Game_State = Egame_state.PAUSE;
            }
        }
    }
    private void Awake()
    {
        value = 100;
        type = Etype.PAWN;
    }
}
