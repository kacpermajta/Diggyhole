using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileController : MonoBehaviour
{
    public GameObject flash;
    public Rigidbody2D mover;
    public float power;
    public bool faceleft;
    public bool timed, explosive, damaging, boomerang, comeback;
    public float lifetime;
    public float blast, force, damage;
    public Transform returnTarget;

    // Start is called before the first frame update
    void Start()
    {
        //mover = gameObject.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

            mover.AddRelativeForce(new Vector2(faceleft? -power: power, 0));


    }
    private void Update()
    {
        if(comeback)
        {
            GoTo(returnTarget);

        }
        else if (timed)
        {
            lifetime -= Time.deltaTime;
            if(lifetime<0)
            {
                ByeBye();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ByeBye();
    }
    public void SetValues(float blast, float force, float damage)
    {
        this.blast = blast;
        this.force = force;
        this.damage = damage;
    }
    public void ByeBye()
    {
        Vector3 hitposition = transform.position;
        if (explosive)
        {
            GameObject.Instantiate(flash, hitposition, Quaternion.identity);
            GameValues.destructor.Destroy(hitposition, (int)(25 * blast));
            if (damaging)
            {
                foreach (agentController target in GameValues.characters)
                {
                    //transform.position.z = 0;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(hitposition, blast);
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        if (colliders[i].gameObject == target.gameObject)
                        {
                            target.Blast(hitposition, blast, force, (int)damage);
                            //               if (!wasGrounded)
                            //                   OnLandEvent.Invoke();
                        }
                    }
                }
            }
        }
        if(boomerang)
        {
            comeback = true;
        }
        else
        GameObject.Destroy(gameObject);
    }

    public void GoTo(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        Vector3 translation = direction.normalized * Time.deltaTime * 30;
        if (direction.magnitude > translation.magnitude)
        {
            transform.position = transform.position + translation;

        }
        else
        {
            transform.position = target.position;
            Destroy(gameObject);
        }
    }

}