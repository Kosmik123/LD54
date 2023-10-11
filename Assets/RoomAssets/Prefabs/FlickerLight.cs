using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
	Light myLight;

	public float maxIntensity = 12.0f;
	public float minIntensity = 8.0f;

	void Start()
	{
		myLight = GetComponent<Light>();
		StartCoroutine(Flicker());
	}

	IEnumerator Flicker()
	{

		float t = 0.0f;
		float duration = Random.Range(0.02f, 0.3f);
		float currIntensity = myLight.intensity;
		float targetIntensity = (currIntensity > 2.0f) ? minIntensity : maxIntensity;
		float variation = Random.Range(-1.0f, 5.0f);
		targetIntensity += variation;

		while (t < duration)
                {
			myLight.intensity = Mathf.Lerp(currIntensity, targetIntensity, t / duration);
			t += Time.deltaTime;
			yield return null;
                }

		StartCoroutine(Flicker());
	}
}