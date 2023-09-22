using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public GameObject target; 

    void Update()
    {
        Vector3 my_new_distance = (target.transform.position - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(my_new_distance);
    }
}
