/*************************************************************************
 *  Copyright (C) 2024 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  Writing.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0.0
 *  Date         :  2024/7/22
 *  Description  :  Initial development version.
 *************************************************************************/

using System.Collections.Generic;

namespace MGS.AIReadBoy
{
    public class Writing
    {
        public string name;
        public string description;
        public string preface;
        public ICollection<Chapter> chapters;
    }

    public class Chapter
    {
        public string Name;
        public string Description;
        public ICollection<Piece> Pieces;
    }

    public class Piece
    {
        public string Name;
        public string File;
    }
}