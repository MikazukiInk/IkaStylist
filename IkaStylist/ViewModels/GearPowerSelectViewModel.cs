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
    public class GearPowerSelectViewModel : ViewModel
    {
        const int SubPowerSize = 3;
        private Gear _Origin;

        public GearPowerSelectViewModel(Gear gear)
        {
            this.Count = 0;
            this.SubPower = new string[SubPowerSize];
            this.GearName += gear.Name;
            _Origin = gear;
        }

        public void Initialize()
        {
            Message = string.Format("{0}つめのギアパワーをえらんでください", this.Count + 1);

        }

        #region GearName変更通知プロパティ
        private string _GearName;

        public string GearName
        {
            get
            { return _GearName; }
            set
            {
                if (_GearName == value)
                    return;
                _GearName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SubPower変更通知プロパティ
        private string[] _SubPower = new string[SubPowerSize];

        public string[] SubPower
        {
            get
            { return _SubPower; }
            set
            {
                if (_SubPower == value)
                    return;
                _SubPower = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Count変更通知プロパティ
        private int _Count;

        public int Count
        {
            get
            { return _Count; }
            set
            {
                if (_Count == value)
                    return;
                _Count = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SelectCommand
        private ListenerCommand<string> _SelectCommand;

        public ListenerCommand<string> SelectCommand
        {
            get
            {
                if (_SelectCommand == null)
                {
                    _SelectCommand = new ListenerCommand<string>(Select);
                }
                return _SelectCommand;
            }
        }

        public void Select(string id)
        {
            string[] temp = (string[])this.SubPower.Clone();
            temp[this.Count] = Gear.IdToName(int.Parse(id));
            this.SubPower = temp;
            this.Count++;

            if (SubPowerSize <= this.Count)
            {
                _Origin.SubPower1.Name = this.SubPower[0];
                _Origin.SubPower2.Name = this.SubPower[1];
                _Origin.SubPower3.Name = this.SubPower[2];

                Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
            }
            else
            {
                Message = string.Format("{0}つめのギアパワーをえらんでください", this.Count + 1);
            }
        }
        #endregion

        #region CancelCommand
        private ListenerCommand<string> _CancelCommand;

        public ListenerCommand<string> CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new ListenerCommand<string>(Cancel);
                }
                return _CancelCommand;
            }
        }

        public void Cancel(string parameter)
        {
            this.Count -= int.Parse(parameter);
            if (this.Count < 0)
            {
                this.Count = 0;
            }
            Message = string.Format("{0}つめのギアパワーをえらんでください", this.Count + 1);
        }
        #endregion

        #region Message変更通知プロパティ
        private string _Message;

        public string Message
        {
            get
            { return _Message; }
            set
            {
                if (_Message == value)
                    return;
                _Message = value;
                RaisePropertyChanged("Message");
            }
        }
        #endregion

    }
}
