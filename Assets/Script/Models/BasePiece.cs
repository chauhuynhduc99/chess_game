﻿using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Script.Models;

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
    protected bool is_it_moved;

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
    public Eplayer Player { get { return _player; } protected set { _player = value; } }
    public Vector2 Location { get; protected set; }
    public cell CurrentCell { get { return _currentCell; } set { _currentCell = value; } }
    public Etype Type { get { return type; } }
    public bool Is_it_moved { get { return is_it_moved; } }

    public void SetOriginalLocation(int x, int y)//Khởi tạo vị trí ban đầu
    {
        is_it_moved = false;
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
            if (Cell.CurrentPiece == null || Cell.CurrentPiece.Player != _player)
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
    public bool Is_legal_Move(cell move)
    {
        return getLegalMoves().Contains(move);
    }
    public bool has_Escaped_move()
    {
        List<cell> moves = getLegalMoves();
        if (Player == Eplayer.BLACK)
        {
            foreach (cell move in moves)
            {
                foreach (BasePiece item in ChessBoard.Current.White_Pieces)
                {
                    foreach (cell target in item.getTarget())
                        if (target == move)
                            moves.Remove(move);
                }
            }
        }
        else
        {
            foreach (cell move in moves)
            {
                foreach (BasePiece item in ChessBoard.Current.Black_Pieces)
                {
                    foreach (cell target in item.getTarget())
                        if (target == move)
                            moves.Remove(move);
                }
            }
        }
        if (moves == null)
            return false;
        else
            return true;
    }
    public void AI_move(cell moveto)
    {
        mousePos = moveto.transform.position;
        cell old_cell = _currentCell;
        if (moveto.CurrentPiece == null)
        {
            moveto.SetPieces(this);
            _currentCell.SetPieces(null);
            _currentCell = moveto;
            Location = mousePos;
            Sound_CTL.Current.PlaySound(Esound.MOVE);
        }
        else
        {
            ChessBoard.Current.All_Active_Pieces.Remove(moveto.CurrentPiece);
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
            Sound_CTL.Current.PlaySound(Esound.HIT);
        }
        BaseGameCTL.Current.SwitchTurn();
        is_it_moved = true;
        old_cell.SetCellState(Ecell_state.SELECTED);
    }
    protected void Start()
    {
        mousePos = transform.position;//Khởi tạo mouse_pos với vị trí ban đàu của quân cờ
    }
    protected void OnMouseDown()
    {
        if (BaseGameCTL.Current.CheckGameState() != Egame_state.PLAYING 
            || BaseGameCTL.Current.CurrentPlayer != Player)
        {
            return;
        }
        _canMovecells = new List<cell>();
        _target = new List<cell>();
        list = new List<Clocation>();
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
            || BaseGameCTL.Current.CurrentPlayer != Player)
        {
            return;
        }
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
                Sound_CTL.Current.PlaySound(Esound.MOVE);
                break;
            }
        }

        //Vị trí thả chuột nằm trong list ô có quân địch thì destroy quân địch và thực hiện thay đổi vị trí cũng như gán cho biến _curentCell ô cờ mới
        //Đồng thời gán quân cờ này cho biến current_Piece của ô cờ mới và gán null cho ô cờ cũ
        foreach (cell item in _target)
        {
            if (ChessBoard.Current.cells[(int)mousePos.x][(int)mousePos.y] == item)
            {
                ChessBoard.Current.All_Active_Pieces.Remove(item.CurrentPiece);
                if (item.CurrentPiece.Player == Eplayer.BLACK)
                    ChessBoard.Current.Black_Pieces.Remove(item.CurrentPiece);
                else
                    ChessBoard.Current.White_Pieces.Remove(item.CurrentPiece);
                if (item.CurrentPiece.Type == Etype.KING)
                    BaseGameCTL.Current.end_game(this.Player);
                Destroy(item.CurrentPiece.gameObject);
                this._currentCell = item;
                this._currentCell.SetPieces(this);
                old_cell.SetPieces(null);
                this.Location = mousePos;
                Sound_CTL.Current.PlaySound(Esound.HIT);
                break;
            }
        }

        //Nếu các trường hợp di chuyển và ăn quân không xảy ra thì Location sẽ có giá trị ban đầu
        //Ta gán giá trị ban đầu cho mousePos để quân cờ trở về vị trí cũ vì đi sai luật
        mousePos = this.Location;
        mousePos.z = -1;
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