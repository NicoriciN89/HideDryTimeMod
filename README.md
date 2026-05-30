# HideDryTimeMod

HideDryTimeMod is a MelonLoader mod for The Long Dark that changes drying time for evolving materials:
- animal hides and pelts
- gut
- birch and maple saplings

The mod applies one selected drying duration to all supported evolve items.

## What the mod does
- Adds ModSettings menu section: Hide Dry Time.
- Lets you choose a preset drying time:
  - Vanilla
  - 6h, 12h, 24h, 48h, 72h, 168h
  - Custom (1-168 in-game hours)
- Converts selected hours to in-game days and updates EvolveItem drying duration.

## Requirements
- The Long Dark (Il2Cpp build)
- MelonLoader
- ModSettings 2.2.5

## Install
1. Copy HideDryTimeMod.dll to the game Mods folder.
2. Start the game once.
3. Open ModSettings and configure drying preset/hours.

## Build from source
1. Open PowerShell in this folder.
2. Run:
   dotnet restore
   dotnet build -c Release

Or use helper script:
- .\build.ps1

## Project structure
- Core.cs: mod entry point and startup logging
- Settings.cs: ModSettings UI, presets, custom slider
- Patches/EvolveItemPatch.cs: Harmony patches that apply drying time
- localization.json: English and Russian labels

## Notes
- Vanilla preset keeps game default timing per item type.
- Any non-vanilla preset applies one unified time to all supported dryable evolve items.

## License
Choose and add your preferred license (for example MIT) before public release.
