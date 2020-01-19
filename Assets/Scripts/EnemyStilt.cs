using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStilt : MonoBehaviour
{
    GameObject Enemy;
    public Transform transformer;
    void Start(){
        Enemy = this.gameObject;
    }

    public void Damage(){
        //var position = transformer.position;
        //position.x -= 5f;
        //position.y -= 5f;
        //transformer.position = position;
        Enemy.GetComponent<BoxCollider2D>().enabled = false;
        Enemy.tag = "Disabled";
    }
}
