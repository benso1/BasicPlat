using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move2d : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 5f;
    public float doubleJumpHeight = 2f;
    private float dashLengthX = 5f;
    private float dashLengthY = 5f;
    private float dashX = 5f;
    private float dashY = 5f;
    public float dashLength = 0.25f;
    public float dashTimer = 0f;
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
    }
    void Update()
    {
        if(dashActive){
            dashTimer -= Time.deltaTime;
        }
        if (dashTimer < 0){
            dashTimer = 0;
            dashActive = false;
        }
        movement = new Vector2(Input.GetAxis("Horizontal"), 0f);
    }

    void FixedUpdate()
    {
        Jump();
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
            rb.AddForce(dashMovement, ForceMode2D.Impulse);
            numDashes--;
            dashActive = true;
            dashTimer = dashLength;
        } 
    }
    void MoveCharacter()
    {
        rb.AddForce(movement * moveSpeed); //Slidy with accelleration
        //rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y); //Dash?
    }

    void Jump()
    {
        if(isGrounded){
            doubleJump = maxJumps;
            if(Input.GetButtonDown("Jump")){
                rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse); //Jump
            }
        }
        else if(Input.GetButtonDown("Jump") && doubleJump > 0){
            doubleJump--;
             rb.AddForce(new Vector2(0f, doubleJumpHeight), ForceMode2D.Impulse); //Double Jump
        }
    }

    void Reset()
    {
        if(Input.GetButtonDown("Cancel")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Restart Scene
        }
    }

    void WallJump(){
        if(!isGrounded){
            if(leftWall || rightWall){
                rb.sharedMaterial = wallFriction;
            }
        }
        else{
            rb.sharedMaterial = noFriction;
        }
    }
} 
