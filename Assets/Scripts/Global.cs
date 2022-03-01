
public  class Global
{
    public static int PLAYERSCOUNT = 4;
    public static int CARDSCOUNT = 52;
    public static int LINE1CARDMAX = 3;
    public static int LINE23CARDMAX = 5;
    public static int SWAPTIME = 45;
    public enum CARD{
        CARD_3,
        CARD_4,
        CARD_5,
        CARD_6,
        CARD_7,
        CARD_8,
        CARD_9,
        CARD_10,
        CARD_J,
        CARD_Q,
        CARD_K,
        CARD_A,
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
