using System;

namespace Revenaant.Project.Messages
{
    public class MatchMadeMessage : IMessage
    {
        private int matchSize;
        private Guid matchObjectType;

        public int MatchSize => matchSize;
        public Guid MatchObjectType => matchObjectType;

        public MatchMadeMessage(int matchSize, Guid matchObjectType)
        {
            this.matchSize = matchSize;
            this.matchObjectType = matchObjectType;
        }
    }
}
