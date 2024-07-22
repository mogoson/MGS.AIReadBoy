/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  LoginUI.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/21
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.UGUI;
using UnityEngine.UI;

namespace MGS.Bookboy
{
    public class LoginUI : UIRespondable<LoginData, LoginData>
    {
        public InputField gitUser;
        public InputField gitRepo;
        public InputField gitToken;
        public InputField qwKey;
        public Button confirm;

        protected override void Awake()
        {
            gitUser.onValueChanged.AddListener(text => { Data.gitUser = text; CheckInteractable(); });
            gitRepo.onValueChanged.AddListener(text => { Data.gitRepo = text; CheckInteractable(); });
            gitToken.onValueChanged.AddListener(text => { Data.gitToken = text; CheckInteractable(); });
            qwKey.onValueChanged.AddListener(text => { Data.qwKey = text; CheckInteractable(); });
            confirm.onClick.AddListener(() => { OnChanged(Data); ToggleActive(false); });
        }

        protected void CheckInteractable()
        {
            confirm.interactable = Data.CheckValid();
        }

        protected override void OnRefresh(LoginData data)
        {
            gitUser.text = data.gitUser;
            gitRepo.text = data.gitRepo;
            gitToken.text = data.gitToken;
            qwKey.text = data.qwKey;
            CheckInteractable();
        }
    }
}