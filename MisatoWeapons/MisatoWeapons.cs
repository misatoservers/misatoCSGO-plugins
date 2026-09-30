using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace MisatoWeapons;

public class MisatoWeapons : BasePlugin
{
    public override string ModuleName => "Misato Weapons";
    public override string ModuleVersion => "1.1.0";
    public override string ModuleAuthor => "viztini";
    public override string ModuleDescription => "Simple weapon chat commands for misatoCSGO";

    private static void Give(CCSPlayerController? player, string weapon, string name)
    {
        if (player == null || !player.IsValid)
            return;

        if (player.PlayerPawn.Value == null)
        {
            player.PrintToChat("You must be spawned to use this.");
            return;
        }

        player.GiveNamedItem(weapon);
        player.PrintToChat($"{name} given.");
    }

    [ConsoleCommand("css_ak", "Give AK-47")]
    public void Ak(CCSPlayerController? player, CommandInfo command)
        => Give(player, "weapon_ak47", "AK-47");

    [ConsoleCommand("css_m4a1", "Give M4A1-S")]
    public void M4A1(CCSPlayerController? player, CommandInfo command)
        => Give(player, "weapon_m4a1_silencer", "M4A1-S");

    [ConsoleCommand("css_m4a4", "Give M4A4")]
    public void M4A4(CCSPlayerController? player, CommandInfo command)
        => Give(player, "weapon_m4a1", "M4A4");

    [ConsoleCommand("css_glock", "Give Glock-18")]
    public void Glock(CCSPlayerController? player, CommandInfo command)
        => Give(player, "weapon_glock", "Glock-18");

    [ConsoleCommand("css_usp", "Give USP-S")]
    public void Usp(CCSPlayerController? player, CommandInfo command)
        => Give(player, "weapon_usp_silencer", "USP-S");
}
