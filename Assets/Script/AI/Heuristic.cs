using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristic
{
    private const int human = 0;
    private const int ai = 1;
    private const int CHECK_BONUS = 50;
    private const int CHECK_MATE_BONUS = 10000;
    private const int DEPTH_BONUS = 100;
    private int[,,] pieceSquare = new int[,,] {  
            // King end game
            {{-50, -30, -30, -30, -30, -30, -30, -50},
            {-30, -30,  0,  0,  0,  0, -30, -30},
            {-30, -10, 20, 30, 30, 20, -10, -30},
            {-30, -10, 30, 40, 40, 30, -10, -30},
            {-30, -10, 30, 40, 40, 30, -10, -30},
            {-30, -10, 20, 30, 30, 20, -10, -30},
            {-30, -20, -10,  0,  0, -10, -20, -30},
            {-50, -40, -30, -20, -20, -30, -40, -50}},
            // King early game
            {{20, 30, 30,  0,  0, 10, 30, 20},
            {20, 20,  0,  0,  0,  0, 20, 20},
            {-10, -20, -20,     -20, -20, -20, -20, -10},
            {-20, -30, -30, -40, -40, -30, -30, -20},
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30}},
            // Queen
            {{-20, -10, -10, -5, -5, -10, -10, -20},
            {-10,  0,  5,  0,  0,  0,  0, -10},
            {-10,  5,  5,  5,  5,  5,  0, -10},
            {0,  0,  5,  5,  5,  5,  0, -5},
            {-5,  0,  5,  5,  5,  5,  0, -5},
            {-10,  0,  5,  5,  5,  5,  0, -10},
            {-10,  0,  0,  0,  0,  0,  0, -10},
            {-20, -10, -10, -5, -5, -10, -10, -20}},
            // Rook
            {{0,  0,  0,  5,  5,  0,  0,  0},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {5, 10, 10, 10, 10, 10, 10,  5},
            {0,  0,  0,  0,  0,  0,  0,  0}},
            // Knight
            {{-50, -40, -30, -30, -30, -30, -40, -50},
            {-40, -20,  0,  5,  5,  0, -20, -40},
            {-30,  5, 10, 15, 15, 10,  5, -30},
            {-30,  0, 15, 20, 20, 15,  0, -30},
            {-30,  5, 15, 20, 20, 15,  5, -30},
            {-30,  0, 10, 15, 15, 10,  0, -30},
            {-40, -20,  0,  0,  0,  0, -20, -40},
            {-50, -40, -30, -30, -30, -30, -40, -50}},
            // Bishop
            {{-20, -10, -10, -10, -10, -10, -10, -20},
            {-10,  5,  0,  0,  0,  0,  5, -10},
            {-10, 10, 10, 10, 10, 10, 10, -10},
            {-10,  0, 10, 10, 10, 10,  0, -10},
            {-10,  5,  5, 10, 10,  5,  5, -10},
            {-10,  0,  5, 10, 10,  5,  0, -10},
            {-10,  0,  0,  0,  0,  0,  0, -10},
            {-20, -10, -10, -10, -10, -10, -10, -20}},
            // Pawn
            {{0,  0,  0,  0,  0,  0,  0,  0},
            {5, 10, 10, -20, -20, 10, 10,  5},
            {5, -5, -10,  0,  0, -10, -5,  5},
            {0,  0,  0, 20, 20,  0,  0,  0},
            {5,  5, 10, 25, 25, 10,  5,  5},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {50, 50, 50, 50, 50, 50, 50, 50},
            {100,  100,  100,  100,  100,  100,  100,  100}}};
    private int[,] pawn_rank = new int[2, 10];
    private int DOUBLED_PAWN_PENALTY = 10;
    private int ISOLATED_PAWN_PENALTY = 20;
    private int BACKWARDS_PAWN_PENALTY = 8;
    private int PASSED_PAWN_BONUS = 20;
    private int ROW(Vector2 k)
    {
        return Convert.ToInt32(k.x);
    }
    private int COL(Vector2 k)
    {
        return Convert.ToInt32(k.y);
    }

    public int calculate(ChessBoard board, Eplayer Player, int depth)
    {
        for (int i = 0; i < 10; i++)
        {
            pawn_rank[human, i] = 0;
            pawn_rank[ai, i] = 7;
        }
        foreach (BasePiece piece in board.All_Active_Pieces())
        {
            if (piece.Type == Etype.PAWN)
            {
                int f = COL(piece.Location) + 1;
                if (piece.Side == human)
                {
                    if (pawn_rank[human, f] < ROW(piece.Location))
                        pawn_rank[human, f] = ROW(piece.Location);
                }
                else
                {
                    if (pawn_rank[ai, f] > ROW(piece.Location))
                        pawn_rank[ai, f] = ROW(piece.Location);
                }
            }
        }
        if (Player == Eplayer.BLACK)
            return - pieceValueAndPosition(board) - mobility(board)
            - check(board) - checkmate(board, depth);
        else
            return pieceValueAndPosition(board) + mobility(board)
            + check(board) + checkmate(board, depth);
    }

    private int index(BasePiece piece, int numPieces)
    {
        Etype type = piece.Type;
        if (type == Etype.KING)
        {
            if (numPieces < 8)
                return 0;
            else
                return 1;
        }
        else if (type == Etype.QUEEN)
            return 2;
        else if (type == Etype.CASTLE)
            return 3;
        else if (type == Etype.KNIGHT)
            return 4;
        else if (type == Etype.BISHOP)
            return 5;
        else
            return 6;
    }

    int eval_light_pawn_structure(Vector2 sq)
    {
        int r = 0;  /* the value to return */
        int f = COL(sq) + 1;  /* the pawn's file */

        /* if there's a pawn behind this one, it's doubled */
        if (pawn_rank[human, f] > ROW(sq))
            r -= DOUBLED_PAWN_PENALTY;

        /* if there aren't any friendly pawns on either side of
           this one, it's isolated */
        if ((pawn_rank[human, f - 1] == 0) &&
                (pawn_rank[human, f + 1] == 0))
            r -= ISOLATED_PAWN_PENALTY;

        /* if it's not isolated, it might be backwards */
        else if ((pawn_rank[human, f - 1] < ROW(sq)) &&
                (pawn_rank[human, f + 1] < ROW(sq)))
            r -= BACKWARDS_PAWN_PENALTY;

        /* add a bonus if the pawn is passed */
        if ((pawn_rank[ai, f - 1] >= ROW(sq)) &&
                (pawn_rank[ai, f] >= ROW(sq)) &&
                (pawn_rank[ai, f + 1] >= ROW(sq)))
            r += (7 - ROW(sq)) * PASSED_PAWN_BONUS;

        return r;
    }

    int eval_dark_pawn_structure(Vector2 sq)
    {
        int r = 0;  /* the value to return */
        int f = COL(sq) + 1;  /* the pawn's file */

        /* if there's a pawn behind this one, it's doubled */
        if (pawn_rank[ai, f] < ROW(sq))
            r -= DOUBLED_PAWN_PENALTY;

        /* if there aren't any friendly pawns on either side of
           this one, it's isolated */
        if ((pawn_rank[ai, f - 1] == 7) &&
                (pawn_rank[ai, f + 1] == 7))
            r -= ISOLATED_PAWN_PENALTY;

        /* if it's not isolated, it might be backwards */
        else if ((pawn_rank[ai, f - 1] > ROW(sq)) &&
                (pawn_rank[ai, f + 1] > ROW(sq)))
            r -= BACKWARDS_PAWN_PENALTY;

        /* add a bonus if the pawn is passed */
        if ((pawn_rank[human, f - 1] <= ROW(sq)) &&
                (pawn_rank[human, f] <= ROW(sq)) &&
                (pawn_rank[human, f + 1] <= ROW(sq)))
            r += ROW(sq) * PASSED_PAWN_BONUS;

        return r;
    }

    private int pieceValueAndPosition(ChessBoard board)
    {
        int value = 0;
        List<BasePiece> blackPieces = board.Black_Active_Pieces();
        List<BasePiece> whitePieces = board.White_Active_Pieces();
        int numPieces = blackPieces.Count() + whitePieces.Count();
        foreach (BasePiece piece in blackPieces)
        {
            Vector2 pos = piece.Location;
            value -= piece.Value;
            value -= pieceSquare[index(piece, numPieces), Convert.ToInt32(pos.x), Convert.ToInt32(pos.y)];
            if (piece.Type == Etype.PAWN)
            {
                value -= eval_dark_pawn_structure(piece.Location);
            }
        }
        foreach (BasePiece piece in whitePieces)
        {
            Vector2 pos = piece.Location;
            value += piece.Value;
            value += pieceSquare[index(piece, numPieces), Convert.ToInt32(pos.x), Convert.ToInt32(pos.y)];
            if (piece.Type == Etype.PAWN)
            {
                value += eval_light_pawn_structure(piece.Location);
            }
        }
        return value;
    }

    private int mobility(ChessBoard board)
    {
        int value = 0;
        foreach(BasePiece item in board.White_Active_Pieces())
        {
            value += item.getLegalMoves().Count();
        }
        foreach (BasePiece item in board.Black_Active_Pieces())
        {
            value -= item.getLegalMoves().Count();
        }
        return value;
    }

    private int check(ChessBoard board)
    {
        return (board.Black_King.isInCheck() ? CHECK_BONUS : 0)
            - (board.White_King.isInCheck() ? CHECK_BONUS : 0);
    }

    private int depthBonus(int depth)
    {
        return (depth == 0) ? 1 : DEPTH_BONUS;
    }

    private int checkmate(ChessBoard board, int depth)
    {
        bool ai_checkmated = board.AI_is_checkmated();
        bool human_checkmated = board.HUMAN_is_checkmated();
        if (AI.Player == Eplayer.BLACK)
            return (ai_checkmated ? CHECK_MATE_BONUS * depthBonus(depth) : 0)
                - (human_checkmated ? CHECK_MATE_BONUS * depthBonus(depth) : 0);
        else
            return (human_checkmated ? CHECK_MATE_BONUS * depthBonus(depth) : 0)
                - (ai_checkmated ? CHECK_MATE_BONUS * depthBonus(depth) : 0);
    }

}