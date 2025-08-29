using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Foothold : MonoBehaviour
{
    [SerializeField]
    private Vector3 _targetPos;
    [SerializeField]
    private Vector3 _startPos;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _waitTime;
    void Start()
    {
        StartCoroutine(MoveRepeat());
    }

    IEnumerator MoveRepeat()
    {
        while (true)
        {
            gameObject.transform.DOMove(_targetPos, _speed);
            yield return new WaitForSeconds(_waitTime);
            gameObject.transform.DOMove(_startPos, _speed);
            yield return new WaitForSeconds(_waitTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어가 발판에 올라왔을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // 플레이어를 발판의 자식으로 설정
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 플레이어가 발판에서 내려왔을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // 부모-자식 관계 해제
        }
    }
}
