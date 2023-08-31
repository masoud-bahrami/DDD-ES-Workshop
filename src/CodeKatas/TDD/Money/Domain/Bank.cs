using System.Collections;

namespace CodeKata.TDD.Domain;

public class Bank
{
    private readonly Hashtable _pairs=new();
    public Money Reduce(
        Expression source, Currency to)
    {
        return source.Reduce(this, to);
    }

    public void AddRate(Currency from, Currency to, int rate)
    {
        _pairs
            .Add(new Pair(from, to), rate);
    }

    public int GetRate(Currency from, Currency to)
    {
        if (from == to)
            return 1;

        return (int)_pairs[new Pair(from, to)] ;
    }

    class Pair
    {
        private Currency from;
        private Currency to;

        public Pair(Currency from, Currency to)
        {
            this.from = from;
            this.to = to;
        }

        public override bool Equals(object? obj)
        {
            Pair that = (Pair)obj;
            return this.from == that.from
                   & this.to == that.to;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}