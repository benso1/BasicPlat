using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    GameObject Player;
    void Start(){
        Player = gameObject.transform.parent.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Ground"){
            Player.GetComponent<Move2d>().SetGrounded(true);
        }
        if(collision.gameObject.tag == "WallRunWall"){
            Player.GetComponent<Move2d>().SetWallGrounded(true);
        }
        if(collision.gameObject.tag == "Finish"){
            Player.GetComponent<Move2d>().CollectableParticles();
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Ground"){
            Player.GetComponent<Move2d>().SetGrounded(false);
        }
        if(collision.gameObject.tag == "WallRunWall"){
            Player.GetComponent<Move2d>().SetWallGrounded(false);
        }
    }

}