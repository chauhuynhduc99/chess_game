using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BasePiece : MonoBehaviour
{
    //Lưu các ô có thể di chuyển tới và ô có quân của địch
    protected List<cell> _target = new List<cell>();
    protected List<cell> _canMovecells = new List<cell>();

    protected cell _currentCell;
    private bool mouse_down;
    protected Vector2 originalLocation;

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
    public Vector2 Location { get;protected set; }
    public cell CurrentCell { get { return _currentCell; } set { _currentCell = value; } }

    public void SetOriginalLocation(int x, int y)//Khởi tạo vị trí ban đầu
    {
        originalLocation = new Vector2(x, y);
        transform.position = new Vector3(x * ChessBoard.Current.CELL_SIZE, y * ChessBoard.Current.CELL_SIZE,-1);
        this.Location = this.transform.position;
        //Gán ô cờ ở vị trí tương ứng cho _currentCell
        _currentCell = ChessBoard.Current.cells[x][y];
        //Gán quân cờ này cho biến Current_Piece của ô cờ vừa gán vào
        _currentCell.SetPieces(this);
        Debug.Log(Location);
    }

    private void Start()
    {
        //Khởi tạo mouse_pos với vị trí ban đàu của quân cờ
        mousePos = transform.position;
    }
    protected void OnMouseDown()
    {
        //Lưu vị trí của quân cờ vào Location
        this.Location = _currentCell.transform.position;
        mouse_down = true;

        Move();
    }
    protected void OnMouseUp()
    {
        mouse_down = false;
        //làm tròn tọa độ cho khớp vô ô cờ
        mousePos.x = Mathf.Round(transform.position.x);
        mousePos.y = Mathf.Round(transform.position.y);

        //Lưu lại biến ô cờ sắp bị thay đổi
        cell old_cell = this._currentCell;

        //Nếu vị trí thả chuột nằm trong list ô có thể đi thì thực hiện thay đổi vị trí cũng như gán cho biến _curentCell ô cờ mới
        //Đồng thời gán quân cờ này cho biến current_Piece của ô cờ mới và gán null cho ô cờ cũ
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

        //Vị trí thả chuột nằm trong list ô có quân địch thì destroy quân địch và thực hiện thay đổi vị trí cũng như gán cho biến _curentCell ô cờ mới
        //Đồng thời gán quân cờ này cho biến current_Piece của ô cờ mới và gán null cho ô cờ cũ
        foreach (cell item in _target)
        {
            if (ChessBoard.Current.cells[(int)mousePos.x][(int)mousePos.y] == item)
            {
                Destroy(item.CurrentPiece.gameObject);
                this._currentCell = item;
                this._currentCell.SetPieces(this);
                old_cell.SetPieces(null);
                this.Location = mousePos;
                break;
            }
        }

        //Nếu các trường hợp trên không xảy ra thì Location sẽ có giá trị ban đầu
        //Ta gán giá trị ban đầu cho mousePos để quân cờ trở về vị trí cũ vì đi sai luật
        mousePos = this.Location;
        mousePos.z = -1;

        EndMove();

    }
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
        //Reset lại list sau khi di chuyển quân cờ
        _canMovecells = new List<cell>();
        _target = new List<cell>();
        //Thực hiện đổi lượt chơi
        BaseGameCTL.Current.SwitchTurn();
    }
}