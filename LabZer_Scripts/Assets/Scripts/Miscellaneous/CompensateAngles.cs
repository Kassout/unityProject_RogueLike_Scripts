using UnityEngine;

public class CompensateAngles : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Inverse(transform.parent.rotation);
    }
}
