using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public GameObject characterPanel;
    public GameObject characterButtonPrefab;
    public List<Character> characters;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Character character in characters)
        {
            GameObject newCharacterButton = Instantiate(characterButtonPrefab, characterPanel.transform);
            newCharacterButton.GetComponent<Image>().sprite = character.normal;
            newCharacterButton.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = character.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onCharacterButtonClicked(int index)
    {
        PlayerManager.instance.SetPlayerData(characters[index]);
        SceneManager.LoadScene("GameScene");
    }
}
