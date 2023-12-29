using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    public static Player instance;

    ContinuousMoveProviderBase moveProvider;

    CharacterController characterController;
    Rigidbody RB;

    public bool useGravity;
    [SerializeField] Hand leftHand;
    [SerializeField] Hand rightHand;
    [SerializeField] float walkSpeed = 1;
    [SerializeField] float runSpeed = 3;
    public bool running { get; set; }

    enum GroundMaterial { Dirt, Metal, Rock, Sand }
    GroundMaterial groundMaterial = GroundMaterial.Dirt;

    [Header("Footsteps")]
    [SerializeField] AudioSource footstepAudio;

    [SerializeField] AudioClip[] dirtWalkClips;
    [SerializeField] AudioClip[] dirtRunClips;

    [SerializeField] AudioClip[] metalWalkClips;
    [SerializeField] AudioClip[] metalRunClips;

    [SerializeField] AudioClip[] rockWalkClips;
    [SerializeField] AudioClip[] rockRunClips;

    [SerializeField] AudioClip[] sandWalkClips;
    [SerializeField] AudioClip[] sandRunClips;

    [SerializeField] float minWalkInterval = 0.75f;
    [SerializeField] float maxWalkInterval = 1;
    float walkInterval;
    [SerializeField] float minRunInterval = 0.3f;
    [SerializeField] float maxRunInterval = 0.5f;
    float runInterval;

    float footstepTimer = 0;

    [Header("Events")]
    public bool inShelter;
    public bool inCave;
    [SerializeField] UnityEvent enterShelter;
    [SerializeField] UnityEvent exitShelter;
    [SerializeField] UnityEvent enterCave;
    [SerializeField] UnityEvent exitCave;

    private bool frozen = false;
    public bool Frozen
    {
        get
        {
            return frozen;
        }
        set
        {
            moveProvider.enabled = !value;
            if(RB != null)
            {
                RB.isKinematic = value;
            }

            frozen = value;
        }
    }

    private void OnEnable()
    {
        if(instance == null)
            instance = this;
    }

    private void OnDisable()
    {
        if(instance == this)
            instance = null;
    }
    
    void Start()
    {
        moveProvider = GetComponent<ContinuousMoveProviderBase>();
        characterController = GetComponent<CharacterController>();
        RB = GetComponent<Rigidbody>();
        moveProvider.useGravity = useGravity;

        if(SceneManager.GetActiveScene().buildIndex != 0)
            StartCoroutine(SaveLoop());
    }

    void Update()
    {
        moveProvider.moveSpeed = running ? runSpeed : walkSpeed;

        if (useGravity && Physics.Raycast(characterController.bounds.center, Vector3.down, characterController.bounds.extents.y + 0.1f))
        {
            // Footsteps
            if(moveProvider.locomotionPhase == LocomotionPhase.Moving)
            {
                if(running)
                {
                    runInterval = Mathf.Lerp(maxRunInterval, minRunInterval, characterController.velocity.magnitude / runSpeed);
                    if(footstepTimer >= runInterval)
                    {
                        switch(groundMaterial)
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
                            case GroundMaterial.Sand:
                                footstepAudio.PlayOneShot(sandRunClips[Random.Range(0, sandRunClips.Length)]);
                                break;
                        }
                        footstepTimer = 0;
                    }
                }
                else
                {
                    walkInterval = Mathf.Lerp(maxWalkInterval, minWalkInterval, characterController.velocity.magnitude / walkSpeed);
                    if(footstepTimer >= walkInterval)
                    {
                        switch(groundMaterial)
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
                            case GroundMaterial.Sand:
                                footstepAudio.PlayOneShot(sandWalkClips[Random.Range(0, sandWalkClips.Length)]);
                                break;
                        }
                        footstepTimer = 0;
                    }
                }
                footstepTimer += Time.deltaTime;
            }
            else if(moveProvider.locomotionPhase == LocomotionPhase.Done)
            {
                if(running)
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
                        case GroundMaterial.Sand:
                            footstepAudio.PlayOneShot(sandRunClips[Random.Range(0, sandRunClips.Length)]);
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
                        case GroundMaterial.Sand:
                            footstepAudio.PlayOneShot(sandWalkClips[Random.Range(0, sandWalkClips.Length)]);
                            break;
                    }
                }
            }
        }
    }

    public void ToggleMovement(bool value)
    {
        moveProvider.moveSpeed = value ? walkSpeed : 0;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {         
        switch(hit.transform.tag)
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
            case "Sand":
                groundMaterial = GroundMaterial.Sand;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
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

    public void UpdateWithLoadedData()
    {
        transform.SetPositionAndRotation(GameManager.instance.GetLoadedPosition(), GameManager.instance.GetLoadedRotation());
    }

    public void SaveData()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if(activeScene.buildIndex != 0)
        {
            GameManager.instance.UpdatePlayerData(transform.position, transform.rotation, activeScene.buildIndex);
            GameManager.instance.SavePlayerData();
        }
    }

    IEnumerator SaveLoop()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        SaveData();
        yield return new WaitForSeconds(30);
        StartCoroutine(SaveLoop());
    }
}
