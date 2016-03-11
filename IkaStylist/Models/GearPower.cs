using System;
using System.Windows;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Livet;

namespace IkaStylist.Models
{
	///<summary>ギアパワーを管理するためのクラス</summary>
    public class GearPower : NotificationObject
    {
    	public GearPower()
    	{
    		_GearPowerId = 0;
    		_GearPowerName = "";
    		_GearPowerImagePath = "";
    	}
    	
		public GearPower( GearPower input )
    	{
    		this._GearPowerId = input.GearPowerId;
    		this._GearPowerName = input.GearPowerName;
    		this._GearPowerImagePath = input.GearPowerImagePath;
    	}
    	
        private int _GearPowerId;
    	public int GearPowerId
    	{
            get
            { return _GearPowerId; }
            set
            {
                if (_GearPowerId == value)
                    return;
                _GearPowerId = value;
                _GearPowerName = (string)IkaUtil.GearPowerDict[_GearPowerId];
                RaisePropertyChanged();
            }
        }

        private string _GearPowerName;
    	public string GearPowerName
    	{
            get
            { return _GearPowerName; }
            set
            {
                if (_GearPowerName == value)
                    return;
                _GearPowerName = value;
                if (IkaUtil.GearPowerDict.ContainsValue(value))
                {
                    _GearPowerId = (int)IkaUtil.GearPowerDict.First(x => x.Value == value).Key;
                }
                else
                {
                    //アボート対策.
                    _GearPowerId = 0;
                }
                RaisePropertyChanged();
            }
        }

        private string _GearPowerImagePath;//現在未使用
    	public string GearPowerImagePath
    	{
            get
            { return _GearPowerImagePath; }
            set
            {
                if (_GearPowerImagePath == value)
                    return;
                _GearPowerImagePath = value;
                RaisePropertyChanged();
            }
        }

    }
}//namespace IkaStylist.Models
