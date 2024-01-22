using UnityEngine;

namespace FinalScripts.Fish
{
    [CreateAssetMenu(fileName = "NewFish", menuName = "Fish/New Fish", order = 1)]
    public class FishSrub : ScriptableObject
    {
        [Header("Fish Properties")] 
        public GameObject prefab;
        public int initialAmountInPool = 5;
        public float weightInRandomTable = 100;
        
        [Header("Zen")]
        [Tooltip("Zen gained from successful punch")]
        public float zenGainedFromPunched = 5;
        [Tooltip("Zen lost when player is hit by this fish")]
        public float zenLostByHit = 10;
        
        [Header("Score")]
        [Tooltip("Score gained by punching fish successfully (NOT IMPLEMENTED!)")]
        public int baseScoreAmount = 10;
        [Tooltip("Score lost when this fish hits the player")]
        public int damageAmount = 20;
        [Tooltip("Score gained when this fish hits a bird")]
        public int scoreFromHittingBird = 20;
        
        [Header("Punch")]
        [Tooltip("Multiplies handVelocity and adds it as a force to the fish")]
        public float punchVelMultiplier = 30;
        [Tooltip("Minimum velocity needed for a successful punch")]
        [Range(0f, 5f)]public float successfulPunchThreshold = 3;
        
        [Header("Skipping")]
        [Tooltip("How many times more forward velocity has to be grater than downwards velocity to skipp")]
        public float fSpeedNeededMultiplier = 1;
        public float attackAngleMaximum = 15;
        [Tooltip("Upwards boost gained from skipping")]
        public float ySkippSpeedAmount = 5;
        
        [Header("Despawning")]
        public float despawnDelay = 2.5f;
        public float despawnTime = 10;
        public float despawnAltitude = -5;
    }
}