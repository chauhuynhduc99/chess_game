using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resource_CTL
{
    private static resource_CTL _instance = null;
    public static resource_CTL Instance
    {
        get
        {
            if(_instance == null)
                _instance = new resource_CTL();
            return _instance;
        }
    }

    #region Property
    private Material Black_cell;
    private Material White_cell;
    private Material Selected_cell_white;
    private Material Selected_cell_black;
    private Material Targeted_cell_black;
    private Material Targeted_cell_white;


    public Material black_cell
    {
        get
        {
            if(Black_cell == null)
            {
                Black_cell = Resources.Load<Material>("Material/Black");
            }
                return Black_cell;
        }
        
    }
    public Material white_cell
    {
        get
        {
            if (White_cell == null)
            {
                White_cell = Resources.Load<Material>("Material/White");
            }
            return White_cell;
        }
    }
    public Material selected_cell_white
    {
        get
        {
            if (Selected_cell_white == null)
            {
                    Selected_cell_white = Resources.Load<Material>("Material/hover_white");
            }
            return Selected_cell_white;
        }
    }
    public Material selected_cell_black
    {
        get
        {
            if (Selected_cell_black == null)
            {
                Selected_cell_black = Resources.Load<Material>("Material/hover_black");
            }
            return Selected_cell_black;
        }

    }
    public Material targeted_cell_black
    {
        get
        {
            if (Targeted_cell_black == null)
            {
                Targeted_cell_black = Resources.Load<Material>("Material/Targeted_black");
            }
            return Targeted_cell_black;
        }
    }
    public Material targeted_cell_white
    {
        get
        {
            if (Targeted_cell_white == null)
            {
                Targeted_cell_white = Resources.Load<Material>("Material/Targeted_white");
            }
            return Targeted_cell_white;
        }
    }

    #endregion

    private resource_CTL() { }

}
