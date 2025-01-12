using DG.Tweening;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserAccount : MonoBehaviour
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
    [SerializeField] private RectTransform _registerTrm;

    [Header("Login")]
    [SerializeField] private TMP_InputField _emailLoginField;
    [SerializeField] private TMP_InputField _passwordLoginField;
    [SerializeField] private TMP_Text _warningLoginText;
    [SerializeField] private RectTransform _loginTrm;

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

        SoundController.Instance.PlayBGM(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_loginPannel.activeSelf == true)
                OnLogin();
            else if (_registerPannel.activeSelf == true)
                OnRegister();
        }
    }

    #region Register

    private IEnumerator Register(string email, string userName, string password)
    {
        if (userName == "")
        {
            _warningRegisterText.text = "Missing UserName";
        }
        else if (userName.Length > 10)
        {
            _warningRegisterText.text = "Username must not exceed 10 characters";
        }
        else if (_passwordRegisterField.text != _passwordChkRegisterField.text)
        {
            _warningRegisterText.text = "Password does not Match!";
        }
        else
        {
            // Firebase에서 이미 존재하는 사용자 이름 확인
            var userRef = _dbRef.Child("UserData").Child(userName);
            var task = userRef.GetValueAsync();
            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to check user name availability: {task.Exception}");
                _warningRegisterText.text = "Failed to check user name availability";
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists) // 이미 존재하는 사용자 이름
                {
                    _warningRegisterText.text = "Username already exists";
                }
                else
                {
                    // 회원가입
                    var authTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);
                    yield return new WaitUntil(() => authTask.IsCompleted);

                    if (authTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to Register:{authTask.Exception}");
                        FirebaseException firebaseEx = authTask.Exception.GetBaseException() as FirebaseException;
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
                        _user = authTask.Result.User;
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

                        _warningRegisterText.text = string.Empty;
                        StartCoroutine(SaveUserName());
                        StartCoroutine(SaveUserPassword());
                        StartCoroutine(SaveUserEmail());
                        SaveUserData(userName, 0, "-"); // 초기화값을 처음에 저장

                        // 회원가입 완료시 패널 off
                        RegisterPannelOff();
                    }
                }
            }
        }
    }

    public void OnRegister()
    {
        SoundController.Instance.PlaySFX(0);
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
            yield break;
        }
        else
        {
            _user = task.Result.User;
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1}, {2})",
                result.User.DisplayName, result.User.UserId, result.User.Email);
            _warningLoginText.text = string.Empty;

            // UserManager
            UserManager.Instance.SetUserInfo(_user.UserId, _user.DisplayName);

            StartCoroutine(LoadUserName());
            StartCoroutine(LoadUserPassword());

            SceneManager.LoadScene("TitleScene");
        }
    }

    public void OnLogin()
    {
        SoundController.Instance.PlaySFX(0);
        StartCoroutine(Login(_emailLoginField.text, _passwordLoginField.text));
    }

    #endregion

    private IEnumerator SaveUserName()
    {
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

    private IEnumerator SaveUserEmail()
    {
        var DBTask = _dbRef.Child("users").Child(_user.UserId)
            .Child("Email").SetValueAsync(_emailRegisterField.text);
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

    private void SaveUserData(string userName, int maxPercent, string clearTime)
    {
        UserData user = new UserData(userName, maxPercent, clearTime);
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

    #region UI

    public void LoginPannelOn()
    {
        SoundController.Instance.PlaySFX(0);
        _loginPannel.SetActive(true);
        StartCoroutine(PannelMove(_loginTrm, true));
    }
    public void LoginPannelOff()
    {
        SoundController.Instance.PlaySFX(0);
        StartCoroutine(PannelMove(_loginTrm, false));
    }
    public void RegisterPannelOn()
    {
        SoundController.Instance.PlaySFX(0);
        _registerPannel.SetActive(true);
        StartCoroutine(PannelMove(_registerTrm, true));
    }
    public void RegisterPannelOff()
    {
        SoundController.Instance.PlaySFX(0);
        StartCoroutine(PannelMove(_registerTrm, false));
    }

    private IEnumerator PannelMove(RectTransform rectTrm, bool isOpen)
    {
        float endPosY = isOpen ? 0f : 1300f;
        Tween tween = rectTrm.DOAnchorPosY(endPosY, 0.5f);
        yield return tween.WaitForCompletion();

        if (!isOpen)
            rectTrm.gameObject.SetActive(false);
    }

    #endregion
}
