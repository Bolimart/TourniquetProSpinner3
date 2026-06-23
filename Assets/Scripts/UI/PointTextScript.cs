using System.Collections;
using TMPro;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class PointTextScript : MonoBehaviour
{
    // ———— Fields ————
    
    public float multiplier = 0f;
    public int points = 0;
    public string text;
    public Color textColor;
    
    public float lifespan = 1f;
    public float fadeOutTime = 0.5f;

    public float downDisplacement = 100f;
    public float downSpeed = 0.15f;
    public AnimationCurve downCurve;

    private string _completeText;
    private float _lifespanTimer = 0;
    private bool _isFadingOut = false;
    private TextMeshProUGUI _tmp;

    private Animator _animator;
    
    // ———— Unity Events ————
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        _tmp = GetComponent<TextMeshProUGUI>();
        if (_tmp == null) Debug.LogError("TextMeshPro manquant sur " + gameObject.name);
        string pointsText = "";
        string multiplierText = "";
        if (points != 0) pointsText = $"\n +{points}pts ";
        if (multiplier != 0) multiplierText = $"+{multiplier}bpm";
        _completeText = text + pointsText + multiplierText;
        _tmp.text = _completeText;
        _tmp.color = textColor;
    }

    
    void Update()
    {
        if (!_isFadingOut && _lifespanTimer <= lifespan)
        {
            _lifespanTimer += Time.unscaledDeltaTime;
        }
        else if (_lifespanTimer > lifespan) // Should do this only once
        {
            _lifespanTimer = 0f;
            _isFadingOut = true;
            FadeOut();
        }
    }
    
    // ———— Methods ————
    
    public void FadeOut() => _animator.SetTrigger("FadeOut");
    
    public void GoDown() => StartCoroutine(GoDownCoroutine());
    private IEnumerator GoDownCoroutine()
    {
        var start = transform.position;
        var destination = transform.position - new Vector3(0, downDisplacement, 0);
        var timer = 0f;
    
        while (timer < downSpeed)
        {
            timer += Time.unscaledDeltaTime;
            var t = downCurve.Evaluate(timer / downSpeed);
            //print(t);
            transform.position = Vector3.Lerp(start, destination, t);
            yield return null;
        }
    
        transform.position = destination;
    }
    
    public string GetText() => _completeText;
}
