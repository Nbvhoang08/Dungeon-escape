using UnityEngine;

public class LockDoor : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            if(col.gameObject.GetComponent<Player>().HasKey)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
