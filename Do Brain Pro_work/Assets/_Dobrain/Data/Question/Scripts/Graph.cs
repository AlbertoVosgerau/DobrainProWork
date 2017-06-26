using UnityEngine;

public class Graph
{
    private bool[][] elements;
    private int size;
    public bool[][] Elements
    {
        get { return elements; }
    }
    public int Size
    {
        get { return size; }
        set { size = value; }
    }
    public Graph(int size)
    {
        this.size = size;
        elements = new bool[size][];
        for (int i = 0; i < size; i++)
        {
            elements[i] = new bool[size];
            for (int j = 0; j < size; j++)
            {
                elements[i][j] = false;
            }
        }
    }
    public Graph(int size, string[] answerGraph)
    : this(size)
    {
        int start = 0, end = 0;
        for (int i = 0; i < answerGraph.Length; i++)
        {
            int ascii_front = answerGraph[i][0].ToString().ToCharArray()[0];
            int ascii_tail = answerGraph[i][1].ToString().ToCharArray()[0];

            if (ascii_front > 64)
                start = ascii_front - 55;
            else
                start = int.Parse(answerGraph[i][0].ToString());

            if (ascii_tail > 64)
                end = ascii_tail - 55;
            else
                end = int.Parse(answerGraph[i][1].ToString());

            elements[start][end] = true;
            elements[end][start] = true;
        }
    }
    public void SetGraph(int start, int end)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if ((i == start && j == end) || (i == end && j == start))
                {
                    elements[i][j] = true;
                }
            }
        }
    }

    public void ResetGraph(Graph correctAnswer)
    {
        if (size != correctAnswer.Size)
        {
            Debug.Log("Exception: size diffrent exception");
            return;
        }
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (elements[i][j] != correctAnswer.Elements[i][j])
                {
                    elements[i][j] = false;
                }
            }
        }
    }
    public bool isEqual(Graph other)
    {
        int size1 = this.size;
        int size2 = other.Size;
        if (size1 != size2)
        {
            return false;
        }
        for (int i = 0; i < size1; i++)
        {
            for (int j = 0; j < size2; j++)
            {
                if (this.elements[i][j] != other.Elements[i][j])
                {
                    return false;
                }
            }
        }
        return true;
    }
    public bool checkLine(int start, int end)
    {
        return elements[start][end];
    }
}