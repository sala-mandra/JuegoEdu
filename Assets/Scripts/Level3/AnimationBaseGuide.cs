using System.Collections;
using UnityEngine;

public class AnimationBaseGuide : MonoBehaviour
{
    [SerializeField] private Material _materialBaseGuide;
    [SerializeField] private float _speed = 0.8f;

    private int _countChangeAlpha;

    private void Start()
    {
        StartCoroutine(StartAnimationBaseGuide());
    }

    private IEnumerator StartAnimationBaseGuide()
    {
        var alpha = 0f;
        var direction = 1f;

        while (true)
        {
            alpha += direction * _speed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            var currentColor = _materialBaseGuide.color;
            currentColor.a = alpha;
            _materialBaseGuide.color = currentColor;

            if (alpha >= 1f)
                direction = -1f;
            else if (alpha <= 0f)
                direction = 1f;

            yield return null;
        }
    }
}