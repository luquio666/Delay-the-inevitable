using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public static Action<Collision> OnRagdollCollisionEnter;

    private void OnCollisionEnter(Collision collision)
    {
        if (OnRagdollCollisionEnter != null)
        {
            OnRagdollCollisionEnter(collision);
        }
    }
}
