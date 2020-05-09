using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_frog : Enemy
{
    private Rigidbody2D rb;
    //private Animator anim;
    private Collider2D coll;


    public LayerMask ground;
    public Transform leftpoint, rightpoint;
    private float leftx, rightx;
    public float speed, jumpForce;

    private bool Faceleft = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();   //调用父类的Start()声明
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    void Movement()
    {
        if (Faceleft)
        {
            if (transform.position.x < leftx + 0.1f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                Faceleft = false;  
            }
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jumpForce);
            }
        }
        else
        {
            if (transform.position.x > rightx - 0.1f) {
                transform.localScale = new Vector3(1, 1, 1);
                Faceleft = true;
            }
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);
            }
        }
    }

    void SwitchAnim()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("downing", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("downing"))
        {
            anim.SetBool("downing", false);
        }
    }

}
