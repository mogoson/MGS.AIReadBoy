/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  LoginData.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/22
 *  Description  :  Initial development version.
 *************************************************************************/

using System;

namespace MGS.AIReadBoy
{
    [Serializable]
    public class LoginData
    {
        public string gitUser;
        public string gitRepo;
        public string gitToken;
        public string qwKey;

        public bool CheckValid()
        {
            return !(string.IsNullOrEmpty(gitUser) || string.IsNullOrEmpty(gitRepo)
                || string.IsNullOrEmpty(gitToken) || string.IsNullOrEmpty(qwKey));
        }
    }
}