namespace Revenaant.Project.Messages
{
    public enum GameOverType
    {
        OutOfTime,
        OutOfMoves
    }

    public class GameOverMessage : IMessage
    {
        private readonly GameOverType gameOverType;
        public GameOverType GameOverType => gameOverType;

        public GameOverMessage(GameOverType gameOverType)
        {
            this.gameOverType = gameOverType;
        }
    }
}
