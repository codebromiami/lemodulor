using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using deVoid.Utils;

public class CreateScreen : MonoBehaviour {

	/// <summary>
    /// The rotation in degrees need to apply to model when the Andy model is placed.
    /// </summary>
    private const float k_ModelRotation = 180.0f;
	/// <summary>
    /// A model to place when a raycast from a user touch hits a plane.
    /// </summary>
    public GameObject cubeGeneratorPrefab;
	/// <summary>
	/// The last anchor tracked
	/// </summary>
	private Anchor lastAnchor;
	/// <summary>
	/// The last prefab created
	/// </summary>
	private GameObject lastGo;

	private void OnEnable() {
		Signals.Get<AnchorCreated>().AddListener(onAnchorCreated);
	}

	public void onAnchorCreated(Anchor anchor, TrackableHit hit){

		if(lastAnchor)
			Destroy(lastAnchor);
		lastAnchor = anchor;
		if(lastGo)
            Destroy(lastGo);

		// Choose the Andy model for the Trackable that got hit.
		GameObject prefab = cubeGeneratorPrefab;
		
		// Instantiate GameObject at the hit pose.
		lastGo = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);

		// Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
		lastGo.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

		// Make GameObject a child of the anchor, we test here because our test script can't put a transform in the anchor
		if(anchor)
			lastGo.transform.parent = anchor.transform;
		else
			lastGo.transform.parent = this.transform;
	}

	private void OnDisable() {
		Signals.Get<AnchorCreated>().RemoveListener(onAnchorCreated);
	}
}
