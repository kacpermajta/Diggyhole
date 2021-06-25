using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class agentController : MonoBehaviour
{
    float jumpDur = 0.6f;
    float swayDur = 0.6f;
    float swayWindow = 0.6f;
    float moveSpeed = 0.04f;
    float jumpSpeed = 0.03f;
    float swaySpeed = 0.03f;
    float leapSpeed = 0.05f;
    float gravity = 0.03f;
    public Rigidbody2D m_Rigidbody2D;
    public CapsuleCollider2D charCollider;
    public TextMesh hpIndicator;
    public TextMesh nametag;
    // public CharacterController thisController;
    public Animator thisAnimator;
    public bool isGrounded, forcegrounded;
    public bool currentAgent;
    public float jumping, sway, blasted, blaststart;
    public float vertical, horizontal;
    public bool facingLeft, melee;
    public int attack;
    public bool landing;
    public float leap;
    public float attacking;
    public int strikeNum;
    public AnimatorControllerParameter running, airborne;
    public Transform directionObj;
    public GameObject[] weapons;
    public GameObject curWpn;
    public Transform HandBone;

    public crossfireController crossfireComp;
    public weaponController weaponComp;

    private Vector3 m_Velocity = Vector3.zero;
    public int hp = 100;

    public bool airborneVal;
    public float airborneNum;

    // Start is called before the first frame update
    void Start()
    {
        if (forcegrounded)
        {
            hpIndicator.text = "";
            nametag.text = "";

        }
        GameValues.characters.Add(this);
        landing = false;
        jumping = 0;
        sway = 0;
        vertical = 0;
        horizontal = 0;
        facingLeft = false;
        leap = 0;
        melee = true;
        thisAnimator.SetBool("melee", false);
        attack = -1;
        attacking = 0;
        //running = thisAnimator.GetParameter(1);
        //airborne = thisAnimator.GetParameter(2);
    }

    private void FixedUpdate()
    {
        if (forcegrounded)
            isGrounded = true;
        else
        {

            bool wasGrounded = isGrounded;
            isGrounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, -0.75f), 0.5f);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    isGrounded = true;
                    //               if (!wasGrounded)
                    //                   OnLandEvent.Invoke();
                }
            }
        }

        if (airborneVal)
            airborneNum = ReachTo(airborneNum, 1);
        else
            airborneNum = ReachTo(airborneNum, 0);
        thisAnimator.SetFloat("airbornenum", airborneNum);

        /*
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Vector3 pointedArea = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pointedArea.z = 0;
                colliders = Physics2D.OverlapCircleAll(pointedArea, 1.3f);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject == gameObject)
                    {
                        Blast(pointedArea, 1, 20);
                        //               if (!wasGrounded)
                        //                   OnLandEvent.Invoke();
                    }
                }
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (blasted > 0)
        {
            setAirborne(true);
            if (!thisAnimator.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    facingLeft = true;
                    directionObj.rotation = Quaternion.Euler(0, 180, 0);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    facingLeft = false;
                    directionObj.rotation = Quaternion.Euler(0, 0, 0);

                }
            }



            if (blaststart>0)
            {
                blaststart -= Time.deltaTime;
            }
            else 
            { 
                //blasted -= Time.deltaTime;
                if(isGrounded)
                    blasted -= Time.deltaTime*9;
                vertical -= gravity * Time.deltaTime*9;

                if (Mathf.Abs(m_Rigidbody2D.velocity.magnitude) < 0.1f)
                {
                    if (!landing)
                    {
                        landing = false;
                        Damage((int)blasted*10);
                    }
                    blasted = 0;
                }
            }

        }
        else
        {
            horizontal = 0;
            vertical = -gravity;
            if (!currentAgent)
            {
                if (isGrounded)
                {
                    setAirborne(false);
                }
                else
                {
                    setAirborne(true);
                }
            }
            else
            {
                horizontal = 0;
                vertical = -gravity;
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    curWpn = EquipWeapon(0);
                    //thisAnimator.SetBool("melee", true);
                    //melee = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    curWpn = EquipWeapon(1);
                    //thisAnimator.SetBool("melee", true);
                    //melee = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    curWpn = EquipWeapon(2);
                    //thisAnimator.SetBool("melee", true);
                    //melee = true;
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    curWpn = EquipWeapon(3);
                    //thisAnimator.SetBool("melee", false);
                    //melee = false;
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    curWpn = EquipWeapon(4);
                    //thisAnimator.SetBool("melee", false);
                    //melee = false;
                }
                thisAnimator.SetBool("running", false);

                if (attacking > 0)
                {
                    ResolveAttack();
                }



                else if (!thisAnimator.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
                {
                    if (Input.GetKey(KeyCode.A) && sway == 0)
                    {
                        if (jumping == 0 && leap == 0)
                        {
                            horizontal = -moveSpeed;
                            facingLeft = true;
                            directionObj.rotation = Quaternion.Euler(0, 180, 0);
                            thisAnimator.SetBool("running", true);
                        }

                    }
                    if (Input.GetKey(KeyCode.D) && sway == 0)
                    {
                        if (jumping == 0 && leap == 0)
                        {
                            horizontal = moveSpeed;
                            facingLeft = false;
                            directionObj.rotation = Quaternion.Euler(0, 0, 0);
                            thisAnimator.SetBool("running", true);

                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    thisAnimator.SetBool("attack", true);
                    attacking = weaponComp.attackTime;
                    attack = 0;
                }


                if (isGrounded)
                {
                    //vertical = 0;
                    thisAnimator.SetBool("sway", false);
                    setAirborne(false);
                    jumping = 0;
                    sway = 0;
                    leap = 0;
                    if (Input.GetKey(KeyCode.W))
                    {
                        jumping = 0.1f;
                        setAirborne(true);
                    }
                    if (Input.GetKey(KeyCode.Space))
                    {
                        jumping = 0.1f;
                        leap = 1;
                        setAirborne(true);
                    }
                }
                else
                {
                    if (Mathf.Abs(m_Rigidbody2D.velocity.x) < 0.2f)
                        leap = 0;
                    setAirborne(true);
                    if (jumping > 0 && leap == 0 && jumping < swayWindow)
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            sway = 0.1f;
                            thisAnimator.SetBool("sway", true);

                        }
                    }

                    if (sway > 0)
                    {
                        vertical *= -1;
                        if (facingLeft)
                        {
                            horizontal += swaySpeed;
                        }
                        else
                        {
                            horizontal -= swaySpeed;
                        }

                        sway += Time.deltaTime; if (sway > swayDur)
                        {
                            sway = 0;
                            thisAnimator.SetBool("sway", false);
                        }
                    }
                }
                if (jumping > 0)
                {
                    jumping += Time.deltaTime;
                    vertical = jumpSpeed;
                    if (jumping > jumpDur)
                        jumping = 0;
                }
                if (leap > 0)
                {

                    if (facingLeft)
                    {
                        horizontal -= leapSpeed * leap;
                    }
                    else
                    {
                        horizontal += leapSpeed * leap;
                    }

                }

                if (horizontal != 0 && isGrounded)
                {
                    //if (Mathf.Abs(m_Rigidbody2D.velocity.x) < 4f)
                    vertical += jumpSpeed;
                }



                //thisController.Move(new Vector3(horizontal, vertical, 0));

            }
        }
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(150 * horizontal, 100 * vertical);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, .05f);


    }

    public GameObject EquipWeapon(int i)
    {
        GameObject.Destroy(curWpn);
        thisAnimator.SetBool("melee", true);
        GameObject newWpn = GameObject.Instantiate(weapons[i], HandBone);
        weaponComp = newWpn.GetComponent<weaponController>();
        thisAnimator.SetBool("twoHanded", weaponComp.twoHanded);

        thisAnimator.SetBool("thrust", weaponComp.thrust);
        melee = weaponComp.melee;
        thisAnimator.SetBool("melee", weaponComp.melee);
        if (!weaponComp.melee && weaponComp.twoHanded)
            thisAnimator.SetFloat("rifle", 1f);
        else

            thisAnimator.SetFloat("rifle", 0f);
        return newWpn;

    }
    public void Blast(Vector2 location, float range, float force, int damage)
    {
        Vector2 hitPoint = Physics2D.ClosestPoint(location, charCollider);
        Vector2 direction = hitPoint - location;

        Debug.Log(direction);
        float multipier = (range - direction.magnitude)/ range;
        Damage((int)(damage * multipier));
        blasted = force  * multipier;
        horizontal = force * 0.2f * direction.x  * multipier;
        vertical = force * 0.2f * direction.y  * multipier;
        setAirborne(true);
        blaststart = 0.1f;

    }
    private void ResolveAttack()
    {
        attacking -= Time.deltaTime;

        if (attack<weaponComp.strikeTimes.Length && attacking <= weaponComp.strikeTimes[attack])
        {
            attack ++;
            {
                if (melee)
                {
                    Vector3 strikepoint = transform.position + crossfireComp.lookingPoint(weaponComp.range, facingLeft);
                    Debug.Log(strikepoint);
                    if (weaponComp.pierce == 0f)
                    {
                        GameValues.destructor.Destroy(strikepoint, 25);
                    }
                    else
                    {
                        GameValues.destructor.Destroy(strikepoint, 25);//, weaponComp.pierce, crossfireComp.angle);
                    }
                    foreach (agentController target in GameValues.characters)
                    {
                        //transform.position.z = 0;
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(strikepoint, 1f);
                        for (int i = 0; i < colliders.Length; i++)
                        {
                            if (colliders[i].gameObject == target.gameObject && target.gameObject != gameObject)
                            {
                                target.Blast(transform.position, weaponComp.range + 1f, 1, 20);
                                //               if (!wasGrounded)
                                //                   OnLandEvent.Invoke();
                            }
                        }
                    }
                }
                else
                {
                    GameObject shotFired = GameObject.Instantiate(weaponComp.missile,
                    new Vector3(crossfireComp.spriteTransform.position.x, crossfireComp.spriteTransform.position.y, 0),
                    Quaternion.Euler(0, 0, 180 * (facingLeft ? -crossfireComp.angle : crossfireComp.angle) / Mathf.PI));

                    shotFired.GetComponent<missileController>().faceleft = facingLeft;

                    if (weaponComp.knockback > 0)
                    {
                        Knockback(weaponComp.knockback);
                    }
                }
            }
        }

        if (attack >= weaponComp.strikeTimes.Length)
        {

            attacking = 0;

            thisAnimator.SetBool("attack", false);
        }
    }
    public void Knockback(float force)
    {
        
        Vector2 direction = -crossfireComp.lookingPoint(1, facingLeft);

        
        
        blasted = force ;
        horizontal = force * 0.2f * direction.x ;
        vertical = force * 0.2f * direction.y ;
        setAirborne(true);
        blaststart = 0.1f;

    }


    public void Damage(int amount)
    {
        hp -= amount;
        hpIndicator.text = hp.ToString();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">initial value of variable</param>
    /// <param name="target">target value of variable</param>
    /// <returns></returns>
    public float ReachTo(float value, float target)
    {
        if (Mathf.Abs(target - value) < Time.deltaTime)
            return target;
        else
        {
            if (value > target)
                return value - Time.deltaTime;
            else
                return value + Time.deltaTime;
        }
    }
    public void setAirborne(bool value)
    {
        airborneVal = value;
        thisAnimator.SetBool("airborne", value);
    }
}
