using UnityEngine;

namespace FinalScripts.Fish
{
    public interface IFish
    {
        public void HitPlayer();
        public void HitGround();
        public void HitWater(Vector3 velocity);
        public void HitBird();
        public void PunchedSuccessful();
        public void PunchedUnsuccessful();
    }
}
