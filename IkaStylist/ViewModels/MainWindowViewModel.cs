using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Data;
using System.IO;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using CsvHelper;

using IkaStylist.Models;

namespace IkaStylist.ViewModels
{

    public class MainWindowViewModel : ViewModel
    {
        const int RequestSize = 5;

        public bool firstTry = true;

        public Searcher Searcher;

        public void Initialize()
        {
            //ギアパワー名　→　ギアパワーIDの変換テーブル初期化
            Gear.initTable();
            //GearPowerNames初期化
            var temp = new ObservableSynchronizedCollection<string>();
            for (int ii = 0; ii < EnumExtension<Gear.Power>.Length(); ii++)
            {
                temp.Add(EnumLiteralAttribute.GetLiteral((Gear.Power)ii));
            }
            this.GearPowerNames = temp;
            //リクエスト初期化
            var tempReq = new Request[RequestSize];
            for (int i = 0; i < this.Requests.Length; i++)
            {
                tempReq[i] = new Request();
            }
            this.Requests = tempReq;
            this.Requests[0].GearPowerID = 1;//条件１は攻撃力に設定
            this.Requests[0].Point = 3;//条件１は攻撃力に設定
            //検索件数初期化
            ResultCount = 0;
            //結果発表の領域初期化
            ResultView = new ObservableSynchronizedCollection<Gear.Equipment>();
        }

        #region SearchCommand
        private ViewModelCommand _SearchCommand;

        public ViewModelCommand SearchCommand
        {
            get
            {
                if (_SearchCommand == null)
                {
                    _SearchCommand = new ViewModelCommand(Search);
                }
                return _SearchCommand;
            }
        }

        public void Search()
        {
            ResultView = new ObservableSynchronizedCollection<Gear.Equipment>();

            //各部位のギアデータをCSVから読み出す
            var atama = Gear.ReadCSV(Gear.Parts.Head);
            var huku = Gear.ReadCSV(Gear.Parts.Cloth);
            var kutu = Gear.ReadCSV(Gear.Parts.Shoes);

            if (this.Searcher == null)
            {
                //検索クラスのインスタンス生成
                this.Searcher = new Searcher();
                //ゴリ押し全パターン検索
                this.Searcher.Init(atama, huku, kutu);
            }

            //絞込を実行してresultに格納
            var result = this.Searcher.Start(this.Requests);

            var temp = new Gear.Equipment();
            for (int i = 0; i < result.Count; i++)
            {
                var atamaName = atama[result[i].HeadID].Name;
                var hukuName = huku[result[i].ClothID].Name;
                var kutuName = kutu[result[i].ShoesID].Name;
                var text = Gear.Equipment.ResultToText(result[i]);
                ResultView.Add(new Gear.Equipment(atamaName, hukuName, kutuName, text));

                if (200 < i)
                    break;
            }
            this.ResultCount = result.Count;
            //とりあえず一番はじめの組み合わせを表示して終わり
            //MessageBox.Show(string.Format("{0}\n{1}\n{2}", hGearName, cGearName, sGearName));
        }
        #endregion

        #region EditCommand
        private ListenerCommand<string> _EditCommand;

        public ListenerCommand<string> EditCommand
        {
            get
            {
                if (_EditCommand == null)
                {
                    _EditCommand = new ListenerCommand<string>(Edit);
                }
                return _EditCommand;
            }
        }

        public void Edit(string parameter)
        {
            using (var vm = new GearEditViewModel(parameter))
            {
                Messenger.Raise(new TransitionMessage(vm, "EditCommand"));
            }
        }
        #endregion

        #region SaveCommand
        private ViewModelCommand _SaveCommand;

        public ViewModelCommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new ViewModelCommand(Save);
                }
                return _SaveCommand;
            }
        }

        public void Save()
        {
            SubRoutine.ConvertDataTableToCsv(this.GearData, @"C:\Users\takada-kazuki\Desktop\test.csv", true);
        }
        #endregion

        #region Requests変更通知プロパティ
        private Request[] _Requests = new Request[RequestSize];
        //UIの検索条件を受け取るやつ
        public Request[] Requests
        {
            get
            { return _Requests; }
            set
            {
                if (_Requests == value)
                    return;
                _Requests = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region GearPowerNames変更通知プロパティ
        private ObservableSynchronizedCollection<string> _GearPowerNames;
        //コンボボックスにギアパワー名を表示するためのなにか
        public ObservableSynchronizedCollection<string> GearPowerNames
        {
            get
            { return _GearPowerNames; }
            set
            {
                if (_GearPowerNames == value)
                    return;
                _GearPowerNames = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ResultCount変更通知プロパティ
        private int _ResultCount;

        public int ResultCount
        {
            get
            { return _ResultCount; }
            set
            {
                if (_ResultCount == value)
                    return;
                _ResultCount = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region GearData変更通知プロパティ
        private DataTable _GearData;

        public DataTable GearData
        {
            get
            { return _GearData; }
            set
            {
                if (_GearData == value)
                    return;
                _GearData = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ResultView変更通知プロパティ
        private ObservableSynchronizedCollection<Gear.Equipment> _ResultView;

        public ObservableSynchronizedCollection<Gear.Equipment> ResultView
        {
            get
            { return _ResultView; }
            set
            {
                if (_ResultView == value)
                    return;
                _ResultView = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region testGear変更通知プロパティ
        private Gear _testGear;

        public Gear testGear
        {
            get
            { return _testGear; }
            set
            {
                if (_testGear == value)
                    return;
                _testGear = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
