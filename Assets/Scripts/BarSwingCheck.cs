using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarSwingCheck : MonoBehaviour
{
    private GameObject Player;
    void Start(){
        Player = gameObject.transform.parent.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Bar"){
            Player.GetComponent<Move2d>().SetBarSwingAvailable(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.tag == "Bar"){
            Player.GetComponent<Move2d>().SetBarSwingAvailable(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Bar"){
            Player.GetComponent<Move2d>().SetBarSwingAvailable(true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.tag == "Bar"){
            Player.GetComponent<Move2d>().SetBarSwingAvailable(false);
        }
    }
}
