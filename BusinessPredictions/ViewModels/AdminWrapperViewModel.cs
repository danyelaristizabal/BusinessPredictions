using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BusinessPredictions
{
    public class AdminWrapperViewModel : Notifier
    {
        public ObservableCollection<Subject> Subjects
        {
            get
            {
                if (PanelsWrapper.GetUserControlWrapper().TryGetPanelByType(typeof(PlayPanel), out UserControl playPanel) &&
                    playPanel is PlayPanel ourPlayPanel)
                    return ourPlayPanel.GameWrapperVM.Subjects;
                else
                    return new ObservableCollection<Subject>();
            }
            set
            {
                if (PanelsWrapper.GetUserControlWrapper().TryGetPanelByType(typeof(PlayPanel), out UserControl playPanel) &&
                    playPanel is PlayPanel ourPlayPanel)
                    ourPlayPanel.GameWrapperVM.Subjects = value;
            }
        }

        private Subject _leftSelectedSubject;
        public Subject LeftSelectedSubject
        {
            get => _leftSelectedSubject;
            set
            {
                _leftSelectedSubject = value;
                LoadSelectedSubjectFrases(_leftSelectedSubject, LeftFrases, LeftFrase, true);
                OnPropertyChanged();
            }
        }

        private bool _idLeft;
        public bool IsLeft
        {
            get => _idLeft;
            set
            {
                _idLeft = value;
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
                LoadSelectedSubjectFrases(_rightSelectedSubject, RightFrases, RightFrase);
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

        private ObservableCollection<Frase> _leftFrases;
        public ObservableCollection<Frase> LeftFrases
        {
            get
            {
                if (_leftFrases == null)
                    _leftFrases = new ObservableCollection<Frase>();

                return _leftFrases;
            }
            set
            {
                _leftFrases = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Frase> _rightFrases;
        public ObservableCollection<Frase> RightFrases
        {
            get
            {
                if (_rightFrases == null)
                    _rightFrases = new ObservableCollection<Frase>();

                return _rightFrases;
            }
            set
            {
                _rightFrases = value;
                OnPropertyChanged();
            }
        }

        public void LoadSelectedSubjectFrases(Subject selectedSubject, ObservableCollection<Frase> frasesList, Frase selectedFrase, bool isLeft = false)
        {
            frasesList.Clear();
            selectedSubject.Frases.ForEach(f =>
            {
                if (f.IsLeft == isLeft)
                    frasesList.Add(f);
            });

            if (frasesList.Count > 0)
                selectedFrase = frasesList[0];
        }

        internal void ClearAllSelected()
        {
            LeftSelectedSubject = new Subject();
            RightSelectedSubject = new Subject();
            RightFrase = new Frase();
            LeftFrase = new Frase();
        }

        internal async void DeleteSelected(TextBox textBox)
        {

            if (textBox.DataContext is Frase fraseToDelete)
            {
                await Task.Run(() =>
                {
                    using (var context = new DataContext())
                    {
                        var frasesToDelete = context.Frases.Where(f => f.Id == fraseToDelete.Id);
                        context.Frases.RemoveRange(frasesToDelete);
                        context.SaveChanges();
                    }

                }).ConfigureAwait(false);
            }

            else if (textBox.DataContext is Subject subjectToDelete)
            {
                await Task.Run(() =>
                {
                    using (var context = new DataContext())
                    {
                        foreach (var subject in context.Subjects.Where(s => s.Id == subjectToDelete.Id).ToList())
                        {
                            foreach (var frase in subject.Frases)
                            {
                                context.Frases.Remove(frase);
                            }
                            context.Subjects.Remove(subject);
                        }
                        context.SaveChanges();
                    }
                }).ConfigureAwait(false);

            }
            throw new NotImplementedException();
        }

        internal async void AddSelected(TextBox textBox)
        {
            if (textBox.DataContext is Frase fraseToAdd)
            {
                await Task.Run(() =>
                {
                    using (var context = new DataContext())
                    {
                        if (fraseToAdd.Text == LeftFrase.Text) // refactor DRY this
                        {
                            fraseToAdd.IsLeft = true;
                            var subject = context.Subjects.FirstOrDefault(x => x.Id == LeftSelectedSubject.Id);
                            if (subject == null) return;
                            fraseToAdd.Subject = subject;
                        }
                        else if (fraseToAdd.Text == RightFrase.Text)
                        {
                            fraseToAdd.IsLeft = false;
                            var subject = context.Subjects.FirstOrDefault(x => x.Id == RightSelectedSubject.Id);
                            if (subject == null) return;
                            fraseToAdd.Subject = RightSelectedSubject;
                            fraseToAdd.Subject = subject;
                        }
                        else
                            return;


                        context.Frases.Add(fraseToAdd);
                        context.SaveChanges();
                    }
                }).ConfigureAwait(false);
            }

            else if (textBox.DataContext is Subject subjectToAdd)
            {
                await Task.Run(() =>
                {
                    using (var context = new DataContext())
                    {
                        context.Subjects.Add(subjectToAdd);

                        context.SaveChanges();
                    }
                }).ConfigureAwait(false);
            }
        }
    }
}
