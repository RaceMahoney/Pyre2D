/** 

* This script controls player movements and other 
* properties associated with the player object.
* Actions such as dashing, movement, colliders
* and score handled here. 

* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System;
using System.Collections;
using UnityEngine;


    public class PlatformerCharacter2D : MonoBehaviour
    {
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 12f;                  // Amount of force added when the player jumps.
    [SerializeField] private float m_DashSpeed = 10f;                   // Speed of the dash action
    [SerializeField] private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private bool m_Dashing;             // Whether the player is dashing or not
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    public Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private float vSpeed;               //Vertical speed
    private int MAX_HEALTH = 5;         // Max health that can be set
    public bool replayMode = false;
  

    [HideInInspector]
    public bool validInput = true;      //Determines if input from the keyboard should be used
    [HideInInspector]
    public int health;                  //Health of the player
    [HideInInspector]
    public int score = 0;               //Current score of the player

    public Vector3 respawnPoint;        //Where the current respawn of the player is
    public GameObject[] enemies;        //Refrence of all enemy objects

    [HideInInspector]
    public DashState dashState;         //Status of Dash

    private Platformer2DUserControl controller;

    public Vector2 savedVelocity;       //The saved velocity at the time
    public float dashTimer;             
    public float maxDash = 8f;

    public int dashStatus = 0;          //start at the full dash status
    //1 = empty
    //2 half

    [HideInInspector]
    public Vector3 startTransform;      //Where the player starts THIS life 
    private float oldTransform;

    public GameObject DeathScreen;      //Reference to the dealth screen UI
    public bool dealthScreenBlocker = false;    //bool to stop bug from occuring in relay ode

    private void Start()
    {
        startTransform = transform.position;
    }

    private void Awake()
        {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
        enemies = GameObject.FindGameObjectsWithTag("Demon");
        controller = GetComponent<Platformer2DUserControl>();

        //Set max health to 
        health = MAX_HEALTH;

        oldTransform = transform.position.x;

        if(DeathScreen.activeInHierarchy)
        {
            DeathScreen.SetActive(false);
        }
        
     
        }

    private void Update()
    {
        // Manually set the speed animation float if in replay mode
        if (replayMode)
        {
            float value;
            
            //check if moving 
            if (oldTransform > transform.position.x)
            {
                //moving right
                value = Mathf.Abs(oldTransform - transform.position.x + 0.9f);
                m_Anim.SetFloat("Speed", value);
            }
            if (oldTransform > transform.position.x)
            {
                //moving left
                value = Mathf.Abs(oldTransform - transform.position.x + 0.9f);
                m_Anim.SetFloat("Speed", value);
            }
            if (oldTransform == transform.position.x)
            {
                //standing still
                value = Mathf.Abs(oldTransform - transform.position.x);
                m_Anim.SetFloat("Speed", value);
            }

            //update what the pervious transform was for the next frame
            oldTransform = transform.position.x;
        }

        if (!replayMode)
        {
            if (Input.GetKeyDown(KeyCode.F8))
            {
                RestartPos();
            }
        }

        if(transform.position == startTransform)
        {
            DeathScreen.SetActive(false);
        }
    }

    private void FixedUpdate()
        {
       
        m_Grounded = false;
        m_Dashing = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

        //check if player has died
            if (health <= 0)
            {
                foreach (GameObject demon in enemies)
                {
                    demon.SetActive(true);

                }
        
                    StartCoroutine(DeathTime());
                    //stop all movement 
                    m_Rigidbody2D.velocity = Vector3.zero;
                    m_Rigidbody2D.angularVelocity = 0f;
                    
                    health = 5;
                    score = 0;
           
            }
        }


    public void Move(float move, bool jump)
        {
        
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
            {

            if (!replayMode)
            {
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                //Only let this happen if NOT in relay mode
                m_Anim.SetFloat("Speed", Mathf.Abs(move));
            }

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                //move the transform instead of the rigidbody


                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.velocity = new Vector2(0f, m_JumpForce);
                
            
                
            }

        }

    public void Dash(bool isDashing)
    {
        
        switch (dashState)
        {
            //state is ready and waiting for input
            case DashState.Ready:
                dashStatus = 0;
                if (isDashing)
                {
                    //check if there is no movement at all, if not set velocity to 10 so can move from a standing positon
                    if (m_Rigidbody2D.velocity.x == 0 && m_FacingRight)
                    {
                        m_Rigidbody2D.velocity = new Vector2(50f, 0); //set the velocity to one in case 
                    }
                    if (m_Rigidbody2D.velocity.x == 0 && !m_FacingRight)
                    {
                        m_Rigidbody2D.velocity = new Vector2(-50f, 0);
                    }
                    //once dash key is pressed, 
                    //save the velocity for later, incrase velocity by 3 and enter dashing state
                    savedVelocity = m_Rigidbody2D.velocity;
                    
                    //m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x * 18f, m_Rigidbody2D.velocity.y);
                    StartCoroutine(DashTime());
                    
                    dashState = DashState.Dashing;
                }
                break;
            //state is dashing and timer is waiting to return player back to saved velocity
            case DashState.Dashing:
                dashStatus = 2;
                
                dashTimer += Time.deltaTime * 3;
                if (dashTimer >= maxDash)
                {
                   m_Rigidbody2D.velocity = savedVelocity;
                   dashState = DashState.Cooldown;
                }
                break;
            //state is in cooldown and will be send back to ready when timer is done
            case DashState.Cooldown:
                dashStatus = 1;
                
                dashTimer -= Time.deltaTime * 3;
                //UnityEngine.Debug.Log(dashTimer);
                if (dashTimer <= 0)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }
                break;
        }
    }

    public void Attack()
        {
            m_Anim.Play("FireHeroAttack");
            
        }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Damage(int dmg)
    {
        //player has been hit by an enemy
        //if (replayMode)
        //{
            //if in replay mode is on, then set the bugs right
            health -= dmg;
        //} 
        gameObject.GetComponent<Animation>().Play("FireHeroHurt");
    }

    public void SetVelocityY(float Y)
    {
        m_Rigidbody2D.velocity = new Vector2(0f, Y);
    }

    public float GetVelocityY()
    {
        float velY = m_Rigidbody2D.velocity.y;
        return velY;
    }

    public void Correction(Vector2 target, float move)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, move * Time.deltaTime);

    }

    public void AddScore(int numberOfCoins)
    {
        score += numberOfCoins;
    }

    public void SetReplayMode()
    {
        //set booleans and reset player stats
        replayMode = true;
        validInput = false;
        health = 5;
        dealthScreenBlocker = true;

        //move the player out of the way so that
        //no accidental trigger happens
    }

    public void RestartPos()
    {
        transform.position = startTransform;
        DeathScreen.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MovingPlatform") 
        {
            //This will make the player a child of the Obstacle
            transform.parent = other.gameObject.transform; //Change "myPlayer" to your player
        }

        if(other.gameObject.tag == "Deathfloor")
        {
            
            health = 0;

        }

        if(other.gameObject.tag == "Campfire")
        {
            //if the player went past the checkpoint, this is will be the last posiiton
            respawnPoint = other.transform.position;
            respawnPoint = new Vector3(transform.position.x, transform.position.y + 1, 0f);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Ladder")
        {
            //turn on climbing animation
            m_Anim.SetBool("Climb", true);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        //ladder code
        m_Anim.SetBool("Climb", false);
        m_Anim.SetBool("Hurt", false);
           
        transform.parent = null;

        //if animator is still off on exit....turn it back on
        if (!m_Anim.enabled)
        {
            m_Anim.enabled = true;
        }

     
    }

    //states of dashing
    public enum DashState
    {
        Ready,
        Dashing,
        Cooldown
    }

    public IEnumerator DashTime()
    {
        float timePassed = 0;
        //loop based off the amount of time passed by
        while(timePassed < 0.2f)
        {
            m_Anim.SetBool("Dash", true);
            timePassed += Time.deltaTime;
            Debug.Log(timePassed);
            m_Rigidbody2D.AddRelativeForce(new Vector2(m_Rigidbody2D.velocity.x *100f, 0f));
            yield return 0; //go to the next frame
        }
        m_Anim.SetBool("Dash", false);
        yield return null;
    }

    public IEnumerator DeathTime()
    {
        //display the death screen for a total of 4 seconds while
        //moving the player back to checkpoint
        if (!dealthScreenBlocker)
        {
            DeathScreen.SetActive(true);
            validInput = false;
            yield return new WaitForSeconds(4);
            DeathScreen.SetActive(false);
            transform.position = respawnPoint;
            validInput = true;
        }
       
    }

    }





