using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deployabe : MonoBehaviour
{
    public enum entryType {crosshair, crosshairAngled, agent }
    public entryType ObjEntryType;
    public float armTime;
    public sceneObject handler;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(armTime >0)
        {
            armTime -= Time.deltaTime;
            if (armTime <= 0)
                handler.SetArmed();

        }

        
    }
}
