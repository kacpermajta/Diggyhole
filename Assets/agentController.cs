using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class agentController : MonoBehaviour
{
    public float agility;

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
    public TMP_Text hpIndicator;
    public TMP_Text nametag;
    // public CharacterController thisController;
    public Animator thisAnimator;
    public bool  forcegrounded;
    public bool currentAgent;
    public float jumping, sway, blasted, blaststart;
    public float kineForceX, kineForceY, kineForceHurt;
    public float jumpcharge;
    public float vertical, horizontal;
    public bool facingLeft, melee;
    public int attack, attackmove;
    public bool landing;
    public float leap;
    public float attacking;
    public int strikeNum;
    public AnimatorControllerParameter running, airborne;
    public Transform directionObj;
    [Tooltip("Standarised: 0-charge , 1-single shot, 2-melee , 3-jump, 4 - auto , 6- boost")]
    public GameObject[] weapons;
    public GameObject curWpn;
    public Transform HandBone;
    public groundingSystem grounding;

    public crossfireController crossfireComp;
    public weaponController weaponComp;

    private Vector3 m_Velocity = Vector3.zero;
    public int hp = 100;
    public bool alive;

    public bool airborneVal;
    public float airborneNum;
    public int actionPts;
    public float outro;
    public troop troopLogic;
    public team teamLogic;

    //display values
    public Color auraColor;
    public ParticleSystem auragone;

    public ParticleSystem impactPart;

    [Tooltip("Force agent has when dives bomb-like")]
    public float impact;
    private IEnumerator impactCoroutine;

    #region media

    public AudioClip[] jumpClips;
    public AudioClip[] landClips;

    public AudioClip[] impactClip;
    public AudioClip[] hitClips;
    public AudioClip[] runClips;
    public AudioClip[] gruntClips;

    public AudioClip[] pickedClips;
    public AudioClip[] doneClips;

    public AudioSource voice;
    public AudioSource sounds;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        kineForceHurt = 0;
        airborneNum = 2;
        airborneVal = true;
        if (grounding.forceGrounded)
        {
            hpIndicator.text = "";
            nametag.text = "";
            crossfireComp.spriteTransform.gameObject.SetActive(false);
            playRandomEffect(ref pickedClips, voice);
        }
        GameValues.characters.Add(this);
        landing = false;
        jumping = 0;
        sway = 0;
        vertical = 0;
        horizontal = 0;
        SetDirLeft(false);
        leap = 0;
        melee = true;
        thisAnimator.SetBool("noTrace", true);
        thisAnimator.SetBool("dead", false);
        attack = 0;
        attackmove = 0;
        attacking = 0;
        //running = thisAnimator.GetParameter(1);
        //airborne = thisAnimator.GetParameter(2);
        if(!grounding.forceGrounded)
            curWpn = EquipWeapon(1);
        jumpcharge = 0;
        kineForceX = 0;
        kineForceY = 0;
    }

    private void FixedUpdate()
    {


        if(!grounding.forceGrounded)
        {
            if (hp > 0 && transform.position.y < 0)
                Damage(1);


        }
        if (Input.GetKey(KeyCode.W) && grounding.high )
            airborneNum = 4;
        else if (airborneNum == 4)
            airborneNum = 2;
        else if (airborneVal)
        {
                ReachTo(ref airborneNum, 2, 2);
        }
            
        //else if (airborneNum > 1f)
        //    airborneNum = 1f;
        else
            airborneNum = 0;
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
        vertical = kineForceY;
        horizontal = kineForceX;
        if (vertical > 0.3f)
            vertical = 0.3f;
        if (vertical < -0.3f)
            vertical = -0.3f;
        if (horizontal > 0.3f)
            horizontal = 0.3f;
        if (horizontal < -0.3f)
            horizontal = -0.3f;
        if (currentAgent)
        {



            if (outro > 0)
            {
                outro -= Time.deltaTime;
                if (outro <= 0)
                {
                    SetInactive();
                    return;
                }

            }
            else if (actionPts <= 0)
            {
                outro = 4;


                playRandomEffect(ref doneClips, voice);
            }

            if (attacking <= 0 && actionPts > 0)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    curWpn = EquipWeapon(0);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    curWpn = EquipWeapon(1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    curWpn = EquipWeapon(2);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    curWpn = EquipWeapon(3);
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    curWpn = EquipWeapon(4);
                }
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    curWpn = EquipWeapon(5);
                }
            }
            thisAnimator.SetBool("running", false);

            if (Input.GetKey(KeyCode.A) && sway == 0)
            {
                if (jumping == 0 && leap == 0 &&( kineForceX == 0 || kineForceY<0.1 ) && attacking <= 0)
                {
                    horizontal = -moveSpeed;
                    SetDirLeft(true);
                    thisAnimator.SetBool("running", true);
                    if (grounding.grounded && !sounds.isPlaying)
                    {
                        playRandomEffect(ref runClips, sounds);
                    }
                }

            }
            if (Input.GetKey(KeyCode.D) && sway == 0)
            {
                if (jumping == 0 && leap == 0 && (kineForceX == 0 || kineForceY < 0.1) && attacking <= 0)
                {
                    horizontal = moveSpeed;
                    SetDirLeft(false);
                    thisAnimator.SetBool("running", true);
                    if (grounding.grounded && !sounds.isPlaying)
                    {
                        playRandomEffect(ref runClips, sounds);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && actionPts > 0 && airborneNum < 3 && weaponComp.cost > 0)
            {
                thisAnimator.SetBool("attack", true);
                attacking = weaponComp.attackTime;
                attack = 0;
                attackmove = 0;
                weaponComp.traceEnabler = 0;
                weaponComp.traceDisabler = 0;
                weaponComp.soundPlayer = 0;

            }

            if (jumping > 0)
            {
                jumping += Time.deltaTime;
                kineForceY = jumpSpeed;
                //Debug.Log("jump: " + kineForceY);
                if (jumping > jumpDur)
                    jumping = 0;
                if (leap != 0)
                {
                    kineForceX = leapSpeed * leap;

                }

            }
        }
        else
        {
            jumping = 0;
            sway = 0;
            if (kineForceX > 0)
            {
                SetDirLeft(true);
            }
            else if(kineForceX<0)
            {
                SetDirLeft(false);
            }
        }





        if (!grounding.grounded || kineForceX != 0 ||kineForceY !=0||!alive)
        {


            if (airborneNum < 3 || kineForceY > -0.05f || !alive)
                kineForceY -= gravity * Time.deltaTime * 3;
            else
                kineForceY = -0.05f;
            //Debug.Log("falling: " + kineForceY);
            setAirborne(true);
            if (attacking > 0)
            {
                ResolveAttack();
            }




            if (blaststart>0)
            {
                blaststart -= Time.deltaTime;

            }
            else if (jumping == 0 && alive)
            {
                if (!grounding.forceGrounded && grounding.touching && Mathf.Abs(m_Rigidbody2D.velocity.magnitude) < 0.5 && kineForceHurt > 0)
                {
                    //Debug.Log("przestój");
                    //landing = false;

                    int strain = (int)((kineForceHurt + Mathf.Abs(kineForceX) + Mathf.Abs(kineForceY)) * 30);
                    //Mathf.Abs((int)kineForceX * 10) + Mathf.Abs((int)kineForceY * 10);
                    if (grounding.grounded || grounding.grabby)
                        strain /= 2;
                    if (strain > 1)
                    {
                        Debug.Log("strain: " + strain);
                        Damage(strain);

                        kineForceX = 0;
                        kineForceY = 0;
                        kineForceHurt = 0;
                    }
                }

                //blasted -= Time.deltaTime;

                //blasted -= Time.deltaTime*9;
                //vertical -= gravity * Time.deltaTime*9;
                if (Mathf.Abs(m_Rigidbody2D.velocity.x) < 0.2f)
                    leap = 0;


                if (grounding.touching)
                {

                    kineForceHurt += ReachTo(ref kineForceX, 0, 4);

                    if(grounding.grounded||kineForceY>0)
                        kineForceHurt += ReachTo(ref kineForceY, 0, 4);
                    //Debug.Log("landing: " + kineForceY);
                    Debug.Log("charge: " + kineForceHurt+", speed: "+ m_Rigidbody2D.velocity.magnitude);
                    sway = 0;

                }
                else
                    kineForceHurt = 0;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (jumping > 0 && leap == 0 && jumping < swayWindow)
                {

                    
                        playRandomEffect(ref gruntClips, sounds);
                    
                    sway = 0.1f;
                    thisAnimator.SetBool("sway", true);
                    airborneNum = 1;


                }

            }


            if (kineForceY<-0.11f && Input.GetKey(KeyCode.S))
            {
                

                thisAnimator.SetBool("down", true);
                vertical *= 1.3f;
                impact += Mathf.Abs( kineForceY/2);
            }
            else 
            {

                thisAnimator.SetBool("down", false);
                if (!grounding.grounded)
                    impact = 0;

                
            }

            if (sway > 0)
            {
                vertical += swaySpeed;
                if (facingLeft)
                {
                    horizontal += swaySpeed;
                }
                else
                {
                    horizontal -= swaySpeed;
                }

                sway += Time.deltaTime; 
                if (sway > swayDur * agility)
                {
                    sway = 0;
                    thisAnimator.SetBool("sway", false);
                }
            }




        }
        else
        {
            //horizontal = 0;
            //vertical = -gravity;
            if (!currentAgent)
            {
                
                setAirborne(false);
                vertical -= 0.5f;
                
            }
            else
            {


                //horizontal = 0;
                //vertical = -gravity;

                

                if (attacking > 0)
                {
                    ResolveAttack();
                }
                else //if (!thisAnimator.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
                {

                    if (impact > 0)
                    {

                        impactCoroutine = GenerateBlastAfter(0.5f,transform.position,impact, impact, impact*2, impact/4);
                        StartCoroutine(impactCoroutine);
                        impact = 0;
                    }
                    //vertical = 0;

                    thisAnimator.SetBool("sway", false);
                    sway = 0;
                    setAirborne(false);
                    jumping = 0;
                    leap = 0;

                    kineForceY = 0;




                    if (airborneNum < 0.5)
                    {
                        if (Input.GetKey(KeyCode.W))
                        {
                            if(airborneNum==0)
                            {
                                playRandomEffect(ref gruntClips, voice);
                            }
                            jumping = 0.1f;
                            setAirborne(true);
                        }
                        if (Input.GetKey(KeyCode.Space))
                        {
                            if (airborneNum == 0)
                            {
                                playRandomEffect(ref gruntClips, voice);
                            }
                            jumping = 0.1f;

                            if (facingLeft)
                            {

                                leap = -1;
                            }
                            else
                            {

                                leap = 1;
                            }
                            setAirborne(true);
                        }
                    }


                }




                //podbicie w kroku
                if (horizontal != 0)
                {
                    if (!grounding.footing)
                        vertical -=2* gravity;
                    //else 
                    //    vertical += gravity;
                }




                //thisController.Move(new Vector3(horizontal, vertical, 0));

            }
        }
        if (grounding.grabby)
        {
            if (facingLeft)
            {
                horizontal = 0.01f;
            }
            else
            {
                horizontal = -0.01f;
            }
        }
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(150 * horizontal, 100 * vertical);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, .05f);


    }

    public GameObject EquipWeapon(int i)
    {

        return EquipWeapon(weapons[i]);
    }

    public GameObject EquipWeapon(GameObject prefab)
    {
        GameObject.Destroy(curWpn);
        GameObject newWpn = GameObject.Instantiate(prefab, HandBone);
        weaponComp = newWpn.GetComponent<weaponController>();
        thisAnimator.SetBool("twoHanded", weaponComp.twoHanded);

        thisAnimator.SetBool("thrust", weaponComp.thrust);
        melee = weaponComp.melee || weaponComp.deployable;
        thisAnimator.SetBool("noTrace", weaponComp.melee || weaponComp.thrown || weaponComp.deployable);
        thisAnimator.SetBool("deploy", weaponComp.deployable);
        thisAnimator.SetBool("thrown", weaponComp.thrown);
        /*if (!weaponComp.melee && weaponComp.twoHanded)
            thisAnimator.SetFloat("rifle", 1f);
        else

            thisAnimator.SetFloat("rifle", 0f);
        */
        thisAnimator.SetFloat("variant", weaponComp.variant);
        GameValues.setGui(weaponComp.icon, weaponComp.cost, actionPts, weaponComp.name);
        return newWpn;
    }

    private IEnumerator BlastAfter(float waitTime, Vector2 location, float range, float force, int damage)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Blast(location, range, force, damage);
        }
    }

    public void Blast(Vector2 location, Vector2 agentlocation, float range, float force, int damage)
    {
        Vector2 hitPoint = Physics2D.ClosestPoint(location, charCollider);
        Vector2 direction =  hitPoint - agentlocation;
        direction.Normalize();


        Debug.Log(direction);
        float multipier = (range - (hitPoint - location).magnitude)/ range;
        Damage((int)(damage * multipier));
        blasted = force  * multipier;

        kineForceX += force * 0.2f * direction.x * multipier;
        kineForceY += force * 0.2f * direction.y * multipier;
        Debug.Log("blasted: " + kineForceY);
        //horizontal = force * 0.2f * direction.x  * multipier;
        //vertical = force * 0.2f * direction.y  * multipier;


        setAirborne(true);
        blaststart = 0.1f;
        

    }
    public void Blast(Vector2 location, float range, float force, int damage)
    {
        Blast(location, location, range, force, damage);
    }

    public bool CheckTimeline(int counter, float[] timeline)
    {
        return counter < timeline.Length && weaponComp.attackTime - attacking >= timeline[counter];
    }    
    private void ResolveAttack()
    {
        //Debug.Log(weaponComp.attackTime - attacking);
        attacking -= Time.deltaTime;
        if (CheckTimeline(attackmove, weaponComp.movementTimes)) 
        {
            attackmove++;
            if (weaponComp.backstep != 0)
            {
                //Debug.Log("backstep!");
                Knockback(weaponComp.backstep);
            }

        }
        if (CheckTimeline(weaponComp.soundPlayer, weaponComp.soundTimes))
        {
            weaponComp.soundPlayer++;
            weaponComp.playRandomEffect();
        }
        if (CheckTimeline(weaponComp.traceEnabler, weaponComp.traceStart)) 
        {
            weaponComp.traceEnabler++;
            weaponComp.traceImg.enabled = true;  
        }

        if (CheckTimeline(weaponComp.traceDisabler, weaponComp.traceEnd)) 
        {
            weaponComp.traceDisabler++;
            weaponComp.traceImg.enabled = false;
        }

        if (CheckTimeline(attack, weaponComp.strikeTimes)) 
        {
            attack ++;
            {
                if (weaponComp.melee)
                {
                    Vector3 strikepoint = crossfireComp.spriteTransform.position;
                        //transform.position + crossfireComp.lookingPoint(weaponComp.range, facingLeft);
                    //Debug.Log(strikepoint);
                    GameValues.destructor.Destroy(strikepoint, (int)(25 * weaponComp.blast));
                    GenerateBlastAt(strikepoint, weaponComp.blast, weaponComp.range, weaponComp.dmg, weaponComp.force);


                    playRandomEffect(ref gruntClips, voice);
                    


                }
                if (weaponComp.missile != null)
                {
                    if (weaponComp.deployable)
                    {
                        //Debug.Log("ziuum");
                        GameObject deployedObject = GameObject.Instantiate(weaponComp.missile, (weaponComp.missile.GetComponent<deployabe>().ObjEntryType == deployabe.entryType.agent) ? transform.position :
                        new Vector3(crossfireComp.spriteTransform.position.x, crossfireComp.spriteTransform.position.y, 0),
                        (weaponComp.missile.GetComponent<deployabe>().ObjEntryType == deployabe.entryType.crosshairAngled) ?
                        Quaternion.Euler(0, 0, 180 * (facingLeft ? -crossfireComp.angle : crossfireComp.angle) / Mathf.PI) : Quaternion.identity);

                        deployedObject.GetComponent<deployabe>().handler.SetValues(weaponComp.blast, weaponComp.force, weaponComp.dmg);

                    }
                    else
                    {

                        GameObject shotFired = GameObject.Instantiate(weaponComp.missile,
                        new Vector3(crossfireComp.spriteTransform.position.x, crossfireComp.spriteTransform.position.y, 0),
                        Quaternion.Euler(0, 0, 180 * (facingLeft ? -crossfireComp.angle : crossfireComp.angle) / Mathf.PI));

                        shotFired.GetComponent<missileController>().faceleft = facingLeft;
                        shotFired.GetComponent<missileController>().SetValues(weaponComp.blast, weaponComp.force, weaponComp.dmg);
                        if (weaponComp.thrown)
                        {
                            playRandomEffect(ref gruntClips, voice);
                            
                            weaponComp.sticky = shotFired;
                            shotFired.GetComponent<missileController>().returnTarget = weaponComp.parent ;
                        }
                    }
                }
                if (weaponComp.knockback != 0)
                {
                    Knockback(weaponComp.knockback);
                }
            }
        }

        if (attacking<=0)
        {

            attacking = 0;
            actionPts -= weaponComp.cost;
            if(actionPts<0)
            {
                Hurt(-actionPts);
                actionPts = 0;
            }
            if (actionPts<=0)
            {
                curWpn = EquipWeapon(GameValues.gameMasterController.blankWpnPrefab);
            }
            else
                GameValues.setGui(weaponComp.icon, weaponComp.cost, actionPts, weaponComp.name);

            thisAnimator.SetBool("attack", false);
        }
    }
    
    public void GenerateBlastAt(Vector3 strikepoint, float blast, float range, float dmg, float force)
    {
        

        foreach (agentController target in GameValues.characters)
        {
            //transform.position.z = 0;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(strikepoint, blast);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject == target.gameObject && target.gameObject != gameObject)
                {
                    target.Blast(strikepoint, transform.position, range + blast, force, (int)dmg);//+1, 1, 20);

                    //               if (!wasGrounded)
                    //                   OnLandEvent.Invoke();
                }
            }
        }
    }

    private IEnumerator GenerateBlastAfter(float waitTime, Vector3 strikepoint, float blast, float range, float dmg, float force)
    {
        bool blasted = false;
        while (!blasted)
        {
            yield return new WaitForSeconds(waitTime);

            playRandomEffect(ref impactClip, sounds);
            
            GenerateBlastAt(strikepoint, range, force, dmg, force);
            blasted = true;
            impactPart.Emit(15);
        }
    }

    public void Knockback(float force)
    {
        
        Vector2 direction = -crossfireComp.lookingPoint(1, facingLeft);

        
        
        blasted = Mathf.Abs( force) ;


        //horizontal = force * 0.2f * direction.x;
        //vertical = force * 0.2f * direction.y;

        kineForceX = force * 0.2f * direction.x;
        kineForceY = force * 0.2f * direction.y;
        setAirborne(true);
        blaststart = 0.1f;

    }

    public bool Hurt(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            hpIndicator.text = "0";
            return true;
        }
        else
        {
            hpIndicator.text = hp.ToString();
            return false;
        }
    }

    public void Damage(int amount)
    {
        Debug.Log("benc: " + amount);

        if (amount > 0)
        {
            playRandomEffect(ref hitClips, voice);

            auragone.Emit(amount / 2);

            if (Hurt(amount))
            {
                auragone.Emit(30);
                hp = 0;
                alive = false;
                thisAnimator.SetBool("dead", true);
                gameObject.GetComponent<Collider2D>().enabled = false;
                if (teamLogic.character.IndexOf(troopLogic) <= teamLogic.charnum)
                {
                    teamLogic.charnum--;
                    if (teamLogic.charnum < 0)
                        teamLogic.charnum = 0;
                }
                teamLogic.character.Remove(troopLogic);

                if (teamLogic.character.Count == 0)
                {
                    if (GameValues.gameMasterController.teams.IndexOf(teamLogic) <= GameValues.gameMasterController.teamnum)
                    {
                        GameValues.gameMasterController.teamnum--;
                        if (GameValues.gameMasterController.teamnum < 0)
                            GameValues.gameMasterController.teamnum = 0;
                    }
                    GameValues.gameMasterController.teams.Remove(teamLogic);
                    if (GameValues.gameMasterController.teams.Count == 1)
                        GameValues.gameMasterController.EndGame();

                }
                if (currentAgent)
                {
                    SetInactive();
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">initial value of variable</param>
    /// <param name="target">target value of variable</param>
    /// <returns></returns>
    public float ReachTo(ref float value, float target)
    {
        return ReachTo(ref value, target, 1);
    }
    public float ReachTo(ref float value, float target, float multiplier)
    {
        if (Mathf.Abs(target - value) < Time.deltaTime * multiplier)
        {
            value = target;
            return Mathf.Abs(target - value);
        }
            
        else
        {
            if (value > target)
            {
                value -= Time.deltaTime * multiplier;
                
            }
                
            else
            {
                value += Time.deltaTime * multiplier;
            }
            return Time.deltaTime * multiplier;


        }
    }

        public void setAirborne(bool value)
    {
        airborneVal = value;
        thisAnimator.SetBool("airborne", value);
    }
    public void SetPreview()
    {
        grounding.forceGrounded = true;
        currentAgent = false;
        transform.localPosition = new Vector3(-0.4f, -1, 0);
        transform.localScale = new Vector3(13, 13, 1);
    }
    public void Entangle(troop curTroop, team curTeam)
    {
        teamLogic = curTeam;
        troopLogic = curTroop;
        hp = troopLogic.hp;
        hpIndicator.text = hp.ToString();
        hpIndicator.color = curTeam.colors;
        nametag.color = curTeam.colors;
    } 
    public void SetActive()
    {
        crossfireComp.spriteTransform.gameObject.SetActive(true);
        currentAgent = true;
        GameValues.gameMasterController.sceneCamera.currenFocus = transform;
        actionPts = 10;
        outro = 0;
        curWpn = EquipWeapon(1);
        //GameValues.setGui(weaponComp.icon, weaponComp.cost, actionPts, weaponComp.name);
        //m_Rigidbody2D.constraints

    }
    public void SetInactive()
    {
        crossfireComp.spriteTransform.gameObject.SetActive(false);
        currentAgent = false;
        thisAnimator.SetBool("running", false);
        GameValues.gameMasterController.startTurn();
    }

    public void playRandomEffect(ref AudioClip[] clips, AudioSource selectedSource )
    {
        selectedSource.clip = clips[Random.Range(0, clips.Length)] as AudioClip;
        selectedSource.Play();
    }

    public void SetDirLeft(bool isLeft)
    {
        facingLeft = isLeft;
        directionObj.rotation = Quaternion.Euler(0, isLeft ? 180 : 0, 0);
        grounding.leftMod = isLeft ? -1 : 1;
    }

}
