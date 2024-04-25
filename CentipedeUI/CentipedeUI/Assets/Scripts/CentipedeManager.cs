using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CentipedeManager : MonoBehaviour
{
    [SerializeField]
    Centipede centipedePrefab;

    [SerializeField]
    CentipedeSegment centipedeSegmentPrefab;

    const int startingSegmentsInGame = 10;
    const float startingSpeedInGame = 70f;
    const int segmentsToAddPerLevel = 3;
    const float speedMultiplierPerLevel = 1.1f;
    const int minSegmentsPerSpawnedCentipede = 5;
    const int maxSegmentsPerSpawnedCentipede = 12;

    int _numSegmentsInGame = 10;
    float _currentSpeed;
    public int NumSegmentsRemainingInGame { get; set; }

    List<Centipede> _spawnedCentipedes = new List<Centipede>();
    Stack<Centipede> _pooledCentipedes = new Stack<Centipede>();
    List<CentipedeSegment> _spawnedSegments = new List<CentipedeSegment>();
    Stack<CentipedeSegment> _pooledSegments = new Stack<CentipedeSegment>();

    public void Awake()
    {
        _currentSpeed = startingSpeedInGame;
        _numSegmentsInGame = startingSegmentsInGame;
    }

    public void SpawnCentipedes()
    {
        NumSegmentsRemainingInGame = _numSegmentsInGame;

        int minCentipedes = Mathf.CeilToInt((float)_numSegmentsInGame / (float)maxSegmentsPerSpawnedCentipede);
        int maxCentipedes = Mathf.FloorToInt((float)_numSegmentsInGame / (float)minSegmentsPerSpawnedCentipede);
        int numCentipedes = Random.Range(minCentipedes, maxCentipedes);

        // divide segments amongst them
        var centipedeLengths = new int[numCentipedes];
        for (int i = 0; i < centipedeLengths.Length; i++)
        {
            centipedeLengths[i] = minSegmentsPerSpawnedCentipede;
        }
        int remainingSegments = _numSegmentsInGame - minSegmentsPerSpawnedCentipede * numCentipedes;
        while (remainingSegments > 0)
        {
            int centipedeIndex = UnityEngine.Random.Range(0, numCentipedes - 1);
            int maxAdditionalSegments = maxSegmentsPerSpawnedCentipede - centipedeLengths[centipedeIndex];
            int clampedAdditionalSegments = Mathf.Min(maxAdditionalSegments, remainingSegments);
            int additionalSegments = UnityEngine.Random.Range(1, clampedAdditionalSegments);
            centipedeLengths[centipedeIndex] += additionalSegments;
            remainingSegments -= additionalSegments;
        }

        // spawn
        for (int i = 0; i < numCentipedes; i++)
        {
            var centipede = SpawnCentipede();

            centipede.Init(this, centipedeLengths[i], _currentSpeed);
        }
    }

    Centipede SpawnCentipede()
    {
        Centipede centipede;

        if (!_pooledCentipedes.TryPop(out centipede))
        {
            centipede = Instantiate(centipedePrefab);
        }

        _spawnedCentipedes.Add(centipede);
        return centipede;
    }

    public CentipedeSegment SpawnSegment()
    {
        CentipedeSegment segment;

        if (!_pooledSegments.TryPop(out segment))
        {
            segment = Instantiate(centipedeSegmentPrefab);
        }

        _spawnedSegments.Add(segment);
        return segment;
    }

    public void UpdateDifficulty()
    {
        _currentSpeed *= speedMultiplierPerLevel;
        _numSegmentsInGame += segmentsToAddPerLevel;
    }
}