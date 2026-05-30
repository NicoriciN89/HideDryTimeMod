using ModSettings;
using System.Reflection;

namespace HideDryTimeMod
{
    internal enum DryTimePreset
    {
        Vanilla = 0,
        Hours6 = 1,
        Hours12 = 2,
        Hours24 = 3,
        Hours48 = 4,
        Hours72 = 5,
        Hours168 = 6,
        Custom = 7,
    }

    internal static class Settings
    {
        internal static HideDryTimeSettings instance;

        internal static void OnLoad()
        {
            instance = new HideDryTimeSettings();
            instance.RefreshFields();
            instance.AddToModSettings("Hide Dry Time", MenuType.Both);
        }

        internal static string Describe()
        {
            if (instance == null)
                return "?";
            if (instance.dryPreset == (int)DryTimePreset.Vanilla)
                return "vanilla";
            return $"{instance.dryTimeHours:0} in-game hours";
        }

        internal static bool UseVanilla() =>
            instance == null || instance.dryPreset == (int)DryTimePreset.Vanilla;

        internal static float GetDryTimeHours()
        {
            if (instance == null || UseVanilla())
                return 0f;
            float h = instance.dryTimeHours;
            return h < 1f ? 1f : h;
        }
    }

    /// <summary>
    /// Mod Settings 2.2.5 UI: Choice presets + slider for custom hours (see VisibilityExample / OnChangeExample).
    /// </summary>
    internal class HideDryTimeSettings : JsonModSettings
    {
        [Section("HD.SECTION", Localize = true)]

        [Name("HD.PRESET", Localize = true)]
        [Description("HD.DESC_PRESET", Localize = true)]
        [Choice(new[] {
            "HD.PRESET_VANILLA", "HD.PRESET_6H", "HD.PRESET_12H", "HD.PRESET_24H",
            "HD.PRESET_48H", "HD.PRESET_72H", "HD.PRESET_168H", "HD.PRESET_CUSTOM"
        }, Localize = true)]
        public int dryPreset = (int)DryTimePreset.Hours24;

        [Name("HD.DRY_HOURS", Localize = true)]
        [Description("HD.DESC_DRY_HOURS", Localize = true)]
        [Slider(1f, 168f, 168, NumberFormat = "{0:0} h")]
        public float dryTimeHours = 24f;

        internal void RefreshFields()
        {
            bool custom = dryPreset == (int)DryTimePreset.Custom;
            SetFieldVisible(nameof(dryTimeHours), custom);
        }

        protected override void OnChange(FieldInfo field, object oldValue, object newValue)
        {
            if (field.Name == nameof(dryPreset))
            {
                ApplyPreset((int)newValue);
                RefreshFields();
                RefreshGUI();
                return;
            }

            if (field.Name == nameof(dryTimeHours))
                dryPreset = (int)DryTimePreset.Custom;

            RefreshFields();
        }

        private static void ApplyPreset(int preset)
        {
            switch ((DryTimePreset)preset)
            {
                case DryTimePreset.Hours6:   Settings.instance.dryTimeHours = 6f;   break;
                case DryTimePreset.Hours12:  Settings.instance.dryTimeHours = 12f;  break;
                case DryTimePreset.Hours24:  Settings.instance.dryTimeHours = 24f;  break;
                case DryTimePreset.Hours48:  Settings.instance.dryTimeHours = 48f;  break;
                case DryTimePreset.Hours72:  Settings.instance.dryTimeHours = 72f;  break;
                case DryTimePreset.Hours168: Settings.instance.dryTimeHours = 168f; break;
            }
        }

        protected override void OnConfirm()
        {
            base.OnConfirm();
            Core.Log?.Msg($"Hide dry time settings saved: {Settings.Describe()}");
        }
    }
}
