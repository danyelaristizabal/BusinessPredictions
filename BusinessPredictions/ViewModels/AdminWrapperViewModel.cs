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

        private Subject _leftSubjectToAdd;
        public Subject LeftSubjectToAdd
        {
            get 
            {
                if (_leftSubjectToAdd != null)
                    _leftSubjectToAdd = new Subject();
                return _leftSubjectToAdd;
            }
            set
            {
                _leftSubjectToAdd = value;
                OnPropertyChanged();
            }
        }

        private Subject _rightSubjectToAdd;
        public Subject RightSubjectToAdd
        {
            get 
            {
                if(_rightSubjectToAdd != null)
                    _rightSubjectToAdd = new Subject();
                return _rightSubjectToAdd;
            } 
            set
            {
                _rightSubjectToAdd = value;
                OnPropertyChanged();
            }
        }

        private Frase _leftFraseToAdd;
        public Frase LeftFraseToAdd
        {
            get
            {
                if (_leftFraseToAdd == null)
                    _leftFraseToAdd = new Frase() { IsLeft = true };

                return _leftFraseToAdd;
            }
            set
            {
                _leftFraseToAdd = value;
                OnPropertyChanged();
            }
        }
        private Frase _rightFraseToAdd;
        public Frase RightFraseToAdd 
        {
            get 
            {
                if (_rightFraseToAdd == null)
                    _rightFraseToAdd = new Frase() { IsLeft = false}; 
               return  _rightFraseToAdd;
            }
            set
            {
                _rightFraseToAdd = value;
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

        internal async void DeleteSelected(object dataContext)
        {

            if (dataContext is Frase fraseToDelete)
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

            else if (dataContext is Subject subjectToDelete)
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
            else 
            { 
                throw new NotImplementedException();
            }
        }

        internal async void AddSelected(object DataContext)
        {
            if (DataContext is Frase fraseToAdd)
            {
                await Task.Run(() =>
                {
                    using (var context = new DataContext())
                    {
                        Subject subjectDb = null;

                        Subject Subjectlocal = null;

                        Subjectlocal = fraseToAdd.IsLeft ? LeftSelectedSubject : RightSelectedSubject;

                        int selectedElementId = fraseToAdd.IsLeft ? LeftSelectedSubject.Id : RightSelectedSubject.Id;

                        subjectDb = context.Subjects.FirstOrDefault(x => x.Id == selectedElementId);

                        if (subjectDb == null) return;

                        fraseToAdd.Subject = subjectDb;

                        context.Frases.Add(fraseToAdd);

                        context.SaveChanges();

                        fraseToAdd.Subject = Subjectlocal;

                        Subjectlocal.Frases.Add(fraseToAdd);

                        DataContext = new Frase() { IsLeft = fraseToAdd.IsLeft }; 

                        OnPropertyChanged(fraseToAdd.IsLeft ? "LeftFraseToAdd" : "RightFraseToAdd");
                        OnPropertyChanged(fraseToAdd.IsLeft ? "LeftFrases" : "RightFrases");
                    }
                }).ConfigureAwait(false);
            }

            else if (DataContext is Subject subjectToAdd)
            {
                await Task.Run(() =>
                {
                    using (var context = new DataContext())
                    {
                        context.Subjects.Add(subjectToAdd);

                        context.SaveChanges();
                        Subjects.Add(subjectToAdd);
                    }
                }).ConfigureAwait(false);
            }

        }
    }
}
