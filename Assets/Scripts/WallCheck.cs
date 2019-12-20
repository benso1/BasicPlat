using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public bool left = true;
    private GameObject Player;
    void Start(){
        Player = gameObject.transform.parent.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Ground"){
            if(left){
                Player.GetComponent<Move2d>().SetLeftWall(true);
            }
            else{
                Player.GetComponent<Move2d>().SetRightWall(true);
            }
        }
        if(collision.gameObject.tag == "Finish"){
            Player.GetComponent<Move2d>().CollectableParticles();
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Ground"){
            if(left){
                Player.GetComponent<Move2d>().SetLeftWall(false);
            }
            else{
                Player.GetComponent<Move2d>().SetRightWall(false);
            }
        }
    }
}
