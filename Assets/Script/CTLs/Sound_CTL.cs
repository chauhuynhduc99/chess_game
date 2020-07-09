using UnityEngine;

public class Sound_CTL : MonoBehaviour
{
    public AudioClip Move;
    public AudioClip Castling;
    public AudioClip Hit;
    public static Sound_CTL Current;

    private void Start()
    {
        Current = this;
    }
    public void PlaySound(Esound currentSound)
    {
        switch (currentSound)
        {
            case Esound.MOVE:
                {
                    Current.GetComponent<AudioSource>().PlayOneShot(Current.Move);
                }
                break;
            case Esound.CASTLING:
                {
                    Current.GetComponent<AudioSource>().PlayOneShot(Current.Castling);
                }
                break;
            case Esound.HIT:
                {
                    Current.GetComponent<AudioSource>().PlayOneShot(Current.Hit);
                }
                break;
        }
    }
}
