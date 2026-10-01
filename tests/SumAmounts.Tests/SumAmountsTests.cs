using Hiring;
using Xunit;

namespace Hiring.Tests;

public class SumAmountsTests
{
    private static Obligation Ob(ulong id, ulong amountDue, ulong priority) => new(id, amountDue, priority);

    private static void AssertSettlement(ulong payment, Obligation[] obligations, ulong unapplied, params Line[] lines)
    {
        Settlement got = SumAmounts.Settle(payment, obligations);
        Assert.Equal(unapplied, got.Unapplied);
        Assert.Equal(lines, got.Lines);
    }

    [Fact]
    public void Empty()
    {
        AssertSettlement(100, [], 100);
    }

    [Fact]
    public void PaysInFull()
    {
        AssertSettlement(
            500,
            [Ob(1, 100, 1), Ob(2, 40, 3), Ob(3, 60, 2)],
            300,
            new Line(1, 100),
            new Line(2, 40),
            new Line(3, 60));
    }

    [Fact]
    public void HigherPriorityIsPaidFirst()
    {
        AssertSettlement(
            60,
            [Ob(1, 80, 1), Ob(2, 50, 9), Ob(3, 25, 9)],
            0,
            new Line(1, 0),
            new Line(2, 50),
            new Line(3, 10));
    }

    [Fact]
    public void SamePriorityPaysEarlierInputFirst()
    {
        AssertSettlement(
            10,
            [Ob(1, 50, 1), Ob(2, 30, 1), Ob(3, 20, 1)],
            0,
            new Line(1, 10),
            new Line(2, 0),
            new Line(3, 0));
    }

    [Fact]
    public void RemainderSpillsToTheNextPriority()
    {
        AssertSettlement(
            120,
            [Ob(1, 100, 1), Ob(2, 40, 5), Ob(3, 60, 5)],
            0,
            new Line(1, 20),
            new Line(2, 40),
            new Line(3, 60));
    }

    [Fact]
    public void DuplicateId()
    {
        Assert.Throws<ArgumentException>(() => SumAmounts.Settle(5, [Ob(1, 10, 1), Ob(1, 4, 2)]));
    }
}
