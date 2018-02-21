using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It controls demon movement, attack, animations and bar life.
/// Instantiate demon damage prefab
/// </summary>

public class Demon_Controller : MonoBehaviour {
    public bool auto = false;                                           //auto = false --> it is controled by player.      auto = true --> continue the path
    public int life = 10;                                               //demon life value
    public GameObject circle;                                           //green circle when this demon is controled by player
    private float maxspeed = 0.7f;                                      //walk speed
    Animator anim;
    private bool faceright = true;                                      //face side of sprite activated
    public bool dead = false;
    public GameObject LifeBar;
    private float relativeLife = 0;                                     //It is used to get relative value between (life <-> lifebar size)
    public GameObject DamageGo;                                         //damage prefab
    public GameObject Spawner;                                          //Demon damage prefab instantiatepoint
    //##### About path
    private Vector3[] custom;                                           //Generated with path points (adding noise to the path)
    private float seed = 0.45f;                                         //random seed
    private List<Transform> path = new List<Transform>();	            //Path with points
    public int currentPoint = 0;
    public GameObject target = null;
    private bool attacking = false;
    private int ratio = 4;                                              //attack ratio

    private bool canAttack = false;
    public GameObject attackPoint;                                      //it is used by golem as attack position

    //##### Audio
    private AudioSource audio_;

    void OnMouseDown()
    {
        SetAuto(); 
    }

    void Start () {
        audio_ = GetComponent<AudioSource>();
        maxspeed = Random.Range(maxspeed - 0.3f, maxspeed);
        SetZ();
        relativeLife = (float)2 / life;
        anim = GetComponent<Animator>();
        anim.SetBool("walk", false);
        anim.SetBool("attack",false);
        anim.SetBool("dying",false);
        GetPath();        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Magic")
        {
            Damage(3);      //get damage
        }
    }

    /// <summary>
    /// Get the path points
    /// </summary>
    void GetPath() {
        GameObject go = GameObject.Find("Path");
        foreach (Transform child in go.transform) {
            path.Add(child);
        }
        randomizePath();
    }
    /// <summary>
    /// Create noise into the path
    /// </summary>
	void randomizePath()
    {
        custom = new Vector3[path.Count];
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].gameObject.name != "End")
            {
                custom[i] = new Vector3(path[i].position.x + Random.Range(-seed, seed), path[i].position.y + Random.Range(-seed, seed), path[i].position.y);
            }
            else
            {
                custom[i] = new Vector3(path[i].position.x, path[i].position.y, path[i].position.y);
            }
        }
    }
    /// <summary>
    /// It is used to disable attack animation
    /// </summary>
    void FixedUpdate() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            anim.SetBool("attack", false);
        }
        circle.SetActive(!auto);
    }
	// Update is called once per frame
	void Update () {
        float distance = 0.7f;                                                                  //min distance to attack
        if (auto == true && dead == false)                                                      //not controled by player
        {
            if (target == null)                                                                 //no target and not controled by player then continue the path
            {
                anim.SetBool("walk", true);
                Vector2 patchPos = new Vector2(this.transform.position.x, this.transform.position.y);
                Vector2 patchCustomPos = new Vector2(custom[currentPoint].x, custom[currentPoint].y);
                needFlip(custom[currentPoint]);
                transform.position = Vector2.MoveTowards(transform.position, custom[currentPoint], Time.deltaTime * maxspeed);
                SetZ();
                if (patchPos == patchCustomPos)
                {                                                                               //Path Point reached, then go to the next path point
                    if (currentPoint == path.Count - 1)
                    {                                                                           //This path point is the last point?
                        Destroy(this.gameObject);
                    }
                    currentPoint++;
                }
            }
            else                                                                                //golem target and not controled by player
            {
                anim.SetBool("walk", false);
                if (dead == false && target != null && attacking == false && Vector3.Distance(target.transform.position, this.transform.position) < distance)
                {
                    attacking = true;
                    needFlip(target.transform.position);                                        //need flip?
                    Attack();
                }
            }
        }
        else {                                                                                  //controled by player
            if (dead == false)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * maxspeed, Input.GetAxis("Vertical") * maxspeed);
                if (GetComponent<Rigidbody2D>().velocity.x != 0 || GetComponent<Rigidbody2D>().velocity.y != 0)
                {
                    if (GetComponent<Rigidbody2D>().velocity.x > 0)
                    {//Go right               
                        if (faceright == false)
                        {
                            Flip();
                        }
                    }
                    else
                    {
                        if (faceright == true && GetComponent<Rigidbody2D>().velocity.x < 0)
                        {
                            Flip();
                        }
                    }
                    anim.SetBool("walk", true);
                    SetZ();
                }
                else
                {
                    anim.SetBool("walk", false);
                }
                if (Input.GetMouseButtonUp(0)&&canAttack==true)                                 //attack
                {
                    canAttack = false;
                    Invoke("EnableAttack",0.5f);
                    Attack();
                }
            }
            else
            {
                foreach (Transform child in gameObject.transform) { if (child.name != "Dying" && child.name != "AttackPoint" && child.name != "spawner" && child.name != "LifeBar" && child.name != "LifeBack") { child.gameObject.GetComponent<SpriteRenderer>().enabled = false; } }
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }
        
    }
    /// <summary>
    /// Enable attack process on min distance
    /// </summary>
    public void EnableAttack() {
        canAttack = true;
    }
    /// <summary>
    /// Attack animation
    /// </summary>
    void Attack() {     
        anim.SetBool("attack", true);
        audio_.clip = Camera.main.GetComponent<Audio_Manager>().Demon_attack[Random.Range(0, Camera.main.GetComponent<Audio_Manager>().Demon_attack.Length)];
        audio_.Play();
        Invoke("DelaySwordDamage",0.2f);
        if (auto == true)
        {
            Invoke("Re_EnableAttack", ratio);
        }
        else {
            Re_EnableAttack();
        }
        
    }

    void Re_EnableAttack()
    {
        attacking = false;
    }
    /// <summary>
    /// Instantiate demon damage prefab on spawner position
    /// </summary>
    void DelaySwordDamage() {
        if (target != null && target.GetComponent<Collider2D>().enabled == true)
        {
            GameObject go = Instantiate(DamageGo, Spawner.transform.position, Quaternion.identity) as GameObject;
            go.name = "Damage";
        }
        if (auto == false) {
            GameObject go = Instantiate(DamageGo, Spawner.transform.position, Quaternion.identity) as GameObject;
            go.name = "Damage";
        }
    }
    /// <summary>
    /// Twinkled when damaged and reduce life
    /// </summary>
    public void Damage(int value) {
        if (life - value > 0)
        {
            LifeBar.transform.localScale = new Vector3(LifeBar.transform.localScale.x - (relativeLife * value), LifeBar.transform.localScale.y, LifeBar.transform.localScale.z);
            StartCoroutine(Twinkle());
            life = life - value;
        }
        else {
            GetComponent<Collider2D>().enabled = false;
            LifeBar.transform.localScale = new Vector3(0, LifeBar.transform.localScale.y, LifeBar.transform.localScale.z);
            life = 0;
            Die();
        }
    }
    /// <summary>
    /// Twinkle
    /// </summary>
    /// <returns></returns>
    IEnumerator Twinkle() {
        for (int i = 0; i < Random.Range(10, 20); i++)
        {
            yield return new WaitForSeconds(0.07f);
            if (dead == true)
            {
                i = 50;
            }
            else {
                foreach (Transform child in gameObject.transform) { if (child.name != "Dying" && child.name != "AttackPoint" && child.name != "spawner" && child.name != "LifeBar" && child.name != "LifeBack") { child.gameObject.GetComponent<SpriteRenderer>().enabled = !child.gameObject.GetComponent<SpriteRenderer>().enabled; } }
            }           
        }
        if (dead == false)
        {
            foreach (Transform child in gameObject.transform) { if (child.name != "Dying" && child.name != "AttackPoint" && child.name != "spawner" && child.name != "LifeBar" && child.name != "LifeBack") { child.gameObject.GetComponent<SpriteRenderer>().enabled = true; } }
        }
    }
    /// <summary>
    /// Set dying animation and destroy
    /// </summary>
    void Die() {
        GetComponent<Collider2D>().enabled = false;
        dead = true;
        anim.SetBool("dying", true);
        audio_.clip = Camera.main.GetComponent<Audio_Manager>().Demon_Dying[Random.Range(0, Camera.main.GetComponent<Audio_Manager>().Demon_Dying.Length)];
        audio_.Play();
        foreach (Transform child in gameObject.transform) { if (child.name != "Dying" && child.name != "AttackPoint" && child.name != "spawner" && child.name != "LifeBar") { child.gameObject.GetComponent<SpriteRenderer>().enabled=false; } }
        Destroy(LifeBar);
        Invoke("DestroyDelay",2);
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
    void SetZ() {
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
    /// <summary>
    /// Set auto mode
    /// </summary>
    public void SetAuto() {
        if (!auto == false && Camera.main.GetComponent<CharacterSelected>().selected == null )
        {
            auto = !auto;
            Camera.main.SendMessage("Set", this.gameObject);
            target = null;
            Invoke("EnableAttack", 0.2f);
        }
        else
        {
            if (!auto == true && Camera.main.GetComponent<CharacterSelected>().selected == this.gameObject) {
                auto = !auto;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                canAttack = false;
                Camera.main.GetComponent<CharacterSelected>().selected = null;
            }
        }
    }
    /// <summary>
    /// It is used by golem, set the target of this demon
    /// </summary>
    /// <param name="go">golem</param>
    public void SetTarget(GameObject go) {
        if (auto == true) {
            target = go;
        }
    }
}
