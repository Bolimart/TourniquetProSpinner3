using UnityEngine;

public class ScatterAround : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int count = 50;
    [SerializeField] private float innerRadius = 5f;
    [SerializeField] private float outerRadius = 10f;
    [SerializeField] private float height = 0f;
    [SerializeField] private float minSize = 0.5f;
    [SerializeField] private float maxSize = 2f;
    [SerializeField] private int seed = 42;
    public SpinAroundPoint spinAroundPoint;

    void Start()
    {
        Random.InitState(seed);

        for (int i = 0; i < count; i++)
        {
            // Distribution uniforme sur un anneau (évite la concentration au centre)
            float t = (i + 0.5f) / count;
            float r = Mathf.Sqrt(Mathf.Lerp(innerRadius * innerRadius, outerRadius * outerRadius, t));
            float theta = i * Mathf.PI * (1f + Mathf.Sqrt(5f)); // spirale de Fibonacci

            Vector3 pos = new Vector3(
                Mathf.Cos(theta) * r,
                height,
                Mathf.Sin(theta) * r
            );

            GameObject go = new GameObject($"Sprite_{i}");
            go.transform.position = pos;
            spinAroundPoint.AddObjects(go);

            // Regarde l'origine sur le plan horizontal
            Vector3 dirToOrigin = (Vector3.zero - pos);
            dirToOrigin.y = 0f;
            go.transform.rotation = Quaternion.LookRotation(dirToOrigin);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprites[Random.Range(0, sprites.Length)];

            float size = Random.Range(minSize, maxSize);
            go.transform.localScale = Vector3.one * size;

            go.transform.SetParent(transform);
        }
    }
}
