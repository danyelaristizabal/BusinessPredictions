using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessPredictions
{
    internal class PlayPanelViewModel : Notifier
    {
        public Random Randomizer { get; set; } = new Random();

        public ObservableCollection<Subject> _subjects;
        public ObservableCollection<Subject> Subjects
        {
            get
            {
                if (_subjects == null)
                    _subjects = new ObservableCollection<Subject>();

                return _subjects;
            }
            set
            {
                _subjects = value;
                OnPropertyChanged();
            }
        }

        private Subject _leftSelectedSubject;
        public Subject LeftSelectedSubject
        {
            get => _leftSelectedSubject;
            set
            {
                _leftSelectedSubject = value;
                LeftFrase = SelectRandomlyOrDefault(_leftSelectedSubject, true);
                OnPropertyChanged();
            }
        }

        private Subject _rightSelectedSubject;
        public Subject RightSelectedSubject
        {
            get => _rightSelectedSubject;
            set
            {
                _rightSelectedSubject = value;
                RightFrase = SelectRandomlyOrDefault(_rightSelectedSubject);
                OnPropertyChanged();
            }
        }

        private Frase _leftFrase;
        public Frase LeftFrase
        {
            get => _leftFrase;
            set
            {
                _leftFrase = value;
                OnPropertyChanged();
                OnPropertyChanged("ConcatenatedSentence");
            }
        }
        public string ConcatenatedSentence
        {
            get => (LeftFrase?.Text ?? default) + " " + (RightFrase?.Text ?? default);
        }

        private Frase _rightFrase;
        public Frase RightFrase
        {
            get => _rightFrase;
            set
            {
                _rightFrase = value;
                OnPropertyChanged();
                OnPropertyChanged("ConcatenatedSentence");
            }
        }

        private Frase SelectRandomlyOrDefault(Subject sourceFrase, bool isLeft = false)
        {
            if (sourceFrase == null || sourceFrase.Frases.Count == 0) return new Frase();
            var oneSideFrases = sourceFrase.Frases.Where(f => f.IsLeft == isLeft);
            int max = oneSideFrases.Count() - 1;
            return oneSideFrases.ElementAt(Randomizer.Next(0, max));
        }
        internal async void LoadData()
        {
            var subjects = new List<Subject>();

            await Task.Run(() =>
            {

                using (var context = new DataContext())
                    subjects = context.Subjects.ToList();

            }).ContinueWith((t) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {

                    subjects.ForEach(subject => Subjects.Add(subject));

                });

            }).ConfigureAwait(false);
        }
    }
}
