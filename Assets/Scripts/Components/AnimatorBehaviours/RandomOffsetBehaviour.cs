using NaughtyAttributes;
using UnityEngine;

public class RandomOffsetBehaviour : StateMachineBehaviour {

    [SerializeField, MinMaxSlider(0, 1)] private Vector2 minMaxRandomOffset;

    private string previousClipName;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        string clipname = animator.GetNextAnimatorClipInfo(layerIndex)[0].clip.name;

        if (clipname == previousClipName) { return; }
        previousClipName = clipname;

        animator.Play(clipname, layerIndex, Random.Range(minMaxRandomOffset.x, minMaxRandomOffset.y));
    }

}