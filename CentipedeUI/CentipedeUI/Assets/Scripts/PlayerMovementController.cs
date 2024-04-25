using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    const float _moveSpeed = 4f;
    const float _screenMargin = 0.02f;

    Collider2D _collider;
    Rect _bounds;
    int _layerMask;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _layerMask = LayerMask.GetMask("Mushroom");

        // get screen bounds
        var bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(_screenMargin, _screenMargin, Camera.main.nearClipPlane));
        var topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f - _screenMargin, 1f - _screenMargin, Camera.main.nearClipPlane));
        _bounds = new Rect(bottomLeft, topRight - bottomLeft);
    }

    void Update()
    {
        var inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputDirection = Vector2.ClampMagnitude(inputDirection, 1f);

        var newPosition = transform.position + (Vector3)(inputDirection * _moveSpeed);

        var extents = _collider.bounds.extents * 0.5f;
        var hitCollider = Physics2D.OverlapBox(newPosition, extents, 0f, _layerMask);
        if (hitCollider != null)
        {
            newPosition = transform.position;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, _bounds.xMin, _bounds.xMax);
        newPosition.y = Mathf.Clamp(newPosition.y, _bounds.yMin, _bounds.yMax);

        transform.position = newPosition;
    }
}