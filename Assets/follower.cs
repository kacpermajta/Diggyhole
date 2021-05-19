using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follower : MonoBehaviour
{
    public Transform currenFocus;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(currenFocus.position.x, currenFocus.position.y, transform.position.z);
    }
}
