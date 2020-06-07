using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BaseGameCTL : MonoBehaviour
{
    public static BaseGameCTL Current;
    private Egame_state Game_state;

    public Egame_state game_state { get { return Game_state; } set { Game_state = value; } }

    // Start is called before the first frame update
    void Awake()
    {
        Current = this;
        game_state = Egame_state.PLAYING;
    }

}
