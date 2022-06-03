using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletEngine
{
    /// <summary>
    /// Class <c>BulletPooler</c> is a Unity script used to manage the general bullet pooler behavior.
    /// </summary>
    public class BulletPooler : MonoBehaviour
    {
        #region Singleton

        // Singleton
        public static BulletPooler Instance { get; private set; }

        #endregion

        #region Fields / Properties

        /// <summary>
        /// Instance field <c>poolDictionary</c> represents a dictionary of Unity <c>Component</c>  sorted by instance ID of the game object they are associate with.
        /// </summary>
        private Dictionary<int, Queue<Component>> _poolDictionary = new Dictionary<int, Queue<Component>>();

        /// <summary>
        /// TODO: add comment.
        /// </summary>
        private Dictionary<int, Transform> _objectStoresDictionary = new Dictionary<int, Transform>();

        /// <summary>
        /// Instance field <c>bulletsToPool</c> is a list of <c>ObjectPoolItem</c> instances representing the different bullets to instantiate inside the pool.
        /// </summary>
        [SerializeField]
        private List<ObjectPoolItem> _bulletsToPool;

        #endregion

        #region MonoBehavior

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            foreach (ObjectPoolItem item in _bulletsToPool)
            {
                int poolKey = item.objectToPool.GetInstanceID();

                GameObject parentGameObject = new GameObject(item.objectToPool.name + " Anchor");

                if (!_objectStoresDictionary.ContainsKey(poolKey))
                {
                    _objectStoresDictionary.Add(poolKey, parentGameObject.transform);
                }

                parentGameObject.transform.SetParent(transform);

                if (!_poolDictionary.ContainsKey(poolKey))
                {
                    _poolDictionary.Add(poolKey, new Queue<Component>());

                    for (int i = 0; i < item.amountToPool; i++)
                    {
                        GameObject obj = Instantiate(item.objectToPool, parentGameObject.transform) as GameObject;

                        obj.SetActive(false);

                        _poolDictionary[poolKey].Enqueue(obj.GetComponent(Type.GetType(item.objectToPool.GetComponent<Bullet>().GetType().ToString())));
                    }
                }
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// This function is responsible for getting a component from pool given his pool key.
        /// </summary>
        /// <param name="poolKey">A integer value representing the instance ID of a prefabricated object associated with the component to get from the pool.</param>
        /// <returns>A Unity <c>Component</c> object representing the bullet game object from pool.</returns>
        private Component GetComponentFromPool(int poolKey)
        {
            if (_poolDictionary[poolKey].Peek().gameObject.activeSelf != true)
            {
                Component componentToReuse = _poolDictionary[poolKey].Dequeue();
                _poolDictionary[poolKey].Enqueue(componentToReuse);

                if (componentToReuse.gameObject.activeSelf == true)
                {
                    componentToReuse.gameObject.SetActive(false);
                }

                return componentToReuse;
            }

            bool shouldExpand = false;
            foreach (ObjectPoolItem objectPoolItem in _bulletsToPool)
            {
                if (objectPoolItem.objectToPool.GetInstanceID() == poolKey)
                {
                    shouldExpand = objectPoolItem.shouldExpand;
                    break;
                }
            }

            if (shouldExpand)
            {
                Component objectTypeToExpand = _poolDictionary[poolKey].Peek();

                Component obj = Instantiate(objectTypeToExpand, _objectStoresDictionary[poolKey]);

                obj.gameObject.SetActive(false);

                _poolDictionary[poolKey].Enqueue(obj.GetComponent(Type.GetType(objectTypeToExpand.GetComponent<Bullet>().GetType().ToString())));

                return obj;
            } 
            else
            {
                Debug.Log("No object pool for " + _poolDictionary[poolKey].Peek());
                return null;
            }
        }

        /// <summary>
        /// This function is responsible for reseting different properties of a pooled object.
        /// </summary>
        /// <param name="componentToReuse">A Unity <c>GameObject</c> representing the prefabricated object to get the bullet component from.</param>
        /// <param name="position">A Unity <c>Vector3</c> structure representing the position coordinate to setup on the request bullet component.</param>
        /// <param name="rotation">A Unity <c>Quaternion</c> structure representing the rotation in polar coordinate to setup on the request bullet component.</param>
        /// <param name="prefab">A Unity <c>GameObject</c> representing the prefabricated object to get the bullet component from.</param>
        private void ResetObject(Component componentToReuse, Vector3 position, Quaternion rotation, GameObject prefab)
        {
            componentToReuse.transform.position = position;
            componentToReuse.transform.rotation = rotation;
            componentToReuse.gameObject.transform.localScale = prefab.transform.localScale;
        }

        #endregion

        #region Public

        /// <summary>
        /// This function is responsible for getting the bullet component from pool associated with the given prefabricated object instance ID.
        /// </summary>
        /// <param name="prefab">A Unity <c>GameObject</c> representing the prefabricated object to get the bullet component from.</param>
        /// <param name="position">A Unity <c>Vector3</c> structure representing the position coordinate to setup on the request bullet component.</param>
        /// <param name="rotation">A Unity <c>Quaternion</c> structure representing the rotation in polar coordinate to setup on the request bullet component.</param>
        /// <returns>A Unity <c>Component</c> object representing the bullet game object from pool.</returns>
        public Component GetBulletFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (_poolDictionary.ContainsKey(poolKey))
            {
                Component componentToReuse = GetComponentFromPool(poolKey);

                ResetObject(componentToReuse, position, rotation, prefab);

                return componentToReuse;
            }
            else
            {
                Debug.Log("No object pool for " + prefab);
                return null;
            }
        }

        #endregion
    }
}