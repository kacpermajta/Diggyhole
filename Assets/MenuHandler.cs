using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public List<MenuTeamHandler> matchTeam;
    public GameObject newTeamButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        GameValues.characters = new List<agentController>();
        SceneManager.LoadScene(1);
        GameValues.gameMasterController.ingame = true;
        GameValues.gameMasterController.teamnum = 0;
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void addTeam()
    {
        if (matchTeam.Count > 5)
            return;
        GameObject newComp = GameObject.Instantiate(GameValues.gameMasterController.menuTeamPrefab,GameValues.gameMasterController.menuCanvas.transform);
        MenuTeamHandler newHandler = newComp.GetComponent<MenuTeamHandler>();
        matchTeam.Add(newHandler);
        newHandler.thisTeam = 
            GameValues.gameMasterController.AddTeam();
        newHandler.overseer = this;
        Resize();


    }
    public void Resize()
    {
        int i;
        float column = 0;
        int offset=0;
        for (i = 0; i < matchTeam.Count; i++)
        {
            if (i > 2)
            {
                offset = i - 3;
                column = 300;
            }
            else
            {
                offset = i;
            }
            matchTeam[i].transform.position = new Vector3(column -110, - offset*90, 0);
            matchTeam[i].Resize(i);
        }
        newTeamButton.transform.position = new Vector3(column -230.7f,  37.1f - (offset+1) * 90, 0);

    }
}
