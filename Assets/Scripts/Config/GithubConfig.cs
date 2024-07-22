/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  GithubConfig.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/22
 *  Description  :  Initial development version.
 *************************************************************************/

using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace MGS.Bookboy
{
    public class GithubCfg
    {
        public string api_accept;
        public string api_version;
        public string api_zipball;
    }

    public sealed class GithubConfig
    {
        static readonly string CGF_FILE = $"{Application.streamingAssetsPath}/Config/Github.json";

        public static GithubCfg LoadCfg()
        {
            var json = File.ReadAllText(CGF_FILE);
            return JsonConvert.DeserializeObject<GithubCfg>(json);
        }
    }
}