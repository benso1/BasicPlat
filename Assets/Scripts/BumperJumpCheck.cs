using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperJumpCheck : MonoBehaviour
{
    private GameObject Player;
    void Start(){
        Player = gameObject.transform.parent.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Bumper"){
            Player.GetComponent<Move2d>().SetBumperAvailable(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Bumper"){
            Player.GetComponent<Move2d>().SetBumperAvailable(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Bumper"){
            Player.GetComponent<Move2d>().SetBumperAvailable(true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.tag == "Bumper"){
            Player.GetComponent<Move2d>().SetBumperAvailable(false);
        }
    }
}
