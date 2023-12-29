using UnityEngine;
using UnityEngine.UI;

public class MenuUI : BaseUI
{
    [SerializeField] GameObject menuCanvas;

    [SerializeField] Button resumeButton;

    void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        resumeButton.interactable = SaveSystem.HasSave();
    }

    public void NewGame()
    {
        SceneLoader.instance.LoadScene(1);
    }

    void ResumeGame()
    {
        SceneLoader.instance.LoadSave(true);
    }

    public void OpenURL(string linkURL)
    {
        Application.OpenURL(linkURL);
    }
}
