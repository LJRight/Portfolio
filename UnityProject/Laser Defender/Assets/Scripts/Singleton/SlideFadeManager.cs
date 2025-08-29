using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SlideFadeManager : MonoBehaviour
{
    public static SlideFadeManager Instance;

    [SerializeField] private RectTransform fadePanel;
    [SerializeField] private float slideInDuration = 1f;
    [SerializeField] private float slideOutDuration = .5f;
    private void Awake()
    {
        fadePanel.anchoredPosition = new Vector2(0, -fadePanel.rect.height);
    }
    /// <summary>
    /// 주어진 씬 인덱스를 슬라이드 애니메이션을 통해 로드하는 함수
    /// </summary>
    /// <param name="sceneIndex">Build Settings 상의 씬 인덱스</param>
    public void SlideToScene(int sceneIndex)
    {
        Sequence slideSequence = DOTween.Sequence();
        slideSequence.Append(
            fadePanel.DOAnchorPos(Vector2.zero, slideInDuration).SetEase(Ease.InOutQuad)
        );
        slideSequence.AppendCallback(() =>
        {
            SceneManager.LoadSceneAsync(sceneIndex).completed += (op) =>
            {
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    fadePanel.DOAnchorPos(new Vector2(0, fadePanel.rect.height), slideOutDuration)
                        .SetEase(Ease.InOutQuad)
                        .OnComplete(() =>
                        {
                            fadePanel.anchoredPosition = new Vector2(0, -fadePanel.rect.height);
                        });
                });
            };
        });
    }
}