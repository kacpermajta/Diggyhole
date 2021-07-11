using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTeamHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public List<MenuCharHandler> teamChar;
    public team thisTeam;
    public GameObject newCharButton;
    public MenuHandler overseer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addCharacter()
    {
        if (teamChar.Count > 3)
            return;
        GameObject newComp =  GameObject.Instantiate(GameValues.gameMasterController.menuCharPrefab, transform);
        MenuCharHandler newHandler = newComp.GetComponent<MenuCharHandler>();
        teamChar.Add(newHandler);
        newHandler.thisTroop = thisTeam.AddChar();
        newHandler.overseer = this;
        newHandler.belonging = thisTeam;
        GameValues.gameMasterController.thisMenuHandler.Resize();
        GameObject.Destroy(newHandler.characterPreview);
        newHandler.characterPreview = GameObject.Instantiate(GameValues.gameMasterController.charPrefabs[0], newHandler.transform);
        newHandler.characterPreview.GetComponent<agentController>().SetPreview();

    }
    public void Resize(int num)
    {
        int i;
        int offset;
        int column = 0;
        if (num > 2)
        {
            offset = num - 3;
            column = 300;
        }
        else
        {
            offset = num;
            column = 0;
        }
        for(i = 0; i<teamChar.Count; i++)
        {

            teamChar[i].transform.position = new Vector3(-255 + column + 45 * i, 0.5f - offset * 90, 0);
        }
        newCharButton.transform.position = new Vector3(-255 + column + 45 * i, 0.5f - offset * 90, 0);
    }
    public void Romove()
    {
        GameValues.gameMasterController.RemoveTeam(thisTeam);
        overseer.matchTeam.Remove(this);
        GameValues.gameMasterController.thisMenuHandler.Resize();
        GameObject.Destroy(gameObject);
    }

}
