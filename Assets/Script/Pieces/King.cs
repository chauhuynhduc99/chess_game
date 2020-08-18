using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : BasePiece
{
    private bool can_do_castling;
    public override void Moving_rule()
    {
        #region King Location
        //0 +1
        c = new Clocation((int)Location.x, (int)Location.y + 1);
        if (c.Check_Location())
            list.Add(c);

        //0 -1
        c = new Clocation((int)Location.x, (int)Location.y - 1);
        if (c.Check_Location())
            list.Add(c);

        //+1 0
        c = new Clocation((int)Location.x + 1, (int)Location.y);
        if (c.Check_Location())
            list.Add(c);

        //-1 0
        c = new Clocation((int)Location.x - 1, (int)Location.y);
        if (c.Check_Location())
            list.Add(c);

        //+1 +1
        c = new Clocation((int)Location.x + 1, (int)Location.y + 1);
        if (c.Check_Location())
            list.Add(c);

        //+1 -1
        c = new Clocation((int)Location.x + 1, (int)Location.y - 1);
        if (c.Check_Location())
            list.Add(c);

        //-1 +1
        c = new Clocation((int)Location.x - 1, (int)Location.y + 1);
        if (c.Check_Location())
            list.Add(c);

        //-1 -1
        c = new Clocation((int)Location.x - 1, (int)Location.y - 1);
        if(c.Check_Location())
            list.Add(c);

        //Nhập thành
        if (is_it_moved == false)
        {
            Castling_left();
            Castling_right();
            if(_canMovecells.Count > 0)
                can_do_castling = true;
        }
        #endregion
    }
    private void Castling_right()
    {
        if (Side == Eside.HUMAN)
        {
            if (ChessBoard.Current.cells[5][0].CurrentPiece == null && ChessBoard.Current.cells[6][0].CurrentPiece == null
            && ChessBoard.Current.cells[7][0].CurrentPiece.Is_it_moved == false
            && ChessBoard.Current.cells[7][0].CurrentPiece.Player == this.Player)
            _canMovecells.Add(ChessBoard.Current.cells[6][0]);
        }
        else
        {
            if (ChessBoard.Current.cells[5][7].CurrentPiece == null && ChessBoard.Current.cells[6][7].CurrentPiece == null
            && ChessBoard.Current.cells[7][7].CurrentPiece.Is_it_moved == false
            && ChessBoard.Current.cells[7][7].CurrentPiece.Player == this.Player)
                _canMovecells.Add(ChessBoard.Current.cells[6][7]);
        }
    }
    private void Castling_left()
    {
        if(Side == Eside.HUMAN)
        {
            if (ChessBoard.Current.cells[0][0].CurrentPiece != null && ChessBoard.Current.cells[1][0].CurrentPiece == null 
                && ChessBoard.Current.cells[2][0].CurrentPiece == null && ChessBoard.Current.cells[3][0].CurrentPiece == null 
                && ChessBoard.Current.cells[0][0].CurrentPiece.Is_it_moved == false 
                && ChessBoard.Current.cells[0][0].CurrentPiece.Player == this.Player)
                _canMovecells.Add(ChessBoard.Current.cells[2][0]);
            else
                _canMovecells.Clear();
        }
        else
        {
            if (ChessBoard.Current.cells[0][7].CurrentPiece != null && ChessBoard.Current.cells[1][7].CurrentPiece == null 
                && ChessBoard.Current.cells[2][7].CurrentPiece == null && ChessBoard.Current.cells[3][7].CurrentPiece == null 
                && ChessBoard.Current.cells[0][7].CurrentPiece.Is_it_moved == false 
                && ChessBoard.Current.cells[0][7].CurrentPiece.Player == this.Player)
                _canMovecells.Add(ChessBoard.Current.cells[2][7]);
            else
                _canMovecells.Clear();
        }
    }
    public bool isInCheck()
    {
        if(Player == Eplayer.BLACK)
        {
            foreach (BasePiece item in ChessBoard.Current.White_Active_Pieces())
            {
                if (item.getTarget().Contains(_currentCell))
                    return true;
            }
        }
        else
        {
            foreach (BasePiece item in ChessBoard.Current.Black_Active_Pieces())
            {
                if (item.getTarget().Contains(_currentCell))
                    return true;
            }
        }
        return false;
    }
    public override void AI_move(cell moveto)
    {
        base.AI_move(moveto);
        if (can_do_castling == true)
        {
            Debug.Log(can_do_castling);
            if (_currentCell == ChessBoard.Current.cells[6][7])
            {
                Castle p = ChessBoard.Current.cells[7][7].CurrentPiece as Castle;
                p.Castling(Side, false);
            }
            else if (_currentCell == ChessBoard.Current.cells[2][7])
            {
                Castle p = ChessBoard.Current.cells[0][7].CurrentPiece as Castle;
                p.Castling(Side, true);
            }
            else
                can_do_castling = false;
        }
    }
    private void Awake()
    {
        value = 1000000;
        type = Etype.KING;
    }
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (can_do_castling == true)
        {
            if (_currentCell == ChessBoard.Current.cells[6][0])
            {
                Castle p = ChessBoard.Current.cells[7][0].CurrentPiece as Castle;
                p.Castling(Side, false);
            }
            else if (_currentCell == ChessBoard.Current.cells[2][0])
            {
                Castle p = ChessBoard.Current.cells[0][0].CurrentPiece as Castle;
                p.Castling(Side, true);
            }
            else
                can_do_castling = false;
        }
        BaseGameCTL.Current.AI_turn();
    }
}