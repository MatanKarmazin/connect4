namespace Ex02.Logic
{
    public enum eToken
    {
        Empty = 0,
        Player1 = 1,
        Player2 = 2,
    }

    public enum ePlayerType
    {
        Human = 0,
        Computer = 1,
    }

    public enum eGameState
    {
        InProgress = 0,
        Won = 1,
        Draw = 2,
        Quit = 3,
    }

    public enum ePlayerIndex
    {
        Player1 = 0,
        Player2 = 1,
    }
}