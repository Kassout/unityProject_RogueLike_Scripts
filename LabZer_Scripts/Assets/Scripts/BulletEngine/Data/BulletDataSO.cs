using System;
using UnityEngine;

namespace BulletEngine
{
    /// <summary>
    /// Class <c>BulletDataSO</c> is a BulletEngine scriptable object containing the different properties a bullet game object holds.
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Bullet Data", fileName = "BulletData", order = 4)]
    public class BulletDataSO : ScriptableObject
    {
        /// <summary>
        /// Instance field <c>bulletPrefab</c> is a Unity <c>GameObject</c> representing the prefabricated bullet game object.
        /// </summary>
        public GameObject bulletPrefab;

        /// <summary>
        /// Instance field <c>bulletData</c> is a BulletEngine <c>BulletData</c> object representing the different context information of the bullet game object with.
        /// </summary>
        public BulletData bulletData;
    }

    /// <summary>
    /// Class <c>BulletData</c> is a BulletEngine script representing the different context information of a bullet game object should hold.
    /// </summary>
    [Serializable]
    public class BulletData
    {
        /// <summary>
        /// Instance field <c>lifeTime</c> represents the life time value of the bullet game object.
        /// </summary>
        public float lifeTime;

        /// <summary>
        /// Instance field <c>impactEffectPrefab</c>  is a Unity <c>GameObject</c> representing the impact effect prefab of the bullet to instantiate on colliding.
        /// </summary>
        public GameObject impactEffectPrefab;

        /// <summary>
        /// Instance field <c>whatIsTarget</c> is a Unity <c>LayerMask</c> structure representing layer levels the bullet can collide with.
        /// </summary>
        public LayerMask whatIsTarget = (1 << 6) | (1 << 9);

        /// <summary>
        /// Instance field <c>bulletDamage</c> represents the bullet game object damage value.
        /// </summary>
        public int bulletDamage;

        /// <summary>
        /// Instance field <c>velocity</c> is a Unity <c>Vector3</c> structure representing the velocity direction vector of the bullet game object.
        /// </summary>
        public Vector2 velocity;

        /// <summary>
        /// Instance field <c>speed</c> represents the speed movement value of the bullet game object.
        /// </summary>
        public float speed;

        /// <summary>
        /// Instance field <c>whatIsWall</c> is a Unity <c>LayerMask</c> structure representing the different layers considered as wall for the bullet game object.
        /// </summary>
        public LayerMask whatIsWall;

        /// <summary>
        /// Instance field <c>rotationChange</c> represents the rotation change value over time of the bullet game object.
        /// </summary>
        public float rotationChange;

        /// <summary>
        /// RODO: add comment.
        /// </summary>
        [HideInInspector]
        public Transform instanceOrigin;
    }
}