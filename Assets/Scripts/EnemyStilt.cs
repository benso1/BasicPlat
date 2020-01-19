using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStilt : MonoBehaviour
{
    GameObject Enemy;
    public ParticleSystem ps;
    public Transform transformer;
    void Start(){
        Enemy = this.gameObject;
        ps.Stop();
    }

    public void Damage(){
        //ps.Play();
        var position = transformer.position;
        //position.x -= 5f;
        position.y -= 5f;
        transformer.position = position;
    }
}
