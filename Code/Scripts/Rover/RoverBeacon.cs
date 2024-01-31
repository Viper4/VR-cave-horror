using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverBeacon : MonoBehaviour
{
    public SphereCollider sphereCollider;
    bool _connected = false;
    bool connected { 
        get 
        {
            return _connected;
        }

        set 
        {
            _connected = value;
            if (value)
            {
                blinker.onColor = connectedColor;
                if (nearbyRover != null && !nearbyRover.nearbyConnections.Contains(transform))
                    nearbyRover.nearbyConnections.Add(transform);
            }
            else
            {
                blinker.onColor = disconnectedColor;
                if(nearbyRover != null)
                    nearbyRover.nearbyConnections.Remove(transform);
            }
        }
    }
    Vector3 connectionOrigin;

    CaveRover nearbyRover;
    List<RoverBeacon> nearbyBeacons = new List<RoverBeacon>();
    RoverController nearbyController;
    [SerializeField] LayerMask ignoreLayers;
    [SerializeField] Blinker blinker;
    [ColorUsage(true, true), SerializeField] Color connectedColor = Color.green;
    [ColorUsage(true, true), SerializeField] Color disconnectedColor = Color.red;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        connectionOrigin = transform.position + sphereCollider.center;
        Collider[] overlappingColliders = Physics.OverlapSphere(connectionOrigin, sphereCollider.radius, ~ignoreLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider overlappingCollider in overlappingColliders)
        {
            switch (overlappingCollider.tag)
            {
                case "Rover":
                    nearbyRover = overlappingCollider.GetComponent<CaveRover>();
                    if (connected)
                        nearbyRover.nearbyConnections.Add(transform);
                    break;
                case "RoverBeacon":
                    if(overlappingCollider.transform != transform)
                    {
                        if (Physics.Raycast(connectionOrigin, overlappingCollider.transform.position - connectionOrigin, out RaycastHit hit, Mathf.Infinity, ~ignoreLayers, QueryTriggerInteraction.Ignore))
                        {
                            if (hit.transform == overlappingCollider.transform)
                            {
                                RoverBeacon otherBeacon = overlappingCollider.GetComponent<RoverBeacon>();
                                Debug.DrawLine(connectionOrigin, hit.point, Color.green, 10f);
                                nearbyBeacons.Add(otherBeacon);
                                if (connected && !otherBeacon.connected)
                                    SetNearbyConnection(true, new List<RoverBeacon>());
                            }
                            else
                            {
                                Debug.DrawLine(transform.position, hit.point, Color.red, 10f);
                            }
                        }
                    }
                    break;
                case "RoverController":
                    if (!connected)
                        SetNearbyConnection(true, new List<RoverBeacon>());
                    connected = true;
                    nearbyController = overlappingCollider.GetComponent<RoverController>();
                    nearbyController.UpdateConnection();
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            switch (other.tag)
            {
                case "Rover":
                    nearbyRover = other.GetComponent<CaveRover>();
                    if (connected)
                        nearbyRover.nearbyConnections.Add(transform);
                    break;
                case "RoverBeacon":
                    if (Physics.Raycast(connectionOrigin, other.transform.position - connectionOrigin, out RaycastHit hit, Mathf.Infinity, ~ignoreLayers, QueryTriggerInteraction.Ignore) && hit.transform == other.transform)
                    {
                        RoverBeacon otherBeacon = other.GetComponent<RoverBeacon>();
                        nearbyBeacons.Add(otherBeacon);
                        if (connected && !otherBeacon.connected)
                            SetNearbyConnection(true, new List<RoverBeacon>());
                    }
                    break;
                case "RoverController":
                    if (!connected)
                        SetNearbyConnection(true, new List<RoverBeacon>());
                    connected = true;
                    nearbyController = other.GetComponent<RoverController>();
                    nearbyController.UpdateConnection();
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            switch (other.tag)
            {
                case "Rover":
                    nearbyRover.nearbyConnections.Remove(transform);
                    break;
                case "RoverBeacon":
                    RoverBeacon otherBeacon = other.GetComponent<RoverBeacon>();
                    nearbyBeacons.Remove(otherBeacon);
                    if (nearbyController == null)
                        CheckChainConnection(new List<RoverBeacon>());
                    break;
                case "RoverController":
                    nearbyController = null;
                    CheckChainConnection(new List<RoverBeacon>());
                    other.GetComponent<RoverController>().UpdateConnection();
                    break;
            }
        }
    }

    public bool CheckChainConnection(List<RoverBeacon> checkedBeacons)
    {
        checkedBeacons.Add(this);
        if(nearbyController != null)
        {
            connected = true;
            return true;
        }
        foreach(RoverBeacon beacon in nearbyBeacons)
        {
            if(!checkedBeacons.Contains(beacon) && beacon.CheckChainConnection(checkedBeacons))
            {
                connected = true;
                return true;
            }
        }
        connected = false;
        return false;
    }

    public void SetNearbyConnection(bool value, List<RoverBeacon> setBeacons)
    {
        setBeacons.Add(this);
        foreach(RoverBeacon beacon in nearbyBeacons)
        {
            beacon.connected = value;
            if(!setBeacons.Contains(beacon))
                beacon.SetNearbyConnection(value, setBeacons);
        }
    }
}
