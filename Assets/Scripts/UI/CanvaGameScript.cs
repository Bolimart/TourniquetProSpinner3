using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CanvaGameScript : MonoBehaviour
{
    // ———— Fields ————

    public GameObject photoPrefab;
    public Sprite[] photos;
    
    public GameObject pointsPrefab; // TODO: Test feature : remove later

    public RectTransform textPointZone;
    private List<GameObject> _pointsText = new();
    
    
    // ———— Unity events ————
    
    void Start()
    {
        SpawnPhoto(RandomPhoto());
        _pointsText = new();
        
        SpawnPointsText(pointsPrefab);
    }

    void Update()
    {
        
    }
    
    // ———— Methods ————

    public void SpawnPhoto(Sprite sprite)
    {
        GameObject photo = Instantiate(photoPrefab, transform);
        photo.GetComponent<Image>().sprite = sprite;
        photo.GetComponent<ImageScript>().OnCorner += () => SpawnPhoto(RandomPhoto()); 
        SpawnPointsText(pointsPrefab);
    }

    public void SpawnPointsText(GameObject tmp)
    {
        OnNewText?.Invoke();
        
        tmp = Instantiate(tmp, transform);
        Vector3 pos = textPointZone.position;

        tmp.transform.position = pos;
        //_pointsText.Add(tmp);
        var script = tmp.GetComponent<PointTextScript>();
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
