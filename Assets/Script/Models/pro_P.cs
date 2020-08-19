using UnityEngine;

public class pro_P : MonoBehaviour
{
    public static pro_P Current;
    public GameObject QuadPrefap;
    public static BasePiece ProPawn;
    private GameObject quad;
    public bool done;

    public void Promotion(BasePiece pawn)
    {
        done = false;
        quad = Instantiate(QuadPrefap, new Vector3(3.5f, 3.5f, -2) , Quaternion.identity);
        quad.transform.parent = this.transform;
        ProPawn = pawn;
    }

    private void Update()
    {
        Current = this;
    }
}
