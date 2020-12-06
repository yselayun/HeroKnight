using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private float movementInputDirection; //lay dau vao dieu huong di chuyen

    public int amountOfJump;

    public bool isFacingRight = true;
    public bool isRunning = true;
    public bool isJumping;
    public bool isGrounded;
    public bool isTouchingWall;
    public bool canJump;
    public bool jumpInput = false;

    private Rigidbody2D rigidbody2d; //
    private Animator anim;

    public int inputJumpCounter = 2;

    public float _speed = 100.0f;
    public float _jumpForce = 150.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;

    public Transform GroundCheck;
    public Transform WallCheck;
    public LayerMask whatIsGround;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJump = inputJumpCounter;
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        checkMovementDirection();
        UpdateAnimations();
        checkIfCanJump();
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        checkSurroudings();
    }
    private void checkInput() // 
    {
        movementInputDirection = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpInput = true;
            Jump();
        }
        if (Input.GetButtonUp("Jump"))
        {
            jumpInput = false;
        }
    }

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

        if (rigidbody2d.velocity.x != 0)
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
        if(isGrounded && rigidbody2d.velocity.y <= 0)
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
    }
    private void Jump()
    {
        
        if (canJump)
        {
            
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, _jumpForce);
            amountOfJump--;
        }
        
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("JumpInput", jumpInput);
        anim.SetFloat("yVelocity", rigidbody2d.velocity.y);
    }
    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + wallCheckDistance, WallCheck.position.y, WallCheck.position.z));
    }
}
