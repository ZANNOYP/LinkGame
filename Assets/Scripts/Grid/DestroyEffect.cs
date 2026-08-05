using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 消除特效
/// </summary>
public class DestroyEffect : MonoBehaviour
{
    private ParticleSystem parSystem;
    private void Awake()
    {
        parSystem = GetComponent<ParticleSystem>();
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 播放特效
    /// </summary>
    public void Play()
    {
        gameObject.SetActive(true);
        parSystem.Play();
        StartCoroutine(DisableAfterPlay());
    }

    /// <summary>
    /// 停止特效
    /// </summary>
    public void Stop()
    {
        parSystem.Stop();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 播放结束自动失活
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisableAfterPlay()
    {
        yield return new WaitForSeconds(parSystem.main.duration);

        gameObject.SetActive(false);
    }
}
