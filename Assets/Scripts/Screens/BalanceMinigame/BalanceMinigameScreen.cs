using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceMinigameScreen : AbstractScreen<BalanceMinigameScreen> {

    [SerializeField] private string[] platformGuids;

    public void StartBalancePlatform(string guid) {
        PlatformView platformView = PlatformView.GetView(guid);


    }

    protected override void OnShow() {
        gameObject.SetActive(true);
    }

    protected override void OnHide() {
        gameObject.SetActive(false);
    }

}