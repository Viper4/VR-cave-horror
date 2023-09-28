using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.Audio;

public class MenuUI : BaseUI
{
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] GameObject menuCanvas;

    [SerializeField] GameObject creditsMenu;

    [SerializeField] Button resumeButton;

    void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        if (SaveSystem.HasSave())
        {
            resumeButton.interactable = false;
        }
    }

    public void NewGame()
    {
        sceneLoader.LoadScene(1, false);
    }

    void ResumeGame()
    {
        sceneLoader.LoadScene(1, true);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void OpenURL(string linkURL)
    {
        Application.OpenURL(linkURL);
    }
}
