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
using UnityEngine;
using UnityEngine.UI;

namespace MGS.AIReadBoy
{
    public class LoginUI : UIRespondable<LoginData, LoginData>
    {
        public InputField gitUser;
        public InputField gitRepo;
        public InputField gitToken;
        public InputField qwKey;
        public Toggle togAccept;
        public Button btnLicense;
        public Button btnConfirm;

        [Space]
        public GameObject license;
        public Button btnBack;
        public Button btnAccept;

        protected virtual void Awake()
        {
            gitUser.onValueChanged.AddListener(text => { Data.gitUser = text; CheckInteractable(); });
            gitRepo.onValueChanged.AddListener(text => { Data.gitRepo = text; CheckInteractable(); });
            gitToken.onValueChanged.AddListener(text => { Data.gitToken = text; CheckInteractable(); });
            qwKey.onValueChanged.AddListener(text => { Data.qwKey = text; CheckInteractable(); });

            togAccept.onValueChanged.AddListener(accept => CheckInteractable());
            btnLicense.onClick.AddListener(() => license.gameObject.SetActive(true));
            btnConfirm.onClick.AddListener(() => { OnChanged(Data); ToggleActive(false); });

            btnBack.onClick.AddListener(() => license.gameObject.SetActive(false));
            btnAccept.onClick.AddListener(() => { togAccept.isOn = true; license.gameObject.SetActive(false); });
        }

        protected void CheckInteractable()
        {
            btnConfirm.interactable = Data.CheckValid() && togAccept.isOn;
        }

        protected override void OnRefresh(LoginData data)
        {
            gitUser.text = data.gitUser;
            gitRepo.text = data.gitRepo;
            gitToken.text = data.gitToken;
            qwKey.text = data.qwKey;
            CheckInteractable();
        }

        public void SetAccept(bool accept)
        {
            togAccept.isOn = accept;
            CheckInteractable();
        }
    }
}