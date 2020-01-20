using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxCollision : MonoBehaviour
{
    GameObject Player;
    void Start(){
        Player = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "DamageBox" && collision.gameObject.GetComponent<EnemyStilt>().Damaging()){
            Debug.Log("Damage");
            Player.GetComponent<Move2d>().Damage();
        }
    }
}
