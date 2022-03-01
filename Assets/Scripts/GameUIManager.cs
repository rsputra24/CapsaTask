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
            Character characterData = Singleton.instance.playerManager.GetPlayerCharacterData(i);
            playersUIObjects[i].transform.GetChild(0).GetComponent<Image>().sprite = characterData.normal;
            playersUIObjects[i].transform.GetChild(1).GetComponent<Text>().text = characterData.name;
        }
    }

    void Update()
    {
        
    }
}
