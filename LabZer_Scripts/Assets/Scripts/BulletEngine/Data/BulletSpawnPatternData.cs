using UnityEngine;

namespace BulletEngine
{
    /// <summary>
    /// Class <c>BulletSpawnPatternData</c> is a BulletEngine scriptable object containing the different properties a bullet spawner game object holds.
    /// </summary>
    [CreateAssetMenu(fileName = "BulletPattern", menuName = "Data/Bullet Pattern Data", order = 2)]
    public class BulletSpawnPatternData : ScriptableObject
    {
        /// <summary>
        /// Instance field <c>minRotation</c> represents the minimum angle value for bullet spawns.
        /// </summary>
        public float minRotation;

        /// <summary>
        /// Instance field <c>maxRotation</c> represents the maximum angle value for bullet spawns.
        /// </summary>
        public float maxRotation;

        /// <summary>
        /// Instance field <c>bulletNumber</c> represents the number of bullets to spawn.
        /// </summary>
        public int bulletNumber = 1;

        /// <summary>
        /// Instance field <c>burstNumber</c> represents the number of burst of bullets to spawn.
        /// </summary>
        public int burstNumber = 1;

        /// <summary>
        /// Instance field <c>isRandom</c> represents the is bullet spawn distribution random status of the bullet spawner game object.
        /// </summary>
        public bool isRandom;

        /// <summary>
        /// Instance field <c>isParent</c> represents the is bullet spawner parent of bullets spawn status of the bullet spawner game object.
        /// </summary>
        public bool isParent = true;

        /// <summary>
        /// Instance field <c>coolDown</c> represents the cooldown value between two bursts of bullets.
        /// </summary>
        public float coolDown;
    }
}