using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PlayerRotationTrigger : MonoBehaviour
{
    [SerializeField] ScreenFade screenFade;

    [SerializeField] bool oneShot = false;
    [SerializeField] bool smoothRotate;
    [SerializeField] float rotateSpeed = 50;
    bool canRotate;

    private void Update()
    {
        if(canRotate)
        {
            if(Quaternion.Angle(Player.instance.transform.rotation, transform.rotation) > 1)
            {
                Player.instance.transform.rotation = Quaternion.RotateTowards(Player.instance.transform.rotation, transform.rotation, rotateSpeed * Time.deltaTime);
            }
            else
            {
                canRotate = false;
                if (oneShot)
                    gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (smoothRotate)
            {
                Debug.Log("Start rotation");
                canRotate = true;
            }
            else
            {
                StartCoroutine(SnapPlayer());
            }
        }
    }

    IEnumerator SnapPlayer()
    {
        screenFade.Fade(1, 0.5f);
        yield return new WaitUntil(() => screenFade.done);
        Player.instance.transform.SetPositionAndRotation(transform.position, transform.rotation);
        Player.instance.transform.GetChild(0).SetPositionAndRotation(transform.position, transform.rotation);
        screenFade.Fade(1, 0, 0.5f);
        if (oneShot)
            gameObject.SetActive(false);
    }
}
