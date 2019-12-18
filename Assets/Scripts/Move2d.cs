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
    private float slideTimer = 0f;
    public float slideActiveTime = 0.25f;
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
    public float shortHopTime = 0.2f;
    private float shortHopTimer = 0f;
    public float stopSpeed = 5f;
    public float slideLength = 5f;
    public float testVeloX = 0f;
    public float testVeloY = 0f;
    private bool isGrounded = false;
    private bool leftWall = false;
    private bool rightWall = false;
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    private bool isWallJumping = false;
    private bool isDashing = false;
    private bool isSliding = false;
    private bool isWallRunning = false;
    private bool jumpParticles = false;
    private bool doubleJumpParticles = false;
    private bool wallJumpParticles = false;
    private bool wallRunParticles = false;
    private bool dashParticles = false;
    private bool slideParticles = false;
    public float particleJumpLength = 0.75f;
    public float particleDoubleJumpLength = 0.75f;
    public float particleWallJumpLength = 0.75f;
    public float particleDashLength = 0.75f;
    public float particleSlideLength = 0.75f;
    private float particleTimer = 0f;
    public float wallRunLength = 0.75f;
    private float wallRunTimer = 0f;
    public Rigidbody2D rb;
    public ParticleSystem ps;
    public ParticleSystem.MainModule main;
    public Transform player;
    private float playerScaleY;
    void Start(){ //Initializes variables
        dampSpeed = 220 / speedCapX;
        speedCapX = speedLimitX;
        speedCapY = speedLimitY;
        main = ps.main;
        SetGravity();
        playerScaleY = player.localScale.y;
        ps.Stop();
    }
    void SetGravity(){ //Set y-velocities to scale with gravity
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
    void DecrementTimers(){ //Lowers the time on all timers
        jumpBufferTimer -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;
        dashBufferTimer -= Time.deltaTime;
        
        if(particleTimer >= 0){
            particleTimer -= Time.deltaTime;
        }
        if(particleTimer < 0){
            UpdateParticles();
        }

        if(dashTimer >= 0){
            dashTimer -= Time.deltaTime;
        }
        if (dashTimer < 0){
            dashTimer = -1f;
            speedCapX = speedLimitX;
            speedCapY = speedLimitY;
        }
        if(slideTimer >= 0){
            slideTimer -= Time.deltaTime;
        }
        if (slideTimer < 0){
            slideTimer = -1f;
            player.localScale = new Vector3(player.localScale.x, playerScaleY, player.localScale.z);
        }
    }
    void SetTimers(){ //Sets timers based on Inputs
        if(Input.GetButtonDown("Jump")){
            jumpBufferTimer = jumpBuffer;
        }
        if(isGrounded){
            coyoteTimer = coyoteBuffer;
            isJumping = false;
        }
        if(Input.GetButtonDown("Fire3")){
            dashBufferTimer = dashBuffer;
            isJumping = false;
        }
    }
    void FixedUpdate(){ //Operates every physics step, could be multiple times per frame
        Run();
        Dash();
        Slide();
        WallJump();
        CapSpeeds();
        DisplayParticles();
    }
    void DisplayParticles(){ //Runs particle effects until animations are added
        if(isJumping && !jumpParticles){
            UpdateParticles();
            isJumping = true;
            main.startColor = new Color(0f, 0f, 0f, 1f);
            particleTimer = particleJumpLength;
            jumpParticles = true;
            ps.Play();
        }
        if(isDoubleJumping && !doubleJumpParticles){
            UpdateParticles();
            isDoubleJumping = true;
            main.startColor = Color.blue;
            particleTimer = particleDoubleJumpLength;
            doubleJumpParticles = true;
            ps.Play();
        }
        if(isDashing && !dashParticles){
            UpdateParticles();
            isDashing = true;
            main.startColor = Color.magenta;
            particleTimer = particleDashLength;
            dashParticles = true;
            ps.Play();
        }
        if(isWallJumping && !wallJumpParticles){
            UpdateParticles();
            isWallJumping = true;
            main.startColor = Color.red;
            particleTimer = particleWallJumpLength;
            wallJumpParticles = true;
            ps.Play();
        }
        if(isSliding && !slideParticles){
            UpdateParticles();
            isSliding = true;
            main.startColor = new Color(1f, 0.637f, 0f, 1f);
            particleTimer = particleSlideLength;
            slideParticles = true;
            ps.Play();
        }
        if(isWallRunning && !wallRunParticles){
            UpdateParticles();
            isWallRunning = true;
            main.startColor = new Color(0.75f, 0f, 1f, 1f);
            particleTimer = wallRunLength;
            wallRunParticles = true;
            ps.Play();
        }
    }
    void UpdateParticles(){ //Lets us know which particles are running so we don't disable them
        wallJumpParticles = false;
        dashParticles = false;
        jumpParticles = false;
        doubleJumpParticles = false;
        slideParticles = false;
        wallRunParticles = false;
        UpdateState();
        ps.Stop();
    }
    void UpdateState(){ //Lets us figure out which state we are in for animation/particle effects
        isJumping = false;
        isDoubleJumping = false;
        isWallJumping = false;
        isDashing = false;
        isSliding = false;
        isWallRunning = false;
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
            UpdateState();
            isDashing = true;
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
    void SetDashVelocity(){ //Sets the direction of the dash
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
    void WallJump(){ //Jump with Space, Double Jump while in air, Wall Jump when on wall
        if(coyoteTimer > 0){
            doubleJump = extraJumps;
            if(jumpBufferTimer > 0){
                SetYVelocity(0, jumpHeight); //Jump
                jumpBufferTimer = 0;
                coyoteTimer = 0;
                UpdateState();
                isJumping = true;
                shortHopTimer = shortHopTime;
            }
        }
        else{
            if(jumpBufferTimer > 0){
                if(leftWall){
                    UpdateState();
                    isWallJumping = true;
                    SetYVelocity(wallJumpX, wallJumpY); //Wall Jump from Left
                    jumpBufferTimer = 0;
                }
                else if(rightWall){
                    UpdateState();
                    isWallJumping = true;
                    SetYVelocity(-wallJumpX, wallJumpY); //Wall Jump from Right
                    jumpBufferTimer = 0;
                }
                else if(doubleJump > 0){
                    doubleJump--;
                    UpdateState();
                    isDoubleJumping = true;
                    SetYVelocity(0, doubleJumpHeight); //Double Jump
                    jumpBufferTimer = 0;
                }
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
    void Slide(){
        if(isDashing && dashY < 0 && isGrounded){
            UpdateState();
            isSliding = true;
            speedCapX = speedLimitX + Mathf.Abs(slideLength);
            float slideDirection = slideLength;
            if(dashX < 0){
                slideDirection = -slideLength;
            }
            AddVelocity(slideDirection, 0);
            slideTimer = slideActiveTime;
            player.localScale = new Vector3(player.localScale.x, playerScaleY / 2f, player.localScale.z);
        }
    }
    void Reset(){//Restart Level on Escape Key
        if(Input.GetButtonDown("Cancel")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Restart Scene
        }
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