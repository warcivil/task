using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace ConsoleApp6
{
    class Task1
    {
        private dynamic _jsonWellParametrs; // wellParameters.json
        private List<String> _unicalId = new List<String>();
        public Task1(dynamic jsonWellParametrs) =>
            _jsonWellParametrs = jsonWellParametrs;

        public string ToString() // 
        {
            string t = "";
            foreach (var item in _jsonWellParametrs)
            {
                if(!_unicalId.Contains((String)item["WellId"]))
                {
                    t +=item["ParameterName"] + "\n";
                    _unicalId.Add((String)item["WellId"]);
                }

            }
            return t;
        }
    }
    class Task2
    {
        private dynamic _jsonFile1; // wellParameters.json
        private dynamic _jsonFile2; // wells.json
        private Dictionary<int, List<double>> _valueList = new Dictionary<int, List<double>>();

        public Task2(dynamic jsonDate1, dynamic jsonDate2)
        {
            _jsonFile1 = jsonDate1;
            _jsonFile2 = jsonDate2;
        }

        public void Find()
        {
            for (int i = 10; i <= 30; i++)
            {
                _valueList.Add(key: i, value: new List<double>());
            }   
            foreach (var ind2 in _jsonFile1)
            {
                if ((int)ind2["WellId"] >= 10 && (int)ind2["WellId"] <= 30)
                {
                        
                    _valueList[(int)ind2["WellId"]].Add((double)ind2["Value"]);
                }
            }
            foreach (var pair in _valueList)
            {
                pair.Value.Sort((pr1, pr2) => { return pr1.CompareTo(pr2); });
            }
        }
        private String findId(int id)
        {
            foreach (var item in _jsonFile2)
            {
                if ((int)item["Id"] == id)
                {
                    return (String)item["Name"];

                }
            }
            return "NoName";
        }
       private String findParamId(int id)
        {
            foreach (var item in _jsonFile1)
            {
                if ((int)item["WellId"] == id)
                {
                    return (String)item["ParameterName"];
                }
            }
            return "NoName";
        }
        private double Sr(List<double> value)
        {
            double sr = 0;
           foreach(var item in value)
            {
                sr += item;  // ищем среднее значение
            }
            return sr/(-1+value.Count);
        }
            
        public String ToString()
        {
            String info = "";
            foreach(var item in _valueList)
            {
                info +=findParamId(item.Key) 
                    + "\n" 
                    + findId(item.Key) + 
                    ": max " + Math.Round(item.Value[item.Value.Count - 1], 2) +
                    " min " + Math.Round(item.Value[0], 2) +
                    " med " + Math.Round(item.Value[item.Value.Count / 2 + 1], 2) + 
                    " avg " + Math.Round(Sr(item.Value), 2) + "\n";
            }
            return info;
        }
    }

    class Task3
    {
        private dynamic _jsonFile1; // departments.json
        private dynamic _jsonFile2; // wells.json
        public Task3(dynamic jsonDate1, dynamic jsonDate2)
        {
            _jsonFile1 = jsonDate1;
            _jsonFile2 = jsonDate2;
        }

        public String ToString()
        {
            string text = "";
            foreach(var item in _jsonFile1)
            {
                text += "\n" + item["Name"] + "\n";
                foreach(var well in _jsonFile2)
                {
                    if(well["X"] != null && well["Y"] != null)
                    if (((double)item["X"] - (double)well["X"]) * ((double)item["X"] - (double)well["X"]) +
                        ((double)item["Y"] - (double)well["Y"]) * ((double)item["Y"] - (double)well["Y"])
                        <= (double)item["Radius"] * (double)item["Radius"]) // (x0-x1)^2 + (y0-y1)^2 < r^2
                            
                            text += well["Name"] + " ";
                }
            }
            text +="\n" + "Название  скважин без координат ";
            foreach (var well in _jsonFile2)
            {
                if (well["X"] == null && well["Y"] == null)
                {
                    text += well["Name"] + " ";
                }    
                    
            }

            return text;
        }


    }
    class Program
        {
        // можно было просто создать 3 dynamic переменных и сократить время дессериализации
        // сейчас задача решается дессериализацией 5 файлов(2 раза wellParametrs, 2раза wells, 1 раз departments)
        // не хорошо, что мы два раза дессериализуем один и тот же файл, но в условиях данной задачи это несущественно
        // из плюсов - лучшая читаемость кода, нет колхоза со строками 

        static dynamic goJson(String PATH) => 
               JsonConvert.DeserializeObject((new StreamReader(PATH)).ReadToEnd()); // конвертируем в json

        static void Main(string[] args)
            {

                Task1 task1 = new Task1(goJson("wellParameters.json"));
                Task2 task2 = new Task2(goJson("wellParameters.json"), goJson("wells.json"));
                Task3 task3 = new Task3(goJson("departments.json"), goJson("wells.json"));
                task2.Find();
                Console.WriteLine("-------task1-------");
                Console.WriteLine(task1.ToString());
                Console.WriteLine("-------task1-------");
                Console.WriteLine("-------task2-------");
                Console.WriteLine(task2.ToString());
                Console.WriteLine("-------task2-------");
                Console.WriteLine("-------task3-------");
                Console.WriteLine(task3.ToString());
                Console.WriteLine("-------task3-------");
                Console.ReadKey();
            }

        }
    }
