/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  GitArchive.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/24
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;
using MGS.WebRequest;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace MGS.AIReadBoy
{
    public class GitArchive
    {
        string cache;
        static GithubApi githubApi;

        static GitArchive()
        {
            githubApi = LoadGithubApi();
        }

        public GitArchive(string cache)
        {
            this.cache = cache;
        }

        public void Request(LoginData loginData, int timeout, Action<string, Exception> onComplete)
        {
            var url = string.Format(githubApi.api_zipball, loginData.gitUser, loginData.gitRepo);
            var headers = BuildHeaders(loginData);
            var handler = WebRequester.Handler.GetRequest(url, timeout, headers);
            handler.OnComplete += onComplete;
        }

        public void Download(LoginData loginData, int timeout, Action<string, Exception> onComplete)
        {
            var url = string.Format(githubApi.api_zipball, loginData.gitUser, loginData.gitRepo);
            var file = $"{cache}/{loginData.gitUser}/{loginData.gitRepo}.zip";
            var headers = BuildHeaders(loginData);
            var handler = WebRequester.Handler.FileRequest(url, timeout, file, headers);
            handler.OnComplete += onComplete;
        }

        IDictionary<string, string> BuildHeaders(LoginData loginData)
        {
            return new Dictionary<string, string>()
            {
                {"Authorization",$"Bearer {loginData.gitToken}"},
                {Headers.KEY_ACCEPT,githubApi.api_accept},
                {"X-GitHub-Api-Version",githubApi.api_version}
            };
        }

        static GithubApi LoadGithubApi()
        {
            var file = $"{Application.streamingAssetsPath}/Github Api";
            using (var request = UnityWebRequest.Get(file))
            {
                var operate = request.SendWebRequest();
                while (!operate.isDone) { }
                var json = request.downloadHandler.text;
                return JsonConvert.DeserializeObject<GithubApi>(json);
            }
        }

        class GithubApi
        {
            public string api_accept;
            public string api_version;
            public string api_zipball;
        }
    }
}