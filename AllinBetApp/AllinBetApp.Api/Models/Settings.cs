namespace AllinBetApp.Api.Models
{
    public class Settings
    {
        public int BlindTimer { get; set; }
        public string BlindIncreaseCondition { get; set; }
        public int PlayersOutToIncrease { get; set; }
        public int RebuyCost { get; set; }
        public int RebuyChips { get; set; }
        public int MaxRebuys { get; set; }
    }
}
