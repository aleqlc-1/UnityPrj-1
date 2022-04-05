using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchChecker
{
    // foreach�� for�� �ȿ� �־ ��ü�� �ݺ��ؼ� �������ϹǷ� IEnumerable<GameObject>
    public static IEnumerator AnimatePotentialMatches(IEnumerable<GameObject> potentialMatches)
    {
        for (float i = 1.0f; i >= 0.3f; i -= 0.1f)
        {
            foreach (var item in potentialMatches)
            {
                Color c = item.GetComponent<SpriteRenderer>().color;
                c.a = i;
                item.GetComponent<SpriteRenderer>().color = c;
            }

            yield return new WaitForSeconds(GameVariables.OpacityAnimationDelay);
        }

        for (float i = 0.3f; i <= 1f; i += 0.1f)
        {
            foreach (var item in potentialMatches)
            {
                Color c = item.GetComponent<SpriteRenderer>().color;
                c.a = i;
                item.GetComponent<SpriteRenderer>().color = c;
            }

            yield return new WaitForSeconds(GameVariables.OpacityAnimationDelay);
        }
    }

    public static bool AreHorizontalOrVerticalNeighbors(Candy c1, Candy c2)
    {
        // ���̳� ���� �����鼭 && �����¿�� �������� Ȯ��
        return (c1.Column == c2.Column || c1.Row == c2.Row) && Mathf.Abs(c1.Column - c2.Column) <= 1 && Mathf.Abs(c1.Row - c2.Row) <= 1;
    }

    public static IEnumerable<GameObject> GetPotentialMatches(CandyArray candies)
    {
        List<List<GameObject>> matches = new List<List<GameObject>>();

        for (int row = 0; row < GameVariables.Rows; row++)
        {
            for (int column = 0; column < GameVariables.Columns; column++)
            {
                var matches1 = CheckHorizontal1(row, column, candies);
                var matches2 = CheckHorizontal2(row, column, candies);
                var matches3 = CheckHorizontal3(row, column, candies);
                var matches4 = CheckVertical1(row, column, candies);
                var matches5 = CheckVertical2(row, column, candies);
                var matches6 = CheckVertical3(row, column, candies);

                if (matches1 != null) matches.Add(matches1);
                if (matches2 != null) matches.Add(matches2);
                if (matches3 != null) matches.Add(matches3);
                if (matches4 != null) matches.Add(matches4);
                if (matches5 != null) matches.Add(matches5);
                if (matches6 != null) matches.Add(matches6);

                if (matches.Count >= 3)
                    return matches[Random.Range(0, matches.Count - 1)];

                if (row >= GameVariables.Rows / 2 && matches.Count > 0 && matches.Count <= 2)
                    return matches[Random.Range(0, matches.Count - 1)];
            }
        }

        return null;
    }

    // �ٷ� �����Ͱ� ���� ���� �밢���� �ִ°Ͱ� ������
    public static List<GameObject> CheckHorizontal1(int row, int column, CandyArray candies)
    {
        if (column <= GameVariables.Columns - 2)
        {
            if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row, column + 1].GetComponent<Candy>()))
            {
                if (row >= 1 && column >= 1)
                {
                    if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row - 1, column - 1].GetComponent<Candy>()))
                    {
                        return new List<GameObject>() { candies[row, column],
                                                        candies[row, column + 1],
                                                        candies[row - 1, column - 1] };

                        if (row <= GameVariables.Rows - 2 && column >= 1)
                        {
                            if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row + 1, column - 1].GetComponent<Candy>()))
                            {
                                return new List<GameObject>() { candies[row, column],
                                                                candies[row, column + 1],
                                                                candies[row + 1, column - 1] };
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    // �ٷ� �����Ͱ� ���� ������ �밢���� �ִ°Ͱ� ������
    public static List<GameObject> CheckHorizontal2(int row, int column, CandyArray candies)
    {
        if (column <= GameVariables.Columns - 3)
        {
            if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row, column + 1].GetComponent<Candy>()))
            {
                if (row >= 1 && column <= GameVariables.Columns - 3)
                {
                    if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row - 1, column + 2].GetComponent<Candy>()))
                    {
                        return new List<GameObject> { candies[row, column],
                                                      candies[row, column + 1],
                                                      candies[row - 1, column + 2]};

                        if (row <= GameVariables.Rows - 2 && column <= GameVariables.Columns - 3)
                        {
                            if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row + 1, column + 2].GetComponent<Candy>()))
                            {
                                return new List<GameObject> { candies[row, column],
                                                              candies[row, column + 1],
                                                              candies[row + 1, column + 2]};
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    public static List<GameObject> CheckHorizontal3(int row, int column, CandyArray candies)
    {
        // �ٷ� �����ʰ� ���� ������ ��ĭ���� �͵� ����
        if (column <= GameVariables.Columns - 4)
        {
            if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row, column + 1].GetComponent<Candy>()) &&
                candies[row, column].GetComponent<Candy>().IsSameType(candies[row, column + 3].GetComponent<Candy>()))
            {
                return new List<GameObject> { candies[row, column],
                                              candies[row, column + 1],
                                              candies[row, column + 3]};
            }
        }

        // �ٷ� �����ʰ� ���� ���� ��ĭ���� �͵� ����
        if (column >= 2 && column <= GameVariables.Columns - 2)
        {
            if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row, column + 1].GetComponent<Candy>()) &&
                candies[row, column].GetComponent<Candy>().IsSameType(candies[row, column - 2].GetComponent<Candy>()))
            {
                return new List<GameObject> { candies[row, column],
                                              candies[row, column + 1],
                                              candies[row, column - 2]};
            }
        }

        return null;
    }

    // �ٷ� �����Ŷ� ���� ���ʾƷ� �밢���� ����
    public static List<GameObject> CheckVertical1(int row, int column, CandyArray candies)
    {
        if (row <= GameVariables.Rows - 2)
        {
            if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row + 1, column].GetComponent<Candy>()))
            {
                if (column >= 1 && row >= 1)
                {
                    if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row - 1, column - 1].GetComponent<Candy>()))
                    {
                        return new List<GameObject> { candies[row, column],
                                                      candies[row + 1, column],
                                                      candies[row - 1, column - 1]};
                    }

                    if (column <= GameVariables.Columns - 2 && row >= 1)
                    {
                        if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row - 1, column + 1].GetComponent<Candy>()))
                        {
                            return new List<GameObject> { candies[row, column],
                                                      candies[row + 1, column],
                                                      candies[row - 1, column + 1]};
                        }
                    }
                }
            }
        }

        return null;
    }

    // �ٷ� �����Ŷ����� ���� �� �밢���� ����
    public static List<GameObject> CheckVertical2(int row, int column, CandyArray candies)
    {
        if (row <= GameVariables.Rows - 3)
        {
            if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row + 1, column].GetComponent<Candy>()))
            {
                if (column >= 1)
                {
                    if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row + 2, column - 1].GetComponent<Candy>()))
                    {
                        return new List<GameObject> { candies[row, column],
                                                      candies[row + 1, column],
                                                      candies[row + 2, column - 1]};

                        if (column <= GameVariables.Columns - 2)
                        {
                            if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row + 2, column + 1].GetComponent<Candy>()))
                            {
                                return new List<GameObject> { candies[row, column],
                                                              candies[row + 1, column],
                                                              candies[row + 2, column + 1]};
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    // �ٷ� �����Ͱ� ���� ���� �� ��ĭ���� �Ͱ��� ����
    public static List<GameObject> CheckVertical3(int row, int column, CandyArray candies)
    {
        if (row <= GameVariables.Rows - 4)
        {
            if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row + 1, column].GetComponent<Candy>()) &&
                candies[row, column].GetComponent<Candy>().IsSameType(candies[row + 3, column].GetComponent<Candy>()))
            {
                return new List<GameObject> { candies[row, column],
                                              candies[row + 1, column],
                                              candies[row + 3, column]};
            }
        }

        if (row >= 2 && row <= GameVariables.Rows - 2)
        {
            if (candies[row, column].GetComponent<Candy>().IsSameType(candies[row + 1, column].GetComponent<Candy>()) &&
                candies[row, column].GetComponent<Candy>().IsSameType(candies[row - 2, column].GetComponent<Candy>()))
            {
                return new List<GameObject> { candies[row, column],
                                              candies[row + 1, column],
                                              candies[row - 2, column]};
            }
        }

        return null;
    }
}
