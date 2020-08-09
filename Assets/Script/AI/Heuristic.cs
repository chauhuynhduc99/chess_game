using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristic
{
    private const int HUMAN = 0;
    private const int AI = 1;
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
            {{20, 30, 10,  0,  0, 10, 30, 20},
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
            {0,  0,  0,  0,  0,  0,  0,  0}}};
    private const int INITIAL_PIECE_MATERIAL = 3450;
    private int[,] dist_bonus = new int[64, 64];
    private int[] bonus_dia_distance = new int[] { 5, 4, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int[] diag_nw = {   0, 1, 2, 3, 4, 5, 6, 7,
                                    1, 2, 3, 4, 5, 6, 7, 8,
                                    2, 3, 4, 5, 6, 7, 8, 9,
                                    3, 4, 5, 6, 7, 8, 9,10,
                                    4, 5, 6, 7, 8, 9,10,11,
                                    5, 6, 7, 8, 9,10,11,12,
                                    6, 7, 8, 9,10,11,12,13,
                                    7, 8, 9,10,11,12,13,14  };
    private int[] diag_ne = {   7, 6, 5, 4, 3, 2, 1, 0,
                                    8, 7, 6, 5, 4, 3, 2, 1,
                                    9, 8, 7, 6, 5, 4, 3, 2,
                                    10, 9, 8, 7, 6, 5, 4, 3,
                                    11,10, 9, 8, 7, 6, 5, 4,
                                    12,11,10, 9, 8, 7, 6, 5,
                                    13,12,11,10, 9, 8, 7, 6,
                                    14,13,12,11,10, 9, 8, 7 };
    private int[,] pawn_rank = new int[2, 10];
    private int[] piece_mat = new int[2];
    private int[] pawn_mat = new int[2];
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

    public Heuristic()
    {
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 64; j++)
            {
                dist_bonus[i, j] = 14 - (Math.Abs(i%8 - j%8) + Math.Abs(i/8 - j/8));
            }
        }
    }

    public int calculate(ChessBoard board, int depth)
    {
        for (int i = 0; i < 10; i++)
        {
            pawn_rank[HUMAN, i] = 0;
            pawn_rank[AI, i] = 7;
        }
        piece_mat[HUMAN] = piece_mat[AI] = pawn_mat[HUMAN] = pawn_mat[AI] = 0;

        BasePiece WhiteKing = null, BlackKing = null;
        foreach (BasePiece piece in board.All_Active_Pieces)
        {
            int player = Convert.ToInt32(piece.Player);
            int side = Convert.ToInt32(piece.Side);
            Vector2 Location = piece.Location;
            Etype type = piece.Type;
            if (type == Etype.KING)
            {
                if (player == 1)
                    WhiteKing = piece;
                else
                    BlackKing = piece;
            }
            else if (type == Etype.PAWN)
            {
                pawn_mat[side] += piece.Value;
                int f = COL(Location) + 1;
                if (side == HUMAN)
                {
                    if (pawn_rank[HUMAN, f] < ROW(Location))
                        pawn_rank[HUMAN, f] = ROW(Location);
                }
                else
                {
                    if (pawn_rank[AI, f] > ROW(Location))
                        pawn_rank[AI, f] = ROW(Location);
                }
            }
            else
                piece_mat[side] += piece.Value;
        }

        return pieceValueAndPosition(board) + mobility(board)
            + check(board) + checkmate(board, depth) + kingSafety(board, WhiteKing, BlackKing);
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
        if (pawn_rank[HUMAN, f] > ROW(sq))
            r -= DOUBLED_PAWN_PENALTY;

        /* if there aren't any friendly pawns on either side of
           this one, it's isolated */
        if ((pawn_rank[HUMAN, f - 1] == 0) &&
                (pawn_rank[HUMAN, f + 1] == 0))
            r -= ISOLATED_PAWN_PENALTY;

        /* if it's not isolated, it might be backwards */
        else if ((pawn_rank[HUMAN, f - 1] < ROW(sq)) &&
                (pawn_rank[HUMAN, f + 1] < ROW(sq)))
            r -= BACKWARDS_PAWN_PENALTY;

        /* add a bonus if the pawn is passed */
        if ((pawn_rank[AI, f - 1] >= ROW(sq)) &&
                (pawn_rank[AI, f] >= ROW(sq)) &&
                (pawn_rank[AI, f + 1] >= ROW(sq)))
            r += (7 - ROW(sq)) * PASSED_PAWN_BONUS;

        return r;
    }

    int eval_dark_pawn_structure(Vector2 sq)
    {
        int r = 0;  /* the value to return */
        int f = COL(sq) + 1;  /* the pawn's file */

        /* if there's a pawn behind this one, it's doubled */
        if (pawn_rank[AI, f] < ROW(sq))
            r -= DOUBLED_PAWN_PENALTY;

        /* if there aren't any friendly pawns on either side of
           this one, it's isolated */
        if ((pawn_rank[AI, f - 1] == 7) &&
                (pawn_rank[AI, f + 1] == 7))
            r -= ISOLATED_PAWN_PENALTY;

        /* if it's not isolated, it might be backwards */
        else if ((pawn_rank[AI, f - 1] > ROW(sq)) &&
                (pawn_rank[AI, f + 1] > ROW(sq)))
            r -= BACKWARDS_PAWN_PENALTY;

        /* add a bonus if the pawn is passed */
        if ((pawn_rank[HUMAN, f - 1] <= ROW(sq)) &&
                (pawn_rank[HUMAN, f] <= ROW(sq)) &&
                (pawn_rank[HUMAN, f + 1] <= ROW(sq)))
            r += ROW(sq) * PASSED_PAWN_BONUS;

        return r;
    }

    private int pieceValueAndPosition(ChessBoard board)
    {
        int value = 0;
        List<BasePiece> blackPieces = board.Black_Pieces;
        List<BasePiece> whitePieces = board.White_Pieces;
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
        foreach(BasePiece item in board.White_Pieces)
        {
            value += item.getLegalMoves().Count();
        }
        foreach (BasePiece item in board.Black_Pieces)
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
        return (depth == 0) ? 1 : DEPTH_BONUS * depth;
    }
    private int checkmate(ChessBoard board, int depth)
    {
        return (board.Black_King.isCheckMate() ? CHECK_MATE_BONUS * depthBonus(depth) : 0)
            - (board.White_King.isCheckMate() ? CHECK_MATE_BONUS * depthBonus(depth) : 0);
    }

    private int eval_lkp(int f)
    {
        int r = 0;

        if (pawn_rank[HUMAN, f] == 6) { }   /* pawn hasn't moved */
        else if (pawn_rank[HUMAN, f] == 5)
            r -= 10;  /* pawn moved one square */
        else if (pawn_rank[HUMAN, f] != 0)
            r -= 20;  /* pawn moved more than one square */
        else
            r -= 25;  /* no pawn on this file */

        if (pawn_rank[AI, f] == 7)
            r -= 15;  /* no enemy pawn */
        else if (pawn_rank[AI, f] == 5)
            r -= 10;  /* enemy pawn on the 3rd rank */
        else if (pawn_rank[AI, f] == 4)
            r -= 5;   /* enemy pawn on the 4th rank */

        return r;
    }
    private int eval_light_king_shield(Vector2 sq)
    {
        int r = 0;
        /* if the king is castled, use a special function to evaluate the
   pawns on the appropriate side */
        if (COL(sq) < 3)
        {
            r += eval_lkp(1);
            r += eval_lkp(2);
            r += eval_lkp(3) / 2;  /* problems with pawns on the c & f files
								  are not as severe */
        }
        else if (COL(sq) > 4)
        {
            r += eval_lkp(8);
            r += eval_lkp(7);
            r += eval_lkp(6) / 2;
        }

        /* otherwise, just assess a penalty if there are open files near
           the king */
        else
        {
            for (int i = COL(sq); i <= COL(sq) + 2; ++i)
                if ((pawn_rank[HUMAN, i] == 0) &&
                        (pawn_rank[AI, i] == 7))
                    r -= 10;
        }

        return r;
    }

    private int eval_dkp(int f)
    {
        int r = 0;

        if (pawn_rank[AI, f] == 1) { }
        else if (pawn_rank[AI, f] == 2)
            r -= 10;
        else if (pawn_rank[AI, f] != 7)
            r -= 20;
        else
            r -= 25;

        if (pawn_rank[HUMAN, f] == 0)
            r -= 15;
        else if (pawn_rank[HUMAN, f] == 2)
            r -= 10;
        else if (pawn_rank[HUMAN, f] == 3)
            r -= 5;

        return r;
    }
    private int eval_dark_king_shield(Vector2 sq)
    {
        int r = 0;

        if (COL(sq) < 3)
        {
            r += eval_dkp(1);
            r += eval_dkp(2);
            r += eval_dkp(3) / 2;
        }
        else if (COL(sq) > 4)
        {
            r += eval_dkp(8);
            r += eval_dkp(7);
            r += eval_dkp(6) / 2;
        }
        else
        {
            for (int i = COL(sq); i <= COL(sq) + 2; ++i)
                if ((pawn_rank[HUMAN, i] == 0) &&
                        (pawn_rank[AI, i] == 7))
                    r -= 10;
        }

        return r;
    }

    private int tropism_to_white_king(ChessBoard board, Vector2 pos)
    {
        int r = 0;
        foreach (BasePiece piece in board.Black_Pieces)
        {
            Etype type = piece.Type;
            int i = Convert.ToInt32(piece.Location.x*8 + piece.Location.y);
            int j = Convert.ToInt32(pos.x * 8 + pos.y);
            if (type == Etype.QUEEN)
                r += (dist_bonus[i, j] * 5) / 2;
            if (type == Etype.CASTLE)
                r += dist_bonus[i, j] / 2;
            if (type == Etype.KNIGHT)
                r += dist_bonus[i, j];
            if (type == Etype.BISHOP)
            {
                r += bonus_dia_distance[Math.Abs(diag_ne[i] - diag_ne[j])];
                r += bonus_dia_distance[Math.Abs(diag_nw[i] - diag_nw[i])];
            }
        }
        return r;
    }

    private int tropism_to_black_king(ChessBoard board, Vector2 pos)
    {
        int r = 0;
        foreach (BasePiece piece in board.White_Pieces)
        {
            Etype type = piece.Type;
            int i = Convert.ToInt32(piece.Location.x * 8 + piece.Location.y);
            int j = Convert.ToInt32(pos.x * 8 + pos.y);
            if (type == Etype.QUEEN)
                r += (dist_bonus[i, j] * 5) / 2;
            if (type == Etype.CASTLE)
                r += dist_bonus[i, j] / 2;
            if (type == Etype.KNIGHT)
                r += dist_bonus[i, j];
            if (type == Etype.BISHOP)
            {
                r += bonus_dia_distance[Math.Abs(diag_ne[i] - diag_ne[j])];
                r += bonus_dia_distance[Math.Abs(diag_nw[i] - diag_nw[i])];
            }
        }
        return r;
    }

    private int kingSafety(ChessBoard board, BasePiece WhiteKing, BasePiece BlackKing)
    {
        Vector2 wk_pos = WhiteKing.Location;
        Vector2 bk_pos = BlackKing.Location;
        int whiteKingSafety =
            (eval_light_king_shield(wk_pos) - tropism_to_white_king(board, wk_pos)) * piece_mat[AI] / INITIAL_PIECE_MATERIAL;
        int blackKingSafety =
            (eval_dark_king_shield(bk_pos) - tropism_to_black_king(board, bk_pos)) * piece_mat[HUMAN] / INITIAL_PIECE_MATERIAL;
        return whiteKingSafety - blackKingSafety;
    }
}
