using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossfireController : MonoBehaviour
{
    public GameObject headBone,offhandBone, hairBone, beardBone;
    public GameObject handBone;
    public Animator thisAnimator;
    public agentController thisAgent;

    public Transform spriteTransform;
    public float angle, angleBuildup, diff;
    // Start is called before the first frame update
    void Start()
    {
        angleBuildup = 0;
    }
    private void FixedUpdate()
    {
        diff=0;
        if (thisAgent.attacking > 0)
        {
            diff = angle - angleBuildup;
        }
        else
        {
            diff = 0 - angleBuildup;
        }
        if (Mathf.Abs(diff) > 0.01)
        {
            angleBuildup += 2*diff * Time.deltaTime;
        }
        else
            angleBuildup = 0;
        
           
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 delPosition = Vector3.Normalize( Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        angle = Mathf.Max(-Mathf.PI / 2, Mathf.Min(Mathf.PI / 2, delPosition.y * 22));
        Vector3 crosshairDestination = lookingPoint(3, false); 
        if (thisAgent.currentAgent)
        {
            spriteTransform.localPosition = crosshairDestination;

            headBone.transform.Rotate(0, 0, angle * 30);
            hairBone.transform.Rotate(0, 0, -angle * 30);
            beardBone.transform.Rotate(0, 0, -angle * 30);
            if (!thisAgent.melee)
            {
                handBone.transform.Rotate(0, 0, angle * 180 / Mathf.PI + 60);

            }
            else
            {
                handBone.transform.Rotate(0, 0, angleBuildup * (180 / Mathf.PI + 60));
            }
            if(thisAgent.weaponComp.twoHanded)
            {
                if (!thisAgent.melee)
                    offhandBone.transform.Rotate(0, 0, angle * 180 / Mathf.PI + 60);
                else
                {
                    offhandBone.transform.Rotate(0, 0, angleBuildup * (180 / Mathf.PI + 60));
                }
            }


            thisAnimator.SetFloat("angle", angle);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
       // headBone.transform.rotation = Quaternion.Euler(0, 0, angle * 180 / Mathf.PI + 90);

    }

    public Vector3 lookingPoint(float distance, bool left)
    {
        return new Vector3(left ? -distance * Mathf.Cos(angle): distance * Mathf.Cos(angle), distance * Mathf.Sin(angle) , 0);
    }

}
