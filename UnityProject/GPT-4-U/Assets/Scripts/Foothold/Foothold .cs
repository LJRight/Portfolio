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
        // �÷��̾ ���ǿ� �ö���� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // �÷��̾ ������ �ڽ����� ����
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // �÷��̾ ���ǿ��� �������� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // �θ�-�ڽ� ���� ����
        }
    }
}
