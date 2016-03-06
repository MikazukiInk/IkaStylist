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
        ///<summary>検索条件の最大サイズ</summary>
        const int RequestSize = 5;

        ///<summary>アタマ装備のデータ群</summary>
        private List<Gear> AtamaData = new List<Gear>();

        ///<summary>フク装備のデータ群</summary>
        private List<Gear> HukuData = new List<Gear>();

        ///<summary>クツ装備のデータ群</summary>
        private List<Gear> KutuData = new List<Gear>();

        ///<summary>コーデ検索クラスの実体</summary>
        public Searcher Searcher;

        ///<summary>メインウィンドウの初期化関数</summary>
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

        ///<summary>[さがす]ボタンの処理</summary>
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
            //検索結果DataGridを初期化
            ResultView = new ObservableSynchronizedCollection<Gear.Equipment>();

            //めんどくさい処理は初回のみ実行
            if (this.Searcher == null)
            {
                //各部位のギアデータをCSVから読み出す
                AtamaData = Gear.ReadCSV(Gear.Parts.Head);
                HukuData = Gear.ReadCSV(Gear.Parts.Cloth);
                KutuData = Gear.ReadCSV(Gear.Parts.Shoes);
                //検索クラスのインスタンス生成
                this.Searcher = new Searcher();
                //ゴリ押し全パターン検索
                this.Searcher.Init(AtamaData, HukuData, KutuData);
            }

            //絞込を実行してresultに格納
            var result = this.Searcher.Start(this.Requests,this.OnlyEnhanced);

            var temp = new Gear.Equipment();
            for (int i = 0; i < result.Count; i++)
            {
                var atamaName = AtamaData[result[i].HeadID].Name;
                var hukuName = HukuData[result[i].ClothID].Name;
                var kutuName = KutuData[result[i].ShoesID].Name;
                var text = Gear.Equipment.ResultToText(result[i]);
                ResultView.Add(new Gear.Equipment(atamaName, hukuName, kutuName, text));

                if (200 < i)
                    break;
            }
            this.ResultCount = result.Count;
        }
        #endregion

        ///<summary>ギア編集のボタン処理</summary>
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
            //CSVファイルが変更されるので既存の検索インスタンスを削除
            this.Searcher = null;   
            using (var vm = new GearEditViewModel(parameter))
            {
                Messenger.Raise(new TransitionMessage(vm, "EditCommand"));
            }
        }
        #endregion

        ///<summary>ギアパワーと要求Pt</summary>
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

        ///<summary>強化済みのみを使用するフラグ</summary>
        #region OnlyEnhanced変更通知プロパティ
        private bool _OnlyEnhanced;

        public bool OnlyEnhanced
        {
            get
            { return _OnlyEnhanced; }
            set
            { 
                if (_OnlyEnhanced == value)
                    return;
                _OnlyEnhanced = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        ///<summary>コンボボックスに表示するギアパワー名のリスト</summary>
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

        ///<summary>検索件数の表示用</summary>
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

        ///<summary>検索結果の表示用DataGridのItemSource</summary>
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

    }
}
