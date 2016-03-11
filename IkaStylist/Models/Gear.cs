using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using System.Text;

using Livet;

namespace IkaStylist.Models
{
    ///<summary>ギアを管理するためのクラス</summary>
    public class Gear : NotificationObject
    {
    	public Gear()
    	{
    		this._GearId = -1;
            this._updateFlag = 0;
    		this._Name = "no input";
    		this._MainPower = new GearPower();
    		this._SubPower1 = new GearPower();
    		this._SubPower2 = new GearPower();
    		this._SubPower3 = new GearPower();
    	}

    	public Gear( Gear input )
    	{
    		this._GearId = input.GearId;
            this._updateFlag = input._updateFlag;
    		this._Name = input.Name;
    		this._MainPower = new GearPower(input.MainPower);
    		this._SubPower1 = new GearPower(input.SubPower1);
    		this._SubPower2 = new GearPower(input.SubPower2);
    		this._SubPower3 = new GearPower(input.SubPower3);
    	}
    	
        static public string IdToName(int id)
        {
            return EnumLiteralAttribute.GetLiteral( (GearPowerKind)(id) );
        }

        //update
        private int _updateFlag;
        public int updateFlag
        {
            get
            { return _updateFlag; }
            set
            {
                if (_updateFlag == value)
                    return;
                _updateFlag = _updateFlag + 1;
                RaisePropertyChanged();
            }
        }

        #region Id変更通知プロパティ
        private int _GearId;

        public int GearId
        {
            get
            { return _GearId; }
            set
            {
                if (_GearId == value)
                    return;
                _GearId = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Name変更通知プロパティ
        private string _Name;

        public string Name
        {
            get
            { return _Name; }
            set
            {
                if (_Name == value)
                    return;
                _Name   = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region MainPower変更通知プロパティ
        private GearPower _MainPower;

        public GearPower MainPower
        {
            get
            { return _MainPower; }
            set
            {
                if (_MainPower == value)
                    return;
                _MainPower = value;
                RaisePropertyChanged();
            }
        }

        public string MainPowerName
        {
            get
            { return _MainPower.GearPowerName; }
            set
            {
                if (_MainPower.GearPowerName == value)
                    return;
                _MainPower.GearPowerName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SubPower1変更通知プロパティ
        private GearPower _SubPower1;

        public GearPower SubPower1
        {
            get
            { return _SubPower1; }
            set
            {
                if (_SubPower1 == value)
                    return;
                _SubPower1 = value;
                RaisePropertyChanged();
            }
        }

        public string SubPower1Name
        {
            get
            { return _SubPower1.GearPowerName; }
            set
            {
                if (_SubPower1.GearPowerName == value)
                    return;
                _SubPower1.GearPowerName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SubPower2変更通知プロパティ
        private GearPower _SubPower2;

        public GearPower SubPower2
        {
            get
            { return _SubPower2; }
            set
            {
                if (_SubPower2 == value)
                    return;
                _SubPower2 = value;
                RaisePropertyChanged();
            }
        }

        public string SubPower2Name
        {
            get
            { return _SubPower2.GearPowerName; }
            set
            {
                if (_SubPower2.GearPowerName == value)
                    return;
                _SubPower2.GearPowerName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SubPower3変更通知プロパティ
        private GearPower _SubPower3;

        public GearPower SubPower3
        {
            get
            { return _SubPower3; }
            set
            {
                if (_SubPower3 == value)
                    return;
                _SubPower3 = value;
                RaisePropertyChanged();
            }
        }

        public string SubPower3Name
        {
            get
            { return _SubPower3.GearPowerName; }
            set
            {
                if (_SubPower3.GearPowerName == value)
                    return;
                _SubPower3.GearPowerName = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}//namespace IkaStylist.Models
