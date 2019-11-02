using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    string menuBGM = "Menu BGM";
    [SerializeField]
    string pressedButtonSound = "ButtonPress";
    [SerializeField]
    string hoverOverSound = "ButtonHover";
    AudioManager audioManager;
    void Start() 
    {
        StartCoroutine(_StartMusic());
    }
    public void StartGame()
    {
        audioManager.StopSound(menuBGM);
        audioManager.PlaySound(pressedButtonSound);
        int scene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(scene);
    }
    public void QuitGame()
    {
        audioManager.PlaySound(pressedButtonSound);
        Application.Quit();
    }
    public void OnMouseOver ()
    {
        audioManager.PlaySound(hoverOverSound);
    }
    public IEnumerator _StartMusic()
    {
        yield return new WaitForSeconds(0.1f);
        audioManager = AudioManager.instance;
        audioManager.PlaySound(menuBGM);
    }
}
