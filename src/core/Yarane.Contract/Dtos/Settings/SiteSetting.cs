namespace Yarane.Contract.Dtos.Settings
{
    public class SiteSetting
    {
        public NehzatConfig NehzatConfig { get; set; }
        public NehzatMelliConfigs NehzatMelliConfigs { get; set; }

        public ConnectigConfig ConnectigConfig { get; set; }
        public PublishConfig PublishConfig { get; set; }
    }
}
