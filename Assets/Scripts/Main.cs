/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  Main.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/21
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEngine;

namespace MGS.AIReadBoy
{
    public class Main : MonoBehaviour
    {
        private Login login;
        private Writings writings;

        private void Awake()
        {
            var cache = Application.persistentDataPath;
            login = new Login(cache);
            writings = new Writings(cache);
        }

        private void Start()
        {
            login.LogIn(OnLogined);
        }

        private void OnLogined(LoginData data)
        {
            writings.Refresh(data);
        }
    }
}