using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace HideDryTimeMod
{
    internal static class LocalizationManager
    {
        private static Dictionary<string, Dictionary<string, string>> _data;

        private static Dictionary<string, Dictionary<string, string>> Data =>
            _data ?? (_data = Load());

        private const string EmbeddedResource = "HideDryTimeMod.localization.json";

        private static Dictionary<string, Dictionary<string, string>> Load()
        {
            string dllDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            string userPath = Path.Combine(
                Path.GetDirectoryName(dllDir) ?? dllDir,
                "UserData", "HideDryTimeMod", "localization.json");

            if (File.Exists(userPath))
            {
                var fromFile = TryLoadJson(File.ReadAllText(userPath, Encoding.UTF8));
                if (fromFile != null)
                {
                    MelonLogger.Msg($"[HideDryTimeMod] Localization override: {userPath}");
                    return fromFile;
                }
            }

            var asm = Assembly.GetExecutingAssembly();
            var stream = asm.GetManifestResourceStream(EmbeddedResource);
            if (stream != null)
            {
                using var reader = new StreamReader(stream, Encoding.UTF8);
                var fromEmbedded = TryLoadJson(reader.ReadToEnd());
                if (fromEmbedded != null)
                    return fromEmbedded;
            }

            return Fallback;
        }

        private static Dictionary<string, Dictionary<string, string>> TryLoadJson(string json)
        {
            try { return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json); }
            catch (Exception ex)
            {
                MelonLogger.Warning($"[HideDryTimeMod] Localization parse error: {ex.Message}");
                return null;
            }
        }

        internal static string Get(string key)
        {
            string lang = Localization.Language ?? "English";
            var data = Data;
            if (data.TryGetValue(lang, out var dict) && dict.TryGetValue(key, out string val))
                return val;
            if (data.TryGetValue("English", out var en) && en.TryGetValue(key, out string enVal))
                return enVal;
            return key;
        }

        private static readonly Dictionary<string, Dictionary<string, string>> Fallback = new()
        {
            ["English"] = new()
            {
                ["HD.SECTION"] = "Animal hide drying",
                ["HD.PRESET"] = "Drying time preset",
                ["HD.DESC_PRESET"] = "Quick presets or Custom to use the slider below.",
                ["HD.PRESET_VANILLA"] = "Vanilla (per hide type)",
                ["HD.PRESET_6H"] = "6 hours",
                ["HD.PRESET_12H"] = "12 hours",
                ["HD.PRESET_24H"] = "24 hours (1 day)",
                ["HD.PRESET_48H"] = "48 hours (2 days)",
                ["HD.PRESET_72H"] = "72 hours (3 days)",
                ["HD.PRESET_168H"] = "168 hours (7 days)",
                ["HD.PRESET_CUSTOM"] = "Custom (slider)",
                ["HD.DRY_HOURS"] = "Custom drying time (in-game hours)",
                ["HD.DESC_DRY_HOURS"] = "Only used when preset is Custom. Range: 1–168.",
            }
        };
    }

    [HarmonyPatch(typeof(Localization), nameof(Localization.Get))]
    internal static class Patch_LocalizationGet
    {
        static void Postfix(string __0, ref string __result)
        {
            if (__0 == null || !__0.StartsWith("HD."))
                return;
            __result = LocalizationManager.Get(__0);
        }
    }

    [HarmonyPatch]
    internal static class Patch_DescriptionText
    {
        static System.Reflection.MethodBase TargetMethod() =>
            AccessTools.PropertyGetter(
                AccessTools.TypeByName("ModSettings.DescriptionHolder"), "Text");

        static void Postfix(ref string __result)
        {
            if (__result == null || !__result.StartsWith("HD."))
                return;
            __result = LocalizationManager.Get(__result);
        }
    }
}
