using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RadialBlurControl : MonoBehaviour
{
    [Range(1, 4)]
    public int iteration = 1;
    public AnimationCurve blurStrengthCurve;
    public AnimationCurve blurWidthCurve;

    public float strength = 500.0f;
    public float width = 0.5f;

    public float timeStart = 0.0f;
    public float duration = 1.0f;

    private RadialBlur _radialBlur;

    // Use this for initialization
    void OnEnable ()
    {
        // Disable if we don't support image effects
        if (!SystemInfo.supportsImageEffects) {
            enabled = false;
            return;
        }

        _radialBlur = Camera.main.GetComponent<RadialBlur>();
        _radialBlur.enabled = false;
        StartCoroutine(RadialBlurCo(timeStart, duration, width, strength));
    }

    IEnumerator RadialBlurCo(float delay, float duration, float width, float strength)
    {
        yield return new WaitForSeconds(delay);
        var effect = _radialBlur;
        if (effect == null)
            yield break;
        effect.enabled = true;

        for (float t = 0; t < 1 && effect != null; t += Time.deltaTime / duration) {
            effect.blurWidth = width * blurWidthCurve.Evaluate(t);
            effect.blurStrength = strength * blurStrengthCurve.Evaluate(t);
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            effect.center = new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
            yield return null;
        }

        if (effect != null) {
            effect.enabled = false;
        }
    }
}