using UnityEngine;

namespace FinalScripts.Fish
{
    [CreateAssetMenu(fileName = "NewFish", menuName = "Fish/New Fish", order = 1)]
    public class FishSrub : ScriptableObject
    {
        [Header("Fish Properties")]
        public GameObject prefab;
        public int initialAmountInPool = 5;
        public float zenFromFish = 5;
        public float scoreMultiplierDistance = 1;
        public float weightInRandomTable = 100;
    }
}