namespace AllinBetApp.Api.Models
{
    public class Game
    {
        public string Id { get; set; }
        public string RoomId { get; set; }
        public List<Player> Players { get; set; }
    }
}
