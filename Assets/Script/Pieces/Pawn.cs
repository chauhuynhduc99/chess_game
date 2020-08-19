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
            c = new Clocation((int)Location.x, (int)Location.y + 2);
            if (c.Check_Location() && ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null && ChessBoard.Current.cells[c.X][c.Y - 1].CurrentPiece == null && is_it_moved == false)
                list.Add(c);
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
            c = new Clocation((int)Location.x, (int)Location.y - 2);
            if (c.Check_Location() && ChessBoard.Current.cells[c.X][c.Y].CurrentPiece == null && ChessBoard.Current.cells[c.X][c.Y + 1].CurrentPiece == null && is_it_moved == false)
                list.Add(c);
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
    public override void AI_move(cell moveto)
    {
        base.AI_move(moveto);
        if ((Location.y == 7 && Side == Eside.HUMAN) || (Location.y == 0 && Side == Eside.AI))
        {
            if (BaseGameCTL.Current.CheckGameState() == Egame_state.PLAYING)
            {
                if (Player == Eplayer.WHITE)
                {
                    GameObject chess_piece = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Pieces/White_Q"));
                    BasePiece p = chess_piece.GetComponent<BasePiece>();
                    p.SetOriginalLocation((int)Location.x, (int)Location.y);
                    chess_piece.transform.parent = ChessBoard.Current.Chess_Pieces.transform;
                    p.CurrentCell.SetPieces(p);
                    ChessBoard.Current.White_Pieces.Add(p);
                    ChessBoard.Current.All_piece.Add(p);
                }
                else
                {
                    GameObject chess_piece = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Pieces/Black_Q"));
                    BasePiece p = chess_piece.GetComponent<BasePiece>();
                    p.SetOriginalLocation((int)Location.x, (int)Location.y);
                    chess_piece.transform.parent = ChessBoard.Current.Chess_Pieces.transform;
                    p.CurrentCell.SetPieces(p);
                    ChessBoard.Current.Black_Pieces.Add(p);
                    ChessBoard.Current.All_piece.Add(p);
                }
                Destroy(gameObject);
            }
        }
    }
    protected void Awake()
    {
        value = 100;
        type = Etype.PAWN;
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
        BaseGameCTL.Current.AI_turn();
    }

}
