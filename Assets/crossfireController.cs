using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossfireController : MonoBehaviour
{
    public GameObject headBone, hairBone, beardBone;
    public GameObject handBone;
    public Animator thisAnimator;
    public agentController thisAgent;

    public Transform spriteTransform;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 delPosition = Vector3.Normalize( Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        angle = Mathf.Max(-Mathf.PI / 2, Mathf.Min(Mathf.PI / 2, delPosition.y * 22));
        Vector3 crosshairDestination = new Vector3(3*  Mathf.Cos(angle), 3 * Mathf.Sin(angle), 0);
        spriteTransform.localPosition = crosshairDestination;

        headBone.transform.Rotate(0, 0, angle * 30);
        hairBone.transform.Rotate(0, 0, -angle * 30);
        beardBone.transform.Rotate(0, 0, -angle * 30);
        if(!thisAgent.melee)
            handBone.transform.Rotate(0, 0, angle * 180 / Mathf.PI + 60);

        thisAnimator.SetFloat("angle", angle);
    }
    private void OnAnimatorIK(int layerIndex)
    {
       // headBone.transform.rotation = Quaternion.Euler(0, 0, angle * 180 / Mathf.PI + 90);

    }
}
