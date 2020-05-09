using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    //刚体
    private Rigidbody2D rb;
    private Animator anim;
    //public AudioSource jumpAudio, hurtAudio, cherryAudio; 前期的音效片段
    public Collider2D headcoll;
    public Collider2D bodycoll;
    public Transform cellingCheck, groundCheck;
    public LayerMask ground, MovePlatform;

    public float runSpeed, crouchSpeed, jumpForce;
    private float speed;
    private float horizontalMove;
    private bool isHurt, isJump, isGround, isShot, isDash, jumpPressed, climbable;

    public int cherry = 0;
    public int gem = 0;
    int jumpCount;
    public Text cherryNum;
    public Text gemNum;

    [Header("Dash参数")]
    public float dashTime;
    private float dashTimeLeft;
    private float lastDash = -1f;
    public float dashCoolDown;
    public float dashSpeed;

    // Start is called before the first frame update
    //加载所需组件
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    //将跳跃和下蹲动作与移动分开，写在Update()内
    private void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        //冲刺判断
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time >= (lastDash + dashCoolDown))
            {
                //执行Dash
                ReadyToDash();
            }
        }
        cherryNum.text = cherry.ToString();
        gemNum.text = gem.ToString();
    }

    // 移动和动作切换需要流畅，写在FixpUdate()内
    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground) ||
                   Physics2D.OverlapCircle(groundCheck.position, 0.2f, MovePlatform);

        if (!isHurt)
        {
            Dash();
            if (isDash)
                return;
            Jump();
            Crouch();
            Climb();
 
            //Shot();
            GroundMovement();
        }

        SwitchAnim();

    }

    //角色移动
    void GroundMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }

    //动画变换
    void SwitchAnim()
    {
        //anim.SetBool("idle", false);
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isHurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 5f)
            {
                isHurt = false;
                anim.SetBool("hurt", false);
            }
        }

        if (climbable && Input.GetKey(KeyCode.E))
        {
            anim.SetBool("climb", true);
            anim.SetBool("falling", false);
        }

        if (isShot)
        {
            anim.SetBool("shot", true);
        }
        else
        {
            anim.SetBool("shot", false);
        }

        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!climbable && !isGround && rb.velocity.y > 1)
        {
            anim.SetBool("jumping", true);
            //Debug.Log(rb.velocity.y);
        }
        else if (rb.velocity.y <= 1)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        else if (bodycoll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
        }
        else
        {
            anim.SetBool("falling", false);
        }
    }


    /// <summary>
    /// 进入触发器，收集物品
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集樱桃
        if (collision.tag == "cherry")
        {
            //cherryAudio.Play();
            //  Destroy(collision.gameObject);
            //   cherry += 1;
            collision.GetComponent<Animator>().Play("Cherryget");
            MusicManerger.instance.CherryAudio();
            //cherryNum.text = cherry.ToString(); 因为不是每一帧调用所以移至Upda()
        }
        //收集宝石
        if (collision.tag == "gem")
        {
            //cherryAudio.Play();
            //Destroy(collision.gameObject);
            //gem += 1;
            //gemNum.text = gem.ToString();
            collision.GetComponent<Animator>().Play("Gemget");
            MusicManerger.instance.CherryAudio();
        }
        //碰撞到死线，重新开始
        if (collision.tag == "deadLine")
        {
            //GetComponent<AudioSource>().enabled = false;

            Invoke("Restart", 1.5f);
            //Invoke("函数名", time) 一定时间后执行函数
        }
        //楼梯区域判断
        if (collision.tag == "stairs")
        {
            climbable = true;
        }
    }
    //退出触发器
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "stairs")
        {
            climbable = false;
        }
    }

    //碰撞敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemies")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();  //实例化敌人

            //若在下落状态碰撞敌人则消灭敌人
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = Vector2.up * jumpForce;
                anim.SetBool("jumping", true);
            }
            //受伤
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                //hurtAudio.Play();
                MusicManerger.instance.HurtAudio();
                rb.velocity = new Vector2(-20, rb.velocity.y);
                anim.SetBool("hurt", true); ;
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                //hurtAudio.Play();
                MusicManerger.instance.HurtAudio();
                rb.velocity = new Vector2(20, rb.velocity.y);
                anim.SetBool("hurt", true);
                isHurt = true;
            }


        }
    }

    //跳跃
    /* void Jump()
     {
         if (Input.GetButton("Jump") && bodycoll.IsTouchingLayers(ground))
         {
             rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
             jumpAudio.Play();
             anim.SetBool("jumping", true);
         }
     } */

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            MusicManerger.instance.JumpAudio();
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            MusicManerger.instance.JumpAudio();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    /// <summary>
    /// 下蹲
    /// </summary>
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                speed = crouchSpeed;
                anim.SetBool("crouching", true);
                headcoll.enabled = false;
            }
            else
            {
                speed = runSpeed;
                anim.SetBool("crouching", false);
                headcoll.enabled = true;
            }

        }
    }

    /// <summary>
    /// 攀爬
    /// </summary>
    void Climb()
    {
        if (climbable && Input.GetKey(KeyCode.E))
        {
            //Debug.Log("climbing");
            rb.velocity = new Vector2(rb.velocity.x, 5f);
        }
        else
        {
            anim.SetBool("climb", false);
        }

    }

    //重新加载
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //樱桃收集计数
    public void CherryCount()
    {
        cherry++;
    }

    //宝石收集计数
    public void GemCount()
    {
        gem++;
    }

    /// <summary>
    /// 射击
    /// </summary>
    /*void Shot()
    {
        if (Input.GetMouseButton(0))
        {
            isShot = true;
            Debug.Log("getMouse");
        }
        else
        {
            isShot = false;
        }
    }*/
    void ReadyToDash()
    {
        isDash = true;

        dashTimeLeft = dashTime;

        lastDash = Time.time;
    }

    void Dash()
    {
        if (isDash) {
            if (dashTimeLeft > 0)
            {
                rb.velocity  =  new Vector2(dashSpeed * horizontalMove, rb.velocity.y);

                dashTimeLeft -= Time.deltaTime;

                ShadowPool.instance.GetFormPool();
            }
        }
        if (dashTimeLeft <= 0)
        {
            isDash = false;
        }
    }
}