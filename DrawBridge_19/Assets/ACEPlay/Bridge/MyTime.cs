using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class MyIntEvent : UnityEvent<int> { }

public class MyTime : MonoBehaviour
{
    public static MyTime instance;
    #region time online
    public bool isFirstLoginNewDay = false;
    public bool isFirstLoginNewWeek = false;
    public bool isCheckedTimeOnline = false;
    DateTime timeOnline;
    [SerializeField]float deltaTime;
    //string format = "dd-MM-yyyy/HH:mm:ss";
    string format = "dd/MM/yyyy HH:mm:ss";//11/03/2020 14:30:11
    string formatTimeSpan = @"d\d\a\y\,hh\:mm\:ss";
    string formatTimeSpanMS = @"mm\:ss";
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(this);
    }

    private IEnumerator Start()
    {
        if (!PlayerPrefs.HasKey("timeZone"))
        {
            string tz = DateTimeOffset.Now.ToString();
            tz = tz.Remove(0, tz.Length - 6);
            PlayerPrefs.SetString("timeZone", tz);
        }
        bool isAgain = false;
        Again:
        if (isAgain) yield return new WaitForSeconds(1.28f);
        UnityWebRequest myHttpWebRequest = UnityWebRequest.Get("https://www.microsoft.com");
        yield return myHttpWebRequest.SendWebRequest();
        try
        {
            string netTime = myHttpWebRequest.GetResponseHeader("date");
            string timezone = DateTimeOffset.Now.ToString();//3/11/2020 9:55:23 AM +07:00
            deltaTime = Time.realtimeSinceStartup;
            timezone = timezone.Remove(0, timezone.Length - 6);//+07:00
            timeOnline = DateTime.Parse(netTime).AddMinutes(BuTruChechLechMuiGio());
        }
        catch
        {
            goto Again;
        }
        /*  ArgumentNullException: String reference not set to an instance of a String.
            Parameter name: s
            System.DateTimeParse.Parse (System.String s, System.Globalization.DateTimeFormatInfo dtfi, System.Globalization.DateTimeStyles styles) (at <fb001e01371b4adca20013e0ac763896>:0)
            System.DateTime.Parse (System.String s) (at <fb001e01371b4adca20013e0ac763896>:0)
            MyTime+<Start>d__8.MoveNext () (at Assets/Tuan Anh/Scripts/Reward AFK/MyTime.cs:41)
            UnityEngine.SetupCoroutine.InvokeMoveNext (System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at <2feaf16e80004e0cadae3f2e05f2a3fa>:0)
*/
        //Debug.Log(netTime);//Tue, 10 Mar 2020 08:59:19 GMT
        //Debug.Log(DateTime.Parse(netTime).AddMinutes(BuTruChechLechMuiGio()).ToString());//3/11/2020 11:19:00 AM
        //Debug.Log("GMT " + timezone);//Mui gio hien tai
        if (!PlayerPrefs.HasKey("FirstLoginDay"))
        {
            isFirstLoginNewDay = true;
            PlayerPrefs.SetString("FirstLoginDay", GetCurrentTimeStr());
        }
        else
        {
            if (CheckNewDay(GetTime(PlayerPrefs.GetString("FirstLoginDay"))))
            {
                isFirstLoginNewDay = true;
                PlayerPrefs.SetString("FirstLoginDay", GetCurrentTimeStr());
            }
        }
        if (!PlayerPrefs.HasKey("FirstLoginWeek"))
        {
            isFirstLoginNewWeek = true;
            PlayerPrefs.SetString("FirstLoginWeek", GetCurrentTimeStr(true));
        }
        else
        {
            if (CheckNewWeek(GetTime(PlayerPrefs.GetString("FirstLoginWeek"))))
            {
                isFirstLoginNewWeek = true;
                PlayerPrefs.SetString("FirstLoginWeek", GetCurrentTimeStr());
            }
        }

        isCheckedTimeOnline = true;
    }

    int BuTruChechLechMuiGio()
    {
        if (PlayerPrefs.HasKey("timeZone"))
        {
            string timezone = DateTimeOffset.Now.ToString();//+07:00
            string timeZoneOld = PlayerPrefs.GetString("timeZone");//+07:00
            timezone = timezone.Remove(0, timezone.Length - 6);//+07:00

            if (!timezone.Equals(timeZoneOld))
            {
                int h = int.Parse(timezone.Remove(3, 3));
                int m = int.Parse(timezone.Remove(0, 4));
                int total = h * 60 + (h / h) * m;

                int h0 = int.Parse(timeZoneOld.Remove(3, 3));
                int m0 = int.Parse(timeZoneOld.Remove(0, 4));
                int total0 = h0 * 60 + (h0 / h0) * m0;

                Debug.Log(total);
                Debug.Log(total0);
                Debug.Log(total0 - total);
                return total0 - total;
            }
            //Debug.Log("OK");
        }
        else
        {
            string tz = DateTimeOffset.Now.ToString();
            tz = tz.Remove(0, tz.Length - 6);
            PlayerPrefs.SetString("timeZone", tz);
            //Debug.Log("tzE " + tz);
        }
        return 0;
    }
    public bool CheckNewDay(DateTime timeOld)
    {
        DateTime now = GetCurrentTime();
        if ((now.Day > timeOld.Day && now.Month == timeOld.Month && now.Year == timeOld.Year)
            || (now.Month > timeOld.Month && now.Year == timeOld.Year)
            || now.Year > timeOld.Year)
        {//new Day
            Debug.Log("New Day!!");
			EvenADS.instance.Even_OpenApp();
			return true;
        }
        return false;
    }
    public bool CheckNewWeek(DateTime timeOld)
    {
        do
        {
            timeOld = timeOld.AddDays(1);
        }
        while ((int)timeOld.DayOfWeek != 1);

        int compare = DateTime.Compare(timeOld, GetCurrentTime());

        if (compare <= 0)//DateTimeNextWeek < now
        {
            Debug.Log("New Week!!");
            return true;
        }
        return false;
    }
    public string GetStrTime(DateTime time, string format = null)
    {
        return time.ToString(string.IsNullOrEmpty(format) ? this.format : format);
    }
    public string GetCurrentTimeStr(bool isResetTime = false)
    {
        DateTime currentTime = DateTime.Now;//timeOnline.AddSeconds(Time.realtimeSinceStartup - deltaTime);
        if (isResetTime) { currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0); }//set timeOld về 0h ngày hôm đó
        return currentTime.ToString(format);//dung de luu lai thoi gian cay trong duoc gieo
    }
    public DateTime GetCurrentTime()
    {
        DateTime currentTime = DateTime.Now;//timeOnline.AddSeconds(Time.realtimeSinceStartup - deltaTime);

        return currentTime;//dung de luu lai thoi gian cay trong duoc gieo
    }
    public DateTime GetTime(string time, string format = null)
    {
        return DateTime.ParseExact(time, string.IsNullOrEmpty(format) ? this.format : format, null);
    }
    public TimeSpan TimeClaimRewardsAfkToNow()
    {
        try
        {
            DateTime DateTimeNow = GetCurrentTime();
            DateTime TimeClaimRewardsAFK;
            TimeSpan interval;
            TimeClaimRewardsAFK = DateTime.ParseExact(PlayerPrefs.GetString("TimeClaimRewardsAFK"), format, null);
            interval = DateTimeNow.Subtract(TimeClaimRewardsAFK);
            return interval;
        }
        catch (Exception e)
        {
            Debug.Log("====Error: " + e.Message);
            return TimeSpan.MinValue;
        }
    }

    public string DisplayTime(TimeSpan timeSpan, string format=null)
    {
        if (timeSpan.TotalSeconds < 1) timeSpan = new TimeSpan(0, 0, 0);
        if (format != null) return timeSpan.ToString(format);
        return /*((int)timeSpan.TotalHours < 10 ? char.ConvertFromUtf32(48) : null) + (int)timeSpan.TotalHours + */timeSpan.ToString(formatTimeSpanMS);
        //return timeSpan.ToString(formatTimeSpan);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.V)) Debug.Log(GetCurrentTimeStr());
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        DateTime DateTimeNow = GetCurrentTime();
    //        DateTime TimeClaimRewardsAFK;
    //        TimeSpan interval;
    //        TimeClaimRewardsAFK = DateTime.ParseExact("18/02/2021 14:30:11", format, null);
    //        interval = DateTimeNow.Subtract(TimeClaimRewardsAFK);
    //        Debug.Log(interval.ToString(formatTimeSpan));
    //        Debug.Log(interval.Add(TimeSpan.FromSeconds(1)).ToString(formatTimeSpan));
    //    }

    //}
    #endregion

    public void CountDownTime(DateTime DateTimeNext, TextMeshProUGUI labelTime, string firstStr, string strColor, TextMeshProUGUI labelTimeClone = null, Action onDone = null, MyIntEvent onIntEvent = null)
    {
        StartCoroutine(TimeCountDown(DateTimeNext, labelTime, firstStr, strColor, labelTimeClone, onDone, onIntEvent));
    }

    IEnumerator TimeCountDown(DateTime DateTimeNext, TextMeshProUGUI labelTime, string firstStr, string strColor, TextMeshProUGUI labelTimeClone = null, Action onDone = null, MyIntEvent onIntEvent = null)
    {
        labelTime.gameObject.SetActive(true);
        DateTime DateTimeNow = GetCurrentTime();
        TimeSpan timeSpan;
        timeSpan = DateTimeNext.Subtract(DateTimeNow);
    Again:
        if (onIntEvent != null) onIntEvent.Invoke((int)timeSpan.TotalSeconds);
        if (!labelTime.gameObject.activeInHierarchy)
        {
            yield return new WaitForSecondsRealtime(0.1f);//child bảng thưởng lúc đầu labelTime off nên cần delay tránh trường hợp mở bảng thưởng lần đầu có mission đã hoàn thành không hiển thị nút collect
            if (!labelTime.gameObject.activeInHierarchy)
                yield break;
        }
        labelTime.text = firstStr + strColor + DisplayTime(timeSpan);
        if (labelTimeClone != null) labelTimeClone.text = labelTime.text;
        //Debug.Log("timeSpan.TotalSeconds: "+ timeSpan.TotalSeconds);
        if (timeSpan.TotalSeconds < 0)
        {
            Debug.Log("Finish!!");
            if (onDone != null)
            {
                Debug.Log("Xử lý Finish!!");
                onDone.Invoke();
            }
            yield break;
        }
        yield return new WaitForSecondsRealtime(1f);
        timeSpan = timeSpan.Add(TimeSpan.FromSeconds(-1));
        goto Again;
    }

    public void CountDownTimeToNextDay(TextMeshProUGUI labelTime, string firstStr, string strColor, TextMeshProUGUI labelTimeClone = null, UnityEvent onNextDay = null)
    {
        StartCoroutine(TimeCountDownNextDay(labelTime, firstStr, strColor, labelTimeClone, onNextDay));
    }

    IEnumerator TimeCountDownNextDay(TextMeshProUGUI labelTime, string firstStr, string strColor, TextMeshProUGUI labelTimeClone = null, UnityEvent onNextDay = null)
    {
    NextCheck:
        labelTime.gameObject.SetActive(true);
        DateTime DateTimeNow = GetCurrentTime();
        //DateTimeNow = DateTimeNow.Add(TimeSpan.FromMinutes(363));
        DateTime DateTimeNext = new DateTime(DateTimeNow.Year, DateTimeNow.Month, DateTimeNow.Day + 1);
        TimeSpan timeSpan;
        timeSpan = DateTimeNext.Subtract(DateTimeNow);
    Again:
        if (!labelTime.gameObject.activeInHierarchy) yield break;
        labelTime.text = firstStr + strColor + DisplayTime(timeSpan);
        if (labelTimeClone != null) labelTimeClone.text = labelTime.text;
        yield return new WaitForSecondsRealtime(1f);
        timeSpan = timeSpan.Add(TimeSpan.FromSeconds(-1));
        //Debug.Log(timeSpan.TotalSeconds);
        if (timeSpan.TotalSeconds < 0)
        {
            Debug.Log("New Day!!");
            if (onNextDay != null) onNextDay.Invoke();
            goto NextCheck;
        }
        goto Again;
    }

    public void CountDownTimeToNextWeek(TextMeshProUGUI labelTime, string firstStr, string strColor, TextMeshProUGUI labelTimeClone = null, UnityEvent onNextWeek = null)
    {
        StartCoroutine(TimeCountDownNextWeek(labelTime, firstStr, strColor, labelTimeClone, onNextWeek));
    }

    IEnumerator TimeCountDownNextWeek(TextMeshProUGUI labelTime, string firstStr, string strColor, TextMeshProUGUI labelTimeClone = null, UnityEvent onNextWeek = null)
    {
    NextCheck:
        labelTime.gameObject.SetActive(true);
        DateTime DateTimeNow = GetCurrentTime();
        DateTime DateTimeNext = DateTimeNow;

        do
        {
            DateTimeNext = new DateTime(DateTimeNext.Year, DateTimeNext.Month, DateTimeNext.Day + 1);
        }
        while ((int)DateTimeNext.DayOfWeek != 1);

        TimeSpan timeSpan;
        timeSpan = DateTimeNext.Subtract(DateTimeNow);
    Again:
        if (!labelTime.gameObject.activeInHierarchy) yield break;
        labelTime.text = firstStr + strColor + DisplayTime(timeSpan, (timeSpan.TotalSeconds < 86400) ? null : formatTimeSpan);
        if (labelTimeClone != null) labelTimeClone.text = labelTime.text;
        yield return new WaitForSecondsRealtime(1f);
        timeSpan = timeSpan.Add(TimeSpan.FromSeconds(-1));
        if (timeSpan.TotalSeconds < 0)
        {
            Debug.Log("New Week!!");
            if (onNextWeek != null) onNextWeek.Invoke();
            goto NextCheck;
        }
        goto Again;
    }

    public bool CheckFinishTime(DateTime time1, DateTime time2, int totalSecToFinish)
    {
        TimeSpan timeSpan;
        timeSpan = time2.Subtract(time1);
        Debug.Log(GetStrTime(time1));
        Debug.Log(GetStrTime(time2));
        Debug.Log(timeSpan.TotalSeconds);
        if (timeSpan.TotalSeconds >= totalSecToFinish)
        {
            Debug.Log("Finish!!");
            return true;
        }
        return false;
    }
}

/*
        DateTime DateTimeNow = DateTime.Now;
        DateTime TimeBuyRemoveADS;
        TimeSpan interval;
        TimeBuyRemoveADS = DateTime.ParseExact(PlayerPrefs.GetString("TimeBuyRemoveADS14D"), format, null);
        interval = DateTimeNow.Subtract(TimeBuyRemoveADS);
*/
