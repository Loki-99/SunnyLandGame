using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public GameObject PanelPause;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    } 

    public void PauseUI()
    {
        PanelPause.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeUI()
    {
        PanelPause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetVolume(float value)
    {
        AudioMixer.SetFloat("Music",value);
    }

}
