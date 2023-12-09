namespace AdventOfCode.Y2023.Day07;

[ProblemName("Camel Cards")]
internal class Solution : ISolver
{

    public object PartOne(string input)
    {
        return input.Split(Environment.NewLine)
            .Select(l => l.Split())
            .Select(l => (hand: new Hand(l[0]), bid: int.Parse(l[1])))
            .OrderBy(x => x.hand)
            .Select((x, i) => (long)(i + 1) * x.bid)
            .Sum();
    }

    public object PartTwo(string input)
    {
        return input.Split(Environment.NewLine)
            .Select(l => l.Split())
            .Select(l => (hand: new Hand(l[0], true), bid: int.Parse(l[1])))
            .OrderBy(x => x.hand)
            .Select((x, i) => (long)(i + 1) * x.bid)
            .Sum();
    }

    private enum HandType : int
    {
        None = 0,
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard,
    }

    private sealed class Hand : IComparable<Hand>
    {
        private readonly HandType _handType;
        private readonly char[] _cards;
        private readonly bool _jokers;

        public Hand(string hand, bool jokers = false)
        {
            _cards = hand.ToCharArray();
            _jokers = jokers;

            var cards = _cards
                .GroupBy(x => x, (x, g) => (card: x, count: g.Count(), order: _cardOrder.IndexOf(x)))
                .OrderByDescending(x => x.count)
                .ThenBy(x => x.order)
                .ToList();

            if (jokers)
            {
                var jCount = cards.FirstOrDefault(x => x.card == 'J').count;
                if (jCount is not 0 and not 5)
                {
                    cards.RemoveAll(x => x.card == 'J');
                    cards[0] = (cards[0].card, cards[0].count + jCount, cards[0].order);
                }
            }

            if (cards[0].count == 5)
                _handType = HandType.FiveOfAKind;
            else if (cards[0].count == 4)
                _handType = HandType.FourOfAKind;
            else if (cards[0].count == 3 && cards[1].count == 2)
                _handType = HandType.FullHouse;
            else if (cards[0].count == 3)
                _handType = HandType.ThreeOfAKind;
            else if (cards[0].count == 2 && cards[1].count == 2)
                _handType = HandType.TwoPair;
            else if (cards[0].count == 2)
                _handType = HandType.OnePair;
            else
                _handType = HandType.HighCard;
        }

        public int CompareTo(Hand other)
        {
            if (other == null) throw new InvalidOperationException();

            var cmp = _handType.CompareTo(other._handType);
            if (cmp != 0) return -cmp;

            foreach (var (l, r) in _cards.Zip(other._cards))
            {
                if (_jokers)
                    cmp = _jokerOrder.IndexOf(l).CompareTo(_jokerOrder.IndexOf(r));
                else
                    cmp = _cardOrder.IndexOf(l).CompareTo(_cardOrder.IndexOf(r));

                if (cmp != 0) return -cmp;
            }

            return 0;
        }

        private static readonly List<char> _cardOrder = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'];
        private static readonly List<char> _jokerOrder = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];
    }
}
