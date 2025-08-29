using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPieces : MonoBehaviour
{
    public GameObject[] pieces;

    public void ShowPieceImages()
    {
        pieces[GameManager.instance.currentPiece].SetActive(true);
    }

    public void HidePieceImages()
    {
        if (GameManager.instance.currentPiece != 0)
        {
            pieces[GameManager.instance.currentPiece - 1].SetActive(false);
        }
    }
}
