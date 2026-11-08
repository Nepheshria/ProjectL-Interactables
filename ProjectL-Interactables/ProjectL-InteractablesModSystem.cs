// Internal
using ProjectLInteractables.Blocks;

// External
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Client;
using ProjectLInteractables.Items;
using Vintagestory.API.MathTools;
using Vintagestory.API.Datastructures;

namespace ProjectL_Interactables;

public class ProjectL_InteractablesModSystem : ModSystem {
    // Called on server and client
    public override void Start(ICoreAPI api) {
        base.Start(api);
        Mod.Logger.Notification("Hello from ProjectL-Interactables: " + Lang.Get(Mod.Info.ModID + ":hello"));

        // Register Blocks
        api.RegisterBlockClass(Mod.Info.ModID + ".bumper", typeof(BlockBumper));
        api.RegisterBlockClass(Mod.Info.ModID + ".ldoorblock", typeof(BlockLDoorBlock));
        api.RegisterBlockClass(Mod.Info.ModID + ".ldoorkeyhole", typeof(BlockLDoorKeyhole));
        api.RegisterBlockClass(Mod.Info.ModID + ".lfgatelock", typeof(BlockLFGateLock));
        api.RegisterBlockClass(Mod.Info.ModID + ".portal", typeof(BlockPortal));

        // Register Items
        api.RegisterItemClass(Mod.Info.ModID + ".ldoorkey", typeof(ItemLDoorKey));
        api.RegisterItemClass(Mod.Info.ModID + ".lfkey", typeof(ItemLFKey));
        api.RegisterItemClass(Mod.Info.ModID + ".llocator", typeof(ItemLLocator));
    }

    public override void StartServerSide(ICoreServerAPI api) {
        base.StartServerSide(api);
        Mod.Logger.Notification("Hello from Interactables ready to Labyrinth!");

        api.ChatCommands.Create("setlocator")
            .WithDescription("Configure the locator target")
            .RequiresPrivilege(Privilege.chat)
            .RequiresPlayer()
            .HandleWith((args) => {
                var byEntity = args.Caller.Entity;
                AssetLocation sound = new(Mod.Info.ModID + ":sounds/effect/BipBoup");
                byEntity.World.PlaySoundAt(sound, byEntity);
                Vec3i pos = byEntity.Pos.AsBlockPos.AsVec3i;
                IPlayer byPlayer = args.Caller.Player;
                IPlayerInventoryManager inventoryManager = byPlayer.InventoryManager;


                if (inventoryManager.ActiveHotbarSlot != null) {
                    // Hotbar selected
                    ItemSlot active_slot = inventoryManager.ActiveHotbarSlot;

                    if (!active_slot.Empty && active_slot.Itemstack.Collectible is ItemLLocator) {
                        ITreeAttribute target = active_slot.Itemstack.Attributes.GetOrAddTreeAttribute("target");
                        target.SetInt("x", pos.X);
                        target.SetInt("y", pos.Y);
                        target.SetInt("z", pos.Z);

                        // To syncronize changes
                        active_slot.MarkDirty();

                        byEntity.World.Logger.Chat(Lang.Get(Mod.Info.ModID + ":locatorconfig") + $": {pos.X}, {pos.Y}, {pos.Z}");

                        return TextCommandResult.Success();
                    }
                    return TextCommandResult.Error(Lang.Get(Mod.Info.ModID + ":notlocator"));
                }

                return TextCommandResult.Error(Lang.Get(Mod.Info.ModID + ":nolocator"));
            });

        api.ChatCommands.Create("setPortal")
            .WithDescription("Configure the portal target")
            .RequiresPrivilege(Privilege.chat)
            .RequiresPlayer()
            .HandleWith((args) => {
                var byEntity = args.Caller.Entity;
                AssetLocation sound = new(Mod.Info.ModID + ":sounds/effect/Pop");
                byEntity.World.PlaySoundAt(sound, byEntity);
                Vec3d pos = byEntity.Pos.XYZ;

                BlockPortal.portal_location = pos;

                return TextCommandResult.Success();
            });
    }

    public override void StartClientSide(ICoreClientAPI api) {
        base.StartClientSide(api);
        Mod.Logger.Notification("Hello from ProjectL-Interactables I hope you like traps!");

    }
}
