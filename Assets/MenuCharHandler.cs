using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCharHandler : MonoBehaviour
{
    public GameObject characterPreview;
    public troop thisTroop;
    public team belonging;
    public MenuTeamHandler overseer;
    public Image background;
    public Text hpIndicator;
    // Start is called before the first frame update
    void Start()
    {
        background.color = belonging.colors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Romove()
    {
        belonging.RemoveChar(thisTroop);
        overseer.teamChar.Remove(this);
        GameValues.gameMasterController.thisMenuHandler.Resize();
        GameObject.Destroy(gameObject);
    }
    public void nextChar()
    {
        int newchar;
        if (thisTroop.prefab == GameValues.gameMasterController.charPrefabs.Length - 1)
            newchar = 0;
        else
            newchar = thisTroop.prefab + 1;
        thisTroop.prefab = newchar;

        GameObject.Destroy(characterPreview);
        characterPreview =  GameObject.Instantiate(GameValues.gameMasterController.charPrefabs[newchar], transform);
        characterPreview.GetComponent<agentController>().SetPreview();
        
    }
    public void setCharacter(int num, GameObject prefab)
    {

    }
    public void addHealth(int num)
    {
        thisTroop.hp += num;
        hpIndicator.text = thisTroop.hp.ToString();

    }
}
