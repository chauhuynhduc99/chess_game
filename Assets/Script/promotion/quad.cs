using UnityEngine;

public class quad : MonoBehaviour
{
    private void OnMouseUp()
    {
        if (pro_P.Current.done == true)
        {
            Destroy(this.gameObject);
            Sound_CTL.Current.PlaySound(Esound.MOVE);
            BaseGameCTL.Current.Game_State = Egame_state.PLAYING;
        }
    }
}