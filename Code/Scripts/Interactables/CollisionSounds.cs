using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
    [SerializeField] Transform audioSourceTransform;
    AudioSource audioSource;

    [SerializeField] float collideThreshold = 0.1f;
    [SerializeField] float minPitch = 1;
    [SerializeField] float maxPitch = 1;
    [SerializeField] float volumeMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = audioSourceTransform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        float collisionStrength = Vector3.Dot(contact.normal, collision.relativeVelocity);
        if (collision.rigidbody != null)
        {
            collisionStrength *= collision.rigidbody.mass;
        }
        if (collisionStrength >= collideThreshold)
        {
            audioSourceTransform.position = contact.point;
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.volume = collisionStrength * volumeMultiplier;
            audioSource.Play();
        }
    }
}
