using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMove : MonoBehaviour
{

    public Transform Cam;
    private Vector3 Offset;
    public float moveRate;
    private float bgSize;
    private float startPointX, startPointY;
    //获取组件
    void Start()
    {
        startPointX = transform.position.x;
        startPointY = transform.position.y;
        bgSize = transform.GetComponent<Collider2D>().bounds.size.x;
        //背景长度的一半
        Offset = new Vector3(bgSize / 2, 0, 0);
        Debug.Log(Offset);
    }
        void Update()
    {
        transform.position = new Vector2(startPointX + Cam.position.x * moveRate, startPointY);
            
        //判断条件：如果背景X轴位置小于相机X轴位置，执行RightRepositionBG（）
        if (transform.position.x < Cam.position.x)
        {
            RightRepositionBg();
        }
    }

    //通过给背景X轴添加Offset，移动背景
    void RightRepositionBg()
    {
        //Debug.Log("ok");
        transform.position = transform.position + Offset;
    }
} 
