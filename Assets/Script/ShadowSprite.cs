using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{ 
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color color;

    [Header("时间控制参数")]
    public float activeTime;//控制显示时间
    public float activeStart;//开始显示的时间点

    [Header("控制不透明度")]
    private float alpha;
    public float alphaSet; //不透明度初始值
    public float alphaMultiplier;//不透明度渐变值


    private void OnEnable()
    {
        Debug.Log("shadow");

        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite; //获得图像

        transform.position = player.position;    //获得坐标
        transform.localScale = player.localScale;//获得方向
        transform.rotation = player.rotation;    //获得角度[防止出错]

        activeStart = Time.time; //activeTime = 系统当前时间
    }


    // Update is called once per frame
    void Update()
    {
        alpha *= alphaMultiplier;

        color = new Color(0.5f, 0.5f, 1, alpha);

        thisSprite.color = color; 

        if (Time.time >= activeStart + activeTime)
        {
            //返回对象池
            ShadowPool.instance.ReturnPool(this.gameObject);
        }

    }
}
