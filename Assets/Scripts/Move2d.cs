using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move2d : MonoBehaviour
{
    public float jumpHeight = 5f;
    public float doubleJumpHeight = 2f;
    public int extraJumps = 1;
    public float wallJumpX = 4f;
    public float wallJumpY = 3f;
    public float dashLengthX = 5f;
    private float dashX = 5f;
    public float dashLengthY = 5f;
    private float dashY = 5f;
    public int maxDashes = 1;
    private float dashTimer = 0f;
    public float dashActiveTime = 0.25f;
    private float jumpBufferTimer = 0f;
    public float jumpBuffer = 0.2f;
    private float coyoteTimer = 0f;
    public float coyoteBuffer = 0.2f;
    private float dashBufferTimer = 0f;
    public float dashBuffer = 0.2f;
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    public float horizontalDamping = 0.2f;
    private float speedCapX = 20f;
    public float speedLimitX = 20f;
    private float speedCapY = 10f;
    public float speedLimitY = 10f;
    public float maxFallSpeed = 20f;
    private float dampSpeed = 0f;
    private int numDashes = 1;
    private int doubleJump = 1;
    public float stopSpeed = 5f;
    public float testVeloX = 0f;
    public float testVeloY = 0f;
    private bool isGrounded = false;
    private bool leftWall = false;
    private bool rightWall = false;
    public Rigidbody2D rb;
    void Start(){ //Initializes variables
        dampSpeed = 220 / speedCapX;
        speedCapX = speedLimitX;
        speedCapY = speedLimitY;
        SetGravity();
    }
    void SetGravity(){
        float gravity = rb.gravityScale;
        wallJumpY *= (3f/4f * gravity);
        doubleJumpHeight *= (3f/4f  *gravity);
        jumpHeight *= (3f/4f * gravity);
        dashLengthY *= (3f/4f * gravity);
    }
    void Update(){ //Once per frame, good for getting Input from the User
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        DecrementTimers();
        SetTimers();
        Reset();
    }
    void FixedUpdate(){ //Operates every physics step, could be multiple times per frame
        Run();
        Dash();
        WallJump();
        CapSpeeds();
    }
    void DecrementTimers(){ //Lowers the time on all timers
        jumpBufferTimer -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;
        dashBufferTimer -= Time.deltaTime;
        dashTimer -= Time.deltaTime;
        if (dashTimer < 0){
            speedCapX = speedLimitX;
            speedCapY = speedLimitY;
        }
    }
    void SetTimers(){ //Sets timers based on Inputs
        if(Input.GetButtonDown("Jump")){
            jumpBufferTimer = jumpBuffer;
        }
        if(isGrounded){
            coyoteTimer = coyoteBuffer;
        }
        if(Input.GetButtonDown("Fire3")){
            dashBufferTimer = dashBuffer;
        }
    }
    void Dash(){ //Lets player dash on Left Shift
        if(isGrounded && dashTimer < 0){ //Resets Dashes on ground
            numDashes = maxDashes; 
        }
        if(dashBufferTimer > 0 && numDashes > 0 && dashTimer < 0){ 
            SetDashDirection();
            speedCapX = speedLimitX + Mathf.Abs(dashX);
            //speedCapY = speedLimitY + Mathf.Abs(dashY);
            numDashes--;
            dashTimer = dashActiveTime;
            dashBufferTimer = 0;
            SetDashVelocity();
        }
    }
    void SetDashDirection(){ //DashLengthX/Y is the length of the dash, DashX/Y is the vector you will dash at
        if(horizontalInput > 0){ 
            dashX = dashLengthX;
        }
        else if(horizontalInput < 0){
            dashX = -dashLengthX;
        }
        else{
            dashX = 0;
        }
        if(verticalInput > 0){
            dashY = dashLengthY;
        }
        else if(verticalInput < 0){
            dashY = -dashLengthY;
        }
        else{
            dashY = 0;
        }
        if(dashX == 0 && dashY == 0){
            dashX = dashLengthX;
        } //8 Direction Dash Setup
    }
    void SetDashVelocity(){
        float horizontalVelocity = rb.velocity.x;
        float verticalVelocity = rb.velocity.y;
        horizontalVelocity += dashX;
        verticalVelocity += dashY;
        if(dashX > 0){
            if(horizontalVelocity < dashX){
                horizontalVelocity = dashX;
            }
        }
        else if(dashX < 0){
            if(horizontalVelocity > -dashX){
                horizontalVelocity = -dashX;
            }
        }
        if(dashY > 0){
            if(verticalVelocity < dashY){
                verticalVelocity = dashY;
            }
        }
        else if(dashY < 0){
            if(verticalVelocity > -dashY){
                verticalVelocity = -dashY;
            }
        }
        rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
    }
    void AddVelocity(float speedX, float speedY){ //Add speed with velocity function
        float horizontalVelocity = rb.velocity.x;
        float verticalVelocity = rb.velocity.y;
        horizontalVelocity += speedX;
        verticalVelocity += speedY;
        rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
    }
    void SetYVelocity(float speedX, float speedY){ //Add Speed to X, and set Y
        float horizontalVelocity = rb.velocity.x;
        horizontalVelocity += speedX;
        rb.velocity = new Vector2(horizontalVelocity, speedY);
    }
    void SetVelocity(float speedX, float speedY){ //Set both X and Y velocity
        rb.velocity = new Vector2(speedX, speedY);
    }
    void WallJump(){ //Jump with Space, Double Jump while in air, Wall Jump when on wall
        if(coyoteTimer > 0){
            doubleJump = extraJumps;
            if(jumpBufferTimer > 0){
                SetYVelocity(0, jumpHeight); //Jump
                jumpBufferTimer = 0;
                coyoteTimer = 0;
            }
        }
        else{
            if(jumpBufferTimer > 0){
                if(leftWall){
                    SetYVelocity(wallJumpX, wallJumpY); //Wall Jump from Left
                }
                else if(rightWall){
                    SetYVelocity(-wallJumpX, wallJumpY); //Wall Jump from Right
                }
                else if(doubleJump > 0){
                    doubleJump--;
                    SetYVelocity(0, doubleJumpHeight); //Double Jump
                }
                jumpBufferTimer = 0;
            }
        }
    }
    void Run(){ //Adjust horizontal speed based on Arrow Keys
        if(rb.velocity.x < stopSpeed && rb.velocity.x > 0 && horizontalInput <= 0){
            SetVelocity(0, rb.velocity.y);
        }
        else if(rb.velocity.x > -stopSpeed && rb.velocity.x < 0 && horizontalInput >= 0){
            SetVelocity(0, rb.velocity.y);
        }
        else{
            AddVelocity(horizontalInput, 0);
        }
    }
    void CapSpeeds(){ //Prevent player from moving too fast
        float horizontalVelocity = rb.velocity.x;
        float verticalVelocity = rb.velocity.y;
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
        testVeloX = rb.velocity.x;
        testVeloY = rb.velocity.y;
    }
    void Reset(){//Restart Level on Escape Key
        if(Input.GetButtonDown("Cancel")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Restart Scene
        }
    }
    public void SetLeftWall(bool exists){ //Setter for leftWall
        leftWall = exists;
    }
    public void SetRightWall(bool exists){ //Setter for rightWall
        rightWall = exists;
    }
    public void SetGrounded(bool exists){ //Setter for isGrounded
        isGrounded = exists;
    }
}