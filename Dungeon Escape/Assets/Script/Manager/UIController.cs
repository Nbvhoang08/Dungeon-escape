using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class UIController : Singleton<UIController>
{
    [SerializeField] private List<UIObject> UIObjects; // Danh sách các UI canvas có sẵn
    [SerializeField] private float scaleTo = 1.2f; // Kích thước khi scale
    [SerializeField] private float duration = 0.25f; // Thời gian hiệu ứng
    public override void Awake()
    {
        base.Awake();
        InitializeUICanvases();
    }
    void Start()
    {
        // OpenUI<ChooseLV>();
        // OpenUI<GamePlay>();
    }
    // Khởi tạo tất cả UI Canvas, đặt chúng ở trạng thái không hoạt động
    private void InitializeUICanvases()
    {
        foreach (var canvas in UIObjects)
        {
            canvas.gameObject.SetActive(false);
        }
    }


    public void OnButtonClick(Button button)
    {
        // Scale button lên và trở về kích thước gốc
        transform.DOScale(scaleTo, duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // Sau khi scale xong, trở về kích thước ban đầu
                transform.DOScale(1f, duration).SetEase(Ease.OutQuad);
            });
    }
    // Mở một UI cụ thể
    public T OpenUI<T>() where T : UIObject
    {
        T canvas = GetUI<T>();
        if (canvas != null)
        {
            canvas.Setup();
            canvas.Open();
        }

        return canvas;
    }

    // Mở một UI với vị trí cha tùy chỉnh
    public T OpenUI<T>(Transform customParent) where T : UIObject
    {
        T canvas = GetUI<T>();
        if (canvas != null)
        {
            canvas.Setup();
            canvas.Open();
        }

        return canvas;
    }

    // Đóng UI sau một khoảng thời gian
    public void CloseUI<T>(float time) where T : UIObject
    {
        T canvas = GetUI<T>();
        if (canvas != null)
        {
            canvas.Close(time);
        }
    }

    // Đóng UI ngay lập tức
    public void CloseUIDirectly<T>() where T :  UIObject
    {
        T canvas = GetUI<T>();
        if (canvas != null)
        {
            canvas.CloseDirectly();
        }
    }

    // Kiểm tra xem một UI có đang mở không
    public bool IsUIOpened<T>() where T : UIObject
    {
        T canvas = GetUI<T>();
        return canvas != null && canvas.gameObject.activeSelf;
    }

    // Lấy một UI cụ thể từ danh sách
    public T GetUI<T>() where T : UIObject
    {
        return UIObjects.Find(c => c is T) as T;
    }

    // Kích hoạt một UI cụ thể
    public void ActiveUI<T>() where T : UIObject
    {
        T canvas = GetUI<T>();
        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
        }
    }

    
    // Đóng tất cả các UI đang mở
    public void CloseAll()
    {
        foreach (var canvas in UIObjects)
        {
            if (canvas.gameObject.activeSelf)
            {
                canvas.Close(0);
            }
        }
    }

}
