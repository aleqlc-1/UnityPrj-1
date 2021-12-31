using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ʈ�� ������ ��ũ��Ʈ�� �ƴϹǷ� MonoBehaviour��ӹ��� �ʾƵ���
public class MatrixGrid// : MonoBehaviour
{
    public static int row = 10;
    public static int column = 20;

    public static Transform[,] grid = new Transform[row, column];

    public static Vector2 RoundVector(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static bool IsInsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < row && (int)pos.y >= 0);
    }

    // �� �� �����ϴ� �޼���
    public static void DeleteRow(int y)
    {
        for (int x = 0; x < row; ++x) // ++x
        {
            GameObject.Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    // ���� ���������ϴ� �޼���
    public static void DecreaseRow(int y)
    {
        for (int x = 0; x < row; ++x) // ++x
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                grid[x, y - 1].position += new Vector3(0, -1, 0); // ������ ��ġ�� ����
            }
        }
    }

    // �������� �Ǵ� ���� ���� ���� ���� ��ĭ�� ������ �޼���
    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < column; ++i) // ++i
        {
            DecreaseRow(i);
        }
    }

    // �� ���� ������ �����ؾ��ϴ��� �Ǵ��ϴ� �޼���
    public static bool IsRowFull(int y)
    {
        for (int x = 0; x < row; ++x) // ++x
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    public static void DeleteWholeRows()
    {
        for (int y = 0; y < column; ++y) // ++y
        {
            if (IsRowFull(y)) // ���� ��á����
            {
                DeleteRow(y); // �� �� �����ϰ�
                DecreaseRowsAbove(y + 1); // �� ���� ��θ� ��ĭ�� ����
                --y; // ���� ��������Ƿ� ������ -1 ����
            }
        }
    }
}
