using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CheckOverlap : MonoBehaviour
{
    public void DestroyOtherIfOverlap()
    {
        Debug.Log("CheckOverlap");
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale, Quaternion.identity, 6);
        Debug.Log(hitColliders.Length);
        if (hitColliders.Length > 0)
        {
            Debug.Log("Destroy: " + hitColliders[0].gameObject.name);
            Destroy(hitColliders[0].gameObject);
        }
    }

    private void FixedUpdate()
    {
        DestroyOtherIfOverlap();
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
