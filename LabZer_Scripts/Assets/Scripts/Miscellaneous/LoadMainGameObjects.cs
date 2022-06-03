using UnityEngine;

public class LoadMainGameObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject objectPooler;

    [SerializeField]
    private GameObject timer;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Instantiate(player);
        PlayerController.Instance.enabled = true;

        if (!ObjectPooler.Instance)
        {
            Instantiate(objectPooler);
        }
        else
        {
            ObjectPooler.Instance.gameObject.SetActive(true);
        }

        Instantiate(timer);
    }
}
