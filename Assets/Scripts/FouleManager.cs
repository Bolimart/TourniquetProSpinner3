using System;
using System.Collections.Generic;
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
    private int crowdNumber;

    [SerializeField] private AnimationCurve volumeCurve; // X = 0..1 (ratio foule), Y = 0..1 (volume)
    [SerializeField] private int maxCrowdCount = 20;

    [SerializeField] private AudioSource[] crowdAudioSources;
    [SerializeField] private AudioSource sadAudioSources;

    void OnEnable()
    {
        _gameEvent.on100Score += AddCrowd;
        _gameEvent.onResetTrickCombo += OnTrickStreakEnded;
        _gameEvent.onTrickPerformed += OnTrickPerformed;
    }

    void OnDisable()
    {
        _gameEvent.on100Score -= AddCrowd;
        _gameEvent.onResetTrickCombo -= OnTrickStreakEnded;
        _gameEvent.onTrickPerformed -= OnTrickPerformed;
    }

    void AddCrowd()
    {
        Func<float, Vector3> circleFunc = k => new Vector3(
            spawnCenter.x + spawnRadius * math.cos(k),
            spawnCenter.y,
            spawnCenter.z + spawnRadius * math.sin(k)
        );
        
        print("Adding crowd");
        crowdNumber++;
        float k = Random.Range(0f, 2f * Mathf.PI);
        var crowd = Instantiate(crowdPrefabs[Random.Range(0, crowdPrefabs.Length)], circleFunc(k), Quaternion.identity);
        crowd.transform.LookAt(playerTransform);
        spinAroundPoint.AddObjects(crowd);
        
        AudioSource picked = crowdAudioSources[Random.Range(0, crowdAudioSources.Length)];
        picked.Play();
    }

    void OnTrickPerformed()
    {
        float ratio = Mathf.Clamp01(crowdNumber / maxCrowdCount);
        print(crowdNumber + " / " + maxCrowdCount + " = " + ratio);
        float volume = volumeCurve.Evaluate(ratio);

        foreach (AudioSource source in crowdAudioSources)
        {
            source.volume = volume;
            source.Play();
        }
    }
    
    void OnTrickStreakEnded()
    {
        sadAudioSources.Play();
        print("Streak End");
    }
}