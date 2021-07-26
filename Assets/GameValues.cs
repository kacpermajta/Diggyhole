using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameValues : MonoBehaviour
{
    public static DTerrain.ClickAndDestroyOptimized destructor;
    public static List<agentController> characters = new List<agentController>();
    public static MasterControler gameMasterController;
    public static GameObject victoryMessage;
    public static Image weaponIcon;
    public static Text weaponName;
    public static Text ap;
    public static float musicValue=0.3f;
    public float soundValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void setGui(Sprite icon, int AP, int maxAP, string name)
    {
        weaponIcon.sprite = icon;
        weaponName.text = name;
        ap.text = "AP: " + AP + "/" + maxAP;
    }

    public static void SetMusicVol()
    {

        GameValues.gameMasterController.audioSource.volume = gameMasterController.thisMenuHandler.musicslider.value;
    }

}
