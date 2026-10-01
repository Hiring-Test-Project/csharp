namespace Hiring;

public sealed record Obligation(ulong Id, ulong AmountDue, ulong Priority);

public sealed record Line(ulong Id, ulong Applied);

public sealed record Settlement(IReadOnlyList<Line> Lines, ulong Unapplied);

public static class SumAmounts
{
    public static Settlement Settle(ulong payment, IReadOnlyList<Obligation> obligations)
    {
        throw new NotImplementedException();
    }
}
