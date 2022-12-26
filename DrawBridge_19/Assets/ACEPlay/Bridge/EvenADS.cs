using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenADS : MonoBehaviour
{
    public static EvenADS instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(this);
    }

    #region Event phục vụ chạy ADS
    int num_PlayGame = 0;
    //Even_PlayGame dùng để check số lần chơi game trong mỗi phiên
    //valueCompare giá trị so sánh
    public void Even_PlayGame(int valueCompare1, int valueCompare2, int valueCompare3)
    {
        num_PlayGame++;
        if (num_PlayGame == valueCompare1 || num_PlayGame == valueCompare2 || num_PlayGame == valueCompare3)
        {
            ACEPlay.Bridge.BridgeController.instance.TrackingDataGame(string.Format("session_play_{0}", num_PlayGame));
        }
    }


    int num_playWin = 0;
    //Even_PlayGameWin dùng để check số lần chơi thắng trong mỗi phiên
    //valueCompare.Item1: D (D2, D5)
    //valueCompare.Item2: value (5, 10)
    public void Even_PlayGameWin((int, int) valueCompare1, (int, int) valueCompare2)
    {
        num_playWin = PlayerPrefs.GetInt("countWin", 0);
        num_playWin++;
        PlayerPrefs.SetInt("countWin", num_playWin);
        int D = PlayerPrefs.GetInt("Day", 0);
        //if (num_playWin == valueCompare1.Item2 || num_playWin == valueCompare2.Item2)
        //{
        //    ACEPlay.Bridge.BridgeController.instance.TrackingDataGame(string.Format("session_playwin_{0}", num_playWin));
        //}
        if ((num_playWin == valueCompare1.Item2 && D <= valueCompare1.Item1) || (num_playWin == valueCompare2.Item2 && D <= valueCompare2.Item1))
        {
            ACEPlay.Bridge.BridgeController.instance.TrackingDataGame(string.Format("session_playwin_{0}_in_d{1}", num_playWin, D));
        }
    }

    bool isSessionStart = false;

    //int numClick = 0;//số lần player tương tác trong game (click vào bất kỳ tính năng nào trong game)
    //Even_SessionStart dùng để check xem mỗi lần mở app player có thao tác gì trong game k hay chỉ mở rồi để đó
    //valueCompare.Item1: D (D2, D5)
    //valueCompare.Item2: value (5, 10)
    public void Even_SessionStart((int, int) valueCompare1, (int, int) valueCompare2)
    {
        if (isSessionStart) return;
        isSessionStart = true;
        int session = PlayerPrefs.GetInt("session_start", 0);
        session++;
        PlayerPrefs.SetInt("session_start", session);

        int D = PlayerPrefs.GetInt("Day", 0);
        if (session == valueCompare1.Item2 || session == valueCompare2.Item2)
        {
            ACEPlay.Bridge.BridgeController.instance.TrackingDataGame(string.Format("session_start_{0}", session));
        }
        if ((session == valueCompare1.Item2 && D <= valueCompare1.Item1) || (session == valueCompare2.Item2 && D <= valueCompare2.Item1))
        {
            ACEPlay.Bridge.BridgeController.instance.TrackingDataGame(string.Format("session_start_{0}_in_d{1}", session, D));
        }
    }





    public void Even_OpenApp()
    {
        if (MyTime.instance != null)
        {
            if (!PlayerPrefs.HasKey("OpenAppD1"))
            {
                PlayerPrefs.SetInt("OpenAppD1", -1);
                return;
            }
            else if (MyTime.instance.isFirstLoginNewDay)
            {
                if (PlayerPrefs.GetInt("OpenAppD1") == -1)
                {
                    PlayerPrefs.SetInt("OpenAppD1", 1);
                    ACEPlay.Bridge.BridgeController.instance.TrackingDataGame("OpenAppD1");
                }
                PlayerPrefs.SetInt("Day", PlayerPrefs.GetInt("Day", 0) + 1);
            }
        }
    }
    #endregion
}
