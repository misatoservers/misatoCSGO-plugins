using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace MisatoHUD;

public class MisatoHUD : BasePlugin
{
    public override string ModuleName => "MisatoHUD";
    public override string ModuleVersion => "1.2.0";
    public override string ModuleAuthor => "viztini";
    public override string ModuleDescription => "misatoCSGO welcome message";

    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnClientPutInServer>(OnClientPutInServer);
    }

    private void OnClientPutInServer(int playerSlot)
    {
        AddTimer(2.0f, () =>
        {
            var player = Utilities.GetPlayerFromSlot(playerSlot);

            if (player == null || !player.IsValid || player.IsBot)
                return;

            player.PrintToChat("misatoCSGO: type !WS to open skinchanger");
        });
    }
}
