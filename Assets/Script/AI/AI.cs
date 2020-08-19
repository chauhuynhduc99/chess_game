using UnityEngine;

public class AI
{
    private Heuristic heuristic = new Heuristic();
    public static Eplayer Player;
    private int depth;
    private BasePiece best_piece;
    private cell best_move;
    private BasePiece Human_best_piece;
    private cell Human_best_move;
    private int s, compare, s0, compare0;
    public void find_move()
    {
        if (BaseGameCTL.Current.CheckGameState() != Egame_state.PLAYING
            || BaseGameCTL.Current.CurrentPlayer != Player)
        {
            return;
        }
        else
        {
            best_move = null;
            best_piece = null;
            Human_best_move = null;
            Human_best_piece = null;
            s = s0 = -10000;
            compare = compare0 = 0;
            depth = ChessBoard.Current.Depth;
            Loop(500);
        }
    }
    private void Loop(int value)
    {
        bool flag = false;
        if(value > 900)
        {
            BaseGameCTL.Current.SwitchTurn();
        }
        foreach (BasePiece item in ChessBoard.Current.AI_Pieces)
        {
            if (item.getLegalMoves().Count == 0)
            {
                continue;
            }
            else
            {
                cell old_cell = item.CurrentCell;
                foreach (cell move in item.getLegalMoves().ToArray())
                {
                    BasePiece piece = move.CurrentPiece;
                    item.Calculate_move(move);
                    foreach (BasePiece human_item in ChessBoard.Current.HUMAN_Pieces)
                    {
                        if (human_item.getLegalMoves().Contains(item.CurrentCell) && item.Value > value)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (ChessBoard.Current.AI_King.isInCheck() || flag)
                    {
                        item.Return(old_cell, move, piece);
                        flag = false;
                        continue;
                    }
                    if (depth > 1)
                    {
                        Loop1(item, move);
                    }
                    else
                    {
                        cal(item, move);
                    }
                    item.Return(old_cell, move, piece);
                }
            }
        }
        if (best_piece != null)
            best_piece.AI_move(best_move);
        else
            Loop(value + 200);
    }
    private void Loop1(BasePiece _piece, cell _move)
    {
        foreach (BasePiece human_item in ChessBoard.Current.HUMAN_Pieces)
        {
            int x = heuristic.calculate(ChessBoard.Current, Eside.HUMAN, 1);
            if (human_item.getLegalMoves().Count == 0)
                continue;
            else
            {
                cell human_old_cell = human_item.CurrentCell;
                foreach (cell human_move in human_item.getLegalMoves().ToArray())
                {
                    BasePiece human_piece = human_move.CurrentPiece;
                    human_item.Calculate_move(human_move);
                    int xin = x - heuristic.calculate(ChessBoard.Current, Eside.HUMAN, 1);
                    if (ChessBoard.Current.HUMAN_King.isInCheck() || xin < 10)
                    {
                        human_item.Return(human_old_cell, human_move, human_piece);
                        continue;
                    }
                    Human_cal(human_item, human_move);
                    human_item.Return(human_old_cell, human_move, human_piece);
                }
            }
        }
        BasePiece human_best_piece = Human_best_piece;
        cell human_best_move = Human_best_move;
        BasePiece piece_in_bestMove = human_best_move.CurrentPiece;
        cell best_piece_OldCell = human_best_piece.CurrentCell;
        human_best_piece.Calculate_move(human_best_move);
        if (depth > 2)
            AI_doing(_piece, _move, 2);
        else
            AI_doing(_piece, _move, 0);
        human_best_piece.Return(best_piece_OldCell , human_best_move, piece_in_bestMove);
    }
    private void Loop2(BasePiece _piece, cell _move)
    {
        foreach (BasePiece human_item in ChessBoard.Current.HUMAN_Pieces)
        {
            int x = heuristic.calculate(ChessBoard.Current, Eside.HUMAN, 1);
            if (human_item.getLegalMoves().Count == 0)
                continue;
            else
            {
                cell human_old_cell = human_item.CurrentCell;
                foreach (cell human_move in human_item.getLegalMoves().ToArray())
                {
                    BasePiece human_piece = human_move.CurrentPiece;
                    human_item.Calculate_move(human_move);
                    int xin = x - heuristic.calculate(ChessBoard.Current, Eside.HUMAN, 1);
                    if (ChessBoard.Current.HUMAN_King.isInCheck() || xin < 10)
                    {
                        human_item.Return(human_old_cell, human_move, human_piece);
                        continue;
                    }
                    Human_cal(human_item, human_move);
                    human_item.Return(human_old_cell, human_move, human_piece);
                }
            }
        }
        BasePiece human_best_piece = Human_best_piece;
        cell human_best_move = Human_best_move;
        BasePiece piece_in_bestMove = human_best_move.CurrentPiece;
        cell best_piece_OldCell = human_best_piece.CurrentCell;
        human_best_piece.Calculate_move(human_best_move);
        if (depth > 2)
            AI_doing(_piece, _move, 3);
        else
            AI_doing(_piece, _move, 0);
        human_best_piece.Return(best_piece_OldCell, human_best_move, piece_in_bestMove);
    }
    private void Loop3(BasePiece _piece, cell _move)
    {
        foreach (BasePiece human_item in ChessBoard.Current.HUMAN_Pieces)
        {
            int x = heuristic.calculate(ChessBoard.Current, Eside.HUMAN, 1);
            if (human_item.getLegalMoves().Count == 0)
                continue;
            else
            {
                cell human_old_cell = human_item.CurrentCell;
                foreach (cell human_move in human_item.getLegalMoves().ToArray())
                {
                    BasePiece human_piece = human_move.CurrentPiece;
                    human_item.Calculate_move(human_move);
                    int xin = x - heuristic.calculate(ChessBoard.Current, Eside.HUMAN, 1);
                    if (ChessBoard.Current.HUMAN_King.isInCheck() || xin < 10)
                    {
                        human_item.Return(human_old_cell, human_move, human_piece);
                        continue;
                    }
                    Human_cal(human_item, human_move);
                    human_item.Return(human_old_cell, human_move, human_piece);
                }
            }
        }
        BasePiece human_best_piece = Human_best_piece;
        cell human_best_move = Human_best_move;
        BasePiece piece_in_bestMove = human_best_move.CurrentPiece;
        cell best_piece_OldCell = human_best_piece.CurrentCell;
        human_best_piece.Calculate_move(human_best_move);
        AI_doing(_piece, _move, 0);
        human_best_piece.Return(best_piece_OldCell, human_best_move, piece_in_bestMove);
    }
    private void AI_doing(BasePiece _piece, cell _move, int nextLoop)
    {
        foreach (BasePiece item in ChessBoard.Current.AI_Pieces)
        {
            int y = heuristic.calculate(ChessBoard.Current, Eside.AI, 1);
            if (item.getLegalMoves().Count == 0)
                continue;
            else
            {
                cell old_cell = item.CurrentCell;
                foreach (cell move in item.getLegalMoves().ToArray())
                {
                    BasePiece piece = move.CurrentPiece;
                    item.Calculate_move(move);
                    int yin = y - heuristic.calculate(ChessBoard.Current, Eside.AI, 1);
                    if (ChessBoard.Current.AI_King.isInCheck() || yin < 10)
                    {
                        item.Return(old_cell, move, piece);
                        continue;
                    }
                    if (nextLoop == 2)
                    {
                        Loop2(_piece, _move);
                    }
                    else if (nextLoop == 3)
                    {
                        Loop3(_piece, _move);
                    }
                    else
                    {
                        cal(_piece, _move);
                    }
                    item.Return(old_cell, move, piece);
                }
            }
        }
    }
    private void cal(BasePiece item, cell move)
    {
        compare = heuristic.calculate(ChessBoard.Current, Eside.AI, 1);
        if (s < compare)
        {
            s = compare;
            best_piece = item;
            best_move = move;
        }
    }
    private void Human_cal(BasePiece item, cell move)
    {
        compare0 = heuristic.calculate(ChessBoard.Current, Eside.HUMAN, 1);
        if (s0 < compare0)
        {
            s0 = compare0;
            Human_best_piece = item;
            Human_best_move = move;
        }
    }
}