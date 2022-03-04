using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    private int money = 0;
    private string playerName = "";
    public Player[] players = new Player[4];

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
        money = PlayerPrefs.GetInt("money", Global.INITIALMONEY);
        playerName = PlayerPrefs.GetString("name");
        CreatePlayers();
    }

    public void CreatePlayers()
    {
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            players[i] = new Player(i);
            if (i > 0) //set players from index other than 0 as bot
            {
                players[i].SetAsBot();
            }
            else
            {
                players[i].SetMoney(money);
            }
        }
    }

    public void ResetData()
    {
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            players[i] = null;
        }
        CreatePlayers();
    }

}
