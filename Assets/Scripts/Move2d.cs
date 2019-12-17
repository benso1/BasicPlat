using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move2d : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 5f;
    public float doubleJumpHeight = 2f;
    public float wallJumpX = 4f;
    public float wallJumpY = 3f;
    public float dashLengthX = 5f;
    public float dashLengthY = 5f;
    private float dashX = 5f;
    private float dashY = 5f;
    public float dashLength = 0.25f;
    public float dashTimer = 0f;
    private float jumpTimer = 0f;
    public float jumpBuffer = 0.2f;
    private float coyoteTimer = 0f;
    public float coyoteBuffer = 0.2f;
    public float horizontalDirection = 0f;
    public float horizontalDamping = 0.2f;
    public float speedCapX = 20f;
    public float speedLimitX = 20f;
    public float speedCapY = 10f;
    public float speedLimitY = 20f;
    public float maxFallSpeed = 20f;
    private float dampSpeed = 0f;
    public float veloX = 0f;
    public float veloY = 0f;
    public int maxDashes = 2;
    public int maxJumps = 1;
    private int numDashes = 2;
    private int doubleJump = 1;
    public bool isGrounded = false;
    public bool wallClimb = false;
    public bool leftWall = false;
    public bool rightWall = false;
    private bool dashActive = false;
    public Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 dashMovement;
    public PhysicsMaterial2D wallFriction;
    public PhysicsMaterial2D noFriction;
    //private bool dashCooling = false;
    //public float dashCooldown = 0.25f;
    //public float dashCooldownTimer = 0f;
    void Start(){
        rb = this.GetComponent<Rigidbody2D>();
        dampSpeed = 220 / speedCapX;
        speedCapX = speedLimitX;
        speedCapY = speedLimitY;
    }
    void Update()
    {
        if(dashActive){
            dashTimer -= Time.deltaTime;
        }
        if (dashTimer < 0){
            dashTimer = 0;
            dashActive = false;
            speedCapX = speedLimitX;
            speedCapY = speedLimitY;
        }
        horizontalDirection = Input.GetAxisRaw("Horizontal");
        jumpTimer -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;
        if(Input.GetButtonDown("Jump")){
            jumpTimer = jumpBuffer;
        }
        if(isGrounded){
            coyoteTimer = coyoteBuffer;
        }
    }

    void FixedUpdate()
    {
        Dash();
        WallJump();
        MoveCharacter();
        Reset();
    }

    void DashLoc(){
        if(Input.GetAxis("Horizontal") > 0){
            dashX = dashLengthX;
        }
        else if(Input.GetAxis("Horizontal") < 0){
            dashX = -dashLengthX;
        }
        else{
            dashX = 0;
        }
        if(Input.GetAxis("Vertical") > 0){
            dashY = dashLengthY;
        }
        else if(Input.GetAxis("Vertical") < 0){
            dashY = -dashLengthY;
        }
        else{
            dashY = 0;
        }
        if(dashX == 0 && dashY == 0){
            dashX = dashLengthX;
        } //8 Direction Dash Setup
        dashMovement = new Vector2(dashX, dashY);
    }
    void Dash(){
        if(isGrounded && !dashActive){
            numDashes = maxDashes; //Resets Dashes
        }
        if(Input.GetButtonDown("Fire3") && numDashes > 0 && !dashActive){ //Fire3 is Left Shift
            DashLoc();
            speedCapX = speedLimitX + Mathf.Abs(dashX);
            speedCapY = speedLimitY + Mathf.Abs(dashY);
            numDashes--;
            dashActive = true;
            dashTimer = dashLength;
            float horizontalVelocity = rb.velocity.x;
            float verticalVelocity = rb.velocity.y;
            horizontalVelocity += dashX;
            verticalVelocity += dashY;
            rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
        }
    }
    void MoveCharacter()
    {
        float horizontalVelocity = rb.velocity.x;
        float verticalVelocity = rb.velocity.y;
        horizontalVelocity += horizontalDirection;
        horizontalVelocity *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * dampSpeed);
        if(horizontalVelocity > speedCapX){
            horizontalVelocity = speedCapX;
        }
        else if(horizontalVelocity < -speedCapX){
            horizontalVelocity = -speedCapX;
        }
        if(verticalVelocity > speedCapY){
            verticalVelocity = speedCapY;
        }
        else if(verticalVelocity < -maxFallSpeed){
            verticalVelocity = -maxFallSpeed;
        }
        rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
        veloX = rb.velocity.x;
        veloY = rb.velocity.y;
        //rb.AddForce(movement * moveSpeed); //Slidy with accelleration
        //rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y); //Dash?
    }

    void Reset()
    {
        if(Input.GetButtonDown("Cancel")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Restart Scene
        }
    }

    void WallJump(){
        if(coyoteTimer > 0){
            rb.sharedMaterial = noFriction;
            doubleJump = maxJumps;
            if(jumpTimer > 0){
                float verticalVelocity = rb.velocity.y;
                verticalVelocity += jumpHeight;
                rb.velocity = new Vector2(rb.velocity.x, verticalVelocity);
                jumpTimer = 0;
                coyoteTimer = 0;
            }
        }
        else{
            if(leftWall || rightWall){
                rb.sharedMaterial = wallFriction;
            }
            if(jumpTimer > 0){
                if(leftWall){
                    float horizontalVelocity = rb.velocity.x;
                    float verticalVelocity = rb.velocity.y;
                    horizontalVelocity += wallJumpX;
                    verticalVelocity += wallJumpY;
                    rb.velocity = new Vector2(horizontalVelocity, verticalVelocity); //Wall Jump from Left
                }
                else if(rightWall){
                    float horizontalVelocity = rb.velocity.x;
                    float verticalVelocity = rb.velocity.y;
                    horizontalVelocity += -wallJumpX;
                    verticalVelocity += wallJumpY;
                    rb.velocity = new Vector2(horizontalVelocity, verticalVelocity); //Wall Jump from Right
                }
                else if(doubleJump > 0){
                    doubleJump--;
                    float verticalVelocity = rb.velocity.y;
                    verticalVelocity += doubleJumpHeight;
                    rb.velocity = new Vector2(rb.velocity.x, verticalVelocity); //Double Jump
                }
                jumpTimer = 0;
            }
        }
    }
} 
