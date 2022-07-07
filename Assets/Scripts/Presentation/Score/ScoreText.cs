using System;
using TMPro;

namespace Presentation.Score
{
    [Serializable]
    public struct ScoreText
    {
        public TMP_Text Text;
        public int? LastShownValue;
    }
}