using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cell : MonoBehaviour
{
    private Transform  Cell_Hover;
    private Ecell_color Color;
    private Ecell_state State;


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
                    break;
                case Ecell_state.HOVER:
                    Cell_Hover.gameObject.SetActive(true);
                    break;
                case Ecell_state.TARGETED:

                    break;
                case Ecell_state.SELECTED:
                    Cell_Hover.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    protected void Start()
    {
        Cell_Hover = this.transform.GetChild(0);
        state = Ecell_state.NORMAL;
    }

    public void SetCellState(Ecell_state cellState)
    {
        state = cellState;
    }

    protected void OnMouseDown()
    {
        Debug.Log("Down");
        if (state != Ecell_state.SELECTED)
            state = Ecell_state.SELECTED;
    }

}
