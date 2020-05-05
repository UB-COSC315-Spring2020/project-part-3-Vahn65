using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///// This Code Covers the Frog Enemy's AI /////
/// <summary>
/// Public Class Changed from MonoBehaviour to Inherit from the EnemyAI Script
/// </summary>
public class FrogAI : EnemyAI
{
    [SerializeField]
    private float RightDistanceCap = 2f;
    [SerializeField]
    private float LeftDistanceCap = 0.2f;
    [SerializeField]
    private float JumpLength = 2f;
    [SerializeField]
    private float JumpHeight = 3f;
    [SerializeField]
    private LayerMask Layer;

    private Collider2D Coll;
    private bool facingLeft = true;
    protected override void Start()
    {
        base.Start();
        Coll = GetComponent<Collider2D>();
        
    }
    /// <summary>
    /// This Test to See if The Frog has gone Beyond the Left Distance Cap
    /// Test to See if the Frog is Touching the ground and if it is then it should jump using Touchinglayers
    /// Also makes sure the Frog sprite is facing the right direction using tansfrom.localScale
    /// Animate.GetBool id Statement is to ensure the Transtion from Jumping to Falling and making sure it Idles correctly
    /// Coll.IsTouchingLayers is to ensure the Transtion from Falling to Idle
    /// Else Statement from line 81 to 94 Needs Both Bools or else the Jumping and Falling Animation Will not work going Right
    /// </summary>
    private void Update()
    {

        if (Animate.GetBool("Jumping"))
        {
            if (rbody2D.velocity.y < .1)
            {
                Animate.SetBool("Falling", true);
                Animate.SetBool("Jumping", false);
            }

        }
        if (Coll.IsTouchingLayers(Layer) && Animate.GetBool("Falling"))
        {
            Animate.SetBool("Falling", false);
        }
    }

    private void Movement()
    {
        if (facingLeft)
        {
            if (transform.position.x > LeftDistanceCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if (Coll.IsTouchingLayers(Layer))
                {
                    rbody2D.velocity = new Vector2(-JumpLength, JumpHeight);
                    Animate.SetBool("Jumping", true);
                    Animate.SetBool("Falling", true);
                }
            }
            else
            {
                facingLeft = false;
                Debug.Log("Frog Hopped Facing Left");
            }
        }


        else
        {
            if (transform.position.x < RightDistanceCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                if (Coll.IsTouchingLayers(Layer))
                {
                    rbody2D.velocity = new Vector2(JumpLength, JumpHeight);
                    Animate.SetBool("Falling", true);
                    Animate.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
                Debug.Log("Frog Hopped Facing Right");
            }
        }
    }

}
