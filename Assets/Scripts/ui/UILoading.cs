using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UILoading : MonoBehaviour
{
    public GameObject loadingObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        DOTween.Init();

        // 创建一个旋转动画，并使其无限重复
        loadingObject.transform.DORotate(new Vector3(0, 0, 360), 2, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
