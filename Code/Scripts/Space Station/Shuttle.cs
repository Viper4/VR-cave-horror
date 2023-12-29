using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shuttle : MonoBehaviour
{
    [SerializeField] ScreenFade screenFade;

    [SerializeField] Animator animator;
    [SerializeField] Transform chairLockPoint;
    [SerializeField] AudioSource doorAudio;
    [SerializeField] AudioSource[] thrusterAudios;
    [SerializeField] AudioSource engineAudio;

    [SerializeField] TextMeshProUGUI frontPanelTitle;
    [SerializeField] TextMeshProUGUI frontPanelText;

    [SerializeField] float deorbitMaxTime = 15;
    [SerializeField] StarSystem starSystem;
    [SerializeField] Vector2 systemSpinRange;
    bool deorbiting;
    float deorbitTime;

    private void Update()
    {
        if(deorbiting)
        {
            float x = deorbitTime / deorbitMaxTime;
            float y = -(x * x + 1);
            starSystem.starOrbitSpeed = starSystem.planetRotateSpeed = Mathf.Lerp(systemSpinRange.x, systemSpinRange.y, y);
            deorbitTime += Time.deltaTime;
        }
    }

    public void LockInChair()
    {
        if(!Player.instance.Frozen)
        {
            SpaceEnvironment.instance.timing = false;
            StartCoroutine(LockInChairRoutine());
        }
    }

    IEnumerator LockInChairRoutine()
    {
        screenFade.Fade(1, 0.5f);
        yield return new WaitUntil(() => screenFade.done);
        Player.instance.Frozen = true;
        Player.instance.transform.SetParent(chairLockPoint);
        Player.instance.transform.SetPositionAndRotation(chairLockPoint.position, chairLockPoint.rotation);
        Player.instance.transform.GetChild(0).SetPositionAndRotation(chairLockPoint.position, chairLockPoint.rotation);
        screenFade.Fade(1, 0, 0.5f);
    }

    public void Launch(Button button)
    {
        button.interactable = false;
        animator.SetTrigger("Station Launch");
        doorAudio.Play();

        frontPanelTitle.text = "UNDOCKING";
        frontPanelText.text = "Autopilot initiated.";
    }

    public void ThrusterEvent(int index)
    {
        thrusterAudios[index].Play();
    }

    public void DeorbitEvent()
    {
        frontPanelTitle.text = "DEORBITING";

        engineAudio.Play();
        StartCoroutine(DeorbitRoutine());
    }

    IEnumerator DeorbitRoutine()
    {
        deorbiting = true;
        yield return new WaitForSeconds(deorbitMaxTime);
        SceneLoader.instance.LoadScene(2);
    }
}
