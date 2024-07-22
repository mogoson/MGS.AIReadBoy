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
using MGS.WebRequest;

namespace MGS.Bookboy
{
    public class Writings
    {
        public event Action<float> OnUpdating;
        public event Action<ICollection<Writing>, Exception> OnUpdated;

        string cache;
        GithubCfg cfg;

        const float downRatio = 0.5f;

        public ICollection<Writing> WritingInfos { private set; get; }

        public Writings(string cache)
        {
            this.cache = cache;
            cfg = GithubConfig.LoadCfg();
        }

        public ICollection<Writing> Load(LoginData loginData)
        {
            WritingInfos = null;
            if (loginData != null && loginData.CheckValid())
            {
                WritingInfos = LoadWritings(loginData);
            }
            return WritingInfos;
        }

        public void UpdateAsync(LoginData loginData)
        {
            WritingInfos = null;
            if (loginData == null || !loginData.CheckValid())
            {
                return;
            }

            Download(loginData, OnDowned);
            void OnDowned(string file, Exception error)
            {
                if (error == null)
                {
                    UnZip(loginData, OnUnzipd);
                }
                else
                {
                    OnUpdated?.Invoke(null, error);
                }
            }
            void OnUnzipd(string dir, Exception error)
            {
                if (error == null)
                {
                    WritingInfos = LoadWritings(loginData);
                    OnUpdated?.Invoke(WritingInfos, null);
                }
                else
                {
                    OnUpdated?.Invoke(null, error);
                }
            }
        }

        void Download(LoginData loginData, Action<string, Exception> onComplete)
        {
            var url = string.Format(cfg.api_zipball, loginData.gitUser, loginData.gitRepo);
            var file = $"{cache}/{loginData.gitUser}/{loginData.gitRepo}.zip";
            var headers = new Dictionary<string, string>()
            {
                {"Authorization",$"Bearer {loginData.gitToken}"},
                {Headers.KEY_ACCEPT,cfg.api_accept},
                {"X-GitHub-Api-Version",cfg.api_version}
            };
            var handler = WebRequester.Handler.FileRequest(url, 120, file, headers);
            handler.OnProgress += progress => { OnUpdating?.Invoke(progress * downRatio); };
            handler.OnComplete += onComplete;
        }

        void UnZip(LoginData loginData, Action<string, Exception> onComplete)
        {
            onComplete?.Invoke(null, null);
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
            foreach (var entrie in entries)
            {
                if (!entrie.file)
                {
                    var chapters = new List<Chapter>();
                    foreach (var child in entrie.children)
                    {
                        var chapter = new Chapter()
                        {
                            Name = Path.GetFileNameWithoutExtension(entrie.path),

                        };
                        chapters.Add(chapter);
                    }
                    var writing = new Writing()
                    {
                        name = Path.GetFileNameWithoutExtension(entrie.path),
                        chapters = chapters,
                    };
                    writings.Add(writing);
                }
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