using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.Utils
{
    [System.Serializable] public class Condition
    {
        [SerializeField] private Disjunction[] and;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            if (and.Length == 0) return true;
            if (and == null) return false;

            foreach (Disjunction disjunction in and)
            {
                if (!disjunction.Check(evaluators))
                {
                    return false;
                }
            }
            return true;
        }

        [System.Serializable] class Disjunction
        {
            [SerializeField] private Predicate[] or;

            

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate predicate in or)
                {
                    if (predicate.Check(evaluators))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        [System.Serializable] class Predicate
        {
            [SerializeField] private string predicate;
            [SerializeField] private string[] parameters;
            [SerializeField] private bool negate = false;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (IPredicateEvaluator evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, parameters);
                    if (result == null)
                    {
                        continue;
                    }
                    if (result == negate)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
