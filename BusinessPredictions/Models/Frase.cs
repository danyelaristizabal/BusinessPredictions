using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessPredictions
{
    public class Frase : Notifier
    {
        public int Id { get; set; }
        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public bool IsLeft { get; set; }

        [ForeignKey("Subject")]
        public int SubjectRefId { get; set; }
        public Subject Subject { get; set; }
    }
}
