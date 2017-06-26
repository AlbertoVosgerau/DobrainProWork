using System.Collections.Generic;

namespace Com.Dobrain.Dobrainproject.Content.Question
{
    public static class LineList
    {
        public static List<Line> lines = new List<Line>();
        public static void AddLine(Line line, Graph answer, Graph correctAnswer)
        {
            int start = line.start.DotId;
            int end = line.end.DotId;
            lines.Add(line);
            answer.SetGraph(start, end);
            if (correctAnswer.checkLine(start, end))
            {
                line.IsAnswerLine = true;
                line.start.Fix = true;
                line.end.Fix = true;
            }
            Render();
        }
        public static void Render()
        {
            foreach (Line line in lines)
            {
                line.Render();
            }
        }
        public static void Clear()
        {
            lines.Clear();
        }
        public static List<Line> Get()
        {
            return lines;
        }
        public static int Count()
        {
            return lines.Count;
        }
    }
}
