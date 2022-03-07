using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    public float randomRangeMin = 5.0f;
    public float randomRangeMax = 35.0f;

    private bool setBotReadyDone = false;

    void Update()
    {
        if (GameManager.instance.currentGameState == Global.GAMESTATE.GAME_SWAPPING)
        {
            if (!setBotReadyDone)
            {
                BotSwapStateBehavior();
            }
        }
        else if(GameManager.instance.currentGameState == Global.GAMESTATE.GAME_COMPARING)
        {
            setBotReadyDone = false;
        }
    }

    void BotSwapStateBehavior()
    {
        for (int i = 1; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = Singleton.instance.playerManager.players[i];
            if (player.GetIsBot() && !setBotReadyDone)
            {
                float readyDelay = Random.Range(randomRangeMin, randomRangeMax);
                StartCoroutine(StartBotSwap(player, readyDelay));
                StartCoroutine(SetBotReady(player, readyDelay));
            }
        }
        setBotReadyDone = true;
    }

    IEnumerator SetBotReady(Player player, float readyDelay)
    {
        yield return new WaitForSeconds(readyDelay);
        player.SetReady(true);
    }

    IEnumerator StartBotSwap(Player player, float readyDelay)
    {
        int swapCount = Random.Range(0, 5);
        float swapIntervalCounter = 0;
        float swapInterval = readyDelay / (float)swapCount;

        while (swapCount > 0)
        {
            swapIntervalCounter += Time.deltaTime;
            if (swapIntervalCounter >= swapInterval)
            {
                swapIntervalCounter = 0;
                swapCount--;
                RandomSwap(player);
            }
            yield return null;
        }
    }

    void RandomSwap(Player player)
    {
        List<Card> cardList = player.GetCardList(0);
        int randomIndexSource = Random.Range(0, cardList.Count);
        int randomIndexTarget = Random.Range(0, cardList.Count);
        if(randomIndexSource == randomIndexTarget)
        {
            if (randomIndexTarget < cardList.Count - 1)
            {
                randomIndexTarget = randomIndexTarget + 1;
            }
            else
            {
                randomIndexTarget = randomIndexTarget - 1;
            }
        }

        Card sourceCard = cardList[randomIndexSource];
        Card targetCard = cardList[randomIndexTarget];

        player.SwapCardBot(sourceCard, targetCard);
    }

}
