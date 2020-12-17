using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.Generic;
using System.Linq;

namespace BusinessPredictions
{
    public class Subject : Notifier
    {
        string _subjectName;
        public string SubjectName
        {
            get => _subjectName;
            set
            {
                _subjectName = value;
                OnPropertyChanged();
            }
        }
        public int Id { get; set; }
        List<Frase> _frases = new List<Frase>();
        public List<Frase> Frases
        {
            get
            {
                if (_frases.Count == 0)
                    using (var dataContext = new DataContext())
                        _frases = dataContext.Frases.Where(f => f.SubjectRefId == Id).ToList();

                return _frases;
            }
            set
            {
                _frases = value;
                OnPropertyChanged();
            }
        }
    }
}
