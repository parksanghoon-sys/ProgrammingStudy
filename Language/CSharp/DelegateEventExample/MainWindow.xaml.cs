using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<Model> models2 = new ObservableCollection<Model>() { new Model { Age = 18, Name = "Test1" }, new Model { Age = 20, Name = "Test2" } };
        ObservableCollection<Model> models3 = new ObservableCollection<Model>() { new Model { Age = 18, Name = "Test1" }, new Model { Age = 20, Name = "Test2" } };
        public ObservableCollection<Model> Models
        {
            get { return _manager.ModelCollection; }           
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChagned([CallerMemberName] string parameter ="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parameter));
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _manager = new ModelManager<Model>(models1,1);
            _manager.NoChangeEvnet += OnNoChange;   
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
            _manager.AddModel(index, new Model { Age = index + 10, Name = "Test3" });            
        }
    }
    public class Model
    {
        public string? Name { get; set; }
        public int Age { get; set; }
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
    public class ModelManager<T>
    {
        public delegate void NoChageDelegate(int setNumber);
        public event NoChageDelegate NoChangeEvnet;
        private Dictionary<int, List<T>> Models = new();
        public ObservableCollection<T> ModelCollection { get; private set; } = new();
        public int SetNumber { get; set; }

        private DateTime[] lastChangeTime = new DateTime[10];
        public ModelManager(List<T> models, int setNumber)
        {
            Models.Add(setNumber,models);
            SetNumber = setNumber;
            lastChangeTime[setNumber] = DateTime.Now;
            StartMonitoring();
        }

        //private async Task StartMonitoring()
        //{
        //    while (true)
        //    {
        //        foreach(var model in Models)
        //        {
        //            if ((DateTime.Now - lastChangeTime[model.Key]).TotalSeconds >= 5)
        //            {
        //                NoChangeEvnet?.Invoke(model.Key);
                                     
        //            }
        //            foreach(var data in model.Value)
        //            {
        //                if(ModelCollection.Contains(data) == false)
        //                {
        //                    ModelCollection.Add(data);
        //                }
                        
        //            }
                    
        //            await Task.Delay(1000); // 5초마다 체크
        //        }
        
        //    }
        //}
        private void StartMonitoring()
        {
            new Thread(() =>
            {
                while (true)
                {
                    foreach (var model in Models)
                    {
                        if ((DateTime.Now - lastChangeTime[model.Key]).TotalSeconds >= 5)
                        {
                            NoChangeEvnet?.Invoke(model.Key);

                        }
                        foreach (var data in model.Value)
                        {
                            if (ModelCollection.Contains(data) == false)
                            {
                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    ModelCollection.Add(data);

                                });
                            }

                        }

                        Thread.Sleep(5000);
                    }
                }
            }).Start();
        }
        public void AddModel(int setNumber ,T model)
        {
            if(Models.ContainsKey(setNumber) == false)
            {
                Models.Add(setNumber, new List<T>());
            }
            Models[setNumber].Add(model);
            lastChangeTime[setNumber] = DateTime.Now;
        }
        public void UpdatePerson(int setNumber, T model)
        {
            if (setNumber < 0 || setNumber >= Models.Count)
                throw new ArgumentException("Index is out of range.");

            Models[setNumber].Add(model);
            lastChangeTime[setNumber] = DateTime.Now;
        }
    }
}
