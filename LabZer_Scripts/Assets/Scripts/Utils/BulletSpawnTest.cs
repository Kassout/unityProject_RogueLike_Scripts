
using BulletEngine;
using UnityEngine;

public class BulletSpawnTest : MonoBehaviour
{
    BulletSpawner bulletSpawner;

    public float shotRate;

    private float shotRateCounter;

    // Start is called before the first frame update
    void Start()
    {
        bulletSpawner = GetComponent<BulletSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        shotRateCounter -= Time.deltaTime;

        if (shotRateCounter <= 0f)
        {
            shotRateCounter = 1f / shotRate;

            bulletSpawner.SpawnBullets(transform.rotation.eulerAngles.z);
        }
    }
}
