using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float shootPower;

    private Rigidbody2D rigidBody;
    private bool isResetted;

    void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        isResetted = false;
        StartCoroutine(Expire());
    }

    IEnumerator Expire()
    {
        yield return new WaitForSeconds(3f);
        if (!isResetted)
        {
            isResetted = true;
            gameObject.SetActive(false);
        }
    }

    public void Shoot(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(Vector3.up * shootPower, ForceMode2D.Impulse);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Mushroom") || other.gameObject.CompareTag("Enemy"))
        {
            isResetted = true;
            gameObject.SetActive(false);
        }
    }
}