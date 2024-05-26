using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

namespace CoinsGeneration
{
    public enum Strategy
    {
        Mountain = 0,
        River = 1,
        Plain = 2
    }

    public static class Fabric
    {
        private static readonly StrategyBase DefaultStrategy = new PlainStrategy();

        private static readonly Dictionary<Strategy, StrategyBase> StrategyMap = new()
        {
            { Strategy.Mountain, new MountainStrategy() },
            { Strategy.River, new RiverStrategy() },
            { Strategy.Plain, DefaultStrategy }
        };

        public static StrategyBase GetStrategy(Strategy strategy)
        {
            return StrategyMap.GetValueOrDefault(strategy, DefaultStrategy);
        }
    }

    public abstract class StrategyBase
    {
        protected readonly Random Random = new();
        public abstract void Apply(List<GameObject> gameObjects, float roadPos);
    }

    public class MountainStrategy : StrategyBase
    {
        public override void Apply(List<GameObject> gameObjects, float roadPos)
        {
            var posX = Random.Next(-1, 2) * 3;
            var len = gameObjects.Count;
            var c = 1;

            for (int i = 0, y = 1; i < len; i++, y += c, roadPos += 5)
            {
                c = y % 5 != 0 ? c : -c;
                gameObjects[i].transform.position = new Vector3(posX, y, roadPos);
                gameObjects[i].SetActive(true);
            }
        }
    }

    public class RiverStrategy : StrategyBase
    {
        public override void Apply(List<GameObject> gameObjects, float roadPos)
        {
            var posX = Random.Next(-1, 2) * 3;
            var len = gameObjects.Count;
            var r = posX == 0 ? Random.Next(0, 2) == 0 ? -3 : 3 : -posX;
            var c = r < 0 ? -1 : 1;
            var border = Math.Abs(posX - r);
                
            for (int i = 0, x = posX; i < len; i++, x += c, roadPos += 5)
            {
                gameObjects[i].transform.position = new Vector3(Math.Abs(r + x) < border ? x : r, 1, roadPos);
                gameObjects[i].SetActive(true);
            }
        }
    }

    public class PlainStrategy : StrategyBase
    {
        public override void Apply(List<GameObject> gameObjects, float roadPos)
        {
            var posX = Random.Next(-1, 2) * 3;
            foreach (var coin in gameObjects)
            {
                coin.transform.position = new Vector3(posX, 1, roadPos);
                roadPos += 5;
                coin.SetActive(true);
            }
        }
    }
}