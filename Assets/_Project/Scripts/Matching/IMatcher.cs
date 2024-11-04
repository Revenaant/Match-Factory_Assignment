using System;

namespace Revenaant.Project
{
    internal interface IMatcher
    {
        bool IsMatch(Guid idToCheck);
        bool IsMatch(IMatcher otherMatcher);
        Guid MatchID { get; }
    }
}
