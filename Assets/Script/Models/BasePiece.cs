using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BasePiece : MonoBehaviour
{
    protected List<cell> _target = new List<cell>();
    protected List<cell> _canMovecells = new List<cell>();
    protected cell _currentCell;
    private bool mouse_down;
    protected Vector2 originalLocation;
    //khởi tạo mousePos
    public Vector3 mousePos;
    public float minX = 0f;
    public float maxX = 7f;
    public float minY = 0f;
    public float maxY = 7f;

    [SerializeField]
    protected Vector3 offsetPosition;
    [SerializeField]
    protected Eplayer _player;


    public Eplayer Player { get { return _player; } protected set { _player = value; } }
    public Vector2 Location { get; private set; }
    public cell CurrentCell { get { return _currentCell; } set { _currentCell = value; } }

    public void SetOriginalLocation(int x, int y)
    {
        originalLocation = new Vector2(x, y);
        this.transform.position = offsetPosition + new Vector3(x * ChessBoard.Current.CELL_SIZE, y * ChessBoard.Current.CELL_SIZE, -1);
        this.Location = this.transform.position;
        _currentCell = ChessBoard.Current.cells[x][y];
        _currentCell.SetPieces(this);
    }

    private void Start()
    {
        mousePos = transform.position;
    }
    private void OnMouseEnter()
    {
        cell.Is_mouse_down = true;
    }
    private void OnMouseExit()
    {
        cell.Is_mouse_down = false;
    }
    protected void OnMouseDown()
    {
        this.Location = _currentCell.transform.position;
        mouse_down = true;
        Move();
    }
    protected void OnMouseUp()
    {
        //hàm thực hiện khi nhả chuột ra quân cờ được gán
        mouse_down = false;
        //làm tròn tọa độ cho khớp vô ô cờ
        mousePos.x = Mathf.Round(transform.position.x);
        mousePos.y = Mathf.Round(transform.position.y);
        cell old_cell = this._currentCell;
        foreach (cell item in _canMovecells)
        {
            if (ChessBoard.Current.cells[(int)mousePos.x][(int)mousePos.y] == item)
            {
                this._currentCell = item;
                this._currentCell.SetPieces(this);
                old_cell.SetPieces(null);
                this.Location = mousePos;
                break;
            }
        }
        foreach (cell item in _target)
        {
            if (ChessBoard.Current.cells[(int)mousePos.x][(int)mousePos.y] == item)
            {
                item.CurrentPiece.gameObject.SetActive(false);
                this._currentCell = item;
                this._currentCell.SetPieces(this);
                old_cell.SetPieces(null);
                this.Location = mousePos;
                break;
            }
        }

        mousePos = this.Location;
        mousePos.z = -1;
        EndMove();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && mouse_down == true)
        {
            //cập nhật vị trí trỏ chuột vào mousePos
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(Mathf.Clamp(mousePos.x, minX, maxX), Mathf.Clamp(mousePos.y, minY, maxY), -1);
        }
        //di chuyển quân cờ tới vị trí mousePos với tốc độ 5000 (gần như ngay lập tức)
        transform.position = Vector3.Lerp(transform.position, mousePos, 5000 * Time.deltaTime);
    }

    public abstract void Move();
    private void EndMove()
    {
        _canMovecells = new List<cell>();
        _target = new List<cell>();
        BaseGameCTL.Current.SwitchTurn();
    }
}