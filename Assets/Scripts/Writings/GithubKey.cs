/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  GithubKey.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/22
 *  Description  :  Initial development version.
 *************************************************************************/

namespace MGS.AIReadBoy
{
    public sealed class GithubKey
    {
        public const string API_ACCEPT = "application/vnd.github+json";
        public const string API_VERSION = "2022-11-28";
        public const string API_ZIPBALL = "https://api.github.com/repos/{0}/{1}/zipball";
    }
}