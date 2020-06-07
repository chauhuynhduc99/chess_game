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
    #endregion

    private resource_CTL() { }
    
}
