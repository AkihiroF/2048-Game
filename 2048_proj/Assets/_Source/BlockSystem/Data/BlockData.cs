using BlockSystem.View;
using UnityEngine;

namespace BlockSystem.Data
{
    public class BlockData
    {
        private readonly BlockView _view;
        public int Score { get; private set; }

        public BlockData(BlockView view)
        {
            _view = view;
        }

        public void OnMerge()
        {
            if (Score == 0)
                Score++;
            
            Score *= 2;
            _view.SetNumber(Score);
        }

        public void OnUpdateInfo(int newScore)
        {
            Score = newScore;
            _view.SetNumber(Score);
        }

        public void OnClear()
        {
            Score = 0;
            _view.SetNumber(Score);
        }
    }
}