using Lightbug.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image talkImage;
    public TalkData talkData;
    public GameObject talkPanel;
    public Text talkText;
    public bool isAction;
    public int talkIndex;

    public LightEnergy lightEnergy;

    void Awake()
    {
        instance = this;

        // ü�� �� �������� Canvas ����
        DontDestroyOnLoad(gameObject);
    }

    void FixedUpdate()
    {
        lightEnergy = GetComponentInChildren<LightEnergy>();
    }

    public void Action()
    {
        Talk(GameManager.instance.talkId);
        talkPanel.SetActive(isAction);
    }

    void Talk(int id)
    {
        string talk = talkData.GetTalk(id,  talkIndex);
        Sprite image = talkData.GetSprite(id, talkIndex);

        if (talk == null)
        {
            isAction = false;
            talkIndex = 0;
            talkText.text = "";
            talkImage.sprite = null;
            talkImage.gameObject.SetActive(false);
            return;
        }

        talkText.text = talk;
        isAction = true;
        talkIndex++;

        if (image != null)
        {
            talkImage.sprite = image;
            talkImage.gameObject.SetActive(true);
        }
        else
        {
            talkImage.gameObject.SetActive(false);
        }
    }
}
