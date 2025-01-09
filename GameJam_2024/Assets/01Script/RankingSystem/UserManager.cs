using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using TMPro;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    [Header("Firebase")]
    private FirebaseAuth _auth; // 인증을 관리할 객체
    private FirebaseUser _user; // 사용자
    private DatabaseReference _dbRef;
    private string userType = "UserData";

    [Header("Main Pannel")]
    [SerializeField] private GameObject _loginPannel;
    [SerializeField] private GameObject _registerPannel;

    [Header("Register")]
    [SerializeField] private TMP_InputField _userNameRegisterField;
    [SerializeField] private TMP_InputField _emailRegisterField;
    [SerializeField] private TMP_InputField _passwordRegisterField;
    [SerializeField] private TMP_InputField _passwordChkRegisterField;
    [SerializeField] private TMP_Text _warningRegisterText;

    [Header("Login")]
    [SerializeField] private TMP_InputField _emailLoginField;
    [SerializeField] private TMP_InputField _passwordLoginField;
    [SerializeField] private TMP_Text _warningLoginText;

    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                _auth = FirebaseAuth.DefaultInstance;
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

    public void UpdateUser(string name, int maxPersent, string clearTime)
    {
        // 데이터 변환
        UserData user = new(name, maxPersent, clearTime);
        string json = JsonUtility.ToJson(user);

        // 레퍼런스 선언 및 데이터 저장
        DatabaseReference reference = _dbRef.Child(userType);
        reference.Child(name).SetRawJsonValueAsync(json);
    }

    #region Register

    private IEnumerator Register(string email, string userName, string password)
    {
        if (userName == "")
        {
            _warningRegisterText.text = "Missing UserName";
        }
        else if (_passwordRegisterField.text != _passwordChkRegisterField.text)
        {
            _warningRegisterText.text = "Password does not Match!";
        }
        else
        {
            // 회원가입
            var task = _auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to Register:{task.Exception}");
                FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                Debug.Log($"Error Code: {errorCode}");

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already in Use";
                        break;
                }

                _warningRegisterText.text = message;
            }
            else
            {
                _user = task.Result.User;
                Firebase.Auth.FirebaseUser newUser = _auth.CurrentUser;
                if (newUser != null)
                {
                    Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                    {
                        DisplayName = userName
                    };
                    newUser.UpdateUserProfileAsync(profile).ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.LogError("UpdateUserProfileAsync was canceled.");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                            return;
                        }

                        Debug.Log("User profile updated successfully.");
                    });
                }

                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            }
            _warningRegisterText.text = string.Empty;
            StartCoroutine(SaveUserName());
            StartCoroutine(SaveUserPassword());
            SaveUserData(userName, 0, "00:00:00"); // 초기화값을 처음에 저장

        }
    }

    public void OnRegister()
    {
        StartCoroutine(Register(_emailRegisterField.text,
            _userNameRegisterField.text, _passwordRegisterField.text));
    }

    #endregion


    #region Login

    private IEnumerator Login(string email, string password)
    {
        var task = _auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to Login:{task.Exception}");
            FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "InvalidEmail Email";
                    break;
                case AuthError.UserNotFound:
                    message = "User Not Found";
                    break;
            }

            _warningLoginText.text = message;
        }

        _user = task.Result.User;
        Firebase.Auth.AuthResult result = task.Result;
        Debug.LogFormat("User signed in successfully: {0} ({1}, {2})",
            result.User.DisplayName, result.User.UserId, result.User.Email);
        _warningLoginText.text = string.Empty;
        StartCoroutine(LoadUserName());
        StartCoroutine(LoadUserPassword());
    }


    public void OnLogin()
    {
        StartCoroutine(Login(_emailLoginField.text, _passwordLoginField.text));
    }

    #endregion

    private IEnumerator SaveUserName()
    {
        Debug.Log(_dbRef);
        Debug.Log(_user.UserId);
        Debug.Log(_userNameRegisterField.text);
        var DBTask = _dbRef.Child("users").Child(_user.UserId)
            .Child("UserName").SetValueAsync(_userNameRegisterField.text);
        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"");
        }
        else
        {
            Debug.Log("UserName Save Completed");
        }
    }

    private IEnumerator SaveUserPassword()
    {
        var DBTask = _dbRef.Child("users").Child(_user.UserId)
            .Child("Password").SetValueAsync(_passwordRegisterField.text);
        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"");
        }
        else
        {
            Debug.Log("Password Save Completed");
        }
    }

    private void SaveUserData(string userName, int maxPersent, string clearTime)
    {
        UserData user = new UserData(userName, maxPersent, clearTime);
        string json = JsonUtility.ToJson(user);

        // Realtime Database에 저장
        DatabaseReference reference = _dbRef.Child("UserData");
        reference.Child(userName).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("회원 데이터 저장 완료!");
            }
            else
            {
                Debug.LogError("회원 데이터 저장 실패");
            }
        });
    }

    private IEnumerator LoadUserName()
    {
        var DBTask = _dbRef.Child("users").Child(_user.UserId)
            .Child("UserName").GetValueAsync();
        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"Failed to Save Data:{DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            Debug.Log("Save Completed");
            Debug.Log($"UserName: {snapshot.Value}");
        }
    }

    private IEnumerator LoadUserPassword()
    {
        var DBTask = _dbRef.Child("users").Child(_user.UserId)
            .Child("Password").GetValueAsync();
        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"Failed to Save Data:{DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            Debug.Log("Save Completed");
            Debug.Log($"UserPassword: {snapshot.Value}");
        }
    }

    #region Main Buttons

    public void LoginPannelOn()
    {
        _loginPannel.SetActive(true);
    }
    public void LoginPannelOff()
    {
        _loginPannel.SetActive(false);
    }
    public void RegisterPannelOn()
    {
        _registerPannel.SetActive(true);
    }
    public void RegisterPannelOff()
    {
        _registerPannel.SetActive(false);
    }

    #endregion
}
