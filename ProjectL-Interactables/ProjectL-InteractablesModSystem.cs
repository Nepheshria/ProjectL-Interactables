// Internal
using ProjectLInteractables.Blocks;

// External
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Client;
using ProjectLInteractables.Items;

namespace ProjectL_Interactables;

public class ProjectL_InteractablesModSystem : ModSystem {
    // Called on server and client
    public override void Start(ICoreAPI api) {
        Mod.Logger.Notification("Hello from ProjectL-Interactables: " + Lang.Get("mymodid:hello"));

        // Register Blocks
        api.RegisterBlockClass(Mod.Info.ModID + ".bumper", typeof(BlockBumper));
        api.RegisterBlockClass(Mod.Info.ModID + ".ldoorblock", typeof(BlockLDoorBlock));
        api.RegisterBlockClass(Mod.Info.ModID + ".ldoorkeyhole", typeof(BlockLDoorKeyhole));

        // Register Items
        api.RegisterItemClass(Mod.Info.ModID + ".ldoorkey", typeof(ItemLDoorKey));
    }

    public override void StartServerSide(ICoreServerAPI api) {
        Mod.Logger.Notification("Hello from Interactables ready to Labyrinth!");
    }

    public override void StartClientSide(ICoreClientAPI api) {
        Mod.Logger.Notification("Hello from ProjectL-Interactables I hope you like traps!");
    }
}
