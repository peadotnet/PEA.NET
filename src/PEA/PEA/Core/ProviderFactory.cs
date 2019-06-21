namespace Pea.Core
{
    public static class ProviderFactory
    {
        public static IProvider<T> Create<T>(int count, IRandom random)
        {
            if (count < 2) return new SimpleProvider<T>();

            return new StochasticProvider<T>(random);
        }
    }
}
