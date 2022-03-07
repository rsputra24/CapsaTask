using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerUI
{
    public Image avatar;
    public Text nameLabel;
    public Text moneyLabel;
    public Text scoreLabel;
    public Text earningLabel;
    public Text rankLabel;
    public Text readyLabel;
}

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;
    public List<PlayerUI> playersUIObjects;
    public GameObject betPanel;
    public GameObject timerPanel;

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
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = Singleton.instance.playerManager.players[i];
            Character characterData = player.GetPlayerCharData();

            playersUIObjects[i].nameLabel.text = characterData.name;
            playersUIObjects[i].moneyLabel.text = player.GetMoney().ToString();
            playersUIObjects[i].scoreLabel.gameObject.SetActive(false);
            playersUIObjects[i].rankLabel.gameObject.SetActive(false);
            playersUIObjects[i].earningLabel.gameObject.SetActive(false);
            SetPlayersEmotion(playersUIObjects[i], Global.CHAR_EMOTION.NORMAL, characterData);
        }
        betPanel.transform.GetChild(1).GetComponent<Text>().text = Global.BET.ToString();
    }

    public void DisplayCardsRankScore(Player player, Global.CARDSRANK rank, int score)
    {
        StopCoroutine(StartDisplayRankAndScore(player, rank, score));
        StartCoroutine(StartDisplayRankAndScore(player, rank, score));
    }

    IEnumerator StartDisplayRankAndScore(Player player, Global.CARDSRANK rank, int score)
    {
        Character characterData = player.GetPlayerCharData();
        int playerIndex = player.playerIndex;
        string scoreString = score.ToString();
        if (score > 0)
        {
            scoreString = "+" + score;
            SetPlayersEmotion(playersUIObjects[playerIndex], Global.CHAR_EMOTION.WIN, characterData);
        }
        else
        {
            SetPlayersEmotion(playersUIObjects[playerIndex], Global.CHAR_EMOTION.LOSE, characterData);
        }
        playersUIObjects[playerIndex].scoreLabel.gameObject.SetActive(false);
        playersUIObjects[playerIndex].rankLabel.gameObject.SetActive(false);

        playersUIObjects[playerIndex].scoreLabel.text = scoreString;
        playersUIObjects[playerIndex].rankLabel.text = rank.ToString();

        yield return new WaitForSeconds(0.3f);
        playersUIObjects[playerIndex].scoreLabel.gameObject.SetActive(true);
        playersUIObjects[playerIndex].rankLabel.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.2f);
        playersUIObjects[playerIndex].scoreLabel.gameObject.SetActive(false);
        playersUIObjects[playerIndex].rankLabel.gameObject.SetActive(false);
    }

    public void DisplayEarning(Player player, int earning)
    {
        StopCoroutine(StartDisplayEarning(player, earning));
        StartCoroutine(StartDisplayEarning(player, earning));
    }

    IEnumerator StartDisplayEarning(Player player, int earning)
    {
        Character characterData = player.GetPlayerCharData();

        int playerIndex = player.playerIndex;
        int totalScore = player.GetScore();
        string scoreString = totalScore.ToString();
        if (totalScore > 0)
        {
            scoreString = "+" + totalScore;
            SetPlayersEmotion(playersUIObjects[playerIndex], Global.CHAR_EMOTION.WIN, characterData);
        }
        else
        {
            SetPlayersEmotion(playersUIObjects[playerIndex], Global.CHAR_EMOTION.LOSE, characterData);
        }

        playersUIObjects[playerIndex].scoreLabel.gameObject.SetActive(false);
        playersUIObjects[playerIndex].earningLabel.gameObject.SetActive(false);

        playersUIObjects[playerIndex].scoreLabel.text = scoreString;
        playersUIObjects[playerIndex].earningLabel.text = earning.ToString();
        yield return new WaitForSeconds(0.3f);

        playersUIObjects[playerIndex].scoreLabel.gameObject.SetActive(true);
        playersUIObjects[playerIndex].earningLabel.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        SetPlayersEmotion(playersUIObjects[playerIndex], Global.CHAR_EMOTION.NORMAL, characterData);
        playersUIObjects[playerIndex].scoreLabel.gameObject.SetActive(false);
        playersUIObjects[playerIndex].earningLabel.gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = Singleton.instance.playerManager.players[i];
            Character characterData = player.GetPlayerCharData();
            playersUIObjects[i].nameLabel.text = characterData.name;
            playersUIObjects[i].moneyLabel.text = player.GetMoney().ToString();
        }
    }

    void SetPlayersEmotion(PlayerUI playerUIObject, Global.CHAR_EMOTION emotion, Character characterData)
    {
        switch (emotion)
        {
            case Global.CHAR_EMOTION.NORMAL:
                playerUIObject.avatar.sprite = characterData.normal;
                break;
            case Global.CHAR_EMOTION.WIN:
                playerUIObject.avatar.sprite = characterData.win;
                break;
            case Global.CHAR_EMOTION.LOSE:
                playerUIObject.avatar.sprite = characterData.lose;
                break;

        }
    }

    void Update()
    {
        int time = (int)GameManager.instance.swapTimeLeft;
        timerPanel.transform.GetChild(0).GetComponent<Text>().text = time.ToString();
    }
}
