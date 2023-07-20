using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControler : Character  
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed =5f;
    [SerializeField] private float speedup =5f;
    
    [SerializeField] private float jumforce = 300f;

    [SerializeField] private Kunai kunaiPrefab ;
    [SerializeField] private ThrowRainbowForm  circlePrefab ;
    [SerializeField] private Transform throwPoint ;
    [SerializeField] private GameObject Attackarea ;

    private bool isGrounded = true;
    private bool isAttack1= false;
    private bool isAttack2= false;
    private bool isJumping=false;
    private bool isGlide = false;
    private bool isRope;
    private bool isClimb = false;
    private bool isSleep = false;
    private bool isDeath;
    

    //private bool isIdle = true;



    private float horizontal;
    private float vertical;

    private int coin = 0;

    private Vector3 savepoint;

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }
    public override void OnInit()
    {
        base.OnInit();
        
        isAttack1 = false;
        isAttack2 = false;

        //transform.position = savepoint;
        ChangeAnim("idle");
        DeActiveAttack();

        savePoint();
        UIManager.instance.SetCoint(coin);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        { 
            return; 
        }
        if (isRope && isClimb)
        {
            isGrounded = false;
        }
        else 
        { 
            isGrounded = Checkgrounded(); 
        }
        

        //Debug.Log(isGrounded);
        //-1 0 1
        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");
        //Debug.Log(horizontal);
        if (isAttack1 || isAttack2)
        {
            
            rb.velocity = Vector2.zero;
            return;
        }
        if (isGrounded && isGlide)
        {
            
            StopGlide();
            ChangeAnim("idle");
            return;
        }
        if (!isRope)
        {
            isClimb = false;
            if (!isGlide)
            {
                rb.gravityScale = 1;
            }
        }
        if (isGrounded)
        {
            
            if (isJumping)
            {               
                return;
            }
            
            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            //run 
            if (Mathf.Abs(horizontal) > 0.1f && isGrounded && !isGlide && !isClimb)
            {
                ChangeAnim("run");
                isSleep= false;
                
            }
            //attack
            if (Input.GetKeyDown(KeyCode.E) && isGrounded)
            {
                Attack();
            }
            //throw
            if (Input.GetKeyDown(KeyCode.Q) && isGrounded)
            {
                Throw();
            }
            if (Input.GetKeyDown(KeyCode.T) && isGrounded)
            {
                Throw2();
            }
            if (Input.GetKeyDown(KeyCode.F) && isGrounded)
            {
                Sleep();
            }

        }
        //Recovery
        if (isSleep)
        {
            TimeRecoverCount -= Time.deltaTime;
        }
        if (TimeRecoverCount < 0)
        {
            Recover(HpRecover);
            TimeRecoverCount = TimeRecover;
        }
        //Glide
        if (Input.GetKeyDown(KeyCode.R))
        {
            Glide();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            StopGlide();
        }
        
        //check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }
        //moving
        if (Mathf.Abs(horizontal) > 0.1f)
        {   
            
            rb.velocity = new Vector2(horizontal  * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        

        // idle
        else if (isGrounded && !isGlide)
        {  
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
        //climb ///////////bug here
        if (Mathf.Abs(vertical) > 0.1f)
        {
            Climb();

        }



    }


    public void Climb()
    {

        if (isRope)
        {

            rb.gravityScale = 0;
            isClimb = true;
            ChangeAnim("climb");
            //isGrounded = false;
            rb.velocity = new Vector2(rb.velocity.x, vertical * Time.deltaTime * speed);
            
        }
    }
    public void Sleep()
    {
        if (isGrounded && rb.velocity.x == 0)
        {
            isSleep = true;
            ChangeAnim("sleep");
        }

    }
    public void WakeUp()
    {
        if (isSleep)
        {
            TimeRecoverCount = TimeRecover;
            isSleep = false;
            ChangeAnim("idle");
            return;
        }
       
    }
    private void Throw2()
    {
        WakeUp();
        ChangeAnim("throw");
        isAttack2 = true;

        Invoke(nameof(Resetattack), 0.5f);
        Instantiate(circlePrefab, transform.position, transform.rotation);
    }

    private bool Checkgrounded()
    {
        
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f,Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
        // return hit.collider != null
    }
    public void Glide()
    {
        
        if (isGrounded || isGlide)
        {
            return;
        }
        isGlide = true;
        rb.gravityScale = 0.15f;
        ChangeAnim("glide");
    }
    public void StopGlide()
    {
        if (!isGlide)
        {
            return;
        }
        isGlide = false;
        rb.gravityScale = 1;
        ChangeAnim("jump");
    }
    public void Attack()
    {
        WakeUp();
        ChangeAnim("attack");
        isAttack1 = true;
        Invoke(nameof(Resetattack), 0.5f);     
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }
    public void Throw()
    {
        WakeUp();
        ChangeAnim("throw");
        isAttack1 = true;
        
        Invoke(nameof(Resetattack), 0.5f);
        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }
    public void Jump()
    {
        WakeUp();
        if(isJumping || !isGrounded)
        {
            return;
        }
        if(isJumping && isGrounded)
        {
            ChangeAnim("idle");
           
        }
               
        rb.AddForce(jumforce * Vector2.up);
        isJumping = true;
        isGlide = true;
        ChangeAnim("jump");

    }
    private void Resetattack()
    {
        isAttack1 = false;
        isAttack2 = false;
        ChangeAnim("idle");
        
    }
    
    
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "platform")
    //    {
    //        OnInit();
    //    }

    //}

    internal void savePoint()
    {
        savepoint = transform.position;
    }
    private void ActiveAttack()
    {
        Attackarea.SetActive(true);
    }
    private void DeActiveAttack()
    {
        Attackarea.SetActive(false);
    }
    public void SetMoving(float horizontal)
    {
        this.horizontal = horizontal;
    }
    public void SetMovingUp(float vertical)
    {
        this.vertical = vertical;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "coin")
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.instance.SetCoint(coin);
            Destroy(collision.gameObject);
            //Debug.Log(coin);
        }
        if (collision.tag == "Deadzone")
        {
            isDeath = true;
            ChangeAnim("die");
            Invoke(nameof(OnInit), 1f);
        }
        if (collision.tag == "rope")
        {
            isRope = true;
        }

    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "rope")
        {
            isRope = false;
            isGrounded= true;
            isClimb = false;
        }
    }
    public void TeleTo(float posx, float posy)
    {
        transform.position = new Vector3(posx, posy, 0);
    }

}

