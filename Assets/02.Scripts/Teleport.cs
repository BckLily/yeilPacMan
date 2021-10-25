using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            Vector3 pos = this.transform.position;
            pos.x *= -0.9f;
            other.transform.position = pos;
        }
    }


}
