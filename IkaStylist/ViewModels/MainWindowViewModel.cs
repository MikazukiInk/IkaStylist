using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Deployment.Application;
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
        ///<summary>アタマ装備のデータ群</summary>
        private List<Gear> AtamaData = new List<Gear>();

        ///<summary>フク装備のデータ群</summary>
        private List<Gear> HukuData = new List<Gear>();

        ///<summary>クツ装備のデータ群</summary>
        private List<Gear> KutuData = new List<Gear>();

        ///<summary>フェス装備のデータ群</summary>
        private List<Gear> FesTData = new List<Gear>();

        ///<summary>コーデ検索クラスの実体</summary>
        public Searcher Searcher;
        ///<summary>コーデ検索クラスの実体　フェス用</summary>
        public Searcher SearcherFes;

        ///<summary>オプション管理クラスの実体</summary>
        private OptManager _OptMgr;
        public OptManager OptMgr
        {
            get
            {
                if (_OptMgr == null)
                {
                    _OptMgr = new OptManager();
                }
                return _OptMgr;
            }
            set
            {
                if (_OptMgr == value)
                    return;
                _OptMgr = value;
            }
        }

        ///<summary>メインウィンドウの初期化関数</summary>
        public void Initialize()
        {
            //タイトルにバージョン情報を付けて初期化
            this.Title = "イカスタイリスト" + "      " + SubRoutine.GetAppliVersion();

            //GearPowerNames初期化
            var temp = new ObservableSynchronizedCollection<string>();
            for (int ii = 0; ii < EnumExtension<GearPowerKind>.Length(); ii++)
            {
                temp.Add(EnumLiteralAttribute.GetLiteral((GearPowerKind)ii));
            }
            this.GearPowerNames = temp;

            //SelectionManager初期化
            this.SelectionMgr = new SelectionManager();

            //リクエスト初期化
            this.OptMgr.ResetRequest();

            //検索件数初期化
            ResultCount = 0;

            //結果発表の領域初期化
            ResultView = new ObservableSynchronizedCollection<Coordinate>();

            var tempVisibility = new bool[Enum.GetNames(typeof(GearPowerKind)).Length];
            for (int i = 0; i < this.ColumnVisibilitys.Length; i++)
            {
                tempVisibility[i] = false;
            }
            this.ColumnVisibilitys = tempVisibility;
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

        ///<summary>[さがす]ボタンの処理</summary>
        public void Search()
        {
            //検索結果DataGridを初期化
            ResultView = new ObservableSynchronizedCollection<Coordinate>();
            SelectionMgr.init();

            //結果発表の領域初期化
            ResultView = new ObservableSynchronizedCollection<Coordinate>();

            var tempVisibility = new bool[Enum.GetNames(typeof(GearPowerKind)).Length];
            for (int i = 0; i < this.ColumnVisibilitys.Length; i++)
            {
                tempVisibility[i] = false;
            }
            this.ColumnVisibilitys = tempVisibility;
            //めんどくさい処理は初回のみ実行
            if (this.Searcher == null)
            {
                //各部位のギアデータをCSVから読み出す
                AtamaData = IOmanager.ReadCSV(GearKind.Head);
                HukuData = IOmanager.ReadCSV(GearKind.Cloth);
                KutuData = IOmanager.ReadCSV(GearKind.Shoes);
                FesTData = IOmanager.ReadCSV(GearKind.FesT);
                //検索クラスのインスタンス生成
                this.Searcher = new Searcher(OptMgr);
                //ゴリ押し全パターン検索
                if (OptMgr.isFestival)
                {
                    this.Searcher.Init(AtamaData, FesTData, KutuData);
                }
                else
                {
                    this.Searcher.Init(AtamaData, HukuData, KutuData);
                }
            }

            //絞込を実行してresultに格納
            var result = this.Searcher.Start();
            var tempVis = new bool[Enum.GetNames(typeof(GearPowerKind)).Length];
            for (int i = 0; i < result.Count; i++)
            {
                ResultView.Add(new Coordinate(result[i]));

                for (int j = 0; j < result[i].points.Length; j++)
                {
                    if (0 < result[i].points[j])
                    {
                        tempVis[j] = true;
                    }
                }

                if (OptMgr.MaxReslutSize < i)
                {
                    break;
                }
            }
            this.ColumnVisibilitys = tempVis;
            this.ResultCount = result.Count;
        }
        #endregion

        #region ResetCommand
        private ViewModelCommand _ResetCommand;

        public ViewModelCommand ResetCommand
        {
            get
            {
                if (_ResetCommand == null)
                {
                    _ResetCommand = new ViewModelCommand(Reset);
                }
                return _ResetCommand;
            }
        }
        ///<summary>[リセット]の処理</summary>
        public void Reset()
        {
            //リクエスト初期化
            this.OptMgr.ResetRequest();
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
            if (OptMgr.isFestival && parameter == "Cloth")
            {
                parameter = "FesT";
            }
            using (var vm = new GearEditViewModel(parameter))
            {
                Messenger.Raise(new TransitionMessage(vm, "EditCommand"));
            }
        }
        #endregion

        #region toggleFesModeプロパティ
        public bool toggleFesMode
        {
            get
            {
                return OptMgr.isFestival;
            }
            set
            {
                this.Searcher = null;
                OptMgr.isFestival = !OptMgr.isFestival;
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

        ///<summary>検索結果で選択しているオブジェクトの取得</summary>
        #region SelectionMgrプロパティ
        public SelectionManager _SelectionMgr;
        public SelectionManager SelectionMgr
        {
            get
            { return _SelectionMgr; }
            set
            {
                if (_SelectionMgr == value)
                    return;
                _SelectionMgr = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SelectedCoordinate変更通知プロパティ
        private Coordinate _SelectedCoordinate;
        
        ///<summary>結果表示DataGridの選択アイテム</summary>
        public Coordinate SelectedCoordinate
        {
            get
            { return _SelectedCoordinate; }
            set
            { 
                if (_SelectedCoordinate == value)
                    return;
                _SelectedCoordinate = value;
                
                //詳細画面更新
                SelectionMgr.update(value);
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
        private ObservableSynchronizedCollection<Coordinate> _ResultView;

        public ObservableSynchronizedCollection<Coordinate> ResultView
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

        ///<summary>タイトルテキスト</summary>
        #region Title変更通知プロパティ
        private string _Title;

        public string Title
        {
            get
            { return _Title; }
            set
            {
                if (_Title == value)
                    return;
                _Title = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ColumnVisibilitys変更通知プロパティ
        private bool[] _ColumnVisibilitys = new bool[Enum.GetNames(typeof(GearPowerKind)).Length];

        public bool[] ColumnVisibilitys
        {
            get
            { return _ColumnVisibilitys; }
            set
            {
                if (_ColumnVisibilitys == value)
                    return;
                _ColumnVisibilitys = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region OpenFolderCommand
        private ViewModelCommand _OpenFolderCommand;

        public ViewModelCommand OpenFolderCommand
        {
            get
            {
                if (_OpenFolderCommand == null)
                {
                    _OpenFolderCommand = new ViewModelCommand(OpenFolder);
                }
                return _OpenFolderCommand;
            }
        }

        public void OpenFolder()
        {
            string path = string.Empty;
            // ClickOnceデータ・ディレクトリのフル・パスを取得する
            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                path = SubRoutine.GetDirectoryName();
            }
            else
            {
                path = ApplicationDeployment.CurrentDeployment.DataDirectory;
            }
            path += @"\GearDataCsv";
            System.Diagnostics.Process.Start(path);
        }
        #endregion

        #region OnlineUpdateCommand
        private ViewModelCommand _OnlineUpdateCommand;

        public ViewModelCommand OnlineUpdateCommand
        {
            get
            {
                if (_OnlineUpdateCommand == null)
                {
                    _OnlineUpdateCommand = new ViewModelCommand(OnlineUpdate);
                }
                return _OnlineUpdateCommand;
            }
        }

        public void OnlineUpdate()
        {
            System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            try
            {
                //ClickOnceでのインストールかをチェック
                if (!ApplicationDeployment.IsNetworkDeployed)
                {
                    MessageBox.Show("更新できないインストール方法です");
                    return;
                }

                ApplicationDeployment currentDeploy = ApplicationDeployment.CurrentDeployment;

                if (currentDeploy.CheckForUpdate())
                {

                    if (MessageBox.Show("最新版が利用できます。更新しますか？", "更新の確認"
                        , MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        currentDeploy.Update();
                    }
                    else
                    {
                        return;
                    }

                    if (MessageBox.Show("更新が完了しました。更新を有効にするにはアプリケーションを再起動する必要があります。再起動しますか？", "再起動の確認"
                        , MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        System.Windows.Forms.Application.Restart();
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    MessageBox.Show("既に最新版です");
                }

            }
            catch (DeploymentException exp)
            {
                MessageBox.Show(exp.Message, "更新エラー");

            }
            finally
            {
                System.Windows.Input.Mouse.OverrideCursor = null;
                //this.Cursor = Cursors.Default;
            }
        }
        #endregion

        #region OpenLinkCommand
        private ListenerCommand<string> _OpenLinkCommand;

        public ListenerCommand<string> OpenLinkCommand
        {
            get
            {
                if (_OpenLinkCommand == null)
                {
                    _OpenLinkCommand = new ListenerCommand<string>(OpenLink);
                }
                return _OpenLinkCommand;
            }
        }

        public void OpenLink(string parameter)
        {
            System.Diagnostics.Process.Start(parameter);
        }
        #endregion

        #region TestCommand
        private ViewModelCommand _TestCommand;

        public ViewModelCommand TestCommand
        {
            get
            {
                if (_TestCommand == null)
                {
                    _TestCommand = new ViewModelCommand(Test);
                }
                return _TestCommand;
            }
        }

        public void Test()
        {
             System.Diagnostics.Process.Start("https://twitter.com/kazuki_mikan");
          
        }
        #endregion

    }
}
