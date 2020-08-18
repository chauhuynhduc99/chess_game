using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_board : MonoBehaviour
{
    ChessBoard board = new ChessBoard();
    private List<BasePiece> Pieces = new List<BasePiece>();
    private List<BasePiece> black_Pieces = new List<BasePiece>();
    private List<BasePiece> white_Pieces = new List<BasePiece>();
    private King black_King;
    private King white_King;
}
