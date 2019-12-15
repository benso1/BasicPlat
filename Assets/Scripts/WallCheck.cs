using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public bool left = true;
    GameObject Player;
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Ground"){
            if(left){
                Player.GetComponent<Move2d>().leftWall = true;
            }
            else{
                Player.GetComponent<Move2d>().rightWall = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Ground"){
            if(left){
                Player.GetComponent<Move2d>().leftWall = false;
            }
            else{
                Player.GetComponent<Move2d>().rightWall = false;
            }
        }
    }
}
