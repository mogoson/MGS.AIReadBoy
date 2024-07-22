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

using System.IO;
using Newtonsoft.Json;

namespace MGS.AIReadBoy
{
    public class Login
    {
        public LoginData LoginData { private set; get; }
        string loginFile;

        public Login(string cache)
        {
            loginFile = $"{cache}/Login.json";
        }

        public LoginData Load()
        {
            LoginData = LoadLoginData(loginFile);
            return LoginData;
        }

        public void LogIn(LoginData data)
        {
            LoginData = data;
            UpdateLoginData(data);
        }

        public void LogOut()
        {
            LoginData = null;
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

        public void UpdateLoginData(LoginData data)
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