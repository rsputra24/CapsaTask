using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    public List<GameObject> playersUIObjects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        
    }

    public void SetUI()
    {
        for(int i=0; i<Global.PLAYERSCOUNT; i++)
        {
            Player player = Singleton.instance.playerManager.players[i];
            Character characterData = player.GetPlayerCharData();

            playersUIObjects[i].transform.GetChild(0).GetComponent<Image>().sprite = characterData.normal;
            playersUIObjects[i].transform.GetChild(1).GetComponent<Text>().text = characterData.name;
            playersUIObjects[i].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = player.GetMoney().ToString();
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = Singleton.instance.playerManager.players[i];
            Character characterData = player.GetPlayerCharData();

            playersUIObjects[i].transform.GetChild(0).GetComponent<Image>().sprite = characterData.normal;
            playersUIObjects[i].transform.GetChild(1).GetComponent<Text>().text = characterData.name;
            playersUIObjects[i].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = player.GetMoney().ToString();
        }
    }

    void Update()
    {
        
    }
}
