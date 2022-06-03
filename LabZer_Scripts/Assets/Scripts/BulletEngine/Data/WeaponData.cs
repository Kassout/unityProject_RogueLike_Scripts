using UnityEngine;

namespace BulletEngine
{
    /// <summary>
    /// Class <c>WeaponData</c> is a BulletEngine scriptable object containing the different properties a weapon game object holds.
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Weapon Data", fileName = "WeaponData", order = 3)]
    public class WeaponData : ScriptableObject
    {
        /// <summary>
        /// Instance field <c>bulletObject</c> is a BulletEngine <c>BulletDataSO</c> scriptable object containing the different properties a bullet game object holds.
        /// </summary>
        public BulletDataSO bulletObject;

        /// <summary>
        /// Instance field <c>bulletSpawnPattern</c> is a BulletEngine <c>BulletSpawnPatternData</c> scriptable object containing the different properties a bullet spawner game object holds.
        /// </summary>
        public BulletSpawnPatternData bulletSpawnPattern;
    }
}

