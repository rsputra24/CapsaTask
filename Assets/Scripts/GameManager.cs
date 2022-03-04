using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Card> cardList;
    public GameObject cardPrefab;
    public GameObject table;
    public List<GameObject> playersDeckObjects;

    private Player[] players = new Player[4];
    public Global.GAMESTATE currentGameState;
    private float swapTimeLeft = 0;

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
        players = Singleton.instance.playerManager.players;
        PrepareCards();
        SetPlayersDeck();
        StartGame();
    }

    void PrepareCards()
    {
        for (int i = 0; i < Global.CARDSCOUNT; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, table.transform);
            Card card = cardObject.GetComponent<Card>();
            card.Init(); // set card facing backward if index is 0
            card.SetCardInfo(i);
            cardList.Add(card);
        }
    }

    void ResetCards()
    {
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            players[i].isReady = false;
            players[i].ResetCards();
            if (i == 0)
            {
                FindObjectOfType<PlayerController>().ResetCards();
            }
        }
        for (int i = 0; i < Global.CARDSCOUNT; i++)
        {
            Card card = cardList[i];
            card.transform.parent = table.transform;
        }
    }

    void StartGame()
    {
        ResetCards();
        GameUIManager.instance.SetUI();
        currentGameState = Global.GAMESTATE.GAME_START;
        swapTimeLeft = Global.SWAPTIME;
        AdvanceNextState();
    }

    IEnumerator StartNewGame()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("start new game");
        StartGame();
    }

    void AdvanceNextState()
    {
        switch (currentGameState)
        {
            case Global.GAMESTATE.GAME_START:
                currentGameState = Global.GAMESTATE.GAME_DISTRIBUTING;
                DistributeCardsToPlayers();
                break;
            case Global.GAMESTATE.GAME_DISTRIBUTING:
                currentGameState = Global.GAMESTATE.GAME_SWAPPING;
                break;
            case Global.GAMESTATE.GAME_SWAPPING:
                currentGameState = Global.GAMESTATE.GAME_COMPARING;
                CompareCards();
                break;
            case Global.GAMESTATE.GAME_COMPARING:
                currentGameState = Global.GAMESTATE.GAME_RESULT;
                PlayerResult();
                break;
            case Global.GAMESTATE.GAME_RESULT:
                StartCoroutine(StartNewGame());
                break;
        }
    }

    void SetPlayersDeck()
    {
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            players[i].line1 = playersDeckObjects[i].transform.GetChild(0).gameObject;
            players[i].line2 = playersDeckObjects[i].transform.GetChild(1).gameObject;
            players[i].line3 = playersDeckObjects[i].transform.GetChild(2).gameObject;
        }
    }

    void ShuffleCards()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            Card tempCard = cardList[i];
            int randomIndex = Random.Range(i, cardList.Count);
            cardList[i] = cardList[randomIndex];
            cardList[randomIndex] = tempCard;
        }
    }

    void Update()
    {
        SwappingState();
    }

    void SwappingState()
    {
        if (currentGameState == Global.GAMESTATE.GAME_SWAPPING)
        {
            swapTimeLeft -= Time.deltaTime;
            if (swapTimeLeft <= 0)
            {
                AdvanceNextState();
            }
            if (players[0].isReady)
            {
                AdvanceNextState();
            }
        }
    }

    void DistributeCardsToPlayers()
    {
        ShuffleCards();
        int cardIndex = 0;
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            for (int j = 0; j < Global.LINE1CARDMAX; j++)
            {
                cardList[cardIndex].transform.parent = players[i].line1.transform;
                players[i].AddCardToList(1, cardList[cardIndex]);
                cardIndex++;
            }
            for (int j = 0; j < Global.LINE23CARDMAX; j++)
            {
                cardList[cardIndex].transform.parent = players[i].line2.transform;
                players[i].AddCardToList(2, cardList[cardIndex]);
                cardIndex++;
            }
            for (int j = 0; j < Global.LINE23CARDMAX; j++)
            {
                cardList[cardIndex].transform.parent = players[i].line3.transform;
                players[i].AddCardToList(3, cardList[cardIndex]);
                cardIndex++;
            }
        }
        AdvanceNextState();
    }

    void CompareCards()
    {
        StartCoroutine(CompareCoroutine());
    }

    IEnumerator CompareCoroutine()
    {
        List<CardRankComboInfo> cardRankComboInfos = new List<CardRankComboInfo>();

        //first line
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = players[i];
            if (i != 0)
            {
                for (int j = 0; j < Global.LINE1CARDMAX; j++)
                {
                    players[i].GetCardList(1)[j].FlipCard();
                }
            }
            CardRankComboInfo cardRankComboInfo = CapsaLogic.CheckCardsRank(player.GetCardList(1));
            cardRankComboInfos.Add(cardRankComboInfo);
            Debug.Log("player " + players[i].playerIndex + ", rank " + cardRankComboInfo.rank + ", number " + cardRankComboInfo.number + ", type " + cardRankComboInfo.type);
        }
        PlayersScoring(cardRankComboInfos);
        yield return new WaitForSeconds(1.5f);
        
        //second line
        for (int i = 1; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = players[i];
            if (i != 0)
            {
                for (int j = 0; j < Global.LINE23CARDMAX; j++)
                {
                    players[i].GetCardList(2)[j].FlipCard();
                }
            }
            CardRankComboInfo cardRankComboInfo = CapsaLogic.CheckCardsRank(player.GetCardList(2));
            cardRankComboInfos.Add(cardRankComboInfo);
        }
        PlayersScoring(cardRankComboInfos);
        yield return new WaitForSeconds(1.5f);

        //third line
        for (int i = 1; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = players[i];
            if (i != 0)
            {
                for (int j = 0; j < Global.LINE23CARDMAX; j++)
                {
                    players[i].GetCardList(3)[j].FlipCard();
                }
            }
            CardRankComboInfo cardRankComboInfo = CapsaLogic.CheckCardsRank(player.GetCardList(3));
            cardRankComboInfos.Add(cardRankComboInfo);
        }
        PlayersScoring(cardRankComboInfos);
        yield return new WaitForSeconds(1.5f);

        AdvanceNextState();
    }

    void PlayersScoring(List<CardRankComboInfo> cardRankComboInfos)
    {
        int size = cardRankComboInfos.Count;
        cardRankComboInfos.Sort((card1, card2) => card1.value.CompareTo(card2.value));
        for (int i = 0; i < size; i++)
        {
            Player player = cardRankComboInfos[i].highestCard.GetCardInfo().holder;
            if (i == 0)
            {
                player.SetScore(-size);
            } else if(i == size - 1)
            {
                player.SetScore(size);
            }
            else
            {
                if(i == 2 || size == 3)
                {
                    player.SetScore(1);
                }
                else if(i == 1)
                {
                    player.SetScore(-1);
                }
            }
            //Debug.Log(cardRankComboInfos[i].value);
        }
    }

    void PlayerResult()
    {
        StopCoroutine(CompareCoroutine());
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = players[i];
            int moneyEarned = player.GetScore() * Global.BET;

            player.SetMoney(moneyEarned);
            Debug.Log("player " + player.playerIndex + " score " + player.GetScore());
        }
        GameUIManager.instance.UpdateUI();
        AdvanceNextState();
    }

    public void LeaveGame()
    {
        Singleton.instance.playerManager.ResetData();
        SceneManager.LoadScene("MenuScene");
    }
}
