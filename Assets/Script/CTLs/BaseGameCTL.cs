using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameCTL : MonoBehaviour
{
    
    private Egame_state _gameState;
    public static BaseGameCTL Current;

    public Eplayer CurrentPlayer { get; private set; }
    public Egame_state GameState { get { return _gameState; } set { _gameState = value; } }

    public void SwitchTurn()
    {
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
    public Egame_state CheckGameState()
    {
        return Egame_state.PLAYING;
    }
    public void end_game(Eplayer winPlayer)
    {
        GameState = Egame_state.END_GAME;
        Debug.Log("WinPlayer : " + winPlayer);
    }

    private void Awake()
    {
        Current = this;
        CurrentPlayer = Eplayer.WHITE;
        GameState = Egame_state.PLAYING;
    }
    private void Update()
    {
        Current = this;
    }
}