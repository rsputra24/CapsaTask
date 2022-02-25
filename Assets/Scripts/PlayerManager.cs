using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Character playerData;

    public static PlayerManager instance;

    public void SetPlayerData(Character character)
    {
        playerData = character;
    }

    public Character GetPlayerData()
    {
        return playerData;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }


}
