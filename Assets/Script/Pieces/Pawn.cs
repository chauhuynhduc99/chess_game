﻿using Assets.Script.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Pawn : BasePiece
{
    public override void Moving_rule()
    {
        #region Pawn Location
        if (this.Player == Eplayer.WHITE)
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
        if((Location.y == 7 && Player == Eplayer.WHITE)|| (Location.y == 0 && Player == Eplayer.BLACK))
        {
            if(BaseGameCTL.Current.GameState == Egame_state.PLAYING)
            {
                pro_P.Current.Promotion(this);
                BaseGameCTL.Current.GameState = Egame_state.PAUSE;
            }
        }
    }
}
