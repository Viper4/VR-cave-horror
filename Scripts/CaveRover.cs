using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveRover : MonoBehaviour
{
    Rigidbody _rigidbody;
    [SerializeField] bool active = true;
    [SerializeField] float force = 2;
    [SerializeField] Joystick joystick;
    [SerializeField] Transform leftTrack;
    [SerializeField] Transform rightTrack;

    [SerializeField] Light[] headlights;
    [SerializeField] MeshRenderer bodyMeshRenderer;
    [SerializeField] Material lightsOnMaterial;
    [SerializeField] Material lightsOffMaterial;

    bool nearBeaconGhost;
    [SerializeField] Transform armGrabPoint;
    bool animatorDone = true;
    [SerializeField] Animator armAnimator;
    [SerializeField] GameObject beaconPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            _rigidbody.AddForceAtPosition(force * (-joystick.direction.z - joystick.direction.x) * leftTrack.forward, leftTrack.position, ForceMode.VelocityChange);
            _rigidbody.AddForceAtPosition(force * (-joystick.direction.z + joystick.direction.x) * rightTrack.forward, rightTrack.position, ForceMode.VelocityChange);
        }
    }

    public void ToggleLights(bool value)
    {
        foreach(Light light in headlights)
        {
            light.enabled = value;
        }
        bodyMeshRenderer.materials[1] = value ? lightsOnMaterial : lightsOffMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Beacon":

                break;
            case "Pickup":

                break;
        }
    }

    public void SetActive(bool value)
    {
        active = value;
        ToggleLights(value);
    }

    public void ArmInteract()
    {
        if (animatorDone)
        {
            StartCoroutine(StartArmInteraction());
            if (nearBeaconGhost)
            {
                armAnimator.SetTrigger("GrabBeacon");
            }
        }
    }

    IEnumerator StartArmInteraction()
    {
        animatorDone = false;

        if (nearBeaconGhost)
            armAnimator.SetTrigger("GrabBeacon");

        yield return new WaitForSeconds(armAnimator.GetCurrentAnimatorStateInfo(0).length);
        animatorDone = true;
    }
}
