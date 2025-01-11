using Firebase.Database;
using PimDeWitte.UnityMainThreadDispatcher;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RankingBoard : MonoBehaviour
{
    [Header("Ranking Block")]
    [SerializeField] private RankingBlock _rankingBlock;
    [SerializeField] private Transform _rankingBlockSpawnTrm;

    [Header("Current User")]
    [SerializeField] private TextMeshProUGUI _userRankText;
    [SerializeField] private TextMeshProUGUI _userNameText;
    [SerializeField] private TextMeshProUGUI _userPercentText;
    [SerializeField] private TextMeshProUGUI _userClearTimeText;

    private DatabaseReference _dbRef;
    private List<UserData> _userDataList = new();

    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                _dbRef = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    private void Start()
    {
        SetRankingBoard();
    }

    private void SetRankingBoard()
    {
        // 기존 데이터 블록 삭제
        _userDataList.Clear();
        RankingBlock[] blockArr = _rankingBlockSpawnTrm.GetComponentsInChildren<RankingBlock>();
        for (int i = 0; i < blockArr.Length; ++i)
            PoolManager.Instance.Push("RankingBlock", blockArr[i].gameObject);

        if (_dbRef != null)
        {
            DatabaseReference userRef = _dbRef.Child("UserData");

            userRef.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    foreach (DataSnapshot userSnapshot in snapshot.Children)
                    {
                        string userName = userSnapshot.Key;
                        UserData userData = ParseUserData(userSnapshot);

                        if (userData.clearTime == "00h 00m 00s")
                            userData.clearTime = "-";
                        _userDataList.Add(userData);
                    }

                    _userDataList.Sort((a, b) =>
                    {
                        int percentComparison = b.maxPercent.CompareTo(a.maxPercent);
                        if (percentComparison != 0)
                        {
                            return percentComparison;
                        }

                        // clearTime이 "-"인지 여부로 우선 비교
                        bool aIsDash = a.clearTime == "-";
                        bool bIsDash = b.clearTime == "-";

                        if (aIsDash && !bIsDash) return 1; // b가 우위
                        else if (!aIsDash && bIsDash) return -1; // a가 우위

                        // 만약 퍼센트가 같다면 클리어타임으로 비교
                        return a.clearTime.CompareTo(b.clearTime);
                    });

                    // Update UI -> main thread
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        // Other Users
                        int rank = 1;
                        for (int i = 0; i < _userDataList.Count; ++i)
                        {
                            if (i != 0)
                            {
                                if (_userDataList[i - 1].maxPercent != _userDataList[i].maxPercent ||
                                _userDataList[i - 1].clearTime != _userDataList[i].clearTime)
                                {
                                    rank = i + 1;
                                    //++rank;
                                }
                            }
                            InstantiateRankingBlock(rank, _userDataList[i]);
                        }
                    });
                }
                else if (task.IsFaulted)
                {
                    Debug.LogError($"Failed to fetch user data: {task.Exception}");
                }
            });
        }
    }

    private void SetCurrentUserUI(int rank, UserData userData)
    {
        _userRankText.text = rank.ToString();
        _userNameText.text = userData.userName;
        _userPercentText.text = userData.maxPercent.ToString() + " %";
        _userClearTimeText.text = userData.clearTime;
    }

    private void InstantiateRankingBlock(int rank, UserData userData)
    {
        // Current User
        if (userData.userName == UserManager.Instance.UserName)
        {
            SetCurrentUserUI(rank, userData);
        }

        GameObject go = PoolManager.Instance.Pop("RankingBlock", _rankingBlockSpawnTrm);
        if (go.TryGetComponent(out RankingBlock block))
        {
            block.SetBlockText(rank.ToString(), userData.userName,
                userData.maxPercent.ToString(), userData.clearTime);
        }
    }

    private UserData ParseUserData(DataSnapshot snapshot)
    {
        string userName = snapshot.Key;
        int maxPercent = int.Parse(snapshot.Child("maxPercent").Value.ToString());
        string clearTime = snapshot.Child("clearTime").Value.ToString();

        return new UserData(userName, maxPercent, clearTime);
    }

    // 새로 고침
    public void OnRefreshButtonClick()
    {
        SetRankingBoard();
    }
}
