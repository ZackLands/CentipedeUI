using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Centipede : MonoBehaviour
{
    [SerializeField, Header("Parts")]
    CentipedeSegment head;

    [SerializeField, Header("Scoring")]
    int pointsHead = 100;

    [SerializeField]
    int pointsBody = 20;

    public Rect Bounds { get; private set; }
    public float ObjectsDistance { get; private set; } = 10f;
    public int Size { get; private set; }
    public float Speed { get; private set; }

    CentipedeManager _centipedeManager;
    List<CentipedeSegment> _segments = new();

    const float _screenMargin = 0.05f;

    public void Init(CentipedeManager centipedeManager, int numSegments, float speed)
    {
        // config:
        _centipedeManager = centipedeManager;
        Size = numSegments;
        Speed = speed;

        // get screen bounds
        var bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(_screenMargin, _screenMargin, Camera.main.nearClipPlane));
        var topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f - _screenMargin, 1f - _screenMargin, Camera.main.nearClipPlane));
        Bounds = new Rect(bottomLeft, topRight - bottomLeft);

        // spawn centipede:
        var headPosition = GridPosition(GetRandomStartingPosition());

        head = _centipedeManager.SpawnSegment();
        head.transform.position = headPosition;
        head.Init(this);
        _segments.Add(head);

        for (int i = 1; i < numSegments; i++)
        {
            var segment = _centipedeManager.SpawnSegment();
            segment.transform.position = headPosition + (Vector3.left * i * ObjectsDistance);
            segment.Init(this);
            _segments.Add(segment);
        }

        for (int i = 0; i < _segments.Count; i++)
        {
            CentipedeSegment segment = _segments[i];
            segment.Ahead = GetSegmentAt(i - 1);
            segment.Behind = GetSegmentAt(i + 1);
        }

        gameObject.SetActive(true);
        for (int i = 0; i < numSegments; i++)
        {
            _segments[i].gameObject.SetActive(true);
        }
    }
    
    Vector3 GetRandomStartingPosition()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.05f, 0.95f), Random.Range(0.7f, 0.95f), 0f));
    }

    CentipedeSegment GetSegmentAt(int index)
    {
        if (index >= 0 && index < _segments.Count)
        {
            return _segments[index];
        }
        else
        {
            return null;
        }
    }

    public void Remove(CentipedeSegment segment)
    {
        int points = segment.IsHead ? pointsHead : pointsBody;
        GameEvents.InvokeIncreaseScore(points);

        GameEvents.InvokeCentipedeHitEvent(GridPosition(segment.transform.position));

        if (segment.Ahead != null)
        {
            segment.Ahead.Behind = null;
        }

        if (segment.Behind != null)
        {
            segment.Behind.Ahead = null;
            segment.Behind.UpdateHeadSegment();
        }

        _segments.Remove(segment);
        Destroy(segment.gameObject);

        _centipedeManager.NumSegmentsRemainingInGame--;
        if (_centipedeManager.NumSegmentsRemainingInGame == 0)
        {
            GameEvents.InvokeNextLevel();
        }
    }

    Vector3 GridPosition(Vector3 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        position.z = 0f;
        return position;
    }
}