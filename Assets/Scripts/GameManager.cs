using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour

{
    ///// [The Following Code Controls Player, UI, Enemies, Animation and other Aspects of this Game] /////

    /// <summary>
    /// The Rigidbody2D Sets the Rigidbody as a Class That can be Called on for Several Functions Retaining to the Player.
    /// The Speed Variable Changes how Fast the Player Moves, While the JumpHeight Variable changes how high the Player can Jump.
    /// The Animator Class Calls the Animator Function of Unity named by its Field Name(Animate) to Regulate Animations and How They Will Trigger.
    /// The Enum creates a list of functions to call to for animation,.
    /// The MovementState Enum function "State" can be used to control diffrent "states" for the purposes of animation.
    /// Sets the int Jems is set to view the Number of Collectable Jems for the player. 
    /// The uses TextMeshPro class to hold the text to be used in the UI
    /// </summary>

    ///// [Inspecter Variables] /////
    [SerializeField]
    private LayerMask Layer;
    [SerializeField]
    private float Speed = 10f;
    [SerializeField]
    private float JumpHeight = 10f;
    [SerializeField]
    private int Jems = 0;
    [SerializeField]
    private TMP_Text JemText;
    [SerializeField]
    private float PlayerHurt = 5f;
    [SerializeField]
    private AudioSource Jem;
    [SerializeField]
    private AudioSource Footstep;
    [SerializeField]
    private int Health;
    [SerializeField]
    private TMP_Text HealthAmount;

    ///// [Start() Variables] /////
    private Rigidbody2D rbody2D;
    private Animator Animate;
    private Collider2D Coll;



    ///// [Finite State Machine] /////
    /// <summary>
    /// Finite State Machine creates States.
    /// FSM Holds Lists of Actions in the form Enums to access states to use in the script for animation and other various other functions
    /// </summary> 
    private enum MovementState { Idle, Running, Jumping, Falling, Hurt }
    private MovementState State = MovementState.Idle;


    ////// [Grabs the Various Components from the Inspector.] /////
    private void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        Animate = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();
        HealthAmount.text = Health.ToString();
    }

    ////// [MOVEMENT CODE] ////////
    ///<summary>
    /// Grabs the Inputs Set in the Unity Inspector as Hotizontal & Vertical for Movement making it so there can be mulipte keys use for movement.
    /// Transform.localScale is used to rotate the Player Sprite left and rightt when Certain Keys(A & D, Right Arrow, Left Arrow) are pushed.
    /// If-Else Statments are there to make sure the animation transtions work properly.
    /// Animate.SetIntger Sets Animation based on Enumerator State.
    /// Uses Vector3 Instead of Vector2 for More Controlled Precise Movement.
    /// I used Transfrom.Position and Deltatime for Better Speed Control, and Postioning Allowing for Better acceleration and deceleration.
    /// AddForce is used for Better applying Gravity to the Player .
    /// OnTriggerEnter2D makes it so if the play comes into contact with somthing labeled a trigger an aevent will happen.
    /// The Collectiable Text for Jems is converated to a String from an int so that it shows up in Game.
    /// </summary> 

    void FixedUpdate()
    {
        if (State != MovementState.Hurt)
        {
            Movement();
        }
        AnimationState();
        Animate.SetInteger("State", (int)State);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Jem.Play();
            Destroy(collision.gameObject);
            Jems += 1;
            JemText.text = Jems.ToString();
            Debug.Log("Jem Collected");


        }

    }
    /// <summary>
    /// This Dictates how an enemy & the Player will be damaged
    /// If the Player is above the Enemy in the falling state the Enemy will take damage
    /// If the Enemy is to my right I should take Damage and move Left
    /// If the Enemy is to my left I should take Damage and move Right
    /// When Jump(); is used it allows the Enemy Killed via Jump to add to the players Jump
    /// Using the FrogAI Script to Trigger an animaton and the Enemies Death
    /// The HealthHandler Method Restarts the Active Level if the Health Amount is less than or Equal to 0 Updates UI Accordingly
    /// </summary>

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyAI Enemy = other.gameObject.GetComponent<EnemyAI>();
            if (State == MovementState.Falling)
            {

                Enemy.JumpedOn();
                Jump();
                print("Bounce!");
                Debug.Log("Enemy Is Hit");

            }
            else
            {
                State = MovementState.Hurt;
                HealthHandler();
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rbody2D.velocity = new Vector2(-PlayerHurt, rbody2D.velocity.y);
                    print("To My Right");
                    Debug.Log("Player is Hit");
                }
                else
                {
                    rbody2D.velocity = new Vector2(PlayerHurt, rbody2D.velocity.y);
                    print("To My Left");
                    Debug.Log("Player is Hit");
                }

            }

        }
    }

    private void HealthHandler()
    {
        Health -= 1;
        HealthAmount.text = Health.ToString();
        if (Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (moveHorizontal > 0)

        {
            transform.localScale = new Vector3(1, 1);
            Animate.SetBool("Running", true);
        }

        else if (moveHorizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1);
            Animate.SetBool("Running", true);
        }

        else
        {
            Animate.SetBool("Running", false);
        }

        Vector3 movement = new Vector3(moveHorizontal, moveVertical);
        transform.position += movement * Time.smoothDeltaTime * Speed;
        rbody2D.AddForce(movement * Speed);
        Debug.Log("Player Has Moved");

        ////// [JUMPING CODE] ////////
        ///<summary>
        /// This Code Allows the Player to Jump.
        /// Input calls the Jump function from Unity built in Control system so the Player can Jump.
        /// Tells Unity when the player is touching the collider to insure that only if touching the "ground" layer can a player jump.
        /// Created the Method Jump in order to expand function
        /// </summary>

        if (Input.GetButtonDown("Jump") && Coll.IsTouchingLayers())
        {

            Jump();
            Debug.Log("Player has Jumped");
        }
    }
    private void Jump()
    {
        rbody2D.velocity = new Vector3(rbody2D.velocity.x, JumpHeight);
        State = MovementState.Jumping;
    }

    ////// [ANIMATION CODE] //////
    /// <summary>
    /// This code block Controls how the various states of animaton transtion,
    /// This is based on simple math and velocity, as well as what state the player is currently in called to in the enumerator
    /// These If-Else Statements control the Velocity and State of the player, 
    /// If-Else Bool Coll.IsTouchingLayers makes it so when the player jumps and returns to touch the collider it reverts the player to an Idle State
    /// Creates a New Method in which one can call to the enum "state" to dictate the velocity of the player in a certain animation state
    /// Sets the State function as an int to be used in Animator
    /// FootStepSound Method Creates an Event in Which the FootStep Sound Can be played when set up in the Animator and calls to the Componet AudioSource and the Var Footstep 
    /// </summary>
    private void AnimationState()
    {

        if (State == MovementState.Jumping)
        {
            if (rbody2D.velocity.y < .1f)
            {
                State = MovementState.Falling;
            }
        }
        else if (State == MovementState.Falling)
        {
            if (Coll.IsTouchingLayers(Layer))
            {
                State = MovementState.Idle;
            }
        }
        else if (State == MovementState.Hurt)
        {
            if (Mathf.Abs(rbody2D.velocity.x) < .1f)
            {
                State = MovementState.Idle;
            }
        }
        else if (Mathf.Abs(rbody2D.velocity.x) > Mathf.Epsilon)
        {
            State = MovementState.Running;
        }
        else
        {
            State = MovementState.Idle;
        }
    }

    private void FootstepSound()
    {
        Footstep.Play();
    }
}




