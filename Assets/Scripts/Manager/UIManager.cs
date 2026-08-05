using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI面板管理器
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    // 面板列表
    public List<BasePanel> panelList = new List<BasePanel>();
    // 面板字典
    private Dictionary<Type, BasePanel> panelDic = new Dictionary<Type, BasePanel>();

    private void Awake()
    {
        instance = this;

        foreach (BasePanel panel in panelList)
        {
            panelDic.Add(panel.GetType(), panel);
        }
    }

    private void Start()
    {
        HidePanel<GamePanel>(false);
        HidePanel<VictoryPanel>(false);
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isFade"></param>
    public void ShowPanel<T>(bool isFade = true) where T : BasePanel
    {
        T panel = panelDic[typeof(T)] as T;
        panel.Show(isFade);
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isFade"></param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        T panel = panelDic[typeof(T)] as T;
        panel.Hide(isFade);
    }

    /// <summary>
    /// 得到面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetPanel<T>() where T : BasePanel
    {
        return panelDic[typeof(T)] as T;
    }
}
