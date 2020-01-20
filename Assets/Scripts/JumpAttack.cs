using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    GameObject Player;
    void Start(){
        Player = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "JumpKill"){
            collision.gameObject.transform.parent.gameObject.GetComponent<EnemyBouncer>().Damage();
            Player.GetComponent<Move2d>().EnemyBounce();
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
    }
}
