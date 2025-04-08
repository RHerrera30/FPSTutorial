using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public  TMP_Text highScoreUI;
    string newGameScene = "SampleScene";

    public AudioClip bg_music;
    public AudioSource main_channel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        main_channel.PlayOneShot(bg_music);
        //Set high score text
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Wave Survived: {highScore}";
    }

    public void StartNewGame()
    {
        main_channel.Stop();
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    
#else
        Application.Quit();
#endif
    }
}
