using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private float movementInputDirection; //lay dau vao dieu huong di chuyen
    private float turnTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;

    public int amountOfJump;
    public int facingDirection = 1;

    public bool isFacingRight = true;
    public bool isRunning = true;
    public bool isJumping;
    public bool isGrounded;
    public bool isTouchingWall;
    public bool canJump;
    public bool jumpInput = false;
    public bool canMove = true;
    public bool canFlip = true;
    public bool isWallSliding;
    public bool isDashing;
    private bool knockback;

    [SerializeField]
    private Vector2 knockbackSpeed;


    private Rigidbody2D rigidbody2d; //
    private Animator anim;

    public int inputJumpCounter = 2;

    public float _speed = 100.0f;
    public float _jumpForce = 150.0f;
    public float groundCheckRadius;
    public float wallCheckDistance = 12.5f;
    public float wallSlideSpeed = 45.0f;
    public float movementSpeedInAir = 90.0f;
    public float airDragMultiplier = 10.0f;
    public float wallHopForce = 100.0f;
    public float wallJumpForce = 200.0f;
    public float turnTimerSet = 0.1f;
    //public float dashTime = 0.2f;
    //public float dashSpeed = 350f;
    //public float distanceBetweenImage = 100f;
    //public float dashCoolDown = 2.5f;

    public Vector2 wallHopDirection;  
    public Vector2 wallJumpDirection;

    public Transform GroundCheck;
    public Transform WallCheck;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        GroundCheck = transform.Find("CheckGround");
        WallCheck = transform.Find("WallCheck");
        rigidbody2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJump = inputJumpCounter;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        checkMovementDirection();
        UpdateAnimations();
        checkIfCanJump();
        checkIfWallSliding();
        //CheckDash();
        checkKnockback();

        //  Debug.Log(rigidbody2d.velocity.y < 0 );

        //  Debug.Log(isGrounded);
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        checkSurroudings();
    }
    //
    private void checkInput() // 
    {
        movementInputDirection = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded == true && rigidbody2d.velocity.y <= 0)
            {
                jumpInput = false;
            }
            jumpInput = true;
            Jump();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jumpInput = false;

        }
        //if(Input.GetKeyDown("Horizontal") && isTouchingWall)
        //{
        //    if(!isGrounded && movementInputDirection != facingDirection)
        //    {
        //        canMove = false;
        //        canFlip = false;
        //        turnTimer = turnTimerSet;
        //    }
        //}
        //if (!canMove)
        //{
        //    turnTimer -= Time.deltaTime;
        //    if(turnTimer <= 0)
        //    {
        //        canMove = true;
        //        canFlip = true;
        //    }
        //}
        //if (Input.GetButtonDown("Dash"))
        //{
        //    isRunning = false;
        //    if(Time.time >= (lastDash + dashCoolDown))
        //        AttemptToDash();
        //}
    }
    //private void AttemptToDash()
    //{
    //    isDashing = true;
    //    dashTimeLeft = dashTime;
    //    lastDash = Time.time;
    //
    //    PlayerAfterImagePool.Instance.GetFromPool();
    //    lastImageXpos = transform.position.x;
    //}

    //

    //private void CheckDash()
    //{
    //    if (isDashing)
    //    {
    //       if(dashTimeLeft > 0)
    //        {
    //            canMove = false;
    //            canFlip = false;
    //            rigidbody2d.velocity = new Vector2(dashSpeed * facingDirection, rigidbody2d.velocity.y);
    //            dashTimeLeft -= Time.deltaTime;
    //
    //            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImage)
    //            {
    //                PlayerAfterImagePool.Instance.GetFromPool();
    //                lastImageXpos = transform.position.x;
    //            }
    //        }
    //       else if(dashTimeLeft <= 0 || isTouchingWall)
    //        {
    //            isDashing = false;
    //            canMove = true;
    //            canFlip = true;
    //        }
    //    }
    //}
    //
    private void checkIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rigidbody2d.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }
    //
    private void checkMovementDirection() //
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }


        if (Mathf.Abs(rigidbody2d.velocity.x) >= 0.01f) 
        {
            isRunning = true;
        
        }
        else if (rigidbody2d.velocity.x == 0)
        {
            isRunning = false;
        
        }
    }

    private void checkSurroudings()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(WallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void checkIfCanJump()
    {
        if ((isGrounded && rigidbody2d.velocity.y <= 0) || isWallSliding)
        {
            amountOfJump = inputJumpCounter;
        }
        if (amountOfJump <= 0)
        {
            canJump = false;
            
        }
        else
        {
            canJump = true;
        }
    }
    private void ApplyMovement() //
    {

        
         rigidbody2d.velocity = new Vector2(_speed * movementInputDirection, rigidbody2d.velocity.y);
        
        
        if(!isGrounded && !isWallSliding && movementInputDirection != 0 && !knockback)
        {
            Vector2 addToForce = new Vector2(movementSpeedInAir * movementInputDirection, 0);
            rigidbody2d.AddForce(addToForce);

            if(Mathf.Abs(rigidbody2d.velocity.x) > _speed)
            {
                rigidbody2d.velocity = new Vector2(_speed * movementInputDirection, rigidbody2d.velocity.y);
            }
        }
        else if(!isGrounded && !isWallSliding && movementInputDirection == 0 && !knockback)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x * airDragMultiplier, rigidbody2d.velocity.y);
        }

        if (isWallSliding)
        {
            if(rigidbody2d.velocity.y < -wallSlideSpeed)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -wallSlideSpeed);
            }
        }
    }
    private void Jump()
    {
        if (canJump && !isWallSliding)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, _jumpForce);
            amountOfJump--;
        }
        else if(isWallSliding && movementInputDirection == 0 && canJump)
        {
            isWallSliding = false;
            amountOfJump--;
            Vector2 addToForce = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
            rigidbody2d.AddForce(addToForce, ForceMode2D.Impulse);
        }
        else if ((isWallSliding || isTouchingWall) && movementInputDirection != 0 && canJump)
        {
            
            isWallSliding = false;
            amountOfJump--;
            Vector2 addToForce = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rigidbody2d.AddForce(addToForce, ForceMode2D.Impulse);
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("JumpInput", jumpInput);
        anim.SetFloat("yVelocity", rigidbody2d.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void disableFlip()
    {
        canFlip = false;
    }

    private void enableFlip()
    {
        canFlip = true;
    }
    public void Flip()
    {

        isFacingRight = !isFacingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
        //
        Vector3 GCScale = GroundCheck.localScale;
        GCScale.x *= -1;
        GroundCheck.localScale = GCScale;
        //
        Vector3 direction;
        direction = WallCheck.localScale;
        
        direction.x *= -1;
        WallCheck.localScale = direction;
        if (direction.x == -1)
        {
            direction = WallCheck.localPosition;
            direction.x = wallCheckDistance;
            WallCheck.localPosition = direction;
        }
        else
        {
            direction = WallCheck.localPosition;
            direction.x = 0;
            WallCheck.localPosition = direction;
        }

        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
        }
    }
    //

    public void KnockBack(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rigidbody2d.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
        anim.SetBool("knockback", knockback);
    }

    private void checkKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rigidbody2d.velocity = new Vector2(0.0f, rigidbody2d.velocity.y);
        }
    }


 
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + wallCheckDistance, WallCheck.position.y, WallCheck.position.z));
    }

}
