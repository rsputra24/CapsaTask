using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsaLogic
{

    public static CardRankComboInfo CheckCardsRank(List<Card> cardList)
    {
        int size = cardList.Count;
        CardRankComboInfo rankComboInfo = new CardRankComboInfo();
        int pairCount = 0;
        Card highestCard = null;
        Card highestCardPair = null;
        Card highestCardFlush = null;
        List<Card> flushCardList = new List<Card>();
        List<List<Card>> pairsList = new List<List<Card>>();

        if (size == 3 || size == 5)
        {
            bool checkHighCard = true;
            bool straight = true;
            for (int i = 0; i < size - 1; i++)
            {
                int typeCount = 1;
                int numberCount = 1;
                CardInfo cardInfo = cardList[i].GetCardInfo();
                Card tempHighestCardPair = null;
                List<Card> pairCardList = new List<Card>();
                for (int j = i + 1; j < size; j++)
                {
                    CardInfo nextCardInfo = cardList[j].GetCardInfo();
                    if (!highestCard) highestCard = cardInfo.value > nextCardInfo.value ? cardList[i] : cardList[j];
                    else highestCard = highestCard.GetCardInfo().value > nextCardInfo.value ? highestCard : cardList[j];
                    if((int) nextCardInfo.number - (int) cardInfo.number != 1)
                    {
                        straight = false;
                    }
                    if (cardInfo.number == nextCardInfo.number)
                    {
                        numberCount++;
                        pairCardList.Add(cardList[j]);
                        tempHighestCardPair = cardInfo.value > nextCardInfo.value ? cardList[i] : cardList[j];
                    }
                    if (cardInfo.type == nextCardInfo.type)
                    {
                        typeCount++;
                        flushCardList.Add(cardList[j]);
                        highestCardFlush = cardInfo.value > nextCardInfo.value ? cardList[i] : cardList[j];
                    }
                }

                ////ranking
                if (typeCount == size)
                {
                    rankComboInfo.highestCard = highestCardFlush;
                    rankComboInfo.type = highestCardFlush.GetCardInfo().type;
                    rankComboInfo.number = highestCardFlush.GetCardInfo().number;
                    rankComboInfo.rank = Global.CARDSRANK.FLUSH;
                    rankComboInfo.cardList = flushCardList;
                    if (straight) rankComboInfo.rank = Global.CARDSRANK.STRAIGHT_FLUSH;
                    break;
                }
                else if (straight)
                {
                    rankComboInfo.highestCard = highestCard;
                    rankComboInfo.type = highestCard.GetCardInfo().type;
                    rankComboInfo.number = highestCard.GetCardInfo().number;
                    rankComboInfo.rank = Global.CARDSRANK.STRAIGHT;
                    rankComboInfo.cardList = cardList;
                    break;
                }
                else if (numberCount >= 2)
                {
                    if (!highestCardPair) highestCardPair = tempHighestCardPair;
                    else { 
                        highestCardPair = highestCardPair.GetCardInfo().value > tempHighestCardPair.GetCardInfo().value ? highestCardPair : tempHighestCardPair;
                        //Debug.Log(highestCardPair.GetCardInfo().value + " vs " + tempHighestCardPair.GetCardInfo().value );
                    }
                    rankComboInfo.highestCard = highestCardPair;
                    rankComboInfo.type = highestCardPair.GetCardInfo().type;
                    rankComboInfo.number = highestCardPair.GetCardInfo().number;
                    pairsList.Add(pairCardList);
                    if(numberCount == 3)
                    {
                        rankComboInfo.rank = Global.CARDSRANK.THREE_KIND;
                    } else if(numberCount == 4)
                    {
                        rankComboInfo.rank = Global.CARDSRANK.FOUR_KIND;
                    }
                    else if(numberCount == 2)
                    {
                        rankComboInfo.rank = Global.CARDSRANK.ONE_PAIR;
                    }
                    checkHighCard = false;
                }
                else if(checkHighCard)
                {
                    rankComboInfo.highestCard = highestCard;
                    rankComboInfo.type = highestCard.GetCardInfo().type;
                    rankComboInfo.number = highestCard.GetCardInfo().number;
                    rankComboInfo.rank = Global.CARDSRANK.HIGH_CARD;
                }
                else
                {
                    //Debug.Log("test ");
                }
            }
            if (pairsList.Count > 0)
            {
                rankComboInfo.pairsList = pairsList;
                if(pairsList.Count == 2)
                {
                    int firstPairCount = pairsList[0].Count;
                    int secondPairCount = pairsList[1].Count;
                    Debug.Log("first " + firstPairCount);
                    Debug.Log("second " + secondPairCount);
                    if (firstPairCount == 1 && secondPairCount == 1)
                    {
                        rankComboInfo.rank = Global.CARDSRANK.TWO_PAIR;
                    }
                    else if(firstPairCount == 2 && secondPairCount == 1 || firstPairCount == 1 && secondPairCount == 2)
                    {
                        //rankComboInfo.rank = Global.CARDSRANK.FULL_HOUSE;
                    }
                }
            }

        }
        else if (size == 5)
        {

        }
        rankComboInfo.value = (int) rankComboInfo.rank + (int) rankComboInfo.number + (float)rankComboInfo.type / 5;
        return rankComboInfo;
    }

}
