using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveRover : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float rotateSpeed = 2;
    [SerializeField] float xAngleLimit = 45;
    [SerializeField] float zAngleLimit = 45;

    [Header("References")]
    Rigidbody _rigidbody;
    Collider _collider;
    [SerializeField] Joystick joystick;
    [SerializeField] Transform leftTrack;
    [SerializeField] Transform rightTrack;
    [SerializeField] GameObject[] headlights;
    [SerializeField] MeshRenderer bodyMeshRenderer;
    [SerializeField] Material lightsOnMaterial;
    [SerializeField] Material lightsOffMaterial;

    bool nearBeaconGhost;
    [SerializeField] Transform armGrabPoint;
    bool animatorDone = true;
    [SerializeField] Animator armAnimator;
    [SerializeField] GameObject beaconPrefab;

    [SerializeField] float volumeMultiplier = 1;
    AudioSource _audio;

    public List<Transform> nearbyConnections = new List<Transform>();
    public float connectionDistance;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _audio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        float eulerX = transform.eulerAngles.x > 180 ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;
        float eulerZ = transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
        transform.eulerAngles = new Vector3(Mathf.Clamp(eulerX, -xAngleLimit, xAngleLimit), transform.eulerAngles.y, Mathf.Clamp(eulerZ, -zAngleLimit, zAngleLimit));

        bool isGrounded = Physics.Raycast(_collider.bounds.center, -transform.up, _collider.bounds.extents.y + 0.15f);
        Debug.DrawLine(_collider.bounds.center, _collider.bounds.center - transform.up * (_collider.bounds.extents.y + 0.1f), Color.red, 0.15f);

        if(nearbyConnections.Count > 0)
        {
            if (_rigidbody.velocity != Vector3.zero)
            {
                connectionDistance = (nearbyConnections[0].position - transform.position).sqrMagnitude;
                for (int i = 1; i < nearbyConnections.Count; i++)
                {
                    float distance = (nearbyConnections[i].position - transform.position).sqrMagnitude;
                    if (distance < connectionDistance)
                        connectionDistance = distance;
                }
                //connectionDistance = Mathf.Sqrt(connectionDistance);
            }

            _audio.volume = joystick.direction.magnitude * volumeMultiplier;

            if (isGrounded)
            {
                _rigidbody.MovePosition(transform.position + (transform.forward * (joystick.direction.z * moveSpeed * Time.deltaTime)));
                _rigidbody.MoveRotation(transform.rotation * Quaternion.Euler(transform.up * (joystick.direction.x * rotateSpeed * Time.deltaTime)));
            }
        }
        else
        {
            _audio.volume = 0;
        }
    }

    public void ToggleLights(bool value)
    {
        foreach(GameObject light in headlights)
        {
            light.SetActive(value);
        }
        List<Material> materials = new List<Material>();
        bodyMeshRenderer.GetMaterials(materials);
        materials[1] = value ? lightsOnMaterial : lightsOffMaterial;
        bodyMeshRenderer.SetMaterials(materials);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "":

                break;
            case "Pickup":

                break;
        }
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
