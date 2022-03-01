using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public GameObject characterPanel;
    public GameObject characterButtonPrefab;
    public List<Character> characters = new List<Character>();

    void Start()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            int index = i;
            Character character = characters[i];
            GameObject newCharacterButton = Instantiate(characterButtonPrefab, characterPanel.transform);
            Button button = newCharacterButton.GetComponent<Button>();
            newCharacterButton.GetComponent<Image>().sprite = character.normal;
            newCharacterButton.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = character.name;
            button.onClick.AddListener(delegate { CharacterButtonClicked(index); });
        }
    }

    void Update()
    {
        
    }

    void AddBots(int usedPlayerIndex, int usedCharacterIndex) {
        for (int i = 0; i < characters.Count; i++) {
            if (usedCharacterIndex == i) {
                continue;
            }
            for (int j = 0; j < Global.PLAYERSCOUNT; j++)
            {
                if (usedPlayerIndex == j) {
                    continue;
                } else if(Singleton.instance.playerManager.GetPlayerCharacterData(j) == null || 
                    Singleton.instance.playerManager.GetPlayerCharacterData(j).name == null ||
                    Singleton.instance.playerManager.GetPlayerCharacterData(j).name == "") {
                    Singleton.instance.playerManager.SetPlayerDataByIndex(j, characters[i]);
                    break;
                }
            }
        }
    }

    void CharacterButtonClicked(int index) {
        Singleton.instance.playerManager.SetPlayerDataByIndex(0, characters[index]);
        AddBots(0, index);
        SceneManager.LoadScene("GameScene");
    }
}
