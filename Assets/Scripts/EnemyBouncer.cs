using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBouncer : MonoBehaviour
{
    GameObject Enemy;
    public Transform transformer;
    public enum Type{
        STAYER, 
    }
    public Type type;
    void Start(){
        Enemy = this.gameObject;
    }

    public void Damage(){
        Destroy(Enemy);
    }
}
