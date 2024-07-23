/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  WritingsUI.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/22
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;
using MGS.UGUI;
using UnityEngine.UI;

namespace MGS.AIReadBoy
{
    public class WritingsUI : UIRespondable<ICollection<Writing>, Writing>
    {
        public event Action OnClickUserEvent;

        public Button btnUser;
        public Text txtUser;
        public UIButtonCollector collector;
        public FilledLoading loading;

        protected ICollection<Writing> data;

        private void Awake()
        {
            btnUser.onClick.AddListener(BtnUser_OnClick);
            collector.OnCellClickEvent += Collector_OnCellClickEvent;
        }

        void BtnUser_OnClick()
        {
            OnClickUserEvent?.Invoke();
        }

        private void Collector_OnCellClickEvent(UIButtonCell cell)
        {
            Writing writing = null;
            foreach (var item in data)
            {
                if (item.name == cell.text.text)
                {
                    writing = item;
                    break;
                }
            }
            OnChanged(writing);
        }

        protected override void OnRefresh(ICollection<Writing> data)
        {
            this.data = data;
            var options = new List<UIButtonOptions>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    var opt = new UIButtonOptions { text = item.name };
                    options.Add(opt);
                }
            }
            collector.Refresh(options);
        }

        public void Refresh(LoginData data)
        {
            txtUser.text = data.gitUser.Substring(0, 1);
        }

        public void ToggleLoading(bool isActive)
        {
            loading.gameObject.SetActive(isActive);
        }
    }
}