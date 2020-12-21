using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<Frase> _frases = new ObservableCollection<Frase>();
        public ObservableCollection<Frase> Frases
        {
            get
            {
                if (_frases.Count == 0)
                    using (var dataContext = new DataContext()) 
                    {
                        foreach (var item in dataContext.Frases.Where(f => f.SubjectRefId == Id))
                        {
                            _frases.Add(item); 
                        }
                    }

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
