/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  Writings.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/21
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using MGS.UGUI;
using MGS.WebRequest;
using MGS.Zip;

namespace MGS.AIReadBoy
{
    public class Writings
    {
        public ICollection<Writing> WritingInfos { private set; get; }
        string cache;
        LoginData loginData;

        UIDialog dialogUI;
        WritingsUI writingsUI;

        public Writings(string cache)
        {
            this.cache = cache;

            dialogUI = UnityEngine.Object.FindObjectOfType<UIDialog>(true);
            writingsUI = UnityEngine.Object.FindObjectOfType<WritingsUI>(true);
            writingsUI.OnClickUserEvent += WritingsUI_OnClickUserEvent;
        }

        private void WritingsUI_OnClickUserEvent()
        {
            Update(loginData);
        }

        public void Refresh(LoginData loginData)
        {
            this.loginData = loginData;
            WritingInfos = LoadWritings(loginData);
            writingsUI.Refresh(WritingInfos);
            writingsUI.Refresh(loginData);
            writingsUI.ToggleActive();
        }

        void Update(LoginData loginData)
        {
            if (loginData == null || !loginData.CheckValid())
            {
                return;
            }

            OnUpdateStart();
            Download(loginData, OnDowned);
            void OnDowned(string file, Exception error)
            {
                if (error == null)
                {
                    UnZip(loginData, OnUnzipd);
                }
                else
                {
                    OnUpdateError(error);
                }
            }
            void OnUnzipd(string dir, Exception error)
            {
                if (error == null)
                {
                    OnUpdateFinished(loginData);
                }
                else
                {
                    OnUpdateError(error);
                }
            }
        }

        void Download(LoginData loginData, Action<string, Exception> onComplete)
        {
            var url = string.Format(GithubKey.API_ZIPBALL, loginData.gitUser, loginData.gitRepo);
            var file = $"{cache}/{loginData.gitUser}/{loginData.gitRepo}.zip";
            var headers = new Dictionary<string, string>()
            {
                {"Authorization",$"Bearer {loginData.gitToken}"},
                {Headers.KEY_ACCEPT,GithubKey.API_ACCEPT},
                {"X-GitHub-Api-Version",GithubKey.API_VERSION}
            };
            var handler = WebRequester.Handler.FileRequest(url, 120, file, headers);
            handler.OnComplete += onComplete;
        }

        void UnZip(LoginData loginData, Action<string, Exception> onComplete)
        {
            var file = $"{cache}/{loginData.gitUser}/{loginData.gitRepo}.zip";
            var dir = $"{cache}/{loginData.gitUser}/{loginData.gitRepo}";
            var operate = Zipper.Handler.UnzipAsync(file, dir);
            operate.OnComplete += onComplete;
        }

        void OnUpdateStart()
        {
            writingsUI.ToggleLoading(true);
        }

        void OnUpdateFinished(LoginData loginData)
        {
            writingsUI.ToggleLoading(false);
            Refresh(loginData);
        }

        void OnUpdateError(Exception error)
        {
            writingsUI.ToggleLoading(false);
            var options = new UIDialogOptions()
            {
                tittle = "Refresh Error",
                closeButton = true,
                content = error.Message,
                yesButton = "OK"
            };
            dialogUI.Refresh(options);
            dialogUI.ToggleActive();
        }

        ICollection<Writing> LoadWritings(LoginData loginData)
        {
            var dir = $"{cache}/{loginData.gitUser}/{loginData.gitRepo}";
            if (!Directory.Exists(dir))
            {
                return null;
            }

            var writings = new List<Writing>();
            var entries = FileSystem.GetEntries(dir);
            foreach (var entry in entries)
            {
                var ws = LoadWritings(entry.children);
                writings.AddRange(ws);
            }
            return writings;
        }

        ICollection<Writing> LoadWritings(ICollection<Entry> entries)
        {
            if (entries == null)
            {
                return null;
            }

            var writings = new List<Writing>();
            foreach (var entry in entries)
            {
                if (!entry.file)
                {
                    var writing = new Writing()
                    {
                        name = Path.GetFileNameWithoutExtension(entry.path),
                        chapters = LoadChapter(entry)
                    };
                    writings.Add(writing);
                }
            }
            return writings;
        }

        ICollection<Chapter> LoadChapter(Entry entry)
        {
            if (entry.children == null)
            {
                return null;
            }

            var chapters = new List<Chapter>();
            foreach (var child in entry.children)
            {
                if (!child.file)
                {
                    var chapter = new Chapter()
                    {
                        Name = Path.GetFileNameWithoutExtension(child.path),
                        Pieces = LoadPieces(child)
                    };
                    chapters.Add(chapter);
                }
            }
            return chapters;
        }

        ICollection<Piece> LoadPieces(Entry entry)
        {
            if (entry.children == null)
            {
                return null;
            }

            var pieces = new List<Piece>();
            foreach (var child in entry.children)
            {
                if (child.file)
                {
                    var piece = new Piece()
                    {
                        Name = Path.GetFileNameWithoutExtension(child.path),
                        File = child.path
                    };
                    pieces.Add(piece);
                }
            }
            return pieces;
        }
    }
}