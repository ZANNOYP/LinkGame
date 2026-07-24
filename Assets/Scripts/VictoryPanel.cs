using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 胜利面板
/// </summary>
public class VictoryPanel : MonoBehaviour
{
    public static VictoryPanel Instance;
    // 显隐速度
    public float fadeSpeed = 2f;
    // 显隐状态
    public bool isShow;

    public TextMeshProUGUI timeText;

    public TextMeshProUGUI titleText;

    private CanvasGroup canvasGroup;
    // 是否渐显隐
    private bool isFade;

    private void Awake()
    {
        Instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Hide(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFade)
            FadeOut();
    }

    /// <summary>
    /// 渐显隐
    /// </summary>
    public void FadeOut()
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
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    public void Show(float spendTime, bool isVictory = true, bool isFade = true)
    {
        if (!isFade)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        isShow = true;
        this.isFade = isFade;
        // 失败
        if (!isVictory)
        {
            titleText.text = "Defeat";
            timeText.text = "Time's up";
        }
        // 成功
        else 
        {
            titleText.text = "Victory!!";
            int spendTimeInt = Mathf.RoundToInt(spendTime);
            timeText.text = "Took " + spendTimeInt + " seconds";
        }
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    public void Hide(bool isFade = true)
    {
        if (!isFade)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        isShow = false;
        this.isFade = isFade;
    }

}
