using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {

    public interface IDialogueNode {

        public string Text { get; }

    }

    public interface IChat : IDialogueNode {

        public AudioClip SpeechAudioClip { get; }
        public int AnswerCount { get; }
        public List<Answer> Answers { get; }

    }

    public interface IAnswer : IDialogueNode {

        bool HideIfVisited { get; }
        bool HideIfSpecificNodeIsVisited { get; }

        bool IsSpecificNodeVisited { get; }
        bool IsVisited { get; set; }

    }

}