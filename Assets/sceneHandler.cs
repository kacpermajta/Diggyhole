using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sceneHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject victoryMessage;
    public Image weaponIcon;
    public Text weaponName;
    public Text ap;
    void Start()
    {
        GameValues.weaponIcon = weaponIcon;
        GameValues.weaponName = weaponName;
        GameValues.ap = ap;
        GameValues.gameMasterController.startTurn();
        GameValues.victoryMessage = victoryMessage;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToMenu()
    {

        SceneManager.LoadScene(0);
    }
    public void setGui(Sprite icon, int AP, int maxAP, string name)
    {
        weaponIcon.sprite = icon;
        weaponName.text = name;
        ap.text = "AP: " + AP + "/" + maxAP;
    }
}
