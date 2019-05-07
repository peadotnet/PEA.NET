﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Pea.Core
{
    public class StochasticProvider<T> : IProvider<T>
    {
        private readonly IRandom _random;
        private readonly SortedList<double, T> _intervals = new SortedList<double, T>();
        private double _max = 0;

        public StochasticProvider(IRandom random)
        {
            _random = random;
        }

        public T GetOne()
        {
            if (!_intervals.Any()) return default(T);

            var rnd = _random.GetDouble(0, _max);
            foreach (var interval in _intervals.Keys)
            {
                if (rnd < interval) return _intervals[interval];
            }

            throw new ApplicationException();
        }

        public IProvider<T> Add(T item, double probability)
        {
            _max = _max + probability;
            _intervals.Add(_max, item);
            return this;
        }
    }
}