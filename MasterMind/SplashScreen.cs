using MasterMind;
using Utils;
using static Utils.HelperFunctions;

namespace MasterMind
{

    public class SplashScreen : GameEngine.IScene
    {
        const int MAX_RUNTIME = 100;
        const int TICKS_PER_FRAME = 2;
        const int MIN_COLOR = 16;
        const int MAX_COLOR = 231;
        const int EMPTY = -1;
        const int COLOR_DELTA = 1;
        const string art = """
                    ███    ███  █████  ███████ ████████ ███████ ██████  ███    ███ ██ ███    ██ ██████ 
                    ████  ████ ██   ██ ██         ██    ██      ██   ██ ████  ████ ██ ████   ██ ██   ██
██████ ██  ██ ████  ██ ████ ██ ███████ ███████    ██    █████   ██████  ██ ████ ██ ██ ██ ██  ██ ██   ██
  ██   ██████ ██    ██  ██  ██ ██   ██      ██    ██    ██      ██   ██ ██  ██  ██ ██ ██  ██ ██ ██   ██
  ██   ██  ██ ████  ██      ██ ██   ██ ███████    ██    ███████ ██   ██ ██      ██ ██ ██   ████ ██████ 
""";
        int tickCount = 0;
        int totalTuntime = 0;
        bool dirty = false;
        char[][] artArray;
        int[][] colorMatrix;
        int startY = 0;
        int startX = 0;
        string outputGraphics = "";
        public Action<Type, object[]> OnExitScreen { get; set; }

        public void init()
        {
            Console.Clear();
            artArray = Create2DArrayFromMultiLineString(art);
            colorMatrix = CreateColorMapFrom(artArray, COLOR_DELTA, MIN_COLOR, MAX_COLOR);

            startY = (int)((Console.WindowHeight - colorMatrix.Length) * 0.25);
            startX = (int)((Console.WindowWidth - colorMatrix[0].Length) * 0.5);

            tickCount = TICKS_PER_FRAME;
        }

        public void input()
        {
        }

        public void update()
        {
            totalTuntime++;

            if (tickCount == TICKS_PER_FRAME)
            {
                colorMatrix = UpdateColors(colorMatrix, COLOR_DELTA, MIN_COLOR, MAX_COLOR);
                outputGraphics = RenderGraphics(artArray, colorMatrix, startX, startY);
                dirty = true;
            }
            else
            {
                tickCount++;
            }

            if (totalTuntime >= MAX_RUNTIME)
            {
                OnExitScreen(typeof(MenuScreen), null);
            }
        }

        public void draw()
        {
            if (dirty)
            {
                dirty = false;
                Console.Clear();
                Console.WriteLine(outputGraphics);
            }
        }

        int[][] UpdateColors(int[][] colors, int delta, int minColor, int maxColor)
        {
            for (int row = 0; row < colors.Length; row++)
            {
                for (int col = 0; col < colors[row].Length; col++)
                {
                    int color = colors[row][col];

                    if (color != EMPTY)
                    {
                        color += delta;
                        color = WrappAround(color, minColor, maxColor);
                        colors[row][col] = color;
                    }
                }
            }

            return colors;
        }

        public string RenderGraphics(char[][] source, int[][] colors, int x, int y)
        {
            string output = "";
            for (int row = 0; row < colors.Length; row++)
            {
                output += ANSICodes.Positioning.SetCursorPos(y + row, x);

                for (int col = 0; col < colors[row].Length; col++)
                {
                    int color = colors[row][col];
                    if (color == EMPTY)
                    {
                        output += $"{source[row][col]}";
                    }
                    else
                    {
                        output += $"\u001b[38;5;{color}m{source[row][col]}\u001b[0m";
                    }
                }
            }
            return output;
        }

        int[][] CreateColorMapFrom(char[][] source, int delta, int minColor, int maxColor)
        {
            int[][] cmap = new int[source.Length][];

            int currentColor = minColor;
            for (int row = 0; row < source.Length; row++)
            {
                cmap[row] = new int[source[row].Length];

                for (int col = 0; col < source[row].Length; col++)
                {
                    char cellValue = source[row][col];
                    if (cellValue != ' ')
                    {
                        cmap[row][col] = currentColor;
                    }
                    else
                    {
                        cmap[row][col] = EMPTY;
                    }
                    currentColor += delta;
                    currentColor = WrappAround(currentColor, minColor, maxColor);
                }
            }

            return cmap;
        }

    }

}