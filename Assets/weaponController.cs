using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponController : MonoBehaviour
{
    new public string name;

    #region logic variables

    public float range;
    public float force, blast, dmg;
    public float knockback, backstep;

    public bool twoHanded, thrust, melee, deployable, thrown;
    /// <summary>
    /// how long takes the attack
    /// </summary>
    public float attackTime;
    /// <summary>
    /// attack AP cost
    /// </summary>
    public int cost;
    /// <summary>
    /// animation variant; 1 is reserved for rifles
    /// </summary>
    public float variant;
    /// <summary>
    /// unused
    /// </summary>
    public float pierce;

    #endregion

    #region objects

    public GameObject missile;
    public GameObject sticky;
    public Transform parent;

    #endregion


    #region timetable variables

    public float[] strikeTimes;
    public float[] movementTimes;
    public float[] traceStart;
    public float[] traceEnd;
    public float[] soundTimes;

    public int traceEnabler;
    public int traceDisabler;
    public int soundPlayer;

    #endregion

    #region media

    public Sprite icon;
    public SpriteRenderer traceImg;
    public AudioSource wpnSource;
    public AudioClip[] effectClips;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (thrown)
        {
            if (sticky != null)
            {
                GoTo(sticky.transform);
            }
            else if (parent.position != transform.position)
            {
                GoTo(parent);
            }
        }
    }
    public void GoTo (Transform target)
    {
        
        Vector3 direction = target.position - transform.position;
        Vector3 translation = direction.normalized * Time.deltaTime*60;
        if(false)//direction.magnitude > translation.magnitude)
        {
            transform.position = transform.position + translation;

        }
        else
        {
            transform.position = target.position;
        }
    }

    void playRandomEffect()
    {
        wpnSource.clip = effectClips[Random.Range(0, effectClips.Length)] as AudioClip;
        wpnSource.Play();
    }
}
