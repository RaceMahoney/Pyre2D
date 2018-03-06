using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It is used to manage the enemy golems.
/// He will attack when demon or rider ontrigger enter
/// He will walk between a - b points
/// </summary>
public class Golem_Controller : MonoBehaviour {
    public int life = 10;                               //life value
    private float maxspeed = 1f;                        //walk speed
    Animator anim;
    private bool faceright = true;                      //face side of sprite activated
    public bool dead = false;
    public GameObject LifeBar;
    public GameObject spawner;                          //Magic attack instantiate point
    private float relativeLife = 0;                     //It is used to get relative value between (life <-> lifebar size)
    public GameObject Magic;                            //Magic attack prefab (It will cause damage to demons)
    private int ratio = 3;                              //attack ratio
    //##### About movement
    public GameObject a;                                //Walking target point a
    public GameObject b;                                //Walking target point b
    private int currentPoint = 0;                       //Handling target points .....  0 = a, 1 = b
    private Vector2[] pos = new Vector2[2];
    public GameObject target = null;
    public bool chase = false;
    private bool attacking = false;
    float distance = 0.5f;                              //attack distance 
    GameObject attackPoint;
    //##### About audio
    private AudioSource audio_;                         //Audiosource

    void Start () {
     //   audio_ = GetComponent<AudioSource>();
        currentPoint = Random.Range(0, 2);
        SetZ();
        relativeLife = (float)2 / life;
        anim = GetComponent<Animator>();
        anim.SetBool("walk", false);
        anim.SetBool("attack", false);
        anim.SetBool("dying", false);
        pos[0] = a.transform.position;                  //Golem will walk between pos[0] and pos[1]
        pos[1] = b.transform.position;                  //Golem will walk between pos[0] and pos[1]
    }

    /// <summary>
    /// Get target
    /// </summary>
    /// <param name="other">demon</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Damage")
        {
            Damage(3);
        }
        if (other.tag == "Demon" && target == null && other.GetComponent<Demon_Controller>().target == null)
        {
            target = other.gameObject;
            needFlip(target.transform.position);
            target.SendMessage("SetTarget", this.gameObject);                       //Set the demon target (this gameobject)
            target.SendMessage("needFlip" , this.transform.position);               
            chase = false;
        }
        if (other.tag == "Rider" && target == null && other.GetComponent<Rider_Controller>().target == null)
        {
            target = other.gameObject;
            needFlip(target.transform.position);
            target.SendMessage("SetTarget", this.gameObject);                       //Set the rider target (this gameobject)
            target.SendMessage("needFlip", this.transform.position);
            chase = false;
        }
    }

    /// <summary>
    /// Golem will chase him!!!
    /// </summary>
    /// <param name="other">demon</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == target) {
            if (other.gameObject.GetComponent<Collider2D>().enabled == false)
            {
                target = null;
            }
            else {
                target = other.gameObject;
                Invoke("EnableChase", 0.5f);
            }    
        }
    }
    
    /// <summary>
    /// It is used to disable attack animation
    /// </summary>
    void FixedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            anim.SetBool("attack", false);
        }
    }
    // Update is called once per frame
    void Update()
    {
            AboutAttack();
            if (dead == false && target==null)                                                  //walk between pos [0] and pos [1]
            {
                anim.SetBool("walk", true);
                Vector2 patchPos = new Vector2(this.transform.position.x, this.transform.position.y);
                Vector2 patchCustomPos = new Vector2(pos[currentPoint].x, pos[currentPoint].y);
                needFlip(pos[currentPoint]);
                transform.position = Vector2.MoveTowards(transform.position, pos[currentPoint], Time.deltaTime * maxspeed / 3);
                SetZ();                                                                         //Set z position
                if (patchPos == patchCustomPos)
                {                                                                               //Path Point reached, then go to the next path point
                    if (currentPoint == 0)
                    {
                        currentPoint++;
                    }
                    else
                    {
                        currentPoint--;
                    }
                }
            } 
    }

    /// <summary>
    /// First it will get if the enemy is on auto mode, if auto mode = true ... it will configure the enemy target (the enemy target will be this golem) and then attack!!!
    /// if enemy auto mode = false ..... attack!!! (remember that if enemy auto mode = false ----> the demon is being Controled by player)
    /// </summary>
    void AboutAttack() {
        bool enemyAuto = false;
        if (target != null) {
            if (target.tag == "Rider")
            {
                enemyAuto = target.GetComponent<Rider_Controller>().auto;
                attackPoint = target.GetComponent<Rider_Controller>().attackPoint;
            }
            else
            {
                enemyAuto = target.GetComponent<Demon_Controller>().auto;
                attackPoint = target.GetComponent<Demon_Controller>().attackPoint;
            }
            if (enemyAuto == true)                              //enemy demon not controled by player
            {
                if (dead == false && target != null && attacking == false && Vector3.Distance(attackPoint.transform.position, this.transform.position) < distance / 2)      //enemy reached
                {
                    attacking = true;
                    needFlip(target.transform.position);        //need flip?
                    Attack();
                }
                if (dead == false && target != null && attacking == false && Vector3.Distance(attackPoint.transform.position, this.transform.position) > distance / 2)      //enemy not reached
                {
                    needFlip(target.transform.position);        //need flip?
                    transform.position = Vector2.MoveTowards(transform.position, attackPoint.transform.position, Time.deltaTime * maxspeed / 3);                            //go to enemy attack position
                    SetZ();
                }
            }
            else                                                //enemy demon controled by player
            {
                if (dead == false && target != null && attacking == false && Vector3.Distance(target.transform.position, this.transform.position) < distance)               //enemy reached
                {
                    attacking = true;
                    needFlip(target.transform.position);        //need flip?
                    Attack();
                }
                if (dead == false && target != null && attacking == false && Vector3.Distance(target.transform.position, this.transform.position) > distance)               //enemy not reached
                {
                    needFlip(target.transform.position);        //need flip?
                    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * maxspeed / 3);
                    SetZ();
                }
            }
        }        
    }

    /// <summary>
    /// Attack animation
    /// </summary>
    void Attack()
    {
        if ((target != null && target.GetComponent<Collider2D>().enabled == true)&&((GetComponent<Rigidbody2D>().velocity.x == GetComponent<Rigidbody2D>().velocity.y) && GetComponent<Rigidbody2D>().velocity.x == 0))
        {
            Invoke("DelayMagic",0.2f);
            anim.SetBool("attack", true);
            audio_.clip = Camera.main.GetComponent<Audio_Manager>().Golem_Attack[Random.Range(0, Camera.main.GetComponent<Audio_Manager>().Golem_Attack.Length)];
            audio_.Play();
            anim.SetBool("walk", false);
            Invoke("Re_EnableAttack", ratio);
        }
    }

    /// <summary>
    /// It is used to handle the attack ratio
    /// </summary>
    void Re_EnableAttack() {
        attacking = false;
    }

    /// <summary>
    /// Instantiate magic prefab
    /// </summary>
    void DelayMagic() {
        if (target != null && target.GetComponent<Collider2D>().enabled==true)
        {
            GameObject go = Instantiate(Magic, spawner.transform.position, Quaternion.identity) as GameObject;
            go.name = "Magic";
            go.transform.parent = this.gameObject.transform;
            go.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    /// <summary>
    /// Twinkled when damaged
    /// Reduce life or die, and reduce lifebar size
    /// </summary>
    public void Damage(int value)
    {
        if (dead == false) {
            if (life - value > 0)                                   
            {
                LifeBar.transform.localScale = new Vector3(LifeBar.transform.localScale.x - (relativeLife * value), LifeBar.transform.localScale.y, LifeBar.transform.localScale.z);
                StartCoroutine(Twinkle());
                life = life - value;
            }
            else
            {
                GetComponent<Collider2D>().enabled = false;
                LifeBar.transform.localScale = new Vector3(0, LifeBar.transform.localScale.y, LifeBar.transform.localScale.z);
                life = 0;
                Die();
            }
        }        
    }

    /// <summary>
    /// Twinkle
    /// </summary>
    /// <returns></returns>
    IEnumerator Twinkle()
    {
        for (int i = 0; i < Random.Range(10, 20); i++)
        {
            yield return new WaitForSeconds(0.07f);
            if (dead == true)
            {
                i = 50;
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            }
        }
        if (dead == false)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    /// <summary>
    /// Set dying animation and destroy
    /// </summary>
    void Die()
    {
        dead = true;
        anim.SetBool("dying", true);
        audio_.clip = Camera.main.GetComponent<Audio_Manager>().Golem_Dying[Random.Range(0, Camera.main.GetComponent<Audio_Manager>().Golem_Dying.Length)];
        audio_.Play();
        foreach (Transform child in gameObject.transform) { if (child.name != "Dying" && child.name != "LifeBar" && child.name != "spawner"&& child.name != "a" && child.name != "b") { child.gameObject.GetComponent<SpriteRenderer>().enabled = false; } }        
        Destroy(LifeBar);
        Invoke("DestroyDelay", 2);
    }

    /// <summary>
    /// Flip this gameobject
    /// </summary>
    void Flip()
    {
        faceright = !faceright;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /// <summary>
    /// Destroy this object
    /// </summary>
    void DestroyDelay()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Set correct Z position
    /// </summary>
    void SetZ()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y);
    }

    /// <summary>
    /// Flip character when needed
    /// </summary>
    /// <param name="customPos">object to look at</param>
    public void needFlip(Vector3 customPos)
    {
        if (customPos.x >= this.transform.position.x && faceright == false) { Flip(); }
        if (customPos.x < this.transform.position.x && faceright == true) { Flip(); }
    }
    void EnableChase()
    {
        chase = true;
    }
}
