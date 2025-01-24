using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GamePlay : UIObject
{
    [SerializeField] private Text LevelName;
     public void Pause()
    {
        Time.timeScale = 0;
        UIController.Instance.OpenUI<Pause>();
        SoundController.Instance.PlayClickSound();
    }
    void Update()
    {
        UpdateLevelText();
    }
    private void UpdateLevelText()
    {
        if (LevelName != null)
        {   
            int levelNumber = SceneManager.GetActiveScene().buildIndex;
            LevelName.text = $"Level: {levelNumber:D2}"; // Hiển thị với 2 chữ số, ví dụ: 01, 02
        }   
    }  
}
