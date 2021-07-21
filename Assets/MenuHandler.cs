using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public List<MenuTeamHandler> matchTeam;
    public GameObject newTeamButton;
    public Slider musicslider;
    public MasterControler gameMaster;
    public AudioSource regualted;

    // Start is called before the first frame update
    void Start()
    {
        //regualted = GameValues.gameMasterController.audioSource;
        gameMaster = GameValues.gameMasterController;
        GameValues.gameMasterController.menuCanvas = gameObject;
        //musicslider.onValueChanged.AddListener(delegate (GameValues.gameMasterController.audioSource.volume));
        //GameValues.gameMasterController.audioSource.volume = 1;
        musicslider.value = GameValues.musicValue;
        //musicslider.onValueChanged.RemoveAllListeners();
        //musicslider.onValueChanged.AddListener(delegate { GameValues.gameMasterController.SetMusVol(); });
        //Debug.Log(GameValues.gameMasterController.audioSource.volume);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GameValues.musicValue + "; "+ GameValues.gameMasterController.teamnum);

        //gameMaster = 
    }
    public void SetMusVol()
    {
        GameValues.musicValue = 
            musicslider.value;
        //gameMaster.SetMusVol();
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
