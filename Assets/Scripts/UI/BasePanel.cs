using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// 面板基类
/// </summary>
public class BasePanel : MonoBehaviour
{
    // 显隐速度
    public float fadeSpeed = 2f;
    // 显隐状态
    private bool isShow;

    protected CanvasGroup canvasGroup;
    // 是否渐显隐
    private bool isFade;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isFade)
            Fade();
    }

    /// <summary>
    /// 渐显隐
    /// </summary>
    protected void Fade()
    {
        if (isShow && canvasGroup.alpha < 1f)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            if (canvasGroup.alpha >= 1f)
            {
                canvasGroup.alpha = 1f;
            }
        }
        if (!isShow && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            if (canvasGroup.alpha <= 0)
            {
                gameObject.SetActive(false);
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    public virtual void Show(bool isFade = true)
    {
        gameObject.SetActive(true);
        if (!isFade)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        isShow = true;
        this.isFade = isFade;
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    public virtual void Hide(bool isFade = true)
    {
        if (!isFade)
        {
            gameObject.SetActive(false);
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        isShow = false;
        this.isFade = isFade;
    }
}
