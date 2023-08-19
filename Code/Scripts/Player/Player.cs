using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{
    ContinuousMoveProviderBase moveProvider;
    CharacterController characterController;

    [SerializeField] Hand leftHand;
    [SerializeField] Hand rightHand;
    [SerializeField] float walkSpeed = 1;
    [SerializeField] float runSpeed = 3;
    public bool running { get; private set; }
    public bool isGrounded { get; private set; }

    enum GroundMaterial { Dirt, Metal, Rock }
    GroundMaterial groundMaterial = GroundMaterial.Dirt;

    [Header("Footsteps")]
    [SerializeField] AudioSource footstepAudio;

    [SerializeField] AudioClip[] dirtWalkClips;
    [SerializeField] AudioClip[] dirtRunClips;

    [SerializeField] AudioClip[] metalWalkClips;
    [SerializeField] AudioClip[] metalRunClips;

    [SerializeField] AudioClip[] rockWalkClips;
    [SerializeField] AudioClip[] rockRunClips;

    [SerializeField] float minWalkInterval = 0.75f;
    [SerializeField] float maxWalkInterval = 1;
    float walkInterval;
    [SerializeField] float minRunInterval = 0.3f;
    [SerializeField] float maxRunInterval = 0.5f;
    float runInterval;

    float timer = 0;

    [Header("Events")]
    public bool inShelter;
    public bool inCave;
    [SerializeField] UnityEvent enterShelter;
    [SerializeField] UnityEvent exitShelter;
    [SerializeField] UnityEvent enterCave;
    [SerializeField] UnityEvent exitCave;

    void Start()
    {
        moveProvider = GetComponent<ContinuousMoveProviderBase>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.Raycast(characterController.bounds.center, Vector3.down, characterController.bounds.extents.y + 0.1f);
        if (isGrounded)
        {
            // Footsteps
            if(moveProvider.locomotionPhase == LocomotionPhase.Moving)
            {
                if (running)
                {
                    runInterval = Mathf.Lerp(maxRunInterval, minRunInterval, characterController.velocity.magnitude / runSpeed);
                    if (timer >= runInterval)
                    {
                        switch (groundMaterial)
                        {
                            case GroundMaterial.Dirt:
                                footstepAudio.PlayOneShot(dirtWalkClips[Random.Range(0, dirtWalkClips.Length)]);
                                break;
                            case GroundMaterial.Metal:
                                footstepAudio.PlayOneShot(metalWalkClips[Random.Range(0, metalWalkClips.Length)]);
                                break;
                            case GroundMaterial.Rock:
                                footstepAudio.PlayOneShot(rockWalkClips[Random.Range(0, rockWalkClips.Length)]);
                                break;
                        }
                        timer = 0;
                    }
                }
                else
                {
                    walkInterval = Mathf.Lerp(maxWalkInterval, minWalkInterval, characterController.velocity.magnitude / walkSpeed);
                    if (timer >= walkInterval)
                    {
                        switch (groundMaterial)
                        {
                            case GroundMaterial.Dirt:
                                footstepAudio.PlayOneShot(dirtRunClips[Random.Range(0, dirtRunClips.Length)]);
                                break;
                            case GroundMaterial.Metal:
                                footstepAudio.PlayOneShot(metalRunClips[Random.Range(0, metalRunClips.Length)]);
                                break;
                            case GroundMaterial.Rock:
                                footstepAudio.PlayOneShot(rockRunClips[Random.Range(0, rockRunClips.Length)]);
                                break;
                        }
                        timer = 0;
                    }
                }
                timer += Time.deltaTime;
            }
            else if (moveProvider.locomotionPhase == LocomotionPhase.Done)
            {
                if (running)
                {
                    switch (groundMaterial)
                    {
                        case GroundMaterial.Dirt:
                            footstepAudio.PlayOneShot(dirtWalkClips[Random.Range(0, dirtWalkClips.Length)]);
                            break;
                        case GroundMaterial.Metal:
                            footstepAudio.PlayOneShot(metalWalkClips[Random.Range(0, metalWalkClips.Length)]);
                            break;
                        case GroundMaterial.Rock:
                            footstepAudio.PlayOneShot(rockWalkClips[Random.Range(0, rockWalkClips.Length)]);
                            break;
                    }
                }
                else
                {
                    switch (groundMaterial)
                    {
                        case GroundMaterial.Dirt:
                            footstepAudio.PlayOneShot(dirtWalkClips[Random.Range(0, dirtWalkClips.Length)]);
                            break;
                        case GroundMaterial.Metal:
                            footstepAudio.PlayOneShot(metalWalkClips[Random.Range(0, metalWalkClips.Length)]);
                            break;
                        case GroundMaterial.Rock:
                            footstepAudio.PlayOneShot(rockWalkClips[Random.Range(0, rockWalkClips.Length)]);
                            break;
                    }
                }
            }
        }
    }

    public void ToggleRun()
    {
        moveProvider.moveSpeed = running ? walkSpeed : runSpeed;
        running = !running;
    }

    public void ToggleMovement(bool value)
    {
        moveProvider.moveSpeed = value ? walkSpeed : 0;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.transform.tag)
        {
            case "Dirt":
                groundMaterial = GroundMaterial.Dirt;
                break;
            case "Metal":
                groundMaterial = GroundMaterial.Metal;
                break;
            case "Rock":
                groundMaterial = GroundMaterial.Rock;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "ShelterIn":
                inShelter = true;
                enterShelter?.Invoke();
                break;
            case "ShelterOut":
                inShelter = false;
                exitShelter?.Invoke();
                break;
            case "CaveIn":
                inCave = true;
                enterCave?.Invoke();
                break;
            case "CaveOut":
                inCave = false;
                exitCave?.Invoke();
                break;
        }
    }
}
