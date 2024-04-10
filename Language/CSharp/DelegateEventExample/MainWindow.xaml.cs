using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DelegateEventExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ModelManager<Model> _manager;
        List<Model> models1 = new List<Model>() { new Model { Age = 18, Name = "Test1" }, new Model { Age = 20, Name = "Test2" } };
        private Timer _timer ;
        public ObservableCollection<Model> Models
        {
            get { return _manager.ModelCollection; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChagned([CallerMemberName] string parameter = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parameter));
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _manager = new ModelManager<Model>(models1, 1);
            _manager.NoChangeEvnet += OnNoChange;
            this.Closing += OnWindowClose;
        }

        private void OnWindowClose(object? sender, CancelEventArgs e)
        {
            _manager.Dispose();
        }

        private void OnNoChange(int modelManager)
        {
            //MessageBox.Show($"{modelManager.SetNumber} NoChagned");
            Debug.Print($"{modelManager} NoChagned");
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var random = new Random();
            var index = random.Next(0, 10);
            _timer = new Timer(delegate
            {
                while(true)
                {
                    _manager.AddModel(index, new Model { Age = index + 10 + random.NextDouble(), Name = "Test3" });
                    Thread.Sleep(10);
                }


            }, null, 100, 0);            
        }

    }
    public class Model
    {
        public string? Name { get; set; }
        public double Age { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj is Model)
            {
                var model = obj as Model;

                if (model != null)
                    return model.Age == this.Age;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public class ModelManager<T> : IDisposable
        where T : class
    {
        public delegate void NoChageDelegate(int setNumber);
        public event NoChageDelegate NoChangeEvnet;

        private Thread _thread;
        private bool _disposed = false;
        private Dictionary<int, List<T>> Models = new();
        private object _lock = new object();
        CAS_Lock _cas = new CAS_Lock();
        public StObservableCollection<T> ModelCollection { get; private set; } = new();

        public bool IsThreadCircle = true;

        private DateTime[] lastChangeTime = new DateTime[11];
        public ModelManager(List<T> models, int setNumber)
        {
            Models.Add(setNumber, models);
            lastChangeTime[setNumber] = DateTime.Now;
            StartMonitoring();
            NoChangeEvnet += NoUpdateEvent;
        }

        private void NoUpdateEvent(int setNumber)
        {
            lock (_lock)
            {
                Models[setNumber].Clear();
            }
        }

        private async void StartMonitoring()
        {
            while (IsThreadCircle)
            {
                lock (_lock)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        ModelCollection.Clear();
                    });

                    foreach (var model in Models)
                    {
                        if ((DateTime.Now - lastChangeTime[model.Key]).TotalSeconds >= 5)
                        {
                            NoChangeEvnet?.Invoke(model.Key);

                        }
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            ModelCollection.AddRange(model.Value);
                        });


                    }
                }

                await Task.Delay(100); // 5초마다 체크
            }
        }

        //private void StartMonitoring()
        //{
        //    _thread = new Thread(() =>
        //    {
        //        while (IsThreadCircle)
        //        {
        //            lock (_lock)
        //            {
        //                App.Current.Dispatcher.BeginInvoke(new Action(() =>
        //                {
        //                    ModelCollection.Clear();
        //                }));

        //                foreach (var model in Models)
        //                {
        //                    var key = model.Key;
        //                    if ((DateTime.Now - lastChangeTime[model.Key]).TotalSeconds >= 5)
        //                    {
        //                        NoChangeEvnet?.Invoke(model.Key);
        //                    }
        //                    App.Current.Dispatcher.BeginInvoke(new Action(() =>
        //                    {
        //                        ModelCollection.AddRange(model.Value);
        //                    }));


        //                }
        //            }
        //            Thread.Sleep(100);
        //        }
        //    });
        //    _thread.Start();
        //}
        public void AddModel(int setNumber, T model)
        {
            lock (_lock)
            {
                if (Models.ContainsKey(setNumber) == false)
                {
                    Models.Add(setNumber, new List<T>());
                }
                Models[setNumber].Add(model);
                lastChangeTime[setNumber] = DateTime.Now;
            }            

        }
        public void UpdatePerson(int setNumber, T model)
        {
            if (setNumber < 0 || setNumber >= Models.Count)
                throw new ArgumentException("Index is out of range.");

            Models[setNumber].Add(model);
            lastChangeTime[setNumber] = DateTime.Now;
        }
        

        ~ModelManager() => Dispose(false);
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_thread.IsAlive)
                {
                    IsThreadCircle = false;
                    _thread.Join();
                    _thread = null;
                }

            }


            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    public class CAS_Lock
    {
        // 0 = false
        // 1 = true
        private volatile int _lock = 0;

        public void Lock()
        {
            while (true)
            {
                if (Interlocked.CompareExchange(ref _lock, 1, 0) == 0)
                {
                    return;
                }
            }
        }

        public void Free()
        {
            _lock = 0;
        }
    }

    public class StObservableCollection<T> : ObservableCollection<T>
    {
        private bool _suppressNotification = false;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            _suppressNotification = true;

            foreach (T item in list)
            {
                Add(item);
            }
            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public void RemoveRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            _suppressNotification = true;

            foreach (T item in list)
            {
                Remove(item);
            }
            _suppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
