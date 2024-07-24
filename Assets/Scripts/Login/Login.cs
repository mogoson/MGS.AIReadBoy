/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  Login.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/21
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.IO;
using MGS.QianWen;
using MGS.UGUI;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace MGS.AIReadBoy
{
    public class Login
    {
        public LoginData LoginData { private set; get; }
        string loginFile;

        LoginUI LoginUI;
        UIDialog dialogUI;

        Action<LoginData> onLogined;

        public Login(string cache)
        {
            loginFile = $"{cache}/Login.json";
            LoginUI = UnityEngine.Object.FindObjectOfType<LoginUI>(true);
            LoginUI.OnChangedEvent += LoginUI_OnChangedEvent;
            dialogUI = UnityEngine.Object.FindObjectOfType<UIDialog>(true);
            LoginUI.RefreshAgreement(LoadAgreement());
        }

        private void LoginUI_OnChangedEvent(LoginData data)
        {
            CheckLoginData(data, OnFinished);
            void OnFinished(bool succed, Exception error)
            {
                if (succed)
                {
                    LoginUI.ToggleActive(false);
                    LoginData = data;
                    UpdateLoginData(LoginData);
                    onLogined?.Invoke(LoginData);
                }
                else
                {
                    var options = new UIDialogOptions()
                    {
                        tittle = "Login Error",
                        closeButton = true,
                        content = error.Message,
                        yesButton = "OK"
                    };
                    dialogUI.Refresh(options);
                    dialogUI.ToggleActive();
                }
            }
        }

        void CheckLoginData(LoginData data, Action<bool, Exception> finished)
        {
            LoginUI.ToggleLoading(true);
            CheckGitInfo();

            void CheckGitInfo()
            {
                new GitArchive("").Request(data, 120, OnGitRequested);
            }
            void OnGitRequested(string result, Exception error)
            {
                if (error == null)
                {
                    CheckQianWenKey();
                }
                else
                {
                    LoginUI.ToggleLoading(false);
                    finished?.Invoke(false, error);
                }
            }
            void CheckQianWenKey()
            {
                var hub = new QianWenHub(data.qwKey);
                var dialog = hub.NewTextDialog();
                dialog.OnRespond += OnQWRespond;
                dialog.Quest("Test");
            }
            void OnQWRespond(string result, Exception error)
            {
                LoginUI.ToggleLoading(false);
                if (error == null)
                {
                    finished.Invoke(true, null);
                }
                else
                {
                    finished?.Invoke(false, error);
                }
            }
        }

        public void LogIn(Action<LoginData> onLogined)
        {
            LoginData = LoadLoginData(loginFile);
            if (LoginData.CheckValid())
            {
                onLogined?.Invoke(LoginData);
            }
            else
            {
                this.onLogined = onLogined;
                LoginUI.Refresh(LoginData);
                LoginUI.ToggleActive();
            }
        }

        public void LogOut()
        {
            LoginData = new LoginData();
        }

        LoginData LoadLoginData(string loginFile)
        {
            if (File.Exists(loginFile))
            {
                var json = File.ReadAllText(loginFile);
                return JsonConvert.DeserializeObject<LoginData>(json);
            }
            return new LoginData();
        }

        void UpdateLoginData(LoginData data)
        {
            var json = string.Empty;
            if (data != null)
            {
                json = JsonConvert.SerializeObject(data);
            }
            File.WriteAllText(loginFile, json);
        }

        string LoadAgreement()
        {
            var file = $"{Application.streamingAssetsPath}/User Agreement";
            using (var request = UnityWebRequest.Get(file))
            {
                var operate = request.SendWebRequest();
                while (!operate.isDone) { }
                return request.downloadHandler.text;
            }
        }
    }
}