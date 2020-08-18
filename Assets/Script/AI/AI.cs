using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    private Heuristic heuristic = new Heuristic();
    public static Eplayer Player;
    private int depth = 0;
    private BasePiece best_piece;
    private cell best_move;
    private int s, compare;
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
            s = -10000;
            compare = 0;
            Loop(depth);
            Debug.Log(s);
            if (ChessBoard.Current.HUMAN_is_checkmated())
                BaseGameCTL.Current.checkmate();
            if (ChessBoard.Current.AI_is_checkmated())
                BaseGameCTL.Current.checkmate();
        }
    }
    private void Loop(int deeplv)
    {
        foreach (BasePiece item in ChessBoard.Current.AI_Pieces)
        {
            if (item.getLegalMoves().Count == 0)
                continue;
            else
            {
                cell old_cell = item.CurrentCell;
                foreach (cell move in item.getLegalMoves().ToArray())
                {
                    BasePiece piece = move.CurrentPiece;
                    item.Calculate_move(move);
                    if (ChessBoard.Current.AI_King.isInCheck())
                    {
                        item.Return(old_cell, move, piece);
                        continue;
                    }

                    if (deeplv > 0)
                    {
                        Loop1(deeplv, item, move, heuristic.calculate(ChessBoard.Current, Player, 1));
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
    }
    private void Loop1(int deeplv1, BasePiece piece1, cell move1, int node)
    {
        foreach (BasePiece item in ChessBoard.Current.HUMAN_Pieces)
        {
            if (item.getLegalMoves().Count == 0)
                continue;
            else
            {
                cell old_cell = item.CurrentCell;
                foreach (cell move in item.getLegalMoves().ToArray())
                {
                    BasePiece piece = move.CurrentPiece;
                    item.Calculate_move(move);

                    if (ChessBoard.Current.HUMAN_King.isInCheck())
                    {
                        item.Return(old_cell, move, piece);
                        continue;
                    }
                    if (Is_worst_move(node))
                    {
                        item.Return(old_cell, move, piece);
                        return;
                    }
                    
                    foreach (BasePiece AIitem in ChessBoard.Current.AI_Pieces)
                    {
                        if (AIitem.getLegalMoves().Count == 0)
                            continue;
                        else
                        {
                            cell old_cell1 = AIitem.CurrentCell;
                            foreach (cell AImove in AIitem.getLegalMoves().ToArray())
                            {
                                BasePiece piece2 = AImove.CurrentPiece;
                                AIitem.Calculate_move(AImove);
                                if (deeplv1 > 1)
                                {
                                    Loop2(deeplv1, piece1, move1, heuristic.calculate(ChessBoard.Current, Player, 1));
                                }
                                else
                                {
                                    cal(piece1, move1);
                                }
                                AIitem.Return(old_cell1, AImove, piece2);
                            }
                        }
                    }
                    item.Return(old_cell, move, piece);
                }
            }
        }
    }
    private void Loop2(int deeplv2, BasePiece piece2, cell move2, int node)
    {
        foreach (BasePiece item in ChessBoard.Current.HUMAN_Pieces)
        {
            if (item.getLegalMoves().Count == 0)
                continue;
            else
            {
                cell old_cell = item.CurrentCell;
                foreach (cell move in item.getLegalMoves().ToArray())
                {
                    BasePiece piece = move.CurrentPiece;
                    item.Calculate_move(move);

                    if (ChessBoard.Current.HUMAN_King.isInCheck())
                    {
                        item.Return(old_cell, move, piece);
                        continue;
                    }
                    if (Is_worst_move(node))
                    {
                        item.Return(old_cell, move, piece);
                        return;
                    }

                    foreach (BasePiece AIitem in ChessBoard.Current.AI_Pieces)
                    {
                        if (AIitem.getLegalMoves().Count == 0)
                            continue;
                        else
                        {
                            cell old_cell1 = AIitem.CurrentCell;
                            foreach (cell AImove in AIitem.getLegalMoves().ToArray())
                            {
                                BasePiece piece3 = AImove.CurrentPiece;
                                AIitem.Calculate_move(AImove);
                                if (deeplv2 > 2)
                                {
                                    Loop3(piece2, move2, heuristic.calculate(ChessBoard.Current, Player, 1));
                                }
                                else
                                {
                                    cal(piece2, move2);
                                }
                                AIitem.Return(old_cell1, AImove, piece3);
                            }
                        }
                    }
                    item.Return(old_cell, move, piece);
                }
            }
        }
    }
    private void Loop3(BasePiece piece3, cell move3, int node)
    {
        foreach (BasePiece item in ChessBoard.Current.HUMAN_Pieces)
        {
            if (item.getLegalMoves().Count == 0)
                continue;
            else
            {
                cell old_cell = item.CurrentCell;
                foreach (cell move in item.getLegalMoves().ToArray())
                {
                    BasePiece piece = move.CurrentPiece;
                    item.Calculate_move(move);

                    if (ChessBoard.Current.HUMAN_King.isInCheck())
                    {
                        item.Return(old_cell, move, piece);
                        continue;
                    }
                    if (Is_worst_move(node))
                    {
                        item.Return(old_cell, move, piece);
                        return;
                    }

                    foreach (BasePiece AIitem in ChessBoard.Current.AI_Pieces)
                    {
                        if (AIitem.getLegalMoves().Count == 0)
                            continue;
                        else
                        {
                            cell old_cell1 = AIitem.CurrentCell;
                            foreach (cell AImove in AIitem.getLegalMoves().ToArray())
                            {
                                BasePiece piece4 = move.CurrentPiece;
                                AIitem.Calculate_move(AImove);
                                cal(piece3, move3);
                                AIitem.Return(old_cell1, AImove, piece4);
                            }
                        }
                    }
                    item.Return(old_cell, move, piece);
                }
            }
        }
    }
    private void cal(BasePiece item, cell move)
    {
        compare = heuristic.calculate(ChessBoard.Current, Player, 1);
        if (s < compare)
        {
            s = compare;
            best_piece = item;
            best_move = move;
        }
    }
    private bool Is_worst_move(int node)
    {
        int x = heuristic.calculate(ChessBoard.Current, Player, 1);
        if (x - node > 300)
        {
            Debug.Log(x);
            return true;
        }
        else
            return false;
    }
}