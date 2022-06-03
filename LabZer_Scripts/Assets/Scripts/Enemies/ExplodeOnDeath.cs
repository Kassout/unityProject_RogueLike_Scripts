using UnityEngine;

/// <summary>
/// Class <c>ExplodeOnDeath</c> is a Unity script used to manage the explode on death behavior.
/// </summary>
public class ExplodeOnDeath : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>bulletNumber</c> represents the number of bullet to shot on game object death.
    /// </summary>
    [SerializeField]
    private int _bulletNumber;

    /// <summary>
    /// Instance field <c>bulletPrefabTag</c> represents the name of the bullet prefabricated object.
    /// </summary>
    [SerializeField]
    private GameObject _bulletPrefab;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        AudioManager.Instance.PlaySFX(4);
        for (int i = 0; i < _bulletNumber; i++)
        {
            Transform bulletTransform = ObjectPooler.Instance.GetObjectFromPool(_bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, 360 * ((i + 1.0f) / _bulletNumber)));
            if (bulletTransform != null)
            {
                bulletTransform.gameObject.SetActive(true);
            }
        }

        Destroy(gameObject);
    }

    #endregion
}