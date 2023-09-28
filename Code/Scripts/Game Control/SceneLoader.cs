using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] ScreenFade screenFade;
    [SerializeField] bool fadeOnLoad = true;
    [SerializeField] float fadeTime = 2;

    [SerializeField] GameObject loadingScreen;
    [SerializeField] TextAnimation loadingAnimation;
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI loadingText;
    bool loadingScene = false;

    private void Start()
    {
        if (fadeOnLoad)
            screenFade.Fade(1, 0, fadeTime);
    }

    public void LoadScene(int index, bool resume, bool fade = true)
    {
        if (resume)
            GameManager.instance.LoadLatestPlayerData();
        StartCoroutine(LoadSceneRoutine(index, fade));
    }

    private IEnumerator LoadSceneRoutine(int sceneIndex, bool fade)
    {
        if (!loadingScene)
        {
            loadingScene = true;

            loadingScreen.SetActive(true);
            loadingScreen.transform.SetPositionAndRotation(Camera.main.transform.position + (Camera.main.transform.forward * 0.5f), Quaternion.LookRotation(Camera.main.transform.forward, Player.instance.transform.up)); loadingScreen.SetActive(true);
            loadingAnimation.Play();

            if (fade)
                screenFade.Fade(0, 1, 2);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

            asyncLoad.allowSceneActivation = false;

            float progress = 0;
            while (progress < 1)
            {
                progress = Mathf.Clamp01(asyncLoad.progress / .9f);

                loadingSlider.value = progress;
                loadingText.text = (Mathf.Round(progress * 1000) / 10) + "%";
                yield return null;
            }
            yield return new WaitUntil(() => screenFade.done);
            asyncLoad.allowSceneActivation = true;
            loadingScene = false;
        }
    }
}
