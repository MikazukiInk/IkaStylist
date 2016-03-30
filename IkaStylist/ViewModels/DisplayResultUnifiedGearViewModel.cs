using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
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

            ResultGearData = new ObservableSynchronizedCollection<Gear>();
            fakeGearData = new List<Gear>();
            pureGearData = new List<Gear>();
            string partsNameE = "";
            if (partsIndex == 0)
            {
                partsName = "アタマ";
                partsNameE = GearKind.Head.ToString();
            }
            else if (partsIndex == 0)
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
        private ObservableSynchronizedCollection<Gear> _ResultGearData;
        public ObservableSynchronizedCollection<Gear> ResultGearData
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
            this.ResultGearData = new ObservableSynchronizedCollection<Gear>(fakeGearData);
        }
    }
}
