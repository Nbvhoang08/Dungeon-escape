using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
     [Header("Movement Settings")]
    public float moveSpeed = 2f; // Tốc độ di chuyển của enemy
    public float moveRadius = 5f; // Bán kính di chuyển tối đa từ điểm ban đầu

    private Vector3 _startPosition; // Vị trí ban đầu của enemy
    private int _direction = 1; // Hướng di chuyển (1 = phải, -1 = trái)
    private SpriteRenderer _spriteRenderer; // SpriteRenderer của enemy

    void Start()
    {
        // Lưu vị trí ban đầu của enemy
        _startPosition = transform.position;

        // Lấy component SpriteRenderer
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the enemy object.");
        }
    }

    void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        // Tính toán vị trí mới của enemy
        float newPositionX = transform.position.x + _direction * moveSpeed * Time.deltaTime;

        // Kiểm tra nếu enemy vượt ra ngoài bán kính cho phép
        if (Mathf.Abs(newPositionX - _startPosition.x) > moveRadius)
        {
            // Đảo chiều di chuyển
            _direction *= -1;
            newPositionX = transform.position.x + _direction * moveSpeed * Time.deltaTime;

            // Lật hướng của SpriteRenderer
            FlipSprite();
        }

        // Cập nhật vị trí của enemy
        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
    }

    private void FlipSprite()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.flipX = _direction < 0;
        }
    }
}
