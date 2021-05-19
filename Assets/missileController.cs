using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileController : MonoBehaviour
{
    public GameObject flash;
    public Rigidbody mover;
    public float power;
    // Start is called before the first frame update
    void Start()
    {
        mover = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

            mover.AddRelativeForce(new Vector3(power, 0, 0));


    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject.Instantiate(flash, transform.position, Quaternion.identity);
        GameObject.Destroy(gameObject);
    }

}