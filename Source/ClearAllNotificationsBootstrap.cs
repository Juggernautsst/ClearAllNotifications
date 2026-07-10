using HarmonyLib;
using Verse;

namespace ClearAllNotifications
{
    [StaticConstructorOnStartup]
    internal static class ClearAllNotificationsBootstrap
    {
        private const string HarmonyId = "juggernautsst.clearallnotifications";

        static ClearAllNotificationsBootstrap()
        {
            new Harmony(HarmonyId).PatchAll();
            Log.Message("[Clear All Notifications] Loaded successfully.");
        }
    }
}
