using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_eagle : Enemy
{
    private Rigidbody2D rb;
    private Collider2D coll;


    public float speed;
    public Transform topPoint, bottomPoint;
    private float topy, bottomy;

    private bool isUp;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        isUp = true;

        topy = topPoint.position.y;
        bottomy = bottomPoint.position.y;
        Destroy(topPoint.gameObject);
        Destroy(bottomPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    //移动限制
    void Movement()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(0, speed);
            if (transform.position.y > topy)
            {
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, -speed);

            if (transform.position.y < bottomy)
            {
                isUp = true;
            }
        }
    }
}
