using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameCTL : MonoBehaviour
{
    
    private Egame_state _gameState;
    public static BaseGameCTL Current;
    public Text txt;
    AI computer = new AI();

    public Eplayer CurrentPlayer { get; private set; }
    public Egame_state Game_State { set { _gameState = value; } }
    public void SwitchTurn()
    {
        if (ChessBoard.Current.Black_King.isInCheck() || ChessBoard.Current.White_King.isInCheck())
            txt.text = "Check!";
        else
            txt.text = "";
        if (CurrentPlayer == Eplayer.WHITE)
            CurrentPlayer = Eplayer.BLACK;
        else
            CurrentPlayer = Eplayer.WHITE;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                ChessBoard.Current.cells[i][j].SetCellState(Ecell_state.NORMAL);
            }
        }
    }
    public void AI_turn()
    {
        if (CurrentPlayer == AI.Player)
        {
            computer.find_move();
        }
    }
    public Egame_state CheckGameState()
    {
        return _gameState;
    }
    public void end_game(Eplayer winPlayer)
    {
        _gameState = Egame_state.END_GAME;
        txt.text = "WinPlayer : " + winPlayer;
    }
    public void checkmate()
    {
        _gameState = Egame_state.END_GAME;
        txt.text = "CHECKMATE!";
    }

    private void Awake()
    {
        Current = this;
        CurrentPlayer = Eplayer.WHITE;
        _gameState = Egame_state.PLAYING;
    }
}