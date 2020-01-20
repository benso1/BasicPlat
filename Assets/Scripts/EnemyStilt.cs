using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStilt : MonoBehaviour
{
    GameObject Enemy;
    public Transform transformer;
    private bool lethal;
    public enum Type{
        STAYER
    }
    public Type type;
    void Start(){
        Enemy = this.gameObject;
        lethal = true;
    }

    public void Damage(){
        lethal = false;
        Destroy(Enemy);
    }
    public bool Damaging(){
        return lethal;
    }
}
