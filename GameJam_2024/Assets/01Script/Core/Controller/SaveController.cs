using Firebase.Database;
using UnityEngine;

public class SaveController : MonoSingleton<SaveController>
{
    private DatabaseReference _dbRef;

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

    public void SavePersent(float percent)
    {
        if (_dbRef != null)
        {
            _dbRef.Child("UserData").Child(UserManager.Instance.UserName).Child("maxPercent").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Exists)
                    {
                        string savePercent = task.Result.Value.ToString();
                        int intSavePercent = int.Parse(savePercent);

                        if (intSavePercent < percent)
                        {
                            _dbRef.Child("UserData").Child(UserManager.Instance.UserName).Child("maxPercent").SetValueAsync(percent)
                            .ContinueWith(task =>
                            {
                                if (task.IsCompleted)
                                {
                                    Debug.Log($"MaxPercent saved for {UserManager.Instance.UserName}: {percent}");
                                }
                                else
                                {
                                    Debug.LogError("Failed to save MaxPercent.");
                                }
                            });
                        }
                    }
                }
            });
        }
    }

    public void SaveTime(int hour, int min, int sec)
    {
        string formattedTime = $"{hour:D2}h {min:D2}m {sec:D2}s";
        if (_dbRef != null)
        {
            _dbRef.Child("UserData").Child(UserManager.Instance.UserName).Child("clearTime").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    string currentTime = "-";
                    if (task.Result.Exists)
                    {
                        currentTime = task.Result.Value.ToString();
                    }

                    // 현재 시간과 새 시간을 비교

                    // 기본값이 아닌경우 비교
                    if (currentTime != "-")
                    {
                        int currentSeconds = ConvertTimeToSeconds(currentTime);
                        int newSeconds = ConvertTimeToSeconds(formattedTime);

                        if (currentSeconds == 0 || newSeconds < currentSeconds)
                        {
                            SaveNewTime(formattedTime);
                        }
                    }
                    else
                        SaveNewTime(formattedTime);
                }
            });
        }
    }

    private void SaveNewTime(string time)
    {
        _dbRef.Child("UserData").Child(UserManager.Instance.UserName).Child("clearTime").SetValueAsync(time)
            .ContinueWith(saveTask =>
            {
                if (saveTask.IsCompleted)
                {
                    Debug.Log($"ClearTime saved for {UserManager.Instance.UserName}: {time}");
                }
                else
                {
                    Debug.LogError("Failed to save ClearTime.");
                }
            });
    }

    // 시간을 초 단위로 변환하는 헬퍼 함수
    private int ConvertTimeToSeconds(string time)
    {
        // "00h 00m 00s" 형식의 문자열을 초 단위로 변환
        string[] parts = time.Split(' ');
        int hours = int.Parse(parts[0].Replace("h", ""));
        int minutes = int.Parse(parts[1].Replace("m", ""));
        int seconds = int.Parse(parts[2].Replace("s", ""));
        return (hours * 3600) + (minutes * 60) + seconds;
    }

    public void SaveBGM(float bgm)
    {
        PlayerPrefs.SetFloat("BGM", bgm);
    }

    public void SaveSFX(float sfx)
    {
        PlayerPrefs.SetFloat("SFX", sfx);
    }
}
