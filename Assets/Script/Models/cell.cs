using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell : MonoBehaviour
{
    private Transform  Cell_Hover;
    private Ecell_color Color;
    private Ecell_state State;
    private BasePiece _currenPiece;
    public static bool Is_mouse_down = false;

    public float size
    {
        get
        {
            return GetComponent<Renderer>().bounds.size.x;
        }
    }

    public Ecell_color color
    {
        get { return Color; }
        set 
        {
            Color = value;
            switch (Color)
            {
                case Ecell_color.BLACK:
                    GetComponent<Renderer>().material = resource_CTL.Instance.black_cell;
                    break;
                case Ecell_color.WHITE:
                    GetComponent<Renderer>().material = resource_CTL.Instance.white_cell;
                    break;
                case Ecell_color.HOVER_WHITE:
                    GetComponent<Renderer>().material = resource_CTL.Instance.selected_cell_white;
                    break;
                case Ecell_color.HOVER_BLACK:
                    GetComponent<Renderer>().material = resource_CTL.Instance.selected_cell_black;
                    break;
                default:
                    break;
            }
        }
    }

    public Ecell_state state
    {
        get { return State; }
        private set 
        {
            State = value;
            switch (State)
            {
                case Ecell_state.NORMAL:
                    Cell_Hover.gameObject.SetActive(false);
                    if (color == Ecell_color.HOVER_WHITE)
                        color = Ecell_color.WHITE;
                    if (color == Ecell_color.HOVER_BLACK)
                        color = Ecell_color.BLACK;
                    break;
                case Ecell_state.HOVER:
                    Cell_Hover.gameObject.SetActive(true);
                    break;
                case Ecell_state.TARGETED:

                    break;
                case Ecell_state.SELECTED:
                    if(color == Ecell_color.WHITE)
                        color = Ecell_color.HOVER_WHITE;
                    if (color == Ecell_color.BLACK)
                        color = Ecell_color.HOVER_BLACK;
                    break;
                default:
                    break;
            }
        }
    }

    protected void Awake()
    {
        Cell_Hover = this.transform.GetChild(0);
    }
    protected void Start()
    {
        state = Ecell_state.NORMAL;
    }

    protected void OnMouseDown()
    {
        if (Is_mouse_down == true)
            state = Ecell_state.SELECTED;
    }
    protected void OnMouseUp()
    {
        state = Ecell_state.NORMAL;
    }

    

    public void SetCellState(Ecell_state cellState)
    {
        state = cellState;
    }

    private void SetPieces(BasePiece piece)
    {
        this._currenPiece = piece;
    }
}
