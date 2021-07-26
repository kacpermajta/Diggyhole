using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundingSystem : MonoBehaviour
{

    public bool grounded, footing, backfooting, grabby, touching, high;
    public GameObject origin;
    public int leftMod;
    public bool forceGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (forceGrounded)
        {
            grounded = true;
            footing = true;
            backfooting = true;
            grabby = true;
            touching = true;
            high = false;
        }
        else
        {
            /*
            grounded = false;
            footing = false;
            backfooting = false;
            grabby = false;
            touching = false;
            high = true;
            */


            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, -0.75f), 0.6f);
            grounded = NotSelf(colliders);

            colliders = Physics2D.OverlapCircleAll(transform.position + new Vector3(leftMod * 0.2f, -0.85f), 0.3f);
            footing = NotSelf(colliders);

            colliders = Physics2D.OverlapCircleAll(transform.position + new Vector3(-leftMod * 0.2f, -0.85f), 0.3f);
            backfooting = NotSelf(colliders);

            colliders = Physics2D.OverlapCapsuleAll(transform.position + new Vector3(leftMod * 0.1f, 0), new Vector2(1, 2.2f), new CapsuleDirection2D(), 0);
            grabby = NotSelf(colliders);

            colliders = Physics2D.OverlapCapsuleAll(transform.position + new Vector3(0, 0), new Vector2(1.1f, 2.6f), new CapsuleDirection2D(), 0);
            touching = NotSelf(colliders);

            colliders = Physics2D.OverlapCapsuleAll(transform.position + new Vector3(0, -3.75f), new Vector2(1.2f, 2.5f), new CapsuleDirection2D(), 0);
            high = !NotSelf(colliders);

        }

    }

    public bool NotSelf(Collider2D[] setArray)
    {
        for (int i = 0; i < setArray.Length; i++)
        {
            if (setArray[i].gameObject != origin)
            {
                return true;
                //               if (!wasGrounded)
                //                   OnLandEvent.Invoke();
            }
        }
        return false;
    }
}
