using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Pea.Chromosome.Implementation.Permutation;
using Pea.Core;
using Pea.Fitness.Implementation.MultiObjective;

namespace PEA_TSP_Example
{
    public class TSPEvaluation : IEvaluation
    {
        public static readonly MultiKey Key = new MultiKey("TSP");

        List<SpatialPoint> TSPPoints;

        public void Init(IEvaluationInitData initData)
        {
            TSPPoints = ((TSPInitData) initData).TSPPoints;
        }

        public IEntity Decode(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
        {
            double totalDistance = 0.0;
            var entity = entities[Key] as TSPEntity;
            var chromosome = entity.Chromosomes[Key[0]] as PermutationChromosome;
            var firstPoint = TSPPoints[chromosome.Genes[0]];
            var previousPoint = firstPoint;

            for (int i = 1; i < chromosome.Genes.Length; i++)
            {
                var point = TSPPoints[chromosome.Genes[i]];
                totalDistance += GetDistance(previousPoint, point);
                entity.Phenotype.Add(previousPoint);
                previousPoint = point;
            }

            totalDistance += GetDistance(previousPoint, firstPoint);
            entity.Phenotype.Add(previousPoint);

            MultiObjectiveFitness fitness = new MultiObjectiveFitness(1);
            fitness.Value[0] = totalDistance;
            entity.Fitness = fitness;

            return entity;
        }

        public double GetDistance(SpatialPoint point1, SpatialPoint point2)
        {
            var latitudeDifference = point2.Latitude - point1.Latitude;
            var longitudeDifference = point2.Longitude - point1.Longitude;
            return Math.Sqrt(latitudeDifference * latitudeDifference + longitudeDifference * longitudeDifference);
        }

        public IList<IEntity> Combine(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
        {
            return new List<IEntity>() { entities[Key] } ;
        }
    }
}

