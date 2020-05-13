using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///// Handles All Enemy and Enemy AI Related behaviors ////
///<summary>
///Public changed to protected virtual to ensure Inheritance from/to other Scripts
///Protected makes it so any child of this script can use the code from this scirpt 
///Virtual makes it so overrides can take place
///</summary>
public class EnemyAI : MonoBehaviour

{
    protected Animator Animate;
    protected Rigidbody2D rbody2D;
    protected AudioSource death;

    protected virtual void Start()
    {
        Animate = GetComponent<Animator>();
        rbody2D = GetComponent<Rigidbody2D>();
        death = GetComponent<AudioSource>();
    }
    public void JumpedOn()
    {
        Animate.SetTrigger("Death");
        death.Play();
    }
    private void Death()
    {
        Destroy(this.gameObject);
    }
}


