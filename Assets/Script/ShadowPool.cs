using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance; //单例模式-使其在其他代码中可以访问

    public GameObject shadowPrefab;    //影子预制体

    public int shadowCount; 

    private Queue<GameObject> availableObject = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;

        //初始化对象池
        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);  //Instantiate生成预制体
            newShadow.transform.SetParent(transform);

            //取消启用，返回对象池 
            ReturnPool(newShadow);
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);

        availableObject.Enqueue(gameObject);
    }

    public GameObject GetFormPool()
    {
        if (availableObject.Count == 0)
        {
            FillPool();
        }
        Debug.Log("ok");

        var outShadow = availableObject.Dequeue(); //从队列开头获取一个对象

        outShadow.SetActive(true);

        return outShadow;
    }
}
