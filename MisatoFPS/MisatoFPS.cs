using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace MisatoFPS;

public class MisatoFPS : BasePlugin
{
    public override string ModuleName => "Misato FPS";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "viztini";
    public override string ModuleDescription => "Optional low-overhead FPS profile for misatoCSGO";

    private bool _enabled;

    [ConsoleCommand("css_fps", "Toggle misatoCSGO FPS mode")]
    public void Fps(CCSPlayerController? player, CommandInfo command)
    {
        string arg = "";

        try
        {
            arg = command.GetArg(1).Trim().ToLowerInvariant();
        }
        catch
        {
        }

        if (arg == "on")
        {
            Enable();
            Notify(player, "misatoCSGO FPS mode: ON");
            return;
        }

        if (arg == "off")
        {
            Disable();
            Notify(player, "misatoCSGO FPS mode: OFF");
            return;
        }

        if (_enabled)
        {
            Disable();
            Notify(player, "misatoCSGO FPS mode: OFF");
        }
        else
        {
            Enable();
            Notify(player, "misatoCSGO FPS mode: ON");
        }
    }

    private void Enable()
    {
        if (_enabled)
            return;

        _enabled = true;

        // Server-side performance profile.
        Execute("g_ragdoll_maxcount 0");
        Execute("g_ragdoll_important_maxcount 0");
        Execute("prop_active_gib_limit 0");
        Execute("prop_active_gib_max_fade_time 0");
        Execute("func_break_max_pieces 0");

        Execute("sv_parallel_packentities 2");
        Execute("sv_parallel_sendsnapshot 2");
        Execute("sv_parallel_checktransmit 2");
        Execute("sv_enable_delta_packing 1");

        Execute("tv_enable 0");
        Execute("sv_logflush 0");
        Execute("sv_logecho 0");

        // Best-effort client-side visual reductions.
        ApplyClients(true);
    }

    private void Disable()
    {
        if (!_enabled)
            return;

        _enabled = false;

        // Safe baseline for this server.
        Execute("g_ragdoll_maxcount 16");
        Execute("g_ragdoll_important_maxcount 4");
        Execute("prop_active_gib_limit 2");
        Execute("prop_active_gib_max_fade_time 1.5");
        Execute("func_break_max_pieces 15");

        Execute("sv_parallel_packentities 2");
        Execute("sv_parallel_sendsnapshot 2");
        Execute("sv_parallel_checktransmit 2");
        Execute("sv_enable_delta_packing 1");

        Execute("tv_enable 0");
        Execute("sv_logflush 0");
        Execute("sv_logecho 0");

        ApplyClients(false);
    }

    private void ApplyClients(bool enabled)
    {
        foreach (var player in Utilities.GetPlayers())
        {
            if (!player.IsValid || player.IsBot)
                continue;

            ApplyClient(player, enabled);
        }
    }

    private void ApplyClient(CCSPlayerController player, bool enabled)
    {
        ExecuteClient(player, $"cl_disable_ragdolls {(enabled ? 1 : 0)}");
        ExecuteClient(player, $"cl_ragdoll_limit {(enabled ? 0 : 20)}");
        ExecuteClient(player, $"cl_playerspraydisable {(enabled ? 1 : 0)}");

        // Best effort. CS2 may reject client settings that are not allowed.
        ExecuteClient(player, $"r_decals {(enabled ? 0 : 2048)}");
        ExecuteClient(player, $"violence_hblood {(enabled ? 0 : 1)}");
        ExecuteClient(player, $"violence_hgibs {(enabled ? 0 : 1)}");
    }

    private void Notify(CCSPlayerController? caller, string message)
    {
        foreach (var player in Utilities.GetPlayers())
        {
            if (player.IsValid)
                player.PrintToChat(message);
        }

        caller?.PrintToChat(message);
    }

    private static void Execute(string command)
    {
        try
        {
            Server.ExecuteCommand(command);
        }
        catch
        {
        }
    }

    private static void ExecuteClient(CCSPlayerController player, string command)
    {
        try
        {
            player.ExecuteClientCommand(command);
        }
        catch
        {
        }
    }
}
