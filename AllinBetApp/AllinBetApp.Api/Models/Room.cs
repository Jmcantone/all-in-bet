namespace AllinBetApp.Api.Models
{
    public class Room
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string HostId { get; set; }
        public bool GameInProgress { get; set; }
        public Settings Settings { get; set; }
        public List<Player> Players { get; set; }
    }
}
