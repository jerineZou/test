using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MouseControlModel : MonoBehaviour
{
    public Camera cam;//发射射线的摄像机
    private GameObject go;//射线碰撞的物体
    public static string btnName;//射线碰撞物体的名字
    private Vector3 screenSpace;
    private Vector3 offset;
    private bool isDrage = false;
    bool rightisDown;
    bool leftisDown;
    bool ispress;
    public float speed = 0.7f;

    public float interval = 1.0f;//按下后超过这个时间则认定为"长按"  
    private float recordTime;

    void Start()
    {
        //隐藏或者显示物体
        //transform.gameObject.SetActive(true);
    }

    void Update()
    {
        //鼠标右键按下，旋转物体
        if (Input.GetMouseButton(1))
        {
            //transform.Rotate(Vector3.up, -Input.GetAxis("Mouse X") * 4, Space.World);
            Vector3 v3 = transform.rotation.eulerAngles;
            transform.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * 6, Space.World);
        }

        //鼠标滚轮滚动，放大缩小
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //范围值限定
            if (Camera.main.fieldOfView <= 100)
                Camera.main.fieldOfView += 2;
            if (Camera.main.orthographicSize <= 20)
                Camera.main.orthographicSize += 0.5F;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //范围值限定
            if (Camera.main.fieldOfView > 2)
                Camera.main.fieldOfView -= 2;
            if (Camera.main.orthographicSize >= 1)
                Camera.main.orthographicSize -= 0.5F;
        }


        //鼠标左键拖拽，物体移动
        //整体初始位置 
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //从摄像机发出到点击坐标的射线
        RaycastHit hitInfo;
        if (isDrage == false)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                //划出射线，只有在scene视图中才能看到
                Debug.DrawLine(ray.origin, hitInfo.point);
                go = hitInfo.collider.gameObject;
                //print(btnName);
                screenSpace = cam.WorldToScreenPoint(go.transform.position);
                offset = go.transform.position - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
                //物体的名字  
                btnName = go.name;
                //组件的名字
            }
            else
            {
                btnName = null;
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 currentPosition = cam.ScreenToWorldPoint(currentScreenSpace) + offset;
            if (btnName != null)
            {
                go.transform.position = currentPosition;
            }
            isDrage = true;
        }
        else
        {
            isDrage = false;
        }

        //一直旋转
        if (ispress)
        {
            if ((Time.time - recordTime) > interval)
            {
                longpress = true;
            }
        }

        if (longpress)
        {
            if (rightisDown)
            {
                transform.Rotate(Vector3.down, speed);
            }
            else if (leftisDown)
            {
                transform.Rotate(Vector3.up, speed);
            }
        }
    }

    public void reset()
    {
        //transform.localEulerAngles = new Vector3(-90, 0, 0);
        //transform.position = startPos;
        transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 1f);
        //transform.DOMove(startPos, 1f);
    }

    bool longpress=false;
    public void clicktoRotation(int fangxiang)
    {
        Debug.Log("单击");
        Vector3 v3 = transform.rotation.eulerAngles;
        Quaternion rotation = Quaternion.Euler(v3);
        if (rightisDown) return;
        if (leftisDown) return;
        if (longpress) { longpress = false; return;}

        switch (fangxiang)
        {
            case 2:
                transform.DORotate(new Vector3(v3[0], v3[1] + 20, v3[2] ), 0.4f);
                break;
            case 3:
                transform.DORotate(new Vector3(v3[0], v3[1] - 20, v3[2]), 0.4f);
                break;
        }
        Debug.Log("单击结束");
    }

    public void startRot(int fangxiang)
    {
        recordTime = Time.time;
        ispress = true;
        if (fangxiang == 2) {leftisDown = true; rightisDown = false; }
        if (fangxiang == 3) {rightisDown = true; leftisDown = false; }
    }
        
    public void stopRot()
    {
        leftisDown = false;
        rightisDown = false;
        ispress = false;
    }
}