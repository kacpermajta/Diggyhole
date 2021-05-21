﻿using System.Collections;
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

   // public CharacterController thisController;
    public Animator thisAnimator;
    public bool isGrounded;
    public bool currentAgent;
    public float jumping, sway;
    public float vertical, horizontal;
    public bool facingLeft, melee;
    public bool attack;
    public float leap;
    public float attacking;
    public AnimatorControllerParameter running, airborne;
    public Transform directionObj;
    public GameObject[] weapons;
    public GameObject curWpn;
    public Transform HandBone;

    public crossfireController crossfireComp;
    public weaponController weaponComp;

    private Vector3 m_Velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        jumping = 0;
        sway = 0;
        vertical = 0;
        horizontal = 0;
        facingLeft = true;
        leap = 0;
        melee = true;
        attack = false;
        attacking = 0;
        //running = thisAnimator.GetParameter(1);
        //airborne = thisAnimator.GetParameter(2);
    }

    private void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + new Vector3(0,-0.65f), 0.6f);
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

    // Update is called once per frame
    void Update()
    {
        horizontal = 0;
        vertical = -gravity;
        
        if (currentAgent)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                curWpn = EquipWeapon(0);
                thisAnimator.SetBool("melee", true);
                melee = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                curWpn = EquipWeapon(1);
                thisAnimator.SetBool("melee", true);
                melee = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                curWpn = EquipWeapon(2);
                thisAnimator.SetBool("melee", true);
                melee = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                curWpn = EquipWeapon(3);
                thisAnimator.SetBool("melee", false);
                melee = false;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                curWpn = EquipWeapon(4);
                thisAnimator.SetBool("melee", false);
                melee = false;
            }
            thisAnimator.SetBool("running", false);

            if (attacking > 0)
            {
                attacking -= Time.deltaTime;
                if (attacking <= 0)
                {
                    thisAnimator.SetBool("attack", false);
                    attacking = 0;
                    if(!melee)
                    {
                        
                        GameObject.Instantiate(weaponComp.missile, 
                            new Vector3(crossfireComp.spriteTransform.position.x, crossfireComp.spriteTransform.position.y, 0), 
                            Quaternion.Euler(0, facingLeft?180:0, 180* crossfireComp.angle / Mathf.PI));
                    }
                }
            }



            else if(!thisAnimator.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
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
                attacking = 1.1f;
            }


            if (isGrounded)
            {
                //vertical = 0;
                thisAnimator.SetBool("sway", false);
                thisAnimator.SetBool("airborne", false);
                jumping = 0;
                sway = 0;
                leap = 0;
                if (Input.GetKey(KeyCode.W))
                {
                    jumping = 0.1f;
                    thisAnimator.SetBool("airborne", true);
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    jumping = 0.1f;
                    leap = 1;
                    thisAnimator.SetBool("airborne", true);
                }
            }
            else
            {
                if (Mathf.Abs(m_Rigidbody2D.velocity.x) < 0.2f)
                    leap = 0;
                thisAnimator.SetBool("airborne", true);
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
                    vertical /= 3;
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
            if(leap > 0)
            {
                
                if (facingLeft)
                {
                    horizontal -= leapSpeed*leap;
                }
                else
                {
                    horizontal += leapSpeed*leap;
                }

            }

            if(horizontal!=0  && isGrounded)
            {
                //if (Mathf.Abs(m_Rigidbody2D.velocity.x) < 4f)
                    vertical += jumpSpeed;
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(150*horizontal, 100 * vertical);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, .05f);


            //thisController.Move(new Vector3(horizontal, vertical, 0));

        }

    }

    public GameObject EquipWeapon(int i)
    {
        GameObject.Destroy(curWpn);
        thisAnimator.SetBool("melee", true);
        GameObject newWpn = GameObject.Instantiate(weapons[i], HandBone);
        weaponComp = newWpn.GetComponent<weaponController>();
        return newWpn;

    }
}
