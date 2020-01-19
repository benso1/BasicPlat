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
        if(collision.gameObject.tag == "DamageBox"){
            Player.GetComponent<Move2d>().Damage();
        }
    }
    private void OnCollisionExit2D(Collision2D collision){
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "SlideKill" && Player.GetComponent<Move2d>().slideAttack()){
            collision.gameObject.transform.parent.gameObject.GetComponent<EnemyStilt>().Damage();
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
    }
}
