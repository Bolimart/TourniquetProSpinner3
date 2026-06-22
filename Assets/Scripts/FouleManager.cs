using UnityEngine;

public class FouleManager : MonoBehaviour
{
    [SerializeField] private GameObject[] crowdPrefabs;
    [SerializeField] private GameObject Plane;

    [SerializeField] private GameEvent _gameEvent;

    [SerializeField] private Transform playerTransform; 
    void OnEnable()
    {
        _gameEvent.on100Score += addCrowd;
    }

    void OnDisable()
    {
        _gameEvent.on100Score -= addCrowd;
    }

    void addCrowd()
    {
        // Get the bounds of the plane
        Bounds planeBounds = Plane.GetComponent<Renderer>().bounds;

        // Generate a random position within the bounds of the plane
        float randomX = Random.Range(planeBounds.min.x/2, planeBounds.max.x/2);
        float randomZ = Random.Range(planeBounds.min.z/2, planeBounds.max.z/2);
        Vector3 randomPosition = new Vector3(randomX, planeBounds.max.y, randomZ);


        // Instantiate a random crowd prefab at the random position
        int randomIndex = Random.Range(0, crowdPrefabs.Length);
        Quaternion rotationTowardsPlayer = Quaternion.LookRotation(playerTransform.position - randomPosition);
        GameObject crowdInstance = Instantiate(crowdPrefabs[randomIndex], randomPosition, rotationTowardsPlayer);

        int randomScale = Random.Range(1, 3);
        crowdInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}
