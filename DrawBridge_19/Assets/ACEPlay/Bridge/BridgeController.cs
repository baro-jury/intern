using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ACEPlay.Bridge
{
    public class BridgeController : MonoBehaviour
    {
        public static BridgeController instance;
        public System.Action ACTION_SHOW_NATIVE;
        public System.Action ACTION_HIDE_NATIVE;
        public System.Action<int> ACTION_LOAD_NATIVE;
        public bool IsVipComplete
        {
            get
            {
                return PlayerPrefs.GetInt("IsVip", 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt("IsVip", value ? 1 : 0);
            }
        }

        public bool CanShowAds //true: show ads, false: not show ads
        {
            get
            {
                return PlayerPrefs.GetInt("CanShowAds", 1) == 1;
            }
            set
            {
                PlayerPrefs.SetInt("CanShowAds", value ? 1 : 0);
            }
        }
        public bool CanShowAdsWithVip //true: show ads, false: not show ads
        {
            get
            {
                return PlayerPrefs.GetInt("CanShowAdsWithVip", 1) == 1;
            }
            set
            {
                PlayerPrefs.SetInt("CanShowAdsWithVip", value ? 1 : 0);
            }
        }
        public bool IsFreeContinue //true: free continue, false: not free
        {
            get
            {
                return PlayerPrefs.GetInt("IsFreeContinue", 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt("IsFreeContinue", value ? 1 : 0);
            }
        }

        public bool IsShowVipAtStart
        {
            get
            {
                return PlayerPrefs.GetInt("IsShowVipAtStart", 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt("IsShowVipAtStart", value ? 1 : 0);
            }
        }

        public bool IsBuyProduct
        {
            get
            {
                return PlayerPrefs.GetInt("IsBuyProduct", 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt("IsBuyProduct", value ? 1 : 0);
            }
        }
        public string DataGamePlay
        {
            get
            {
                return PlayerPrefs.GetString("DataGamePlay", "");
            }
            set
            {
                PlayerPrefs.SetString("DataGamePlay", value);
            }
        }

        public List<string> NonConsumableList = new List<string>();
        public List<string> VipPackageList = new List<string>();
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else Destroy(this.gameObject);
        }

        private void Start()
        {

        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                PlayerPrefs.DeleteAll();
            }
        }
        public void AddNonConsumableLists(string productSku)
        {
            if (!NonConsumableList.Contains(productSku))
            {
                NonConsumableList.Add(productSku);
            }
        }
        public void AddVipPackageLists(string productSku)
        {
            if (!VipPackageList.Contains(productSku))
            {
                VipPackageList.Add(productSku);
            }
        }
        public void ShowBeginPack()
        {
            //if (IsShowVipAtStart )
            //{

            //}
            //else
            //    return;
        }
        public void ShowBannerAd()
        {
            Debug.Log("=====Banner Show success!=====");
        }
        public void HideBannerAD()
        {
            Debug.Log("=====Banner Hide success!=====");
        }

        public bool ShowIntersitialAd(UnityEvent onClosed)
        {
            Debug.Log("=====Intersitial Show success!=====");
            if (onClosed != null) onClosed.Invoke();
            return true;
        }

        public bool CheckShowInter()
        {
            return true;
        }

        public bool CheckTimeShowInter()
        {
            return true;
        }

        public bool ShowRewardedAd(UnityEvent onRewarded, UnityEvent onClosed)
        {
            Debug.Log("=====Rewarded Show success!=====");
            if (onRewarded != null) onRewarded.Invoke();
            return true;
        }

        public bool ShowIntersitialRewardedAd(UnityEvent onRewarded, UnityEvent onClosed)
        {
            Debug.Log("=====IntersitialRewarded Show success!=====");
            if (onRewarded != null) onRewarded.Invoke();
            return true;
        }

        public void ShowNative()
        {
            Debug.Log("NATIVE -----x- show");
        }
        public void HideNative()
        {
            Debug.Log("NATIVE -----x- hide");
        }
        public void LoadNative(int id = 0)
        {
            Debug.Log("NATIVE -----x- load" + id);
        }
        public void PurchaseProduct(string productSku, UnityStringEvent onDonePurchaseEvent)
        {
            Debug.Log("=====Purchase product success!=====");
            if (onDonePurchaseEvent != null) onDonePurchaseEvent.Invoke(productSku);
        }

        public void RestorePurchase()
        {
            Debug.Log("=====Restore Purchase success!=====");
        }

        public void RateGame(UnityEvent onRateRewarded)
        {
            Debug.Log("=====Rate Success=====");
            if (onRateRewarded != null) onRateRewarded.Invoke();
        }

        public void ShareGame(UnityEvent onShareRewarded)
        {
            Debug.Log("=====Share Success=====");
            if (onShareRewarded != null) onShareRewarded.Invoke();
        }

        public void PublishHighScore(int score)
        {
            Debug.Log("=====Publish Score:" + score + "Success=====");
        }
        //hien thi ban xep hang
        public void ShowLeaderBoard()
        {
            Debug.Log("=====Show Leaderboard success!=====");
        }

        public void ShowFacebook(UnityEvent onOpenFBRewarded)
        {
            Debug.Log("=====Show Facebook Success=====");
            if (onOpenFBRewarded != null) onOpenFBRewarded.Invoke();
        }

        public void ShowInstagram(UnityEvent onInstaRewarded)
        {
            Debug.Log("=====Show Instagram Success=====");
            if (onInstaRewarded != null) onInstaRewarded.Invoke();
        }

        public void ShowTwitter(UnityEvent onTwitterRewarded)
        {
            Debug.Log("=====Show Twitter Success=====");
            if (onTwitterRewarded != null) onTwitterRewarded.Invoke();
        }
        public void Moregames()
        {
            Debug.Log("=====Show Moregames success!=====");
        }
        public void CheckTutComplete(bool value = false)
        {
            Debug.Log("=====Tuturial success!=====");
        }
        public void TrackingDataGame(string index)
        {
            //Debug.Log("=====TrackingDataGame success!=====");
        }
        public void TrackingDataGame(string eventName, Parameter[] parameterTracking)
        {
            Debug.Log("=====TrackingDataGame success!=====");
        }

        public void UserPropertyData(string properties, string value)
        {
            Debug.Log(string.Format("=====Set User Property: {0} Data: {1}===== ", properties, value));
        }
        public void ShowWebsite()
        {
            Debug.Log("=====Show Website success!=====");
        }
        public bool CheckShowInterReward()
        {
            return true;
        }
    }
}

public class Parameter
{
    public string key = "";
    public string string_value = "";
    public int int_value = -1;
    public float float_value = -1;

    public Parameter(string key, string value)
    {
        this.key = key;
        this.string_value = value;
    }
    public Parameter(string key, int value)
    {
        this.key = key;
        this.int_value = value;
    }
    public Parameter(string key, float value)
    {
        this.key = key;
        this.float_value = value;
    }
}

public class UnityStringEvent : UnityEvent<string>
{
}


