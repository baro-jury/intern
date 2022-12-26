using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FuckingAds : MonoBehaviour
{
    public void showInter()
    {
        ACEPlay.Bridge.BridgeController.instance.ShowIntersitialAd(null);
    }

    public void showReward()
    {
        UnityEvent isDone = new UnityEvent();
        isDone.AddListener(() =>
        {
            // xu ly phan thuong

        });

        ACEPlay.Bridge.BridgeController.instance.ShowRewardedAd(isDone, null);
    }

    public void buyInapp(string id)
    {
        // remove ads: "com.vicenter.drawbridge.removeads"
        UnityStringEvent isDone = new UnityStringEvent();
        isDone.AddListener((result) =>
        {
            // xu ly phan thuong

        });

        ACEPlay.Bridge.BridgeController.instance.PurchaseProduct(id, isDone);
    }
}
