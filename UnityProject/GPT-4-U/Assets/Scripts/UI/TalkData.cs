using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData : MonoBehaviour
{
    Dictionary<int, string[]> data;
    Dictionary<int, Sprite[]> spriteData;

    public Sprite[] sprites;

    void Awake()
    {
        data = new Dictionary<int, string[]>();
        spriteData = new Dictionary<int, Sprite[]>();
        GenerateData();

        DontDestroyOnLoad(gameObject);
    }

    void GenerateData()
    {
        
        data.Add(100, new string[] // ���ѷα�
        { 
            "...", 
            "...........으으 어지러워", 
            ".....뭐야 여긴... 어디지?",
            "어두워서 보이지 않아.. 난 왜 여기에 있는거지..?",
            "(머리에 통증이 느껴질뿐 아무 기억도 나지 않는다)",
            "아무런 기억이 안 나... 어라 이건 뭐지?",
            "(손전등을 발견한다)",
            "이건.. 손전등? 휴 다행히 작동하는군.",
            "기억은 안 나지만.. 여기 뭔가 이상해. 어서 여기서 나가야겠어."
        });

        data.Add(200, new string[]
        {
            "HP를 회복했다!"
        });

        // 열쇠 획득
        data.Add(1000, new string[] 
        {
            "(열쇠를 획득했다)",
        }); 

        // 3번째 표지판
        data.Add(2000, new string[] 
        { 
            "Tab키를 누르면 블루라이트 기능을 사용할 수 있습니다",
            "블루라이트에는 특수한 능력이 있다는데..."
        });

        // 1번째 표지판
        data.Add(3000, new string[]
        {
            "열쇠를 찾아서 다음 문으로 이동하세요"
        });

        // 2번째 표지판
        data.Add(4000, new string[]
        {
            "주의! 몬스터가 등장합니다",
            "빛을 잘 이용하면 몬스터를 잡을 수도..?"
        });

        // 4번째 표지판
        data.Add(5000, new string[]
        {
            "Final Stage!",
            "블루라이트에는 사실 숨겨진 기능이 하나 더 있습니다",
            "무운을 빕니다."
        });

        data.Add(10000, new string[] // ù��° ����
        { 
            "이건 뭐지? 열쇠는 아닌 것 같은데..",
            "\"카메라 보고 웃으세요 하나 둘 셋~!\"",
            "이 사진.. 그래 내게는 가족이 있었어.",
            "처음으로 사진을 같이 찍던 날이네, 정말 행복했었지"
        });

        data.Add(20000, new string[] // �ι�° ����
        {
            "\"휴 오늘도 힘들었다. 빨리 집에 가야지\"",
            "이건... 내가 사들고 갔던 치킨 상자잖아.",
            "우리 아이들은 후라이드를 좋아했지",
            "치킨을 사간 날 기뻐하며 '아빠 최고!' 라고 외치던 그 목소리가 들리는 것 같아.."
        });

        data.Add(30000, new string[] // ����° ����
        {
            "이 신발 한 짝.. 그래 그 때 난 신발이 벗겨지는 것도 몰랐네",
            "절박하게 날 부르던 목소리, 다른 건 아무것도 중요하지 않았어.",
            "그저 달려가는 것만 생각했었지"
        });

        data.Add(40000, new string[] // �׹�° ����
        {
            "\"곰돌이랑 같이 잘래요!\"",
            "이건.. 막내가 제일 좋아하던 곰인형이네",
            "곰돌이도 데려가야한다고 울먹이던 너를 위해 몸을 뒤로 돌렸었지",
            "연기가 더 짙어지던 걸 느꼈지만 괜찮았어, 네가 웃는 모습을 볼 수만 있다면..",
            "그리고 나는....지금 여기에.."
        });

        data.Add(50000, new string[] // �ټ���° ����
        {
            "\"영원히 네 곁에 있을게\"",
            "이건 결혼 반지..?",
            "그 때 이 반지를 손에 껴주며 영원한 사랑을 약속했었지",
            "그래.. 내 가족.. 내 아이들은 내가 필요해",
            "돌아가자. 내게는 지킬 사람들이 있어"
        });

        spriteData.Add(100, new Sprite[]
        {
            null, null, null, null, null, null, sprites[0], null, null
        });

        spriteData.Add(200, new Sprite[]
        {
            sprites[8]
        });

        spriteData.Add(1000, new Sprite[]
        {
            sprites[1]
        });

        spriteData.Add(10000, new Sprite[]
        {
            sprites[2], sprites[3], null, null
        });

        spriteData.Add(20000, new Sprite[]
        {
            null, sprites[4], null, null
        });

        spriteData.Add(30000, new Sprite[]
        {
            sprites[5], null, null 
        });

        spriteData.Add(40000, new Sprite[]
        {
            sprites[6], sprites[6], null, null, null,
        });

        spriteData.Add(50000, new Sprite[]
        {
            sprites[7], sprites[7], null, null, null,
        });

    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == data[id].Length)
        {
            return null;
        }
        else
        {
            return data[id][talkIndex];
        }
    }

    public Sprite GetSprite(int id, int talkIndex)
    {
        if (spriteData.ContainsKey(id) && talkIndex < data[id].Length)
        {
            return spriteData[id][talkIndex];   
        }

        return null;
    }
}
