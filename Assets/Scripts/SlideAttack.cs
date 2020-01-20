using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAttack : MonoBehaviour
{
    GameObject Player;
    void Start(){
        Player = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision){
        if(collision.gameObject.tag == "SlideKill" && Player.GetComponent<Move2d>().slideAttack()){
            //Player.GetComponent<Move2d>().Killed(collision.gameObject.transform.parent.gameObject);
            collision.gameObject.transform.parent.gameObject.GetComponent<EnemyStilt>().Damage();
            Debug.Log("Slide");
            Debug.Log(collision.gameObject.tag);
            //Physics.IgnoreCollision(collision.gameObject.transform.parent.gameObject.GetComponent<Collider>(), Player.GetComponent<Collider>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
    }
}
