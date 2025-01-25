using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Win : UIObject
{
     // Start is called before the first frame update
    public Sprite OnVolume;
    public Sprite OffVolume;
    
    [SerializeField] private Image buttonImage;
    private RectTransform _panelTransform;
    [SerializeField] private List<Image> starImages; // Danh sách các hình ảnh sao
    [SerializeField] private Sprite fullStarSprite;  // Sprite cho sao đầy
    [SerializeField] private Sprite emptyStarSprite; // Sprite cho sao trống
    private void Awake()
    {
        _panelTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // Đặt kích thước ban đầu và vị trí giữa màn hình
        _panelTransform.localScale = Vector3.zero;
        _panelTransform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true); // Bỏ qua Time.timeScale
        UpdateStarDisplay(Player.Instance.StarScore);
    }  
    void Update()
    {
        UpdateButtonImage();
    }
     public void UpdateStarDisplay(int starCount)
    {
        for (int i = 0; i < starImages.Count; i++)
        {
            if (i < starCount)
            {
                starImages[i].sprite = fullStarSprite; // Sao đầy
            }
            else
            {
                starImages[i].sprite = emptyStarSprite; // Sao trống
            }
        }
    }
    public void NextBtn()
    {
        Time.timeScale = 1;
        SoundController.Instance.PlayClickSound();
        StartCoroutine(NextSence());
    }
    public void LoadNextScene()
    {
        int lvIndex =  SceneManager.GetActiveScene().buildIndex + 1; // Tăng chỉ số level
        string nextSceneName = "Level 0" + lvIndex;

        // Kiểm tra xem scene tiếp theo có tồn tại hay không
        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // Nếu không tồn tại scene tiếp theo, quay về Home
            SceneManager.LoadScene("Home");
            UIController.Instance.CloseAll();
            UIController.Instance.OpenUI<Level>();
        }
}
    IEnumerator NextSence()
    {
        yield return new WaitForSeconds(0.3f);
        LoadNextScene();
        UIController.Instance.CloseUIDirectly<Win>();
    }
    public void HomeBtn()
    {
        
        Time.timeScale = 1;
        StartCoroutine(LoadHome());
        SoundController.Instance.PlayClickSound();
    }
    IEnumerator LoadHome()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Home");
        UIController.Instance.CloseAll();
        UIController.Instance.OpenUI<Level>();
    }
    public void SoundBtn()
    {
        SoundController.Instance.TurnOn = !SoundController.Instance.TurnOn;
        UpdateButtonImage();
        SoundController.Instance.PlayClickSound();
    }
    private void UpdateButtonImage()
    {
        if (SoundController.Instance.TurnOn)
        {
            buttonImage.sprite = OnVolume;
        }
        else
        {
            buttonImage.sprite = OffVolume;
        }
    }  
}
