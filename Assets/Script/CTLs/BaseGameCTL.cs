using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class BaseGameCTL : MonoBehaviour
{


    public static BaseGameCTL Current;

    private Egame_state _gameState;
    public Eplayer CurrentPlayer { get; private set; }

    public Egame_state GameState
    {
        get { return _gameState; }
        set { _gameState = value; }
    }


    void Awake()
    {
        Current = this;
        CurrentPlayer = Eplayer.WHITE;
        GameState = Egame_state.PLAYING;
    }

    void Start()
    {
        // StartCoroutine(UpdateTime());
    }

    /// <summary>
    /// Chuyển lượt chơi
    /// </summary>
    public void SwitchTurn()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (ChessBoard.Current.cells[i][j].state != Ecell_state.NORMAL)
                    ChessBoard.Current.cells[i][j].SetCellState(Ecell_state.NORMAL);
            }
        }
        if (CurrentPlayer == Eplayer.WHITE)
            CurrentPlayer = Eplayer.BLACK;
        else
            CurrentPlayer = Eplayer.WHITE;
        Debug.Log("switch");
    }

    /// <summary>
    /// Kiểm tra trạng thái của bàn cờ, xem đã kết thúc hay chưa
    /// </summary>
    public Egame_state CheckGameState()
    {

        return Egame_state.PLAYING;
    }

    public void GameOver(Eplayer winPlayer)
    {
        GameState = Egame_state.GAMEOVER;
        Debug.Log("WinPlayer : " + winPlayer);
    }

}
