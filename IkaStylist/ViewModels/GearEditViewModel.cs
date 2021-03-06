﻿using System;
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
    public class GearEditViewModel : ViewModel
    {
        //ギアデータのファイル名
        private string FileName;

        ///<summary>コンストラクタ</summary>
        /// <param name="fname">ギアの部位名</param>
        public GearEditViewModel(string fname)
        {
            this.FileName = fname;
            makeBrandNames();
            this.filterBrandId = 0;
            this.UnInputed = false;
            this.UnInputedNum = 1;
            this.filterF = 0;
            this.filterAll = true;
            this.filterOnlyStrength = false;
            this.filterOnlyNotStrength = false;
        }

        private void makeBrandNames()
        {
            //BrandNames初期化
            var temp = new ObservableSynchronizedCollection<string>();
            temp.Add("全てのブランド");
            for (int ii = 1; ii < IkaUtil.brands.Length; ii++)
            {
                temp.Add(IkaUtil.brands[ii].Name);
            }
            this.BrandNames = temp;
        }
        
        ///<summary>ギアエディット画面の初期化</summary>
        public void Initialize()
        {
            GearData        = new ObservableSynchronizedCollection<Gear>();
            GearData_Master = new List<Gear>();
            var data = IOmanager.ReadCSV(FileName);
            for (int i = 0; i < data.Count; i++)
            {
                GearData.Add(data[i]);
                GearData_Master.Add(data[i]);
            }
        }

        public void filtering()
        {
            if (GearData_Master == null)
            {
                return;
            }
            List<Gear> tmp = new List<Gear>(GearData_Master);
            if (filterBrandId != 0)
            {
                tmp.RemoveAll(x => x.Brand.Id != filterBrandId);
            }

            if (filterOnlyStrength)
            {
                //強化済みのみ.
                tmp.RemoveAll(x => x.unInputedGearPower > 0);
            }
            else if (filterOnlyNotStrength)
            {
                //未強化のみ.
                tmp.RemoveAll(x => x.unInputedGearPower < UnInputedNum);
            }
            this.GearData = new ObservableSynchronizedCollection<Gear>(tmp);
        }

        #region ギアデータのマスターファイル, これに保存する.
        private List<Gear> _GearData_Master;
        public List<Gear> GearData_Master
        {
            get
            { return _GearData_Master; }
            set
            {
                if (_GearData_Master == value)
                    return;
                _GearData_Master = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region GearData変更通知プロパティ
        private ObservableSynchronizedCollection<Gear> _GearData;
        public ObservableSynchronizedCollection<Gear> GearData
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

        #region SelectedGear変更通知プロパティ
        private Gear _SelectedGear;

        public Gear SelectedGear
        {
            get
            { return _SelectedGear; }
            set
            {
                if (_SelectedGear == value)
                {
                    return;
                }
                if (value == null)
                {
                    return;
                }
                _SelectedGear = getMasterGear(value);
                RaisePropertyChanged();
            }
        }
        #endregion

        #region getMasterGear
        private Gear getMasterGear(Gear selectGear)
        {
            foreach(Gear m_gear in GearData_Master)
            {
                if (m_gear.Id == selectGear.Id)
                {
                    return m_gear;
                }
            }
            return selectGear;//普通ありえない.
        }
        #endregion

        #region SelectGearPowerCommand
        private ViewModelCommand _SelectGearPowerCommand;

        public ViewModelCommand SelectGearPowerCommand
        {
            get
            {
                if (_SelectGearPowerCommand == null)
                {
                    _SelectGearPowerCommand = new ViewModelCommand(SelectGearPower, CanSelectGearPower);
                }
                return _SelectGearPowerCommand;
            }
        }

        public bool CanSelectGearPower()
        {
            return this.SelectedGear != null;
        }

        public void SelectGearPower()
        {
            using (var vm = new GearPowerSelectViewModel(this.SelectedGear))
            {
                Messenger.Raise(new TransitionMessage(vm, "SelectCommand"));
                this.filtering();
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
            //CSVファイルに保存して、ウィンドウを閉じる
            //var list = new List<Gear>(GearData);

            if (IOmanager.WriteCSV(GearData_Master, FileName) == true)
            {
                Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
            }
        }
        #endregion

        #region BrandImg変更通知プロパティ
        private BitmapImage _BrandImg;
        public BitmapImage BrandImg
        {
            get
            {
                if ( filterBrandId <= 0 && filterBrandId >= IkaUtil.brands.Length)//0は[なし].
                {
                    return new BitmapImage();
                }
                else
                {
                    return IkaUtil.getBrandBitmapByImgName(IkaUtil.brands[filterBrandId].imgName);
                }
            }
            set
            {
                RaisePropertyChanged();
            }
        }
        #endregion

        ///<summary>コンボボックスに表示するギアパワー名のリスト</summary>
        #region BrandNames変更通知プロパティ
        private ObservableSynchronizedCollection<string> _BrandNames;
        //コンボボックスにギアパワー名を表示するためのなにか
        public ObservableSynchronizedCollection<string> BrandNames
        {
            get
            { return _BrandNames; }
            set
            {
                if (_BrandNames == value)
                    return;
                _BrandNames = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region filterBrandId
        private int _filterBrandId;
        public int filterBrandId
        {
            get
            { return _filterBrandId; }
            set
            {
                if (_filterBrandId == value)
                    return;
                _filterBrandId = value;
                BrandImg = new BitmapImage();//RaisePropertyChanged発火用.
                filtering();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region UnInputed
        private bool _UnInputed;
        public bool UnInputed
        {
            get
            { return _UnInputed; }
            set
            {
                if (_UnInputed == value)
                    return;
                _UnInputed = value;
                filtering();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region UnInputedNum
        private int _UnInputedNum;
        public int UnInputedNum
        {
            get
            { return _UnInputedNum; }
            set
            {
                if (_UnInputedNum == value)
                    return;
                if (value > 3 || value < 1)
                    return;
                _UnInputedNum = value;
                filtering();
                RaisePropertyChanged();
            }
        }
        #endregion

        private int filterF;

        private bool _filterAll;
        public bool filterAll
        {
            get
            {
                return _filterAll;
            }
            set
            {
                if (_filterAll == value)
                    return;
                _filterAll = value;
                filtering();
                RaisePropertyChanged();
            }
        }

        private bool _filterOnlyStrength;
        public bool filterOnlyStrength
        {
            get
            {
                return _filterOnlyStrength;
            }
            set
            {
                if (_filterOnlyStrength == value)
                    return;
                _filterOnlyStrength = value;
                filtering();
                RaisePropertyChanged();
            }
        }

        private bool _filterOnlyNotStrength;
        public bool filterOnlyNotStrength
        {
            get
            {
                return _filterOnlyNotStrength;
            }
            set
            {
                if (_filterOnlyNotStrength == value)
                    return;
                _filterOnlyNotStrength = value;
                filtering();
                RaisePropertyChanged();
            }
        }
    }
}
