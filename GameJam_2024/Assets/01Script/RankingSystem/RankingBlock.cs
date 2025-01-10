using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class RankingBlock : MonoBehaviour
{
    private TextMeshProUGUI _rankText;
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _percentText;
    private TextMeshProUGUI _clearTimeText;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _rankText = transform.Find("Text_Rank").GetComponent<TextMeshProUGUI>();
        _nameText = transform.Find("Text_UserName").GetComponent<TextMeshProUGUI>();
        _percentText = transform.Find("Text_BestPercent").GetComponent<TextMeshProUGUI>();
        _clearTimeText = transform.Find("Text_BestClearTime").GetComponent<TextMeshProUGUI>();

        SetBlockText(string.Empty, string.Empty, string.Empty, string.Empty);
    }

    public void SetBlockText(string rank, string name, string persent, string clearTime)
    {
        _rankText.text = rank;
        _nameText.text = name;
        _percentText.text = persent + " %";
        _clearTimeText.text = clearTime;
    }
}
