using NUnit.Framework;
using UnityEngine;

[System.Serializable]
public struct Snapshot
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Vector3 velocity;
    public Vector3 angularVelocity;
    public bool isActive;

    public Snapshot(Transform t, Rigidbody rb, GameObject obj)
    {
        position = t.position;
        rotation = t.rotation;
        scale = t.localScale;
        velocity = rb != null ? rb.linearVelocity : Vector3.zero;
        angularVelocity = rb != null ? rb.angularVelocity : Vector3.zero;
        isActive = obj.activeSelf;
    }
    public void Apply(Transform t, Rigidbody rb, GameObject obj)
    {
        obj.SetActive(isActive);
        t.position = position;
        t.rotation = rotation;
        t.localScale = scale;

        if (rb != null && !rb.isKinematic)
        {
            rb.linearVelocity = velocity;
            rb.angularVelocity = angularVelocity;
        }
    }
}