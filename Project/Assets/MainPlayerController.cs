using UnityEngine;

public class MainPlayerController : MonoBehaviour
{

	/* ... */

	void Start() { /* ... */ }

	void Update()
	{
		if (OVRInput.Get(OVRInput.Button.One)) Debug.LogWarning("Button is pressed");
	}

	/* ... */
}