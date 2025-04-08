using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; set; }
    
    private string highscoreKey = "BestWaveSavedValue";
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        DontDestroyOnLoad(this);
    }
    
    public void SaveHighScore(int score)
    {
            PlayerPrefs.SetInt(highscoreKey, score);
    }

    public int LoadHighScore()
    {
        if (PlayerPrefs.HasKey(highscoreKey))
        {
            return PlayerPrefs.GetInt(highscoreKey);
        }
        else
        {
            return 0;
        }
    }
}
