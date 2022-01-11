using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour ������� �� Ŭ������ ��üȭ �� Matrix[row, column]�� �׻� null �߹Ƿ� ���������
// ������Ƽ�θ� �� Ŭ�������� �����ִ°� ������
public class PuzzlePiece
{
    public int CurrentRow { get; set; }
    public int CurrentColumn { get; set; }

    public int OriginalRow { get; set; }
    public int OriginalColumn { get; set; }

    public GameObject obj { get; set; }
}
