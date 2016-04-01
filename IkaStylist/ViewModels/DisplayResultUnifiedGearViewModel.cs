using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using IkaStylist.Models;

namespace IkaStylist.ViewModels
{
    public class DisplayResultUnifiedGearViewModel : ViewModel
    {
        private int partsIndex = 0;
        private bool fakeBrand = false;
        private bool pureBrand = false;
        private bool onlyExchange = false;
        private int fakeBrandNum = 1;
        private int pureBrandNum = 1;

        ///<summary>オプション管理クラスの実体</summary>
        private OptManager _OptMgr;
        public OptManager OptMgr
        {
            get
            {
                if (_OptMgr == null)
                {
                    _OptMgr = OptManager.GetInstance();
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

        public DisplayResultUnifiedGearViewModel()
        {
            this.Initialize();
        }

        public void Initialize()
        {
            partsIndex = OptMgr.UnifiedGP_selectParts;
            fakeBrand = OptMgr.UnifiedGP_displayFakeGear;
            pureBrand = OptMgr.UnifiedGP_displayPureGear;
            onlyExchange = OptMgr.UnifiedGP_displayOnlyExchange;
            fakeBrandNum = OptMgr.UnifiedGP_fakeGearNum;
            pureBrandNum = OptMgr.UnifiedGP_pureGearNum;

            ResultGearData = new List<Gear>();
            ResultGearDataGrid1 = new ObservableSynchronizedCollection<Gear>();
            ResultGearDataGrid2 = new ObservableSynchronizedCollection<Gear>();
            ResultGearDataGrid3 = new ObservableSynchronizedCollection<Gear>();
            fakeGearData = new List<Gear>();
            pureGearData = new List<Gear>();
            string partsNameE = "";
            if (partsIndex == 0)
            {
                partsName = "アタマ";
                partsNameE = GearKind.Head.ToString();
            }
            else if (partsIndex == 1)
            {
                partsName = "フク";
                partsNameE = GearKind.Cloth.ToString();
            }
            else {
                partsName = "クツ";
                partsNameE = GearKind.Shoes.ToString();
            }
            var data = IOmanager.ReadCSV(partsNameE);
            for (int i = 0; i < data.Count; i++)
            {
                fakeGearData.Add(data[i]);
                pureGearData.Add(data[i]);
            }
            filtering();
        }

        #region
        private string _partsName;
        public string partsName
        {
            get
            { return _partsName; }
            set
            {
                if (_partsName == value)
                    return;
                _partsName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ギアデータのマスターファイル, これに保存する.
        private List<Gear> _fakeGearData;
        public List<Gear> fakeGearData
        {
            get
            { return _fakeGearData; }
            set
            {
                if (_fakeGearData == value)
                    return;
                _fakeGearData = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ギアデータのマスターファイル, これに保存する.
        private List<Gear> _pureGearData;
        public List<Gear> pureGearData
        {
            get
            { return _pureGearData; }
            set
            {
                if (_pureGearData == value)
                    return;
                _pureGearData = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region GearData変更通知プロパティ
        private List<Gear> _ResultGearData;
        public List<Gear> ResultGearData
        {
            get
            { return _ResultGearData; }
            set
            {
                if (_ResultGearData == value)
                    return;
                _ResultGearData = value;
                RaisePropertyChanged();
            }
        }
        private ObservableSynchronizedCollection<Gear> _ResultGearDataGrid1;
        public ObservableSynchronizedCollection<Gear> ResultGearDataGrid1
        {
            get
            { return _ResultGearDataGrid1; }
            set
            {
                if (_ResultGearDataGrid1 == value)
                    return;
                _ResultGearDataGrid1 = value;
                RaisePropertyChanged();
            }
        }
        private ObservableSynchronizedCollection<Gear> _ResultGearDataGrid2;
        public ObservableSynchronizedCollection<Gear> ResultGearDataGrid2
        {
            get
            { return _ResultGearDataGrid2; }
            set
            {
                if (_ResultGearDataGrid2 == value)
                    return;
                _ResultGearDataGrid2 = value;
                RaisePropertyChanged();
            }
        }
        private ObservableSynchronizedCollection<Gear> _ResultGearDataGrid3;
        public ObservableSynchronizedCollection<Gear> ResultGearDataGrid3
        {
            get
            { return _ResultGearDataGrid3; }
            set
            {
                if (_ResultGearDataGrid3 == value)
                    return;
                _ResultGearDataGrid3 = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public void filtering()
        {
            if (fakeGearData == null || pureGearData == null)
            {
                return;
            }

            //偽ブランド
            if (fakeBrand)
            {
                fakeGearData.RemoveAll(x => x.getFakeBrandNum < fakeBrandNum);
            }
            else
            {
                fakeGearData.Clear();
            }
            //純ブランド
            if (pureBrand)
            {
                pureGearData.RemoveAll(x => x.getPureBrandNum < pureBrandNum);
            }
            else
            {
                pureGearData.Clear();
            }

            if (fakeBrand && pureBrand)
            {
                foreach (Gear tmp in pureGearData)
                {
                    bool exist = false;
                    foreach (Gear tmpFake in fakeGearData)
                    {
                        if (tmpFake.Name == tmp.Name)
                        {
                            exist = true;
                        }
                    }
                    if (!exist)
                    {
                        fakeGearData.Add(tmp);
                    }
                }
            }
            else
            {
                fakeGearData.AddRange(pureGearData);
            }
            //交換可能ギアか？
            if (onlyExchange)
            {
                fakeGearData.RemoveAll(x => !x.canExchange);
            }
            this.ResultGearData = new List<Gear>(fakeGearData);
            this.ResultGearData.Sort(CompareGearAscendingOrderByName);
            makeResultListGrid();
        }

        private void resetResultListGrid()
        {
            this.ResultGearDataGrid1.Clear();
            this.ResultGearDataGrid2.Clear();
            this.ResultGearDataGrid3.Clear();
        }
            
        private void makeResultListGrid()
        {
            resetResultListGrid();
            for( int i = 0; i < this.ResultGearData.Count; i++)
            {
                if (i < 10)
                {
                    this.ResultGearDataGrid1.Add(this.ResultGearData[i]);
                }
                else if( i < 20)
                {
                    this.ResultGearDataGrid2.Add(this.ResultGearData[i]);
                }
                else
                {
                    this.ResultGearDataGrid3.Add(this.ResultGearData[i]);
                }
            }
            updateGridVisiblity();
        }

        #region ソートボタンコマンド.
        //名前.
        private ViewModelCommand _SortAscendingOrderByNameCOM;
        public ViewModelCommand SortAscendingOrderByNameCOM
        {
            get
            {
                if (_SortAscendingOrderByNameCOM == null)
                {
                    _SortAscendingOrderByNameCOM = new ViewModelCommand(SortAscendingOrderByName);
                }
                return _SortAscendingOrderByNameCOM;
            }
        }
        public void SortAscendingOrderByName()
        {
            this.ResultGearData.Sort(CompareGearAscendingOrderByName);
            makeResultListGrid();
        }

        private ViewModelCommand _SortDescendingOrderByNameCOM;
        public ViewModelCommand SortDescendingOrderByNameCOM
        {
            get
            {
                if (_SortDescendingOrderByNameCOM == null)
                {
                    _SortDescendingOrderByNameCOM = new ViewModelCommand(SortDescendingOrderByName);
                }
                return _SortDescendingOrderByNameCOM;
            }
        }
        public void SortDescendingOrderByName()
        {
            this.ResultGearData.Sort(CompareGearDescendingOrderByName);
            makeResultListGrid();
        }

        // ブランド.
        private ViewModelCommand _SortAscendingOrderByBrandCOM;
        public ViewModelCommand SortAscendingOrderByBrandCOM
        {
            get
            {
                if (_SortAscendingOrderByBrandCOM == null)
                {
                    _SortAscendingOrderByBrandCOM = new ViewModelCommand(SortAscendingOrderByBrand);
                }
                return _SortAscendingOrderByBrandCOM;
            }
        }
        public void SortAscendingOrderByBrand()
        {
            this.ResultGearData.Sort(CompareGearAscendingOrderByBrand);
            makeResultListGrid();
        }

        private ViewModelCommand _SortDescendingOrderByBrandCOM;
        public ViewModelCommand SortDescendingOrderByBrandCOM
        {
            get
            {
                if (_SortDescendingOrderByBrandCOM == null)
                {
                    _SortDescendingOrderByBrandCOM = new ViewModelCommand(SortDescendingOrderByBrand);
                }
                return _SortDescendingOrderByBrandCOM;
            }
        }
        public void SortDescendingOrderByBrand()
        {
            this.ResultGearData.Sort(CompareGearDescendingOrderByBrand);
            makeResultListGrid();
        }

        // メインパワー.
        private ViewModelCommand _SortAscendingOrderByMainPowerCOM;
        public ViewModelCommand SortAscendingOrderByMainPowerCOM
        {
            get
            {
                if (_SortAscendingOrderByMainPowerCOM == null)
                {
                    _SortAscendingOrderByMainPowerCOM = new ViewModelCommand(SortAscendingOrderByMainPower);
                }
                return _SortAscendingOrderByMainPowerCOM;
            }
        }
        public void SortAscendingOrderByMainPower()
        {
            this.ResultGearData.Sort(CompareGearAscendingOrderByMainPower);
            makeResultListGrid();
        }

        private ViewModelCommand _SortDescendingOrderByMainPowerCOM;
        public ViewModelCommand SortDescendingOrderByMainPowerCOM
        {
            get
            {
                if (_SortDescendingOrderByMainPowerCOM == null)
                {
                    _SortDescendingOrderByMainPowerCOM = new ViewModelCommand(SortDescendingOrderByMainPower);
                }
                return _SortDescendingOrderByMainPowerCOM;
            }
        }
        public void SortDescendingOrderByMainPower()
        {
            this.ResultGearData.Sort(CompareGearDescendingOrderByMainPower);
            makeResultListGrid();
        }
        #endregion

        #region ソート系
        // 名前.
        private static int CompareGearAscendingOrderByName(Gear a, Gear b)
        {
            return string.Compare(a.Name, b.Name);
        }
        private static int CompareGearDescendingOrderByName(Gear a, Gear b)
        {
            return string.Compare(b.Name, a.Name);
        }

        // ブランド.
        private static int CompareGearAscendingOrderByBrand(Gear a, Gear b)
        {
            return a.Brand.Id - b.Brand.Id;
        }
        private static int CompareGearDescendingOrderByBrand(Gear a, Gear b)
        {
            return b.Brand.Id - a.Brand.Id;
        }

        // メインパワー.
        private static int CompareGearAscendingOrderByMainPower(Gear a, Gear b)
        {
            return a.MainPower.Id - b.MainPower.Id;
        }
        private static int CompareGearDescendingOrderByMainPower(Gear a, Gear b)
        {
            return b.MainPower.Id - a.MainPower.Id;
        }
        #endregion

        #region
        private void updateGridVisiblity()
        {
            this.isVisiblityGrid2 = System.Windows.Visibility.Visible;
            this.isVisiblityGrid3 = System.Windows.Visibility.Visible;
        }

        public System.Windows.Visibility isVisiblityGrid2
        {
            get
            {
                if (ResultGearData.Count > 10)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Collapsed;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }
        public System.Windows.Visibility isVisiblityGrid3
        {
            get
            {
                if (ResultGearData.Count > 20)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Collapsed;
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
