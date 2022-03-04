
public  class Global
{
    public static int PLAYERSCOUNT = 4;
    public static int CARDSCOUNT = 52;
    public static int LINE1CARDMAX = 3;
    public static int LINE23CARDMAX = 5;
    public static int SWAPTIME = 50;
    public static int INITIALMONEY = 100000;
    public static int BOTMONEY = 95000;
    public static int BET = 5000;
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

    public enum GAMESTATE
    {
        GAME_START,
        GAME_DISTRIBUTING,
        GAME_SWAPPING,
        GAME_COMPARING,
        GAME_RESULT
    }

    public enum PLAYERSTATE
    {
        PLAYER_IDLE,
        PLAYER_SWAPPING,
        PLAYER_DONE,
        PLAYER_LEAVE
    }

    public enum AUDIOCLIPS
    {
        BUTTONCLICK,
        FLIP,
        WIN,
        LOSE
    };

    public enum CARDSRANK{
        STRAIGHT_FLUSH = 150,
        FOUR_KIND = 120,
        FULL_HOUSE = 100,
        FLUSH = 80,
        STRAIGHT = 60,
        THREE_KIND = 40,
        TWO_PAIR = 30,
        ONE_PAIR = 20,
        HIGH_CARD = 8
    };
}
