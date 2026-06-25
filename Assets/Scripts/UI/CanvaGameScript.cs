using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CanvaGameScript : MonoBehaviour
{
    // ———— Fields ————

    public GameObject photoPrefab;
    public Sprite[] photos;
    
    public GameObject pointsPrefab;

    public RectTransform textPointZone;
    private List<GameObject> _pointsText = new();
    private PointTextScript _prefabScript;
    
    
    // ———— Unity events ————
    
    void Start()
    {
        _pointsText = new();
        _prefabScript = pointsPrefab.GetComponent<PointTextScript>();
    }

    void Update()
    {
        
    }
    
    // ———— Methods ————

    public void SpawnPhoto(Sprite sprite)
    {
        GameObject photo = Instantiate(photoPrefab, transform);
        photo.GetComponent<Image>().sprite = sprite;
    }

    public void SpawnPointsText(int points, float multiplier, string text)
    {
        
        OnNewText?.Invoke();
        
        
        _prefabScript.points = points;
        _prefabScript.multiplier = multiplier;
        _prefabScript.text = text;
        var pointText = Instantiate(pointsPrefab, transform);
        Vector3 pos = textPointZone.position;

        pointText.transform.position = pos;
        _pointsText.Add(pointText);
        var script = pointText.GetComponent<PointTextScript>();
        OnNewText += script.GoDown;

    }

    public void RemoveLastPointsText()
    {
        OnNewText -= _pointsText[^1].GetComponent<PointTextScript>().GoDown;
        Destroy(_pointsText[^1]);
        _pointsText.RemoveAt(_pointsText.Count - 1);
    }
    
    public Sprite RandomPhoto() => photos[Random.Range(0, photos.Length)];

    public event System.Action OnNewText;
}
