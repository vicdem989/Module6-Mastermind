
namespace MasterMind
{
    public enum GAME_TYPE : int
    {
        PLAYER_VS_NPC,
        PLAYER_VS_PLAYER
    }

    public enum COLORS : int
    {
        RED,
        YELLOW,
        GREEN,
        BLUE,
        MAGENTA,
        CYAN,
    }
    public class MasterMindGame : GameEngine.IScene
    {
        GAME_TYPE type;
        public Action<Type, object[]> OnExitScreen { get; set; }

        int[] solution;
        public MasterMindGame(GAME_TYPE gameType)
        {
            type = gameType;
        }

        #region GameEngine.IScene
        public void init()
        {
            if (type == GAME_TYPE.PLAYER_VS_NPC)
            {
                int[] colors = new[] { (int)COLORS.BLUE, (int)COLORS.CYAN, (int)COLORS.GREEN, (int)COLORS.MAGENTA };
                solution = CreateSequence(colors, 4, false);
                Console.WriteLine(string.Join(",", solution));
            }
        }

        public void input()
        {
            if (type == GAME_TYPE.PLAYER_VS_PLAYER)
            {

            }
            else
            {

            }
        }
        public void update()
        {

        }

        public void draw()
        {

        }

        #endregion

        private int[] CreateSequence(int[] source, int length = 4, bool duplicates = true)
        {
            List<int> sequence = new();
            Random rnd = new((int)DateTime.Now.Ticks);

            while (sequence.Count < length)
            {
                int index = rnd.Next(0, source.Length);
                int peg = source[index];

                if (duplicates == false)
                {
                    if (sequence.IndexOf(peg) == -1)
                    {
                        sequence.Add(peg);
                    }
                }
                else
                {
                    sequence.Add(peg);
                }
            }


            return sequence.ToArray();
        }

    }
}