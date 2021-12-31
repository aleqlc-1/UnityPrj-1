using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisObject : MonoBehaviour
{
    private float lastFall = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // ���̵�
        {
            transform.position += new Vector3(-1, 0, 0);

            if (IsValidGridPosition()) // ��ȿ�ϸ�
            {
                UpdateMatrixGrid(); // update
            }
            else
            {
                transform.position += new Vector3(1, 0, 0); // ����ġ��
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) // ���̵�
        {
            transform.position += new Vector3(1, 0, 0);

            if (IsValidGridPosition()) // ��ȿ�ϸ�
            {
                UpdateMatrixGrid(); // update
            }
            else // ��ȿ���� ������
            {
                transform.position += new Vector3(-1, 0, 0); // ����ġ��
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) // ȸ��
        {
            transform.Rotate(new Vector3(0, 0, -90));

            if (IsValidGridPosition()) // ��ȿ�ϸ�
            {
                UpdateMatrixGrid(); // update
            }
            else // ��ȿ���� ������
            {
                transform.Rotate(new Vector3(0, 0, 90)); // ����ȸ��������
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1) // �Ʒ�Ű ���� �Ǵ� 1�ʸ��� ���� ������, ==�־ȵ���
        {
            transform.position += new Vector3(0, -1, 0);
            
            if (IsValidGridPosition()) // ��ȿ�ϸ�
            {
                UpdateMatrixGrid(); // update
            }
            else // ��ȿ���� ������(�� �̻� ������ �� ������)
            {
                transform.position += new Vector3(0, 1, 0); // ����ġ��

                MatrixGrid.DeleteWholeRows(); // �� �����ؾ��ϴ��� üũ

                enabled = false; // �� ������ ������ ��ũ��Ʈ�� ��Ȱ��ȭ�Ͽ� ���̻� input�� ������ ���� �ʵ���

                FindObjectOfType<Spawner>().SpawnRandom(); // ���ο� ���� ����
            }

            lastFall = Time.time; // �� �ڵ� ������ ������ ��û���� �����͹���
        }
    }

    private bool IsValidGridPosition()
    {
        foreach (Transform child in transform) // �� ��ũ��Ʈ�� ������ ������Ʈ�� �ڽİ�ü���� ���� �ҷ��ͼ�
        {
            Vector2 v = MatrixGrid.RoundVector(child.position);

            if (!MatrixGrid.IsInsideBorder(v)) return false;

            if (MatrixGrid.grid[(int)v.x, (int)v.y] != null && MatrixGrid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }

        return true;
    }

    private void UpdateMatrixGrid()
    {
        // ��� grid�� null �Ҵ�
        for (int y = 0; y < MatrixGrid.column; ++y) // ++y
        {
            for (int x = 0; x < MatrixGrid.row; ++x) // ++x
            {
                if (MatrixGrid.grid[x, y] != null)
                {
                    // MatrixGrid.grid[x, y]�� �� ��ũ��Ʈ�� ������ ������Ʈ�� �ڽİ�ü���
                    if (MatrixGrid.grid[x, y].parent == transform)
                    {
                        MatrixGrid.grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform child in transform)
        {
            Vector2 v = MatrixGrid.RoundVector(child.position);
            MatrixGrid.grid[(int)v.x, (int)v.y] = child;
        }
    }
}
