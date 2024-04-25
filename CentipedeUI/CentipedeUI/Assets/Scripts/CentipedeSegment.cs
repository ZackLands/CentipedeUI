using UnityEngine;

public class CentipedeSegment : MonoBehaviour
{
    [SerializeField]
    Sprite _headSprite;

    [SerializeField]
    Sprite _bodySprite;

    SpriteRenderer _renderer;
    Collider2D _collider;
    int _layerMask;
    Centipede _centipede;

    public CentipedeSegment Ahead{ get; set; }
    public CentipedeSegment Behind { get; set; }
    public bool IsHead => Ahead == null;

    Vector3 _direction;
    Vector3 _targetPosition;

    public void Init(Centipede centipede)
    {
        _centipede = centipede;
        _direction = Vector3.right * centipede.ObjectsDistance + Vector3.down * centipede.ObjectsDistance;
        _targetPosition = transform.position;
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _layerMask = LayerMask.NameToLayer("Mushroom");
    }

    void Update()
    {
        Sprite sprite = _bodySprite;
        if (IsHead)
        {
            sprite = _headSprite;
        }

        if (_renderer.sprite != sprite)
        {
            _renderer.sprite = sprite;
        }

        if (IsHead && Vector3.Distance(transform.position, _targetPosition) < 0.1f)
        {
            UpdateHeadSegment();
        }

        Vector3 currentPosition = transform.position;
        float speed = _centipede.Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(currentPosition, _targetPosition, speed);

        Vector3 movementDirection = (_targetPosition - currentPosition).normalized;
        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void UpdateHeadSegment()
    {
        Vector3 gridPosition = GridPosition(transform.position);

        _targetPosition = gridPosition;
        _targetPosition.x += _direction.x;

        var extents = _collider.bounds.extents * 0.5f;
        var hitCollider = Physics2D.OverlapBox(_targetPosition, extents, 0f, _layerMask);
        var bounds = _centipede.Bounds;
        if (hitCollider != null || _targetPosition.x < bounds.xMin || _targetPosition.x > bounds.xMax)
        {
            BounceAndDrop();
        }

        if (Behind != null)
        {
            Behind.UpdateBodySegment();
        }
    }

    void BounceAndDrop()
    {
        // bounce
        _direction.x = -_direction.x;
        _targetPosition.x = GridPosition(transform.position).x;

        // drop
        Vector3 gridPosition = GridPosition(transform.position);
        _targetPosition.y = gridPosition.y + _direction.y;
        var bounds = _centipede.Bounds;
        if (_targetPosition.y < bounds.yMin || _targetPosition.y > bounds.yMax)
        {
            _direction.y = -_direction.y;
        }
    }
    
    void UpdateBodySegment()
    {
        _targetPosition = GridPosition(Ahead.transform.position);
        _direction = Ahead._direction;

        if (Behind != null)
        {
            Behind.UpdateBodySegment();
        }
    }

    Vector3 GridPosition(Vector3 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }

    public void Remove()
    {
        _centipede.Remove(this);
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (IsHead && other.CompareTag("Mushroom"))
        {
            BounceAndDrop();
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            Remove();
        }

        if (other.gameObject.CompareTag("Player"))
        {
            GameEvents.InvokeGameOver();
        }
    }
}