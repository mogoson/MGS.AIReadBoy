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
using Newtonsoft.Json;

namespace MGS.AIReadBoy
{
    public class Login
    {
        public LoginData LoginData { private set; get; }
        string loginFile;

        LoginUI LoginUI;

        Action<LoginData> onLogined;

        public Login(string cache)
        {
            loginFile = $"{cache}/Login.json";
            LoginUI = UnityEngine.Object.FindObjectOfType<LoginUI>(true);
            LoginUI.OnChangedEvent += LoginUI_OnChangedEvent;
        }

        private void LoginUI_OnChangedEvent(LoginData data)
        {
            LoginData = data;
            UpdateLoginData(LoginData);
            onLogined?.Invoke(LoginData);
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
    }
}