using UnityEngine;

namespace BulletEngine
{
    /// <summary>
    /// Class <c>Bullet</c> is an abstract Unity script representing the base properties and methods for a bullet component.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Bullet : MonoBehaviour
    {
        #region Fields / Properties

        /// <summary>
        /// Instance field <c>lifeTime</c> represents the life time value of the bullet game object.
        /// </summary>
        protected float lifeTime;

        /// <summary>
        /// Instance field <c>impactEffectPrefabTag</c> is a Unity <c>GameObject</c> representing the impact effect prefab of the bullet to instantiate on colliding.
        /// </summary>
        protected GameObject impactEffectPrefab;

        /// <summary>
        /// Instance field <c>whatIsTarget</c> is a Unity <c>LayerMask</c> structure representing layer levels the bullet can collide with.
        /// </summary>
        protected LayerMask whatIsTarget = (1 << 6) | (1 << 9);

        /// <summary>
        /// Instance field <c>timer</c> represents the time value since the bullet game object is enabled.
        /// </summary>
        protected float timer;

        /// <summary>
        /// Instance field <c>bulletDamage</c> represents the bullet game object damage value.
        /// </summary>
        protected int bulletDamage;

        #endregion

        #region MonoBehavior

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected virtual void OnEnable()
        {
            timer = lifeTime;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected abstract void Update();

        /// <summary>
        /// OnBecameInvisible is called when the renderer is no longer visible by any camera.
        /// </summary>
        private void OnBecameInvisible()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Protected

        protected virtual Vector3 CheckBulletColliderCollision(Vector3 prevPosition, CapsuleCollider2D collider)
        {
            // Check for collisions
            RaycastHit2D hit = Physics2D.CapsuleCast(prevPosition, collider.size, collider.direction, transform.rotation.eulerAngles.z, (transform.position - prevPosition).normalized, (transform.position - prevPosition).magnitude, whatIsTarget);
            if (hit.collider != null)
            {
                if (impactEffectPrefab)
                {
                    Transform impactEffectTransform = ObjectPooler.Instance.GetObjectFromPool(impactEffectPrefab, hit.point, Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, hit.normal)));
                    if (impactEffectTransform != null)
                    {
                        impactEffectTransform.gameObject.SetActive(true);
                    }
                }

                AudioManager.Instance.PlaySFX(13);

                // In case of collision with target, damage him.
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerHealthController.Instance.DamagePlayer();
                    if (PlayerController.Instance.isDashing)
                    {
                        return hit.point;
                    }
                }
                else if (hit.collider.TryGetComponent<EnemyHealthController>(out EnemyHealthController enemyHealthController))
                {
                    enemyHealthController.TakeDamage(bulletDamage, true);
                }
                else if (hit.collider.TryGetComponent<Breakable>(out Breakable breakable))
                {
                    breakable.DestroyProp();
                }
                else if (hit.collider.TryGetComponent<BossController>(out BossController boss))
                {
                    boss.TakeDamage(bulletDamage, true);
                }

                gameObject.SetActive(false);
            }

            return hit.point;
        }

        /// <summary>
        /// This function is responsible for checking the collision status of the bullet and process the functions it trigger.
        /// </summary>
        /// <param name="prevPosition">A Unity <c>Vector3</c> structure representing the coordinates of the previous bullet game object position.</param>
        protected virtual Vector3 CheckBulletCollision(Vector3 prevPosition)
        {
            // Check for collisions
            RaycastHit2D hit = Physics2D.Raycast(prevPosition, (transform.position - prevPosition).normalized, (transform.position - prevPosition).magnitude, whatIsTarget);
            if (hit.collider != null)
            {
                if (impactEffectPrefab)
                {
                    Transform impactEffectTransform = ObjectPooler.Instance.GetObjectFromPool(impactEffectPrefab, hit.point, Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, hit.normal)));
                    if (impactEffectTransform != null)
                    {
                        impactEffectTransform.gameObject.SetActive(true);
                    }
                }

                AudioManager.Instance.PlaySFX(13);

                // In case of collision with target, damage him.
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerHealthController.Instance.DamagePlayer();
                    if (PlayerController.Instance.isDashing)
                    {
                        return hit.point;
                    }
                }
                else if (hit.collider.TryGetComponent<EnemyHealthController>(out EnemyHealthController enemyHealthController))
                {
                    enemyHealthController.TakeDamage(bulletDamage, true);
                }
                else if (hit.collider.TryGetComponent<Breakable>(out Breakable breakable))
                {
                    breakable.DestroyProp();
                }
                else if (hit.collider.TryGetComponent<BossController>(out BossController boss))
                {
                    boss.TakeDamage(bulletDamage, true);
                }

                gameObject.SetActive(false);
            }

            return hit.point;
        }

        #endregion
    }
}