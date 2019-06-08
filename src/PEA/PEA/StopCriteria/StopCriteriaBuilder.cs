using Pea.Core;
using Pea.StopCriteria.Implementation;
using System;

namespace Pea.StopCriteria
{
    public class StopCriteriaBuilder
    {
        public enum CriteriaOperatorTypes
        {
            None,
            And,
            Or
        }

        public CriteriaOperatorTypes CurrentOperator { get; private set; } = CriteriaOperatorTypes.None;

        public IStopCriteria Criteria { get; set; }

        private StopCriteriaBuilder() { }

        public static StopCriteriaBuilder StopWhen()
        {
            return new StopCriteriaBuilder();
        }

        public StopCriteriaBuilder When(IStopCriteria criteria)
        {
            if (criteria == null) throw new ArgumentNullException(nameof(criteria));

            if (CurrentOperator == CriteriaOperatorTypes.None && Criteria != null) throw new InvalidOperationException();

            if (CurrentOperator == CriteriaOperatorTypes.None)
            {
                Criteria = criteria;
                return this;
            }

            Criteria = Compose(Criteria, criteria);
            CurrentOperator = CriteriaOperatorTypes.None;
            return this;
        }

        public StopCriteriaBuilder Or()
        {
            if (CurrentOperator != CriteriaOperatorTypes.None || Criteria == null) throw new InvalidOperationException();

            CurrentOperator = CriteriaOperatorTypes.Or;
            return this;
        }

        public StopCriteriaBuilder And()
        {
            if (CurrentOperator != CriteriaOperatorTypes.None || Criteria == null) throw new InvalidOperationException();

            CurrentOperator = CriteriaOperatorTypes.And;
            return this;
        }

        public StopCriteriaBuilder TimeoutElapsed(int timeoutMilliseconds)
        {
            var criteria = new TimeOutStopCriteria(timeoutMilliseconds);
            return When(criteria);
        }

        public StopCriteriaBuilder FitnessLimitExceeded(IFitness fitnessLimit)
        {
            var criteria = new FitnessLimitExceededStopCriteria(fitnessLimit);
            return When(criteria);
        }

        public IStopCriteria Build()
        {
            if (CurrentOperator != CriteriaOperatorTypes.None) throw new InvalidOperationException();

            if (Criteria == null) return new TimeOutStopCriteria();

            return Criteria;
        }

        private IStopCriteria Compose(IStopCriteria criteria1, IStopCriteria criteria2)
        {
            switch(CurrentOperator)
            {
                case CriteriaOperatorTypes.And:
                    return new AndStopCriteria(criteria1, criteria2);

                case CriteriaOperatorTypes.Or:
                    return new OrStopCriteria(criteria1, criteria2);

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
