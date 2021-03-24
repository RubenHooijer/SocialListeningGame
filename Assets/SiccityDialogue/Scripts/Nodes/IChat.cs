namespace Dialogue {
    public interface IChat {

        public int AnswerCount { get; }
        public void AnswerQuestion(int index);

    }
}