public static class InternalZenEventManager
{
   public delegate void InternalZenEvents();
   public static InternalZenEvents updateVisualZenBar;
   public static InternalZenEvents showSparkles;
   public static InternalZenEvents hideSparkles;
   public static InternalZenEvents showPromptText;
   public static InternalZenEvents hidePromptText;
   public static InternalZenEvents spawnWeakPoints;
   public static InternalZenEvents stopSpawnWeakPoints;

   public static InternalZenEvents startChargeVfx;
   public static InternalZenEvents stopChargeVfx;
}
