using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

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
        /// <param name="saveFlag">セーブ成功フラグ</param>
        public GearEditViewModel(string fname)
        {
            this.FileName = fname;
        }

        ///<summary>ギアエディット画面の初期化</summary>
        public void Initialize()
        {
            GearData = new ObservableSynchronizedCollection<Gear>();
            var data = IOmanager.ReadCSV(FileName);
            for (int i = 0; i < data.Count; i++)
            {
                GearData.Add(data[i]);
            }
        }

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
                    return;
                _SelectedGear = value;
                RaisePropertyChanged();
            }
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
            var list = new List<Gear>(GearData);

            if (IOmanager.WriteCSV(list, FileName) == true)
            {
                Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
            }
        }
        #endregion


    }
}
