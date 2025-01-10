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
    private KeyValuePair<int, UserData> _currentUserData;

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
        Debug.Log("SetRankingBoard");

        _userDataList.Clear();

        while (_rankingBlockSpawnTrm.childCount > 0)
        {
            Transform child = _rankingBlockSpawnTrm.GetChild(0);
            if (child.TryGetComponent(out RankingBlock block))
            {
                PoolManager.Instance.Push("RankingBlock", child.gameObject);
            }
        }

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
                        _userDataList.Add(userData);
                    }

                    _userDataList.Sort((a, b) =>
                    {
                        int percentComparison = b.maxPercent.CompareTo(a.maxPercent);
                        if (percentComparison == 0)
                        {
                            // 만약 퍼센트가 같다면 클리어타임으로 비교
                            return a.clearTime.CompareTo(b.clearTime);
                        }
                        return percentComparison;
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
