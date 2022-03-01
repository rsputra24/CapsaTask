using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
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
        return players[index].GetPlayerData();
    }

    private void Start()
    {
        for(int i=0; i<Global.PLAYERSCOUNT; i++)
        {
            players[i] = new Player();
        }
    }

}
