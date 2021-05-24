using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverComponent : MonoBehaviour {

	[Header("Settings")]
	[SerializeField] private float speed = 5f;

	//[Header("References")]
	//[SerializeField] private Animator animator;
	//[SerializeField, AnimatorParam("animator", AnimatorControllerParameterType.Bool)] private string walkBool;

	private BGCcTrs pathTRS = null;

	public void SetPath(BGCurve path) {
		if (pathTRS != null) {
			pathTRS.ObjectToManipulate = null;
			pathTRS.Speed = 0;
		}

		if (path == null) { return; }
		if (!path.TryGetComponent(out BGCcTrs newPathTRS)) {
			Debug.LogError($"No TRS found on {path.name}, please add one");
			return; 
		}

		newPathTRS.ObjectToManipulate = transform;
		newPathTRS.MoveObject = false;
		newPathTRS.Speed = 0;
		newPathTRS.DistanceRatio = 0;

		pathTRS = newPathTRS;
	}

	public void SetSpeed(float newSpeed) {
		speed = newSpeed;
		pathTRS.Speed = speed;
	}

	public void EnableMovement(bool isMoving) {
		pathTRS.MoveObject = isMoving;
		pathTRS.Speed = isMoving ? speed : 0;
	}
	
}