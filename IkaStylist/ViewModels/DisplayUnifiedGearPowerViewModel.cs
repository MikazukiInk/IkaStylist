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
    public class DisplayUnifiedGearPowerViewModel : ViewModel
    {
        public DisplayUnifiedGearPowerViewModel()
        {
        	
        }

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

        public void Initialize()
        {
            makePartsNames();
        }

        private void makePartsNames()
        {
            //makePartsNames初期化
            var temp = new List<string>();
            temp.Add("アタマ");
            temp.Add("フク");
            temp.Add("クツ");
            this.PartsNames = temp;
        }

        #region BrandNames変更通知プロパティ
        private List<string> _PartsNames;
        //コンボボックスにギアパワー名を表示するためのなにか
        public List<string> PartsNames
        {
            get
            { return _PartsNames; }
            set
            {
                if (_PartsNames == value)
                    return;
                _PartsNames = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region
        private int _selectParts;
        public int selectParts
        {
            get
            { return _selectParts; }
            set
            {
                if (_selectParts == value)
                    return;
                _selectParts = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        private bool _pureBrand;
        public bool pureBrand
        {
            get
            {
                return _pureBrand;
            }
            set
            {
                if (_pureBrand == value)
                    return;
                _pureBrand = value;
                RaisePropertyChanged();
            }
        }

        private int _pureBrandNum;
        public int pureBrandNum
        {
            get
            {
                return _pureBrandNum;
            }
            set
            {
                if (_pureBrandNum == value)
                    return;
                if (value < 2 || value > 3)
                    return;
                _pureBrandNum = value;
                RaisePropertyChanged();
            }
        }

        private bool _fakeBrand;
        public bool fakeBrand
        {
            get
            {
                return _fakeBrand;
            }
            set
            {
                if (_fakeBrand == value)
                    return;
                _fakeBrand = value;
                RaisePropertyChanged();
            }
        }

        private int _fakeBrandNum;
        public int fakeBrandNum
        {
            get
            {
                return _fakeBrandNum;
            }
            set
            {
                if (_fakeBrandNum == value)
                    return;
                if (value < 1 || value > 3)
                    return;
                _fakeBrandNum = value;
                RaisePropertyChanged();
            }
        }

        #region DisplayResultCommand
        private ListenerCommand<string> _DisplayResultCommand;

        public ListenerCommand<string> DisplayResultCommand
        {
            get
            {
                if (_DisplayResultCommand == null)
                {
                    _DisplayResultCommand = new ListenerCommand<string>(DisplayResult);
                }
                return _DisplayResultCommand;
            }
        }

        public void DisplayResult(string parameter)
        {
            using (var vm = new DisplayResultUnifiedGearViewModel())
            {
                Messenger.Raise(new TransitionMessage(vm, "DisplayResultCommand"));
            }
        }
        #endregion

    }
}
