using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2d : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 5f;
    public bool isGrounded = false;
    public bool wallClimb = false;
    public bool leftWall = false;
    public bool rightWall = false;
    public Rigidbody2D rb;
    private Vector3 movement;
    public int maxDashes = 2;
    private int numDashes = 2;
    private Vector2 dashMovement;
    private float dashLengthX = 5f;
    private float dashLengthY = 5f;
    private float dashX = 0.2f;
    private float dashY = 0.2f;
    private bool dashActive = false;
    public float dashCooldown = 1f;
    public float dashTimer = 0f;
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
        movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
    }

    void FixedUpdate()
    {
        Jump();
        moveCharacter();
        Dash();
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
        }
        dashMovement = new Vector2(dashX, dashY);
    }
    void Dash(){
        if(isGrounded && !dashActive){
            numDashes = maxDashes;
        }
        if(Input.GetButtonDown("Fire3") && numDashes > 0 && !dashActive){
            DashLoc();
            rb.AddForce(dashMovement, ForceMode2D.Impulse);
            numDashes--;
            dashActive = true;
            dashTimer = dashCooldown;
        }
        //rb.velocity = dashMovement;
        //Fire3 is Left Shift, Can change in unity/edit/project settings/input/axes
    }
    void moveCharacter()
    {
        rb.AddForce(movement * moveSpeed); //Slidy with accelleration
        //rb.velocity = dashMovement * moveSpeed; //Dash?
        //transform.position += movement * Time.deltaTime * moveSpeed; //Normal movement
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded == true){
            rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse); //Jump
        }
    }
} 
