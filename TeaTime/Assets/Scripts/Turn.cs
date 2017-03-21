using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public Vector3 Rotation;

    public void Update()
    {
        transform.eulerAngles += Rotation * Time.deltaTime;
    }
}
