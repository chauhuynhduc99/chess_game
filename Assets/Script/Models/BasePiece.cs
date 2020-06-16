using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BasePiece : MonoBehaviour
{
    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    protected Eplayer _player;
    
    private Vector2 originalLocation;

    public Eplayer Player { get { return _player; } protected set { _player = value; } }

    public Vector2 Location { get; private set; }

    public void SetOriginalLocation(int x, int y)
    {
        originalLocation = new Vector2(x, y);
        this.transform.position = offsetPosition + new Vector3(x * ChessBoard.Current.CELL_SIZE, y * ChessBoard.Current.CELL_SIZE,-1);
    }

    public abstract void Move();
}
