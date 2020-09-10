﻿using SramCommons.Models;

namespace SramComparer.Helpers
{
    public interface ISramComparer<in TSramFile, TSramGame>
        where TSramFile : SramFileBase, ISramFile<TSramGame>
        where TSramGame : struct
    {
        void CompareSram(TSramFile currFile, TSramFile compFile, IOptions options);
    }
}