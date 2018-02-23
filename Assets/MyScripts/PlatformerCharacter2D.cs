using System;
using System.Collections;
using UnityEngine;


    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 12f;                  // Amount of force added when the player jumps.
        [SerializeField] private float m_DashSpeed = 10f;
        [SerializeField] private bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private bool m_Dashing;             // Whether the player is dashing or not
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private float vSpeed;               //Vertical speed
        private int MAX_HEALTH = 5;

        [HideInInspector]
        public bool validInput = true;
        [HideInInspector]
        public int health;


        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();


            //Set max health to 
            health = MAX_HEALTH;

     
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
            if(health <= 0)
            {
                m_Anim.Play("FireHeroDie");

                m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                validInput = false;
                Application.LoadLevel(Application.loadedLevel);
            }

        

        }


        public void Move(float move, bool crouch, bool jump, bool dash)
        {
        
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
            {

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

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

            //if player should dash...
            if(dash && !m_Dashing)
            {
                m_Dashing = true;
                m_Anim.SetBool("Dash", true);
                
                if (m_FacingRight)
                {
                    m_Rigidbody2D.velocity = new Vector2(m_DashSpeed, 0f);  
                }
                if (!m_FacingRight)
                {
                    m_Rigidbody2D.velocity = new Vector2(-m_DashSpeed, 0f);
                }
                

            }
            //keep dash off if button is not pressed
            if (!dash)
            {
                m_Dashing = false;
                m_Anim.SetBool("Dash", false);
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
            health -= dmg;
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

        

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "MovingPlatform") 
            {
                //This will make the player a child of the Obstacle
                transform.parent = other.gameObject.transform; //Change "myPlayer" to your player
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Ladder")
            {
                //turn on climbing animation
                m_Anim.SetBool("Climb", true);

            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            m_Anim.SetBool("Climb", false);
            m_Anim.SetBool("Hurt", false);
           
            transform.parent = null;

            //if animator is still off on exit....turn it back on
            if (!m_Anim.enabled)
            {
                m_Anim.enabled = true;
            }
        }

        public IEnumerator Knockback(float knockDur, float knockbackPwr, Vector3 knockbackDir)
        {
            float timer = 0;

            while(knockDur > timer)
            {
                timer += Time.deltaTime;
                m_Rigidbody2D.AddForce(new Vector3(knockbackDir.x * -100, knockbackDir.y * knockbackPwr, transform.position.z));
                
            }

            yield return 0;
        }

    }

