using System.Collections;
using UnityEngine;

// Short rotation wobble used as "can't do that" feedback.
// Rotates instead of moving so it doesn't fight layout groups.
public class ShakeFeedback : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.3f;

    [SerializeField]
    private float angle = 12f;

    [SerializeField]
    private float frequency = 40f;

    private Coroutine _coroutine;

    public static void Play(Component target)
    {
        var shake = target.GetComponent<ShakeFeedback>();
        if (shake == null)
            shake = target.gameObject.AddComponent<ShakeFeedback>();

        shake.Play();
    }

    public void Play()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            var damping = 1f - elapsed / duration;
            var z = Mathf.Sin(elapsed * frequency) * angle * damping;
            transform.localRotation = Quaternion.Euler(0, 0, z);
            yield return null;
        }

        transform.localRotation = Quaternion.identity;
        _coroutine = null;
    }

    private void OnDisable()
    {
        transform.localRotation = Quaternion.identity;
        _coroutine = null;
    }
}
