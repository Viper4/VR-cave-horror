using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveRover : MonoBehaviour
{
    [SerializeField] bool active = true;

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

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float eulerX = transform.eulerAngles.x > 180 ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;
        float eulerZ = transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
        transform.eulerAngles = new Vector3(Mathf.Clamp(eulerX, -xAngleLimit, xAngleLimit), transform.eulerAngles.y, Mathf.Clamp(eulerZ, -zAngleLimit, zAngleLimit));

        bool isGrounded = Physics.Raycast(_collider.bounds.center, -transform.up, _collider.bounds.extents.y + 0.05f);

        if(active && isGrounded)
        {
            _rigidbody.MovePosition(transform.position + (transform.forward * (joystick.direction.z * moveSpeed * Time.deltaTime)));
            _rigidbody.MoveRotation(transform.rotation * Quaternion.Euler(transform.up * (joystick.direction.x * rotateSpeed * Time.deltaTime)));
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
