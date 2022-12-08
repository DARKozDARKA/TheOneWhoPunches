using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeBase.Infrastructure.States.GameLoop
{
    public class PlayersScore
    {
        private Dictionary<int, int> _scores = new();
        public Action<int, int> OnScoreChanged;

        public void AddPlayer(int ID) =>
            _scores.Add(ID, 0);

        public void RemovePlayer(int ID) =>
            _scores.Remove(ID);

        public void ScorePlayer(int ID)
        {
            _scores[ID]++;
            OnScoreChanged?.Invoke(ID, _scores[ID]);
        }

        public int GetPlayerScore(int ID) =>
            _scores[ID];

        public void ResetScore()
        {
            for (int i = 0; i < _scores.Count; i++)
                _scores[_scores.ElementAt(i).Key] = 0;
        }
    }
}