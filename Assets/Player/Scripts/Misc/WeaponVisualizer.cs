using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVisualizer : MonoBehaviour
{
    [Header("Stats")]
    public PlayerStats stats;

    
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z) * stats.weaponRadius, transform.position.y + Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z) * stats.weaponRadius, 0));    }
}
