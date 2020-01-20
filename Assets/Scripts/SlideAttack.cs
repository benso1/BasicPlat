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
            Debug.Log("Slide");
            collision.gameObject.transform.parent.gameObject.GetComponent<EnemyStilt>().Damage();
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
    }
}
