using System.Collections;
using UnityEngine;

// Short rotation wobble used as "can't do that" feedback.
// Rotates instead of moving so it doesn't fight layout groups.
public class ShakeFeedback : MonoBehaviour
{
    [SerializeField] private float _duration = 0.3f;

    [SerializeField] private float _angle = 12f;

    [SerializeField] private float _frequency = 40f;

    private Coroutine coroutine;

    private void OnDisable()
    {
        transform.localRotation = Quaternion.identity;
        coroutine = null;
    }

    public static void Play(Component target)
    {
        var shake = target.GetComponent<ShakeFeedback>();
        if (shake == null)
            shake = target.gameObject.AddComponent<ShakeFeedback>();

        shake.Play();
    }

    public void Play()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        var elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.unscaledDeltaTime;
            var damping = 1f - elapsed / _duration;
            var z = Mathf.Sin(elapsed * _frequency) * _angle * damping;
            transform.localRotation = Quaternion.Euler(0, 0, z);
            yield return null;
        }

        transform.localRotation = Quaternion.identity;
        coroutine = null;
    }
}