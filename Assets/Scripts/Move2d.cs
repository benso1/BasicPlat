﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move2d : MonoBehaviour
{
//Jump
    public float jumpHeight = 5f;
//Double Jump
    public float doubleJumpHeight = 2f;
    private int extraJumps = 1;
    private int doubleJump = 1;
//Wall Jump
    public float wallJumpX = 4f;
    public float wallJumpY = 3f;
    private bool leftWallLast = false;
    private bool rightWallLast = false;
    public float wallJumpBackSpeed = 0.2f;
//Dash
    public float dashLengthX = 5f;
    private float dashX = 5f;
    public float dashLengthY = 5f;
    private float dashY = 5f;
    private int maxDashes = 1;
    private int numDashes = 1;
//Wall Run
    public float wallRunDropSpeed = 0.1f;
//Slide
    public float slideLength = 5f;
    private bool keepSliding = false;
//Timers
    private float dashTimer = 0f;
    public float dashActiveTime = 0.25f;
    private float slideTimer = 0f;
    public float slideActiveTime = 0.25f;
    private float shortHopTime = 0.2f;
    private float shortHopTimer = 0f;
//Buffers
    private float jumpBufferTimer = 0f;
    public float jumpBuffer = 0.2f;
    private float wallJumpBufferTimer = 0f;
    public float wallJumpBuffer = 0.2f;
    private float coyoteBufferTimer = 0f;
    public float coyoteBuffer = 0.2f;
    private float dashBufferTimer = 0f;
    public float dashBuffer = 0.2f;
    private float wallRunBufferTimer = 0f;
    public float wallRunBuffer = 0.2f;
//Speed and Velocity
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    public float stopSpeed = 5f;
    public float horizontalDamping = 0.2f;
    private float dampSpeed = 0f;
    private float speedCapX = 20f;
    public float speedLimitX = 20f;
    private float speedCapY = 10f;
    public float speedLimitY = 10f;
    public float maxFallSpeed = 20f;
    public float testVeloX = 0f;
    public float testVeloY = 0f;
//States
    private bool leftWall = false;
    private bool rightWall = false;
    private bool isGrounded = false;
    private bool isWallGrounded = false;
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    private bool isWallJumping = false;
    private bool isDashing = false;
    private bool isSliding = false;
    private bool isWallRunning = false;
//Particle Effects
    private bool jumpParticles = false;
    private bool doubleJumpParticles = false;
    private bool wallJumpParticles = false;
    private bool wallRunParticles = false;
    private bool dashParticles = false;
    private bool slideParticles = false;
    private float particleJumpLength = 0.75f;
    private float particleDoubleJumpLength = 0.75f;
    private float particleWallJumpLength = 0.75f;
    private float particleDashLength = 0.75f;
    private float particleSlideLength = 0.75f;
    private float particleWallRunLength = 0.75f;
    private float particleTimer = 0f;
//Stored Objects
    private float playerScaleY;
    public Transform player;
    public Rigidbody2D rb;
    public ParticleSystem ps;
    public ParticleSystem.MainModule main;
//Functions
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
        coyoteBufferTimer -= Time.deltaTime;
        dashBufferTimer -= Time.deltaTime;

        if(wallJumpBufferTimer >= 0){
            wallJumpBufferTimer -= Time.deltaTime;
            if(leftWallLast && isWallJumping){
                if(horizontalInput < -wallJumpBackSpeed){
                    horizontalInput = -wallJumpBackSpeed;
                }
            }
            else if(rightWallLast && isWallJumping){
                if(horizontalInput > wallJumpBackSpeed){
                    horizontalInput = wallJumpBackSpeed;
                }
            }
        }
        if(wallJumpBufferTimer < 0){
            leftWallLast = false;
            rightWallLast = false;
        }

        if(wallRunBufferTimer >= 0){
            wallRunBufferTimer -= Time.deltaTime;
        }
        if(wallRunBufferTimer < 0){
            //Wall Run Ends
        }
        
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
        if(slideTimer >= 0 && slideTimer < 5f){
            slideTimer -= Time.deltaTime;
        }
        if (slideTimer < 0){
            slideTimer = 5f;
            player.localScale = new Vector3(player.localScale.x, playerScaleY, player.localScale.z);
            if(keepSliding){
                slideTimer = 0f;
                player.localScale = new Vector3(player.localScale.x, playerScaleY / 2f, player.localScale.z);
            }
        }
    }
    void SetTimers(){ //Sets timers based on Inputs
        if(Input.GetButtonDown("Jump")){ //Space
            jumpBufferTimer = jumpBuffer;
        }
        if(isGrounded){
            coyoteBufferTimer = coyoteBuffer;
            isJumping = false;
        }
        if(Input.GetButtonDown("Fire3")){ //Left Shift
            dashBufferTimer = dashBuffer;
            isJumping = false;
        }
        if(Input.GetButton("Fire1")){ //Left Ctrl
            wallRunBufferTimer = wallRunBuffer;
            isJumping = false;
        }
    }
    void Reset(){//Restart Level on Escape Key
        if(Input.GetButtonDown("Cancel")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Restart Scene
        }
    }
    void FixedUpdate(){ //Operates every physics step, could be multiple times per frame
        Run();
        Dash();
        Slide();
        WallJump();
        WallRun();
        CapSpeeds();
        DisplayParticles();
    }
    void UpdateState(){ //Lets us figure out which state we are in for animation/particle effects
        isJumping = false;
        isDoubleJumping = false;
        isWallJumping = false;
        isDashing = false;
        isSliding = false;
        isWallRunning = false;
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
    void Dash(){ //Lets player dash on Left Shift
        if(isGrounded){ //Resets Dashes on ground
            numDashes = maxDashes; 
        }
        if(dashBufferTimer > 0 && numDashes > 0 && dashTimer < 0){ 
            SetDashDirection();
            //speedCapX = speedLimitX + Mathf.Abs(dashX);
            //speedCapY = speedLimitY + Mathf.Abs(dashY);
            numDashes--;
            dashTimer = dashActiveTime;
            dashBufferTimer = 0;
            UpdateState();
            isDashing = true;
            SetDashVelocity();
        }
    }
    void Slide(){ //Slide when you dash into the ground
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
    void WallJump(){ //Jump with Space, Double Jump while in air, Wall Jump when on wall
        if(coyoteBufferTimer > 0){
            doubleJump = extraJumps;
            if(jumpBufferTimer > 0){
                SetYVelocity(0, jumpHeight); //Jump
                jumpBufferTimer = 0;
                coyoteBufferTimer = 0;
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
                    SetVelocity(wallJumpX, wallJumpY); //Wall Jump from Left
                    jumpBufferTimer = 0;
                    wallJumpBufferTimer = wallJumpBuffer;
                    leftWallLast = true;
                    rightWallLast = false;
                }
                else if(rightWall){
                    UpdateState();
                    isWallJumping = true;
                    SetVelocity(-wallJumpX, wallJumpY); //Wall Jump from Right
                    jumpBufferTimer = 0;
                    wallJumpBufferTimer = wallJumpBuffer;
                    rightWallLast = true;
                    leftWallLast = false;
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
    void WallRun(){ //Run on certain background walls with Left Ctrl
        if(isWallGrounded && wallRunBufferTimer > 0){
            UpdateState();
            isWallRunning = true;
            wallRunBufferTimer = 0;
            if(rb.velocity.y < -wallRunDropSpeed){
                SetYVelocity(0, -wallRunDropSpeed);
            }
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
            particleTimer = particleWallRunLength;
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
        horizontalVelocity = dashX;
        verticalVelocity = dashY;
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
    public void SetLeftWall(bool exists){ //Setter for leftWall
        leftWall = exists;
    }
    public void SetRightWall(bool exists){ //Setter for rightWall
        rightWall = exists;
    }
    public void SetGrounded(bool exists){ //Setter for isGrounded
        isGrounded = exists;
    }
    public void SetWallGrounded(bool exists){ //Setter for isGrounded
        isWallGrounded = exists;
    }
    public void SetKeepSliding(bool exists){ //Setter for keepSliding
        keepSliding = exists;
    }
}