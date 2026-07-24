using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 时间条管理器
/// </summary>
public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance;
    // 当前剩余时间
    public float nowTime;
    // 是否开始计时
    public bool isStart;
    // 最大时间
    public float maxTime = 30f;
    // 时间滑动块
    public Slider timeSlider;

    public float SpendTime => spendTime;

    private float spendTime;

    private float nowMaxTime;

    private void Awake()
    {
        Instance = this;
        StopTimer();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            nowTime -= Time.deltaTime;
            RefreshUI();
            if (nowTime <= 0)
            {
                nowTime = 0;
                StopTimer();
                VictoryPanel.Instance.Show(spendTime, false);
                MapManager.Instance.ClearView();
                GamePanel.Instance.Hide();
            }
        }
    }

    /// <summary>
    /// 开始计时
    /// </summary>
    public void StartTimer()
    {
        nowTime = maxTime;
        nowMaxTime = maxTime;
        isStart = true;
    }

    /// <summary>
    /// 停止计时
    /// </summary>
    /// <returns></returns>
    public float StopTimer()
    {
        isStart = false;
        spendTime += (nowMaxTime - nowTime);
        return spendTime;
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public void RefreshUI()
    {
        timeSlider.value = nowTime / maxTime;
    }

    /// <summary>
    /// 延长剩余时间
    /// </summary>
    /// <param name="time"></param>
    public void AddTime(float time)
    {
        if (!isStart) return;
        spendTime += (nowMaxTime - nowTime);
        nowTime = Mathf.Min(nowTime + time, maxTime);
        nowMaxTime = nowTime;
        RefreshUI();
    }

    /// <summary>
    /// 重置消耗时间
    /// </summary>
    public void ResetSpendTime()
    {
        spendTime = 0;
    }
}
