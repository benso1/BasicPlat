using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCollision : MonoBehaviour
{
    GameObject Player;
    void Start(){
        Player = this.gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "DamageBox"){// && Player.GetComponent<Move2d>().lastKilled() == collision.gameObject){
            Player.GetComponent<Move2d>().Damage();
            Debug.Log("Ouch");
            //Debug.Log(collision.gameObject.tag);
        }
    }
    private void OnCollisionExit2D(Collision2D collision){
    }
}
