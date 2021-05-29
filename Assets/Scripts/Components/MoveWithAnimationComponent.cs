using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveWithAnimationComponent : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] private float moveDelay;
    [SerializeField] private PathView pathView;
    [SerializeField, AnimatorParam("GetAnimator", AnimatorControllerParameterType.Trigger)] private int movingAnimation;
    [SerializeField, AnimatorParam("GetAnimator", AnimatorControllerParameterType.Trigger)] private int haltAnimation;

    [Header("References")]
    [SerializeField] private MoverComponent mover;
    [SerializeField] private Animator[] animators;

    [Space()]
    [SerializeField] private UnityEvent OnPathEnd;

    public void StartMoving() {
        CoroutineHelper.Delay(moveDelay, FollowPath);
        CoroutineHelper.Delay(() => pathView.Trs.DistanceRatio >= 1, PathEnd);
    }

    private void FollowPath() {
        animators.Foreach(x => x.SetTrigger(movingAnimation));
        mover.SetPath(pathView.Path);
        mover.EnableMovement(true);
    }

    private void PathEnd() {
        animators.Foreach(x => x.SetTrigger(haltAnimation));
        OnPathEnd?.Invoke();
    }

    private Animator GetAnimator() {
        return animators[0];
    }

}