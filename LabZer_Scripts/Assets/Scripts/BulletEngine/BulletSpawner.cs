using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BulletEngine
{
    /// <summary>
    /// Class <c>BulletSpawner</c> is a Unity script used to manage the general bullet spawner behavior.
    /// </summary>
    public class BulletSpawner : MonoBehaviour
    {
        #region Fields / Properties

        /// <summary>
        /// Instance field <c>weaponDatas</c> is an array of BulletEngine <c>WeaponData</c> scriptable objects containing the different properties a weapon game object holds.
        /// </summary>
        [SerializeField]
        private WeaponData[] _weaponDatas;

        /// <summary>
        /// Instance field <c>index</c> represents the index of the current weapon data in use from the bullet spawner.
        /// </summary>
        [SerializeField]
        private int _index = 0;

        /// <summary>
        /// Instance field <c>isSequenceRandom</c> represents the is sequence random status of the bullet spawner game object.
        /// </summary>
        [SerializeField]
        private bool _isSequenceRandom;

        /// <summary>
        /// Instance field <c>isSpawnAutomatic</c> represents the is spawn automatic status of the bullet spawner game object.
        /// </summary>
        [SerializeField]
        private bool _isSpawnAutomatic;

        /// <summary>
        /// Instance field <c>timer</c> represents the time value past since the last bullet spawns.
        /// </summary>
        private float _timer;

        /// <summary>
        /// Instance field <c>timer</c> is an array of angle values representing the distribution of the bullets to spawn.
        /// </summary>
        private float[] _rotations;

        #endregion

        #region MonoBehavior

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            if (_weaponDatas.Length > 0)
            {
                _timer = GetSpawnPatternData().coolDown;
            }
        }

        private void OnEnable()
        {
            _timer = GetSpawnPatternData().coolDown;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (_isSpawnAutomatic)
            {
                if (_timer <= 0)
                {
                    SpawnBullets();

                    _timer = GetSpawnPatternData().coolDown;

                    if (_isSequenceRandom)
                    {
                        _index = Random.Range(0, _weaponDatas.Length);
                    }
                    else
                    {
                        _index++;

                        if (_index >= _weaponDatas.Length)
                        {
                            _index = 0;
                        }
                    }

                    _rotations = new float[GetSpawnPatternData().bulletNumber];
                }
                _timer -= Time.deltaTime;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// This function is responsible for getting the data of the spawn pattern from the weapon data.
        /// </summary>
        /// <returns>A BulletEngine <c>BulletSpawnPatternData</c> scriptable object instance containing the different spawn pattern properties.</returns>
        private BulletSpawnPatternData GetSpawnPatternData()
        {
            return _weaponDatas[_index].bulletSpawnPattern;
        }

        /// <summary>
        /// This function is responsible for getting the data of the bullet from the weapon data.
        /// </summary>
        /// <returns>A BulletEngine <c>BulletDataSO</c> scriptable object instance containing the different bullet properties.</returns>
        private BulletDataSO GetBulletData()
        {
            return _weaponDatas[_index].bulletObject;
        }

        /// <summary>
        /// This function is responsible for spawning a single bullet.
        /// </summary>
        /// <param name="rotation">A float value representing the bullet spawning rotation point.</param>
        /// <param name="target">A Unity <c>Transform</c> component representing the position, rotation and scale of the bullet spawning point.</param>
        private void SpawnSingleBullet(float rotation, Transform target)
        {
            _rotations = new float[GetSpawnPatternData().bulletNumber];
            if (GetSpawnPatternData().isRandom)
            {
                RandomRotations();
            }

            // Spawn Single Bullet
            IFireable bullet = (IFireable)BulletPooler.Instance.GetBulletFromPool(GetBulletData().bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, rotation + _rotations[0]));

            /// TODO: replace null check by field validation method.
            if (GetBulletData().bulletData != null)
            {
                if (GetSpawnPatternData().isParent)
                {
                    GetBulletData().bulletData.instanceOrigin = bullet.GetGameObject().transform.parent;
                }

                bullet.InitialiseBullet(GetBulletData().bulletData);

                if (target)
                {
                    if (bullet is GuidedBullet)
                    {
                        GuidedBullet guidedBullet = (GuidedBullet)bullet;
                        guidedBullet.target = target;
                    }
                    else if (bullet is ChasingBullet)
                    {
                        ChasingBullet chasingBullet = (ChasingBullet)bullet;
                        chasingBullet.target = target;
                    }
                }
            }

            if (GetSpawnPatternData().isParent)
            {
                bullet.GetGameObject().transform.parent = transform;
            }

            bullet.GetGameObject().SetActive(true);
        }

        /// <summary>
        /// This function is responsible for spawning multiple bullets.
        /// </summary>
        /// <param name="rotation">A float value representing the bullet spawning rotation point.</param>
        private void SpawnMultipleBullets(float rotation)
        {
            _rotations = new float[GetSpawnPatternData().bulletNumber];
            if (GetSpawnPatternData().isRandom)
            {
                RandomRotations();
            }
            else
            {
                DistributedRotations();
            }

            // Spawn Bullets
            for (int j = 0; j < GetSpawnPatternData().bulletNumber; j++)
            {
                IFireable bullet = (IFireable)BulletPooler.Instance.GetBulletFromPool(GetBulletData().bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, rotation + _rotations[j]));

                /// TODO: replace null check by field validation method.
                if (GetBulletData().bulletData != null)
                {
                    if (GetSpawnPatternData().isParent)
                    {
                        GetBulletData().bulletData.instanceOrigin = bullet.GetGameObject().transform.parent;
                    }

                    bullet.InitialiseBullet(GetBulletData().bulletData);
                }

                if (GetSpawnPatternData().isParent)
                {
                    bullet.GetGameObject().transform.parent = transform;
                }

                bullet.GetGameObject().SetActive(true);
            }
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (GetSpawnPatternData().isParent)
            {
                transform.DetachChildren();
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// This function is responsible for computing the bullet angle distribution in a random mode.
        /// </summary>
        /// <returns>An array of float representing the different angles for bullet spawn distribution.</returns>
        public float[] RandomRotations()
        {
            for (int i = 0; i < GetSpawnPatternData().bulletNumber; i++)
            {
                _rotations[i] = Random.Range(GetSpawnPatternData().minRotation, GetSpawnPatternData().maxRotation);
            }

            return _rotations;
        }

        /// <summary>
        /// This function is responsible for computing the bullet angle distribution in a normal mode.
        /// </summary>
        /// <returns>An array of float representing the different angles for bullet spawn distribution.</returns>
        public float[] DistributedRotations()
        {
            for (int i = 0; i < GetSpawnPatternData().bulletNumber; i++)
            {
                float fraction = (float)i / ((float)GetSpawnPatternData().bulletNumber - 1);
                float difference = GetSpawnPatternData().maxRotation - GetSpawnPatternData().minRotation;
                float fractionOfDifference = fraction * difference;

                _rotations[i] = fractionOfDifference + GetSpawnPatternData().minRotation;
            }

            return _rotations;
        }

        /// <summary>
        /// This function is responsible for spawning bullet in bursts mode.
        /// </summary>
        /// <param name="rotation">A float value representing the bullet spawning rotation point.</param>
        /// <param name="target">A Unity <c>Transform</c> component representing the position, rotation and scale of the bullet spawning point.</param>
        public IEnumerator SpawnBulletBursts(float rotation = 0.0f, Transform target = null)
        {
            for (int i = 0; i < GetSpawnPatternData().burstNumber; i++)
            {
                if (GetSpawnPatternData().bulletNumber > 1)
                {
                    SpawnMultipleBullets(rotation);
                }
                else
                {
                    SpawnSingleBullet(rotation, target);
                }

                yield return new WaitForSeconds(GetSpawnPatternData().coolDown);
            }
        }

        /// <summary>
        /// This function is responsible for spawning bullets.
        /// </summary>
        /// <param name="rotation">A float value representing the bullet spawning rotation point.</param>
        /// <param name="target">A Unity <c>Transform</c> component representing the position, rotation and scale of the bullet spawning point.</param>
        public void SpawnBullets(float rotation = 0.0f, Transform target = null)
        {
            if (GetSpawnPatternData().bulletNumber > 1)
            {
                SpawnMultipleBullets(rotation);
            }
            else
            {
                SpawnSingleBullet(rotation, target);
            }
        }

        #endregion
    }
}