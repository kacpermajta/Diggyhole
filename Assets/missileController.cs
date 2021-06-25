using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileController : MonoBehaviour
{
    public GameObject flash;
    public Rigidbody2D mover;
    public float power;
    public bool faceleft;
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

    private void OnCollisionEnter(Collision collision)
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hitposition = transform.position;
        GameObject.Instantiate(flash, hitposition, Quaternion.identity);
        GameValues.destructor.Destroy(hitposition, 50);
        foreach (agentController target in GameValues.characters)
        {
            //transform.position.z = 0;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(hitposition, 1.3f);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject == target.gameObject)
                {
                    target.Blast(hitposition, 1.3f, 1, 20);
                    //               if (!wasGrounded)
                    //                   OnLandEvent.Invoke();
                }
            }
        }

        GameObject.Destroy(gameObject);
    }

}