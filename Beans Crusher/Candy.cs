using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public BonusType Bonus { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public string Type { get; set; }
    
    public Candy()
    {
        Bonus = BonusType.None;
    }

    public void Initialize(string type, int row, int column)
    {
        Column = column;
        Row = row;
        Type = type;
    }

    public static void SwapRowColumn(Candy c1, Candy c2)
    {
        int temp = c1.Row;
        c1.Row = c2.Row;
        c2.Row = temp;

        temp = c1.Column;
        c1.Column = c2.Column;
        c2.Column = temp;
    }

    public bool IsSameType(Candy otherCandy)
    {
        // string.Compare�� �� ���ڿ��� ������ 0 ��ȯ
        // ����°���ڿ� true������ ��ҹ��� ����, false������ ��ҹ��� ����
        return string.Compare(this.Type, otherCandy.Type) == 0;
    }
}
