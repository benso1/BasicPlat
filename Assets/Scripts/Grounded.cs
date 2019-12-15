using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    GameObject Player;
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Ground"){
            Player.GetComponent<Move2d>().isGrounded = true;
        }
    }

     private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Ground"){
            Player.GetComponent<Move2d>().isGrounded = false;
        }
    }

}