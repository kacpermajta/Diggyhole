using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
    public TextMesh hpIndicator;
    public TextMesh nametag;
    // public CharacterController thisController;
    public Animator thisAnimator;
    public bool isGrounded, forcegrounded;
    public bool currentAgent;
    public float jumping, sway, blasted, blaststart;
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

        if (forcegrounded)
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
        if(!forcegrounded)
            curWpn = EquipWeapon(1);
        jumpcharge = 0;
    }

    private void FixedUpdate()
    {

        if (forcegrounded)
            isGrounded = true;
        else
        {
            if (hp > 0 && transform.position.y < 0)
                Damage(1);

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
            airborneNum = ReachTo(airborneNum, 12,5);
        else if (airborneNum > 1f)
            airborneNum = 1f;
        else
            airborneNum = ReachTo(airborneNum, 0,5);
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

        
        
        if (blasted != 0)
        {
            setAirborne(true);
            if (attacking > 0)
            {
                ResolveAttack();
            }
            else
            //if (!thisAnimator.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
            {

                if (currentAgent)
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        SetDirLeft(true);
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        SetDirLeft(false);

                    }
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
                        outro = 4;
                }
                else
                {
                    if (horizontal > 0)
                    {
                        SetDirLeft(true);
                    }
                    else
                    {
                        SetDirLeft(false);
                    }
                }

            }



            if (blaststart>0)
            {
                blaststart -= Time.deltaTime;

            }
            else 
            {
                //blasted -= Time.deltaTime;
                if (isGrounded)
                    blasted = ReachTo(blasted, 0, 7);
                    //blasted -= Time.deltaTime*9;
                vertical -= gravity * Time.deltaTime*9;

                if (Mathf.Abs(m_Rigidbody2D.velocity.magnitude) < 0.1f)
                {
                    if (!landing)
                    {
                        landing = false;
                        Damage(Mathf.Abs((int)blasted*10));
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
                horizontal = 0;
                vertical = -gravity;
                if (attacking <= 0&& actionPts>0)
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

                if (attacking > 0)
                {
                    ResolveAttack();
                }



                else //if (!thisAnimator.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
                {
                    if (Input.GetKey(KeyCode.A) && sway == 0)
                    {
                        if (jumping == 0 && leap == 0)
                        {
                            horizontal = -moveSpeed;
                            SetDirLeft(true);
                            thisAnimator.SetBool("running", true);
                            if(isGrounded && !sounds.isPlaying)
                            {
                                playRandomEffect(ref runClips, sounds);
                            }
                        }

                    }
                    if (Input.GetKey(KeyCode.D) && sway == 0)
                    {
                        if (jumping == 0 && leap == 0)
                        {
                            horizontal = moveSpeed;
                            SetDirLeft(false);
                            thisAnimator.SetBool("running", true);
                            if (isGrounded && !sounds.isPlaying)
                            {
                                playRandomEffect(ref runClips, sounds);
                            }
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.Mouse0)&&actionPts>0&& airborneNum<10 &&weaponComp.cost>0)
                {
                    thisAnimator.SetBool("attack", true);
                    attacking = weaponComp.attackTime;
                    attack = 0;
                    attackmove = 0;
                    weaponComp.traceEnabler = 0;
                    weaponComp.traceDisabler = 0;
                    weaponComp.soundPlayer = 0;

                }


                if (isGrounded)
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
                            leap = 1;
                            setAirborne(true);
                        }
                    }
                }
                else
                {
                    if (Mathf.Abs(m_Rigidbody2D.velocity.x) < 0.2f)
                        leap = 0;
                    setAirborne(true);
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (jumping > 0 && leap == 0 && jumping < swayWindow)
                        {

                            if (airborneNum == 0)
                            {
                                playRandomEffect(ref gruntClips, sounds);
                            }
                            sway = 0.1f;
                            thisAnimator.SetBool("sway", true);
                            airborneNum = 1;


                        }
                        else
                        {
                            airborneNum = Mathf.Min(airborneNum,9f);
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

                        sway += Time.deltaTime; if (sway > swayDur*agility)
                        {
                            sway = 0;
                            thisAnimator.SetBool("sway", false);
                        }
                    }
                    else if(airborneNum>10)
                    {
                        if (Input.GetKey(KeyCode.S))
                        {
                           
                            thisAnimator.SetBool("down", true);
                            vertical *= 3;
                            impact += Time.deltaTime;
                        }
                        else
                        {

                            thisAnimator.SetBool("down", false);
                            impact = 0;

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
        horizontal = force * 0.2f * direction.x  * multipier;
        vertical = force * 0.2f * direction.y  * multipier;
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
                        Debug.Log("ziuum");
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
        horizontal = force * 0.2f * direction.x ;
        vertical = force * 0.2f * direction.y ;
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

        playRandomEffect(ref hitClips, voice);
        
        auragone.Emit(amount / 2);

        if (Hurt(amount))
        {
            auragone.Emit(30);
            hp = 0;
            thisAnimator.SetBool("dead", true);
            gameObject.GetComponent<Collider2D>().enabled = false;
            if(teamLogic.character.IndexOf(troopLogic)<=teamLogic.charnum)
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
            if(currentAgent)
            {
                SetInactive();
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">initial value of variable</param>
    /// <param name="target">target value of variable</param>
    /// <returns></returns>
    public float ReachTo(float value, float target)
    {
        return ReachTo(value, target, 1);
    }
    public float ReachTo(float value, float target, float multiplier)
    {
        if (Mathf.Abs(target - value) < Time.deltaTime)
            return target;
        else
        {
            if (value > target)
                return value - Time.deltaTime*multiplier;
            else
                return value + Time.deltaTime * multiplier;
        }
    }

        public void setAirborne(bool value)
    {
        airborneVal = value;
        thisAnimator.SetBool("airborne", value);
    }
    public void SetPreview()
    {
        forcegrounded = true;
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
