using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public Player[] players = new Player[4];
    public GameObject playerPrefab;
    public static PlayerManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
        public void SetPlayerDataByIndex(int index, Character character)
    {
        players[index].SetPlayerData(character);
    }

    public Player GetPlayerData(int index)
    {
        return players[index];
    }
    public Character GetPlayerCharacterData(int index)
    {
        return players[index].GetPlayerCharData();
    }

    private void Start()
    {
        CreatePlayers();
    }

    public void CreatePlayers()
    {
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            players[i] = Instantiate(playerPrefab, transform).GetComponent<Player>();
            players[i].SetIndex(i);
            if (i > 0) //set players from index other than 0 as bot
            {
                players[i].SetAsBot();
            }
            else
            {
                int money = PlayerPrefs.GetInt("money", Global.INITIALMONEY);
                players[i].SetMoney(money);
            }
        }
    }

    public void ResetData()
    {
        foreach(Player player in players)
        {
            Destroy(player.gameObject);
        }
        CreatePlayers();
    }

}
