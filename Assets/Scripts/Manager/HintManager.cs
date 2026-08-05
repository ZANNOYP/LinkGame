using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 提示管理器
/// </summary>
public class HintManager : MonoBehaviour
{
    public static HintManager Instance;
    // 提示间隔时间
    public float tipInterval = 0.5f;
    // 提示前 定时时间
    public float beforeTime = 5f;
    // 提示时间
    public float tipTime = 3f;
    // 最大可使用提示次数
    public int hintMaxCount = 3;

    // 提示可消除格子
    private BlockView viewTip1;
    private BlockView viewTip2;
    // 提示协程
    private Coroutine tipCoroutine;
    // 提示前 协程
    private Coroutine beforeTipCoroutine;
    // 当前剩余提示次数
    private int hintCount;
    // 是否正在提示
    private bool isHint;
    // 提示次数文本
    [SerializeField]
    private TextMeshProUGUI textHintCount;

    private void Awake()
    {
        Instance = this;
        ResetHintCount();
    }
    
    /// <summary>
    /// 开始提示
    /// </summary>
    public void StartHint()
    {
        MusicManager.Instance.PlayEff(EffType.Button, 0.4f);
        if (isHint) return;

        if (hintCount <= 0)
        {
            Debug.Log("提示次数耗尽！！");
            return;
        }

        ShowTip();
        hintCount--;
        RefreshUI();
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public void RefreshUI()
    {
        textHintCount.text = hintCount.ToString();
    }

    /// <summary>
    /// 重置提示次数
    /// </summary>
    public void ResetHintCount()
    {
        hintCount = hintMaxCount;
        RefreshUI();
    }

    /// <summary>
    /// 提示
    /// </summary>
    private void ShowTip()
    {
        GridCell a;
        GridCell b;
        (a, b) = MapManager.Instance.GetAnyMatch();
        if (a != null && b != null)
        {
            viewTip1 = a.view;
            viewTip2 = b.view;
            tipCoroutine = StartCoroutine(TipCoroutine());
        }
    }

    /// <summary>
    /// 提示协程
    /// </summary>
    /// <param name="tipInterval"></param>
    /// <returns></returns>
    private IEnumerator TipCoroutine()
    {
        isHint = true;
        bool active = true;
        float timer = 0;
        while (timer <= tipTime)
        {
            yield return new WaitForSeconds(tipInterval);
            timer += tipInterval;
            viewTip1?.SetTip(active);
            viewTip2?.SetTip(active);
            active = !active;
        }
        viewTip1?.SetTip(false);
        viewTip2?.SetTip(false);
        isHint = false;
    }

    /// <summary>
    /// 停止提示协程
    /// </summary>
    public void StopTipCoroutinue()
    {
        if (tipCoroutine != null)
        {
            StopCoroutine(tipCoroutine);
            tipCoroutine = null;

            viewTip1?.SetTip(false);
            viewTip2?.SetTip(false);
            isHint = false;
        }
    }

    

    
}
