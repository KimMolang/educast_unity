﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Unity;

using Firebase.Auth;

/*
 * #if UNITY_EDITOR
 * FirebaseApp.DefaultInstance.SetEditorDatabaseUrl
 * #endif
 */
using Firebase.Unity.Editor;


public class DataManager
{
    // 순위 정보를 담는 Rank 클래스를 작성합니다.
    class Rank
    {
        // Firebase 와 동일하게 작성
        // JsonUtility 를 이용하여 객체를 Jason 문자열로 바꿀 땐 Property를 사용하면 안된다.
        // Property를 사용하면 객체의 속성 값을 못 가져온다.
        public string name; //{ get; set; }
        public int score; //{ get; set; }
        public int timestamp; //{ get; set; }

        public Rank(string name, int score, int timestamp)
        {
            this.name = name;
            this.score = score;
            this.timestamp = timestamp;
        }
    }

    public DatabaseReference reference { get; set; }

    public void Init()
    {
        // DB 경로를 설정해 instance를 초기화합니다.
        // 어떤 경로가 DB가 존재하는 경로인지 설정
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(
           "https://unitygameserver-eb5a5.firebaseio.com/");
    }

    public void PrintData()
    {
        // rank 데이터 셋에 접근
        reference = FirebaseDatabase.DefaultInstance.GetReference("rank");

        // 데이터 셋의 모든 데이터를 JSON 형태로 가져옴 (비동기로 가져옴)
        reference.GetValueAsync().ContinueWith(task =>
        {
            // 가져오고자 하는 데이터가 많을 경우
            // 이 부분이 오랫동안 시간이 소요될 수 있다.
            // 그러면 다음 코드 실행이 늦을 수 있기에 비동기로 가져온다.

            // 성공적으로 데이터를 가져온 경우
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // JSON 데이터의 각 원소에 접근할 수 있다.
                foreach (DataSnapshot data in snapshot.Children)
                {
                    // 해당 원소의 정보를 출력
                    IDictionary rank = (IDictionary)data.Value;
                    Debug.Log("Name : " + rank["name"] + " / Score : " + rank["score"]);
                }
            }
        });
    }

    public void PushData()
    {
        // 해당 DB의 특정 지점을 가리킬 수 있다. RootReference를 가리키게 한다.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        // 예시 데이터 생성
        Rank rank = new Rank("최용숙", 99, 1553588214);

        // 예시 데이터를 JSON 형태로 변환
        string json = JsonUtility.ToJson(rank);

        Debug.Log(json);

        // 데이터를 삽입하기 위해 별도의 ID, Key 값 생성
        string key = reference.Child("rank").Push().Key;

        // 생성된 키 값의 자식으로 예시 데이터를 삽입
        reference.Child("rank").Child(key).SetRawJsonValueAsync(json);
    }
}

// ref : https://firebase.google.com/docs/auth/unity/password-auth?hl=ko
public class LoginManager
{
    // Firebase 인증 기능 객체
    private FirebaseAuth auth;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Join(string email, string password)
    {

        // 인증객체를 이용해 이메일과 비밀번호로 회원가입 수행
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser user = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                user.DisplayName, user.UserId);
            Debug.Log("[회원가입 성공] email : " + email);
        });
    }

    public void Login(string email, string password)
    {
        // 인증객체를 이용해 이메일과 비밀번호로 회원가입 수행
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }


            Firebase.Auth.FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);
            Debug.Log("[로그인 성공] 이메일 : " + user.Email);
        });
    }
}

public class GameManager : MonoBehaviour
{
    private DataManager dataManager;
    private LoginManager loginManager;

    private void Start()
    {
        dataManager = new DataManager();
        dataManager.Init();
        dataManager.PrintData();

        loginManager = new LoginManager();
        loginManager.Init();
        //loginManager.Join ("testID_01@gmail.com", "123123");
        loginManager.Login("testID_01@gmail.com", "1231");
    }
}