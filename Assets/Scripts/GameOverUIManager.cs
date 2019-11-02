using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField]
    string pressedButtonSound = "ButtonPress";
    [SerializeField]
    string hoverOverSound = "ButtonHover";
    void Start()
    {
        StartCoroutine(_SetAudioManager());
    }
    public void QuitGame()
    {
        audioManager.PlaySound(pressedButtonSound);
        Application.Quit();
    }
    public void RestartGame()
    {
        audioManager.PlaySound(pressedButtonSound);
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
    }
    public void OnMouseOver()
    {
        audioManager.PlaySound(hoverOverSound);
    }
    public IEnumerator _SetAudioManager()
    {
        yield return new WaitForSeconds(0.1f);
        audioManager = AudioManager.instance;
    }
}
