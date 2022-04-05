using System;

[Flags] // using System; �� �Ӽ��� �ϳ��� ������ ���� �� ����
public enum BonusType
{
    None,
    DestroyWholeRowColumn
}

public enum GameState
{
    None,
    SelectionStarted,
    Animating
}

public static class BonusTypeChecker
{
    public static bool ContainsDestroyWholeRowColumn(BonusType bt)
    {
        // & BonusType.DestroyWholeRowColumn?
        return (bt & BonusType.DestroyWholeRowColumn) == BonusType.DestroyWholeRowColumn;
    }
}
