using Model;

namespace Manager
{
    public static class GameController
    {
        public static PlayerInfo PlayerInfo { get; set; } = new PlayerInfo();
        public static PlayerStatus PlayerStatus { get; set; } = new PlayerStatus();
    }
}