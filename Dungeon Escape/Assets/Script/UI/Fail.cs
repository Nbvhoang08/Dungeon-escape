using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;
public class Fail : UIObject
{
     // Start is called before the first frame update
    public Sprite OnVolume;
    public Sprite OffVolume;
    
    [SerializeField] private Image buttonImage;
    private RectTransform _panelTransform;
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
    }  
    void Update()
    {
        UpdateButtonImage();
    }
    public void RetryBtn()
    {
        Time.timeScale = 1;
        StartCoroutine(Retry());
        Debug.Log("?");
    }
    IEnumerator Retry()
    {
        yield return new WaitForSeconds(0.3f);
         // Load lại scene hiện tại
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        UIController.Instance.CloseUIDirectly<Fail>();

      
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
