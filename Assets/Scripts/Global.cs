using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Global
{
    public static int PLAYERSCOUNT = 4;
    public static int CARDSCOUNT = 52;
    public static int LINE1CARDMAX = 3;
    public static int LINE23CARDMAX = 5;
    public enum CARD{
        CARD_A,
        CARD_K,
        CARD_Q,
        CARD_J,
        CARD_10,
        CARD_9,
        CARD_8,
        CARD_7,
        CARD_6,
        CARD_5,
        CARD_4,
        CARD_3,
        CARD_2
    };

    public enum CARDTYPE
    {
        DIAMOND,
        CLUB,
        HEART,
        SPADE
    };
}
