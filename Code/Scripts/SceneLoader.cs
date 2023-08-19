using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] ScreenFade screenFade;
    [SerializeField] bool fadeOnStart = true;

    [SerializeField] GameObject loadingScreen;
    [SerializeField] TextAnimation loadingAnimation;
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI loadingText;
    bool loadingScene = false;

    private void Start()
    {
        if (fadeOnStart)
            screenFade.Fade(1, 0);
    }

    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneRoutine(index));
    }

    private IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        if (!loadingScene)
        {
            loadingAnimation.Play();

            loadingScene = true;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

            loadingScreen.SetActive(true);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / .9f);

                loadingSlider.value = progress;
                loadingText.text = (progress * 100) + "%";
                yield return null;
            }
            screenFade.Fade(0, 1);
            yield return new WaitForSeconds(screenFade.fadeDuration);
            asyncLoad.allowSceneActivation = true;
            loadingScene = false;
        }
    }
}
