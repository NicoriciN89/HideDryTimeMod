using MelonLoader;

[assembly: MelonInfo(typeof(HideDryTimeMod.Core), "HideDryTimeMod", "1.0.0", "NnicolaeN")]
[assembly: MelonGame("Hinterland", "TheLongDark")]
[assembly: MelonColor(255, 160, 120, 60)]

namespace HideDryTimeMod
{
    public sealed class Core : MelonMod
    {
        public const string Version = "1.0.0";
        internal static MelonLogger.Instance Log;

        public override void OnInitializeMelon()
        {
            Log = LoggerInstance;
            Settings.OnLoad();
            Log.Msg($"HideDryTimeMod v{Version} loaded. Drying: {Settings.Describe()}");
        }
    }
}
