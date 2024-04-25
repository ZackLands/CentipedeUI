using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    List<Sprite> mushroomSprites;

    [SerializeField]
    int _pointValue = 1;

    SpriteRenderer _renderer;
    int state = 0;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void ChangeState()
    {
        GameEvents.InvokeIncreaseScore(_pointValue);

        if (state < mushroomSprites.Count - 1)
        {
            state++;
            _renderer.sprite = mushroomSprites[state];
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            ChangeState();
        }
    }
}