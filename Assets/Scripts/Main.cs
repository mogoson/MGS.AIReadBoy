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

using System;
using System.Collections.Generic;
using MGS.UGUI;
using UnityEngine;
using UnityEngine.UI;

namespace MGS.AIReadBoy
{
    public class Main : MonoBehaviour
    {
        public UIHandler uiHandler;
        public Button loadBtn;
        public Button updateBtn;

        Login login;
        Writings writings;

        private void Awake()
        {
            InitLogicModule();
            InitUIModule();


        }

        private void Start()
        {
            CheckLogin();
        }

        void InitLogicModule()
        {
            var cache = Application.persistentDataPath;
            login = new Login(cache);
            writings = new Writings(cache);
            writings.OnUpdating += OnWritingsUpdating;
            writings.OnUpdated += OnWritingsUpdated; ;
        }

        void OnWritingsUpdated(ICollection<Writing> writings, Exception error)
        {
            if (error == null)
            {
                uiHandler.writingsUI.Refresh(writings);
            }
            else
            {
                Debug.LogError(error);
                var options = new UIDialogOptions()
                {
                    tittle = "Error",
                    closeButton = true,
                    content = error.Message,
                    yesButton = "OK"
                };
                uiHandler.uiDialog.Refresh(options);
                uiHandler.uiDialog.ToggleActive();
            }
        }

        void OnWritingsUpdating(float progress)
        {
            Debug.Log($"progress: {progress}");
        }

        void InitUIModule()
        {
            uiHandler.loginUI.OnChangedEvent += OnLoginChanged;
            uiHandler.writingsUI.OnChangedEvent += OnWritingsChanged;
        }

        void OnWritingsChanged(ICollection<Writing> writings)
        {

        }

        void OnLoginChanged(LoginData data)
        {
            login.LogIn(data);
            writings.UpdateAsync(data);
        }

        void CheckLogin()
        {
            var data = login.Load();
            uiHandler.loginUI.Refresh(data);
            uiHandler.loginUI.ToggleActive();
        }
    }
}