using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class BasePiece : MonoBehaviour
{
    #region Field
    //List lưu các ô có thể di chuyển tới và ô có quân của địch
    protected List<cell> _target = new List<cell>();
    protected List<cell> _canMovecells = new List<cell>();
    protected List<Clocation> list = new List<Clocation>();
    protected Clocation c;
    protected cell _currentCell;
    protected int value;
    private bool mouse_down;
    private Eside side;
    protected Etype type;
    protected bool is_it_moved = false;

    [SerializeField]
    protected Vector3 offsetPosition;
    [SerializeField]
    protected Eplayer _player;

    protected Vector3 mousePos;
    private float minX = 0;
    private float maxX = 7;
    private float minY = 0;
    private float maxY = 7;
    #endregion
    public Eside Side { get { return side; } set { side = value; } }
    public int Value { get { return value; } }
    public bool Is_it_active { get; set; }
    public Eplayer Player { get { return _player; } protected set { _player = value; } }
    public Vector2 Location { get; protected set; }
    public cell CurrentCell { get { return _currentCell; } set { _currentCell = value; } }
    public Etype Type { get { return type; } }
    public bool Is_it_moved { get { return is_it_moved; } }

    public void SetOriginalLocation(int x, int y)//Khởi tạo vị trí ban đầu
    {
        is_it_moved = false;
        Is_it_active = true;
        transform.position = new Vector3(x, y, -1);
        this.Location = this.transform.position;
        //Gán ô cờ ở vị trí tương ứng cho _currentCell
        _currentCell = ChessBoard.Current.cells[x][y];
        //Gán quân cờ này cho biến Current_Piece của ô cờ vừa gán vào
        _currentCell.SetPieces(this);
    }
    public abstract void Moving_rule();
    protected void Legal_Moves()
    {
        foreach (Clocation item in list)
        {
            cell Cell = ChessBoard.Current.cells[item.X][item.Y];
            if (Cell.CurrentPiece == null)
                _canMovecells.Add(Cell);
            else if (Cell.CurrentPiece.Player != _player)
                _target.Add(Cell);
        }
    }
    protected void Show_Move_steps()
    {
        foreach (var item in _canMovecells)
            item.SetCellState(Ecell_state.HOVER);
        foreach (var item in _target)
            item.SetCellState(Ecell_state.TARGETED);
    }
    private void EndMove()
    {
        foreach(cell item in _canMovecells)
            item.SetCellState(Ecell_state.NORMAL);
        foreach (cell item in _target)
            item.SetCellState(Ecell_state.NORMAL);
        _currentCell.SetCellState(Ecell_state.NORMAL);
    }
    public List<cell> getLegalMoves()
    {
        list.Clear();
        _canMovecells.Clear();
        Moving_rule();
        foreach (Clocation item in list)
        {
            cell Cell = ChessBoard.Current.cells[item.X][item.Y];
            if (Cell.CurrentPiece == null || (Cell.CurrentPiece != null && Cell.CurrentPiece.Player != _player))
                _canMovecells.Add(Cell);
        }
        return _canMovecells;
    }
    public List<cell> getTarget()
    {
        list.Clear();
        _target.Clear();
        Moving_rule();
        foreach (Clocation item in list)
        {
            cell Cell = ChessBoard.Current.cells[item.X][item.Y];
            if (Cell.CurrentPiece != null && Cell.CurrentPiece.Player != _player)
                _target.Add(Cell);
        }
        return _target;
    }
    public virtual void AI_move(cell moveto)
    {
        mousePos = moveto.transform.position;
        cell old_cell = _currentCell;
        if (moveto.CurrentPiece == null)
        {
            moveto.SetPieces(this);
            _currentCell.SetPieces(null);
            _currentCell = moveto;
            Location = mousePos;
            //Sound_CTL.Current.PlaySound(Esound.MOVE);
        }
        else
        {
            ChessBoard.Current.All_piece.Remove(moveto.CurrentPiece);
            moveto.CurrentPiece.Is_it_active = false;
            if (moveto.CurrentPiece.Player == Eplayer.BLACK)
                ChessBoard.Current.Black_Pieces.Remove(moveto.CurrentPiece);
            else
                ChessBoard.Current.White_Pieces.Remove(moveto.CurrentPiece);
            if (moveto.CurrentPiece.Type == Etype.KING)
                BaseGameCTL.Current.end_game(Player);
            Destroy(moveto.CurrentPiece.gameObject);
            moveto.SetPieces(this);
            _currentCell.SetPieces(null);
            _currentCell = moveto;
            Location = mousePos;
            //Sound_CTL.Current.PlaySound(Esound.HIT);
        }
        if(BaseGameCTL.Current.CheckGameState() == Egame_state.PLAYING)
            BaseGameCTL.Current.SwitchTurn();
        is_it_moved = true;
        old_cell.SetCellState(Ecell_state.SELECTED);
    }
    public void Calculate_move(cell moveto)
    {
        this.Location = new Vector2(moveto.transform.position.x, moveto.transform.position.y);
        _currentCell.SetPieces(null);
        _currentCell = moveto;
        if (moveto.CurrentPiece != null && moveto.CurrentPiece.Player != _player)
        {
            moveto.CurrentPiece.Is_it_active = false;
        }
        moveto.SetPieces(this);
    }
    public void Return(cell old_cell, cell return_from_cell, BasePiece piece)
    {
        this.Location = new Vector2(old_cell.transform.position.x, old_cell.transform.position.y);
        _currentCell.SetPieces(piece);
        _currentCell = old_cell;
        old_cell.SetPieces(this);
        if (return_from_cell.CurrentPiece != null)
            return_from_cell.CurrentPiece.Is_it_active = true;
    }
    protected void Start()
    {
        mousePos = transform.position;//Khởi tạo mouse_pos với vị trí ban đàu của quân cờ
    }
    protected void OnMouseDown()
    {
        if (BaseGameCTL.Current.CheckGameState() != Egame_state.PLAYING 
            || BaseGameCTL.Current.CurrentPlayer != Player || side == Eside.AI)
        {
            return;
        }
        _canMovecells.Clear();
        _target.Clear();
        list.Clear();
        //Lưu vị trí của quân cờ vào Location
        this.Location = _currentCell.transform.position;
        mouse_down = true;
        _currentCell.SetCellState(Ecell_state.SELECTED);
        Moving_rule();
        Legal_Moves();
        Show_Move_steps();
    }
    protected virtual void OnMouseUp()
    {
        if (BaseGameCTL.Current.CheckGameState() != Egame_state.PLAYING
            || BaseGameCTL.Current.CurrentPlayer != Player || side == Eside.AI)
        {
            return;
        }
        mouse_down = false;
        //làm tròn tọa độ cho khớp vô ô cờ
        mousePos.x = Mathf.Round(transform.position.x);
        mousePos.y = Mathf.Round(transform.position.y);

        //Lưu lại biến ô cờ sắp bị thay đổi
        cell old_cell = this._currentCell;

        cell new_cell = ChessBoard.Current.cells[(int)mousePos.x][(int)mousePos.y];
        if (_canMovecells.Contains(new_cell))
        {
            this._currentCell = new_cell;
            this._currentCell.SetPieces(this);
            old_cell.SetPieces(null);
            if (!ChessBoard.Current.HUMAN_King.isInCheck())
            {
                this.Location = mousePos;
                Sound_CTL.Current.PlaySound(Esound.MOVE);
            }
            else
            {
                this._currentCell = old_cell;
                old_cell.SetPieces(this);
                new_cell.SetPieces(null);
            }
        }
        else if (_target.Contains(new_cell))
        {
            new_cell.CurrentPiece.Is_it_active = false;
            if (!ChessBoard.Current.HUMAN_King.isInCheck())
            {
                ChessBoard.Current.All_piece.Remove(new_cell.CurrentPiece);
                new_cell.CurrentPiece.Is_it_active = false;
                if (new_cell.CurrentPiece.Player == Eplayer.BLACK)
                    ChessBoard.Current.Black_Pieces.Remove(new_cell.CurrentPiece);
                else
                    ChessBoard.Current.White_Pieces.Remove(new_cell.CurrentPiece);
                if (new_cell.CurrentPiece.Type == Etype.KING)
                    BaseGameCTL.Current.end_game(this.Player);
                Destroy(new_cell.CurrentPiece.gameObject);
                this._currentCell = new_cell;
                this._currentCell.SetPieces(this);
                old_cell.SetPieces(null);
                this.Location = mousePos;
                Sound_CTL.Current.PlaySound(Esound.HIT);
            }
            else
            {
                new_cell.CurrentPiece.Is_it_active = true;
            }
        }

        mousePos = this.Location;
        mousePos.z = -1;
        transform.position = mousePos;
        EndMove();
        if (_currentCell != old_cell)
        {
            is_it_moved = true;
            BaseGameCTL.Current.SwitchTurn();
            old_cell.SetCellState(Ecell_state.SELECTED);
        }
    }
    protected void Update()
    {
        if (Input.GetMouseButton(0) && mouse_down == true)
        {
            //cập nhật vị trí trỏ chuột vào mousePos
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(Mathf.Clamp(mousePos.x, minX, maxX), Mathf.Clamp(mousePos.y, minY, maxY), -2);
        }
        //di chuyển quân cờ tới vị trí mousePos
        transform.position = mousePos;
    }
}