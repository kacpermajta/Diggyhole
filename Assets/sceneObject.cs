using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneObject : MonoBehaviour
{
    public Collider2D objectCollider;
    bool useGravity, destroyable;
    int health;
    public bool armed, ready;
    public bool mine;
    public float timer;
    public GameObject flash;
    public float blast, force, damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ready)
        {
            timer -= Time.deltaTime;
            if(timer<0)
            {

                    Debug.Log("bum");
                    Vector3 hitposition = transform.position;
                    GameObject.Instantiate(flash, hitposition, Quaternion.identity);
                    GameValues.destructor.Destroy(hitposition, (int)(25 * blast));
                    foreach (agentController target in GameValues.characters)
                    {
                        //transform.position.z = 0;
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(hitposition, blast);
                        for (int i = 0; i < colliders.Length; i++)
                        {
                            if (colliders[i].gameObject == target.gameObject)
                            {
                                target.Blast(hitposition, blast * 1.5f, force, (int)damage);
                                //               if (!wasGrounded)
                                //                   OnLandEvent.Invoke();
                            }
                        }
                    }

                    GameObject.Destroy(gameObject);
                
            }
        }
        else if (armed)
        {

            if (mine)
            {
                bool explode = false;
                foreach (agentController target in GameValues.characters)
                {
                    //transform.position.z = 0;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blast);
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        if (colliders[i].gameObject == target.gameObject && target.gameObject != gameObject)
                        {
                            //target.Blast(strikepoint, transform.position, weaponComp.range + weaponComp.blast, weaponComp.force, (int)weaponComp.dmg);//+1, 1, 20);
                            ready = true;
                            //               if (!wasGrounded)
                            //                   OnLandEvent.Invoke();
                        }
                    }
                }

            }
        }
    }
    public void SetArmed()
    {
        armed = true;

        objectCollider.enabled = true;

    }
    public void SetValues(float blast, float force, float damage)
    {
        this.blast = blast;
        this.force = force;
        this.damage = damage;
    }

}
