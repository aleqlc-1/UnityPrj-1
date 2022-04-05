using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CandyArray
{
    GameObject[,] candies = new GameObject[GameVariables.Rows, GameVariables.Columns];

    private GameObject backup1;
    private GameObject backup2;

    // candies�迭�� �� �߿��� ���ڷι��� row,column�� �ش��ϴ� ���� get,set�ϴ� �ε���
    public GameObject this[int row, int column]
    {
        get
        {
            try
            {
                return candies[row, column];
            }
            catch(Exception e)
            {
                throw;
            }
        }

        set
        {
            candies[row, column] = value;
        }
    }

    public void Swap(GameObject g1, GameObject g2)
    {
        backup1 = g1;
        backup2 = g2;

        var g1Candy = g1.GetComponent<Candy>();
        var g2Candy = g2.GetComponent<Candy>();

        int g1Row = g1Candy.Row;
        int g1Column = g1Candy.Column;
        int g2Row = g2Candy.Row;
        int g2Column = g2Candy.Column;

        // ĵ�� �迭���� ���ӿ�����Ʈ�� �ٲٴ°Ű�
        var temp = candies[g1Row, g1Column];
        candies[g1Row, g1Column] = candies[g2Row, g2Column];
        candies[g2Row, g2Column] = temp;

        // ĵ�� ��ü�� ������ ��ũ��Ʈ�� ��,�� ���� �ٲٴ°�
        Candy.SwapRowColumn(g1Candy, g2Candy);
    }

    public void UndoSwap()
    {
        Swap(backup1, backup2);
    }

    public MatchesInfo GetMatches(GameObject go)
    {
        MatchesInfo matchesInfo = new MatchesInfo();

        var horizontalMatches = GetMatchesHorizontally(go);

        // ��ġ�� ĵ��� �߿� ��ü�� �����ϴ� ���ʽ�ĵ�� ���ԵǾ�������
        if (ContainsDestroyWholeRowColumnBonus(horizontalMatches))
        {
            horizontalMatches = GetEntireRow(go); // �� ĵ�� ���Ե� ���� ��� ĵ��(ĵ���翡 �������) �����ͼ�

            if (!BonusTypeChecker.ContainsDestroyWholeRowColumn(matchesInfo.BonusesContained))
            {
                matchesInfo.BonusesContained = BonusType.DestroyWholeRowColumn;
            }
        }

        matchesInfo.AddObjectRange(horizontalMatches); // ��ġ�� ĵ�𸮽�Ʈ�� ����

        var verticalMatches = GetMatchesVertically(go);

        if (ContainsDestroyWholeRowColumnBonus(verticalMatches))
        {
            verticalMatches = GetEntireColumn(go);

            if (!BonusTypeChecker.ContainsDestroyWholeRowColumn(matchesInfo.BonusesContained))
            {
                matchesInfo.BonusesContained = BonusType.DestroyWholeRowColumn;
            }
        }

        matchesInfo.AddObjectRange(verticalMatches); // ��ġ�� ĵ�𸮽�Ʈ�� ����

        return matchesInfo;
    }

    public IEnumerable<GameObject> GetMatches(IEnumerable<GameObject> gos)
    {
        List<GameObject> matches = new List<GameObject>();

        foreach (var go in gos)
        {
            matches.AddRange(GetMatches(go).MatchedCandy); // AddRange�� ���� ��Ҹ� �ѹ��� ����Ʈ�� �߰�
        }

        return matches.Distinct();
    }

    private IEnumerable<GameObject> GetMatchesHorizontally(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(go);

        var candy = go.GetComponent<Candy>();

        // search left
        if (candy.Column != 0) // ���� ���� ĵ��� ���ʰ˻� ���ϵ���
        {
            for (int column = candy.Column - 1; column >= 0; column--)
            {
                if (candies[candy.Row, column].GetComponent<Candy>().IsSameType(candy))
                    matches.Add(candies[candy.Row, column]);
                else // �ٸ� ĵ�� �˻��Ǵ¼��� �ٷ� ����
                    break;
            }
        }

        // search right
        if (candy.Column != GameVariables.Columns - 1) // ���� ������ ĵ��� �����ʰ˻� ���ϵ���
        {
            for (int column = candy.Column + 1; column < GameVariables.Columns; column++)
            {
                if (candies[candy.Row, column].GetComponent<Candy>().IsSameType(candy))
                    matches.Add(candies[candy.Row, column]);
                else // �ٸ� ĵ�� �˻��Ǵ¼��� �ٷ� ����
                    break;
            }
        }

        // �������� ĵ�� 3���϶����� ��ġ����
        if (matches.Count < GameVariables.MinimumMatches) matches.Clear();

        // ����Ʈ���� �ߺ����� �� �Ѱ��� ĵ�� ����
        return matches.Distinct();
    }

    private IEnumerable<GameObject> GetMatchesVertically(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(go);

        var candy = go.GetComponent<Candy>();

        if (candy.Row != 0)
        {
            for (int row = candy.Row - 1; row >= 0; row--)
            {
                if (candies[row, candy.Column].GetComponent<Candy>().IsSameType(candy))
                    matches.Add(candies[row, candy.Column]);
                else
                    break;
            }
        }

        if (candy.Row != GameVariables.Rows - 1)
        {
            for (int row = candy.Row + 1; row < GameVariables.Columns; row++)
            {
                if (candies[row, candy.Column].GetComponent<Candy>().IsSameType(candy))
                    matches.Add(candies[row, candy.Column]);
                else
                    break;
            }
        }

        // �������� ĵ�� 3���϶����� ��ġ����
        if (matches.Count < GameVariables.MinimumMatches) matches.Clear();

        // �ߺ����� ��ä�� ����
        // matches����Ʈ ��ü�� �ߺ����� �� ���� �ƴ�
        // matches.Distinct().ToList() �ؼ� �ٸ� ����Ʈ�� ������ �ߺ����� ��ä�� ���
        return matches.Distinct();
    }

    private bool ContainsDestroyWholeRowColumnBonus(IEnumerable<GameObject> matches)
    {
        if (matches.Count() >= GameVariables.MinimumMatches) // IEnumerable<>�� Count�� ()�ؾ���
        {
            foreach (var item in matches)
            {
                if (BonusTypeChecker.ContainsDestroyWholeRowColumn(item.GetComponent<Candy>().Bonus))
                {
                    return true;
                }
            }
        }

        return false;
    }

    // ���ڷ� ������ ĵ�� �ش��ϴ� Row�� ��� ĵ�� ����Ʈ�� ��Ƽ� ��ȯ�ϴ� �޼���
    // ����Ʈ�� ������ �ݺ��ؼ� ������ �� �ֵ��� IEnumerable<GameObject>�� ��ȯ
    private IEnumerable<GameObject> GetEntireRow(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        int row = go.GetComponent<Candy>().Row;

        for (int column = 0; column < GameVariables.Columns; column++)
        {
            matches.Add(candies[row, column]);
        }

        return matches;
    }

    private IEnumerable<GameObject> GetEntireColumn(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        int column = go.GetComponent<Candy>().Column;

        for (int row = 0; row < GameVariables.Rows; row++)
        {
            matches.Add(candies[row, column]);
        }

        return matches;
    }

    public void Remove(GameObject item)
    {
        candies[item.GetComponent<Candy>().Row, item.GetComponent<Candy>().Column] = null;
    }

    // IEnumerable<> �ڷ����� ���ڸ� �ݺ����Ⱑ���� ����Ʈ��
    public AlteredCandyInfo Collapse(IEnumerable<int> columns)
    {
        AlteredCandyInfo collapseInfo = new AlteredCandyInfo();

        foreach (var column in columns) // ��� ����
        {
            for (int row = 0; row < GameVariables.Rows - 1; row++) // ���� �� ���鼭
            {
                if (candies[row, column] == null) // ����ִ� ���� ������
                {
                    for (int row2 = row + 1; row2 < GameVariables.Rows; row2++)
                    {
                        if (candies[row2, column] != null) // �ٷ� ���� ĵ�� ������
                        {
                            candies[row, column] = candies[row2, column]; // ��ĭ �Ʒ���
                            candies[row2, column] = null; // �ٷ� ���� ��Եǰ�

                            if (row2 - row > collapseInfo.maxDistance)
                                collapseInfo.maxDistance = row2 - row;

                            // ���ο� ��,�� �� �Ҵ�
                            candies[row, column].GetComponent<Candy>().Row = row;
                            candies[row, column].GetComponent<Candy>().Column = column;

                            collapseInfo.AddCandy(candies[row, column]);

                            break;
                        }
                    }
                }
            }
        }

        return collapseInfo;
    }

    // ����� ��� ����Ʈ�� ��Ƽ� ����
    public IEnumerable<CandyInfo> GetEmptyItemsOnColumn(int column)
    {
        List<CandyInfo> emptyItems = new List<CandyInfo>();

        for (int row = 0; row < GameVariables.Rows; row++)
        {
            if (candies[row, column] == null)
            {
                emptyItems.Add(new CandyInfo() { Row = row, Column = column });
            }
        }

        return emptyItems;
    }
}
