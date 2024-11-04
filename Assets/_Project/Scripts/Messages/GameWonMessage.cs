namespace Revenaant.Project.Messages
{
    public class GameWonMessage : IMessage
    {
        private int stars;
        private bool isFullClear;

        public int Stars => stars;
        public bool IsFullClear => isFullClear;

        public GameWonMessage(int stars, bool isFullClear)
        {
            this.stars = stars;
            this.isFullClear = isFullClear;
        }
    }
}
