using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    [SerializeField] Vector3 playerResetPosition;
    [SerializeField] float rayHeight = 14;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (Physics.Raycast(new Vector3(other.transform.position.x, rayHeight, other.transform.position.z), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                other.transform.position = hit.point + Vector3.up * other.bounds.size.magnitude;
            }
            else
            {
                if (other.transform != Player.instance.transform)
                    other.transform.position = Player.instance.transform.position;
                else
                    Player.instance.transform.position = playerResetPosition;
            }
        }
    }
}
