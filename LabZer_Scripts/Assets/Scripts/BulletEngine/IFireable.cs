using UnityEngine;

namespace BulletEngine
{
    /// <summary>
    /// Interface <c>IFireable</c> defines the different functions a Fireable class should implements.
    /// </summary>
    public interface IFireable
    {
        /// <summary>
        /// This function is responsible for initializing the bullet game object.
        /// </summary>
        /// <param name="bulletData">A BulletEngine <c>BulletData</c> instance representing the information context to setup the current bullet game object with.</param>
        void InitialiseBullet(BulletData bulletData);

        /// <summary>
        /// This function is responsible getting the bullet game object.
        /// </summary>
        /// <returns>A Unity <c>GameObject</c> representing the bullet game object.</returns>
        GameObject GetGameObject();
    }
}