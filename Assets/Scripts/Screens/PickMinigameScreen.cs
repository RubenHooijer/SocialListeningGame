using System.Collections.Generic;
using UnityEngine;

public class PickMinigameScreen : AbstractScreen<PickMinigameScreen> {

    [SerializeField] private VoidEventChannelSO onEndPickMinigame;

    [SerializeField] private List<AnimatedButton> pickingStones;
    [SerializeField] private int correctIndex;
    [SerializeField] private AnimatedWidget[] animatedWidgets;

    protected override void OnShow() {
        for (int i = 0; i < pickingStones.Count; i++) {
            pickingStones[i].OnClickReferenced += OnStoneClicked;
        }
        gameObject.SetActive(true);
    }

    protected override void OnHide() {
        for (int i = 0; i < pickingStones.Count; i++) {
            pickingStones[i].OnClickReferenced -= OnStoneClicked;
        }
        animatedWidgets.Foreach(x => x.Hide());
        CoroutineHelper.Delay(2, () => gameObject.SetActive(false));
    }

    private void OnStoneClicked(AnimatedButton stone) {
        if (!pickingStones.Contains(stone)) { return; }

        int stoneIndex = pickingStones.IndexOf(stone);
        if (stoneIndex == correctIndex) {
            OnCorrectStoneClicked();
        }
    }

    private void OnCorrectStoneClicked() {
        onEndPickMinigame.Raise();
        Hide();
    }

}