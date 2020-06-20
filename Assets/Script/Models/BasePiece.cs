using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BasePiece : MonoBehaviour
{
    private bool mouse_down;
    public static bool is_mouse_down;
    //khởi tạo mousePos
    public Vector3 mousePos;
    public float minX = 0f;
    public float maxX = 7f;
    public float minY = 0f;
    public float maxY = 7f;

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
        this.transform.position = offsetPosition + new Vector3(x * ChessBoard.Current.CELL_SIZE, y * ChessBoard.Current.CELL_SIZE, -1);
    }

    void Start()
    {
        mousePos = transform.position;
    }
    void OnMouseDown()
    {
        //hàm thực hiện khi nhấp chuột vào quân cờ được gán
        mouse_down = true;
    }
    public void OnMouseUp()
    {
        //hàm thực hiện khi nhả chuột ra quân cờ được gán
        mouse_down = false;
        //làm tròn tọa độ cho khớp vô ô cờ
        mousePos.x = Mathf.Round(transform.position.x);
        mousePos.y = Mathf.Round(transform.position.y);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && mouse_down == true)
        {
            is_mouse_down = mouse_down;
            //cập nhật vị trí trỏ chuột vào mousePos
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(Mathf.Clamp(mousePos.x, minX, maxX), Mathf.Clamp(mousePos.y, minY, maxY), -1);
        }
        //di chuyển quân cờ tới vị trí mousePos với tốc độ 5000 (gần như ngay lập tức)
        transform.position = Vector3.Lerp(transform.position, mousePos, 5000 * Time.deltaTime);

        is_mouse_down = mouse_down;
    }


    public abstract void Move();
}
