using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseRotation : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Inverse(transform.parent.rotation);
    }
}
