using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follower : MonoBehaviour
{
    public Transform currenFocus;
    public Camera followingCamera;


    // Start is called before the first frame update
    void Start()
    {
        
        GameValues.gameMasterController.sceneCamera = this;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = 0;
        if (currenFocus.position.y > 30)
        {
            
            offset = currenFocus.position.y - 30;
            followingCamera.orthographicSize = currenFocus.position.y - 24;
        }
        transform.position = new Vector3(currenFocus.position.x, currenFocus.position.y- offset*0.8f, transform.position.z);

    }
}
