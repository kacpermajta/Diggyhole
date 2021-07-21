using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterControler : MonoBehaviour
{
    public List<team> teams = new List<team>();
    public follower sceneCamera;

    public bool[] active;
    public GameObject blankWpnPrefab;
    public GameObject[] charPrefabs;
    public GameObject menuTeamPrefab;
    public GameObject menuCharPrefab;
    public GameObject menuCanvas;
    public MenuHandler thisMenuHandler;
    public bool ingame;
    public int teamnum;
    public AudioSource audioSource;
    public float soundValue;
    public float musicValue;

    private static GameObject instance;
    Object[] myMusic;
    public AudioClip[] backClips;
    public List<Color> colorList;

    public GameObject GetChar(int num)
    {
        return charPrefabs[num];
    }

    void Awake()
    {
        //myMusic = Resources.LoadAll("Music", typeof(AudioClip));
        //audioSource.clip = myMusic[0] as AudioClip;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameValues.gameMasterController = this;
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);
        ingame = false;
        musicValue = 1;

        //audioSource.Play();
        //audioSource[0].Play();
    }





  

    void playRandomMusic()
    {
        audioSource.clip = backClips[Random.Range(0, backClips.Length)] as AudioClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
            playRandomMusic();



        audioSource.volume =  GameValues.musicValue;
        if (ingame)
        {


            




        }
    }

    public void RemoveTeam(team removed)
    {
        colorList.Add(removed.colors);
        teams.Remove(removed);
    }

    public team AddTeam()
    {
        team newTeam = new team();
        teams.Add(newTeam);
        newTeam.colors = colorList[colorList.Count - 1];
        colorList.RemoveAt(colorList.Count - 1);
        return newTeam;
    }

    public void EndGame()
    {
        GameValues.victoryMessage.SetActive(true);

    }

    public void startTurn()
    {
        team curTeam = teams[teamnum];
        troop curCharacter = curTeam.character[curTeam.charnum];
        if (curCharacter.model==null)
        {
            curCharacter.model = GameObject.Instantiate(charPrefabs[curCharacter.prefab], new Vector3(15, 50, 0), Quaternion.identity);
            curCharacter.model.GetComponent<agentController>().Entangle(curCharacter, curTeam);
        }
        curCharacter.model.GetComponent<agentController>().SetActive();

        teamnum++;
        if (teamnum == teams.Count)
            teamnum = 0;
        curTeam.charnum++;
        if (curTeam.charnum == curTeam.character.Count)
            curTeam.charnum = 0;

    }
    public void SetMusVol()
    {
        musicValue = thisMenuHandler.musicslider.value;
    }

}

public class troop
{
    public int prefab;
    string name;
    public int hp;
    public GameObject model;
    public troop ()
    {
        prefab = 0;
        name = "New";
        hp = 100;
        model = null;
    }
    

    public void SetChar(int num)
    {
        prefab = num;
    }

}

public class team
{
    public List<troop> character;
    public int charnum;
    public Color colors;
    public team()
    {
        character = new List<troop>();
        charnum = 0;
    }
    public void RemoveChar(troop removed)
    {
        character.Remove(removed);
    }
    public troop AddChar()
    {
        troop newTroop = new troop();
        character.Add(newTroop);
        return newTroop;
    }
}
