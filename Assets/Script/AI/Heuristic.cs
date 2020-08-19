using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Heuristic
{
    private const int CHECK_BONUS = 300;
    private const int CHECK_MATE_BONUS = 900;
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
            {5, 10, 10, -30, -30, 10, 10,  5},
            {5, -5, -10,  0,  0, -10, -5,  5},
            {0,  0,  0, 20, 20,  0,  0,  0},
            {5,  5, 10, 25, 25, 10,  5,  5},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {50, 50, 50, 50, 50, 50, 50, 50},
            {100,  100,  100,  100,  100,  100,  100,  100}}};

    public int calculate(ChessBoard board, Eside side, int depth)
    {
        List<BasePiece> AIPieces = board.AI_Pieces;
        List<BasePiece> HumanPieces = board.HUMAN_Pieces;
        if (side == Eside.AI)
            return 0 - pieceValueAndPosition(AIPieces, HumanPieces) - mobility(AIPieces, HumanPieces)
            - check(board) - checkmate(board, depth);
        else
            return 0 + pieceValueAndPosition(AIPieces, HumanPieces) + mobility(AIPieces, HumanPieces)
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

    private int pieceValueAndPosition(List<BasePiece> AIPieces, List<BasePiece> HumanPieces)
    {
        int value = 0;
        int numPieces = AIPieces.Count() + HumanPieces.Count();
        foreach (BasePiece piece in AIPieces)
        {
            Vector2 pos = piece.Location;
            value -= piece.Value;
            value -= pieceSquare[index(piece, numPieces), 7 - Convert.ToInt32(pos.y), 7 - Convert.ToInt32(pos.x)];
            value -= piece.getTarget().Count;
        }
        foreach (BasePiece piece in HumanPieces)
        {
            Vector2 pos = piece.Location;
            value += piece.Value;
            value += pieceSquare[index(piece, numPieces), Convert.ToInt32(pos.y), Convert.ToInt32(pos.x)];
            value += piece.getTarget().Count;
        }
        return value;
    }

    private int mobility(List<BasePiece> AIPieces, List<BasePiece> HumanPieces)
    {
        int value = 0;
        foreach(BasePiece item in HumanPieces)
        {
            value += item.getLegalMoves().Count();
        }
        foreach (BasePiece item in AIPieces)
        {
            value -= item.getLegalMoves().Count();
        }
        return value;
    }

    private int check(ChessBoard board)
    {
        return (board.AI_King.isInCheck() ? CHECK_BONUS : 0)
            - (board.HUMAN_King.isInCheck() ? CHECK_BONUS : 0);
    }

    private int depthBonus(int depth)
    {
        return (depth == 0) ? 1 : DEPTH_BONUS;
    }

    private int checkmate(ChessBoard board, int depth)
    {
        return (board.AI_is_checkmated() ? CHECK_MATE_BONUS * depthBonus(depth) : 0)
                - (board.HUMAN_is_checkmated() ? CHECK_MATE_BONUS * depthBonus(depth) : 0);
    }

}