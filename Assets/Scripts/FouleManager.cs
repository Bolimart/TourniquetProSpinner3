using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FouleManager : MonoBehaviour
{

    public float spawnRadius = 30f;
    public Vector3 spawnCenter;
    public SpinAroundPoint spinAroundPoint;
    
    [SerializeField] private GameObject[] crowdPrefabs;

    [SerializeField] private GameEvent _gameEvent;

    [SerializeField] private Transform playerTransform; 
    
    void OnEnable()
    {
        _gameEvent.on100Score += AddCrowd;
    }

    void OnDisable()
    {
        _gameEvent.on100Score -= AddCrowd;
    }

    void AddCrowd()
    {
        Func<float, Vector3> circleFunc = k => new Vector3(spawnCenter.x + spawnRadius * math.cos(k), spawnCenter.y, spawnCenter.z + spawnRadius * math.sin(k));
        
        float k = Random.Range(0f, 2f * Mathf.PI);
        var crowd = Instantiate(crowdPrefabs[Random.Range(0, crowdPrefabs.Length)], circleFunc(k), Quaternion.identity);
        crowd.transform.LookAt(playerTransform);
        spinAroundPoint.AddObjects(crowd);
    }
}
