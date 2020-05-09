using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 移动平台
/// </summary>
public class MovePlatform : MonoBehaviour
{

    private Collider2D coll;
    public float speed, waitTime;
    public LayerMask player;
    public Transform[] movePos;

    //判断是否在顶端
    private int i;
    private Transform playDeftransform;


    // Start is called before the first frame update
    void Start()
    {
        i = 1;
        coll = GetComponent<Collider2D>();
        playDeftransform = GameObject.FindGameObjectWithTag("Player").transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //新的一种移动方式，在青蛙上试试看
        transform.position = Vector2.MoveTowards(transform.position, movePos[i].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, movePos[i].position) < 0.1f)
        {
            if (waitTime < 0.0f)
            {
                if (i == 0)
                {
                    i = 1; 
                }
                else
                {
                    i = 0;
                }

                waitTime = 0.5f;

            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.transform.parent = gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.transform.parent = playDeftransform;
        }
    }
}
