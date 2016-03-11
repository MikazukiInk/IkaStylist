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
    		_Id = 0;
    		_Name = "";
    		_ImagePath = "";
    	}
    	
		public GearPower( GearPower input )
    	{
    		this._Id = input.Id;
    		this._Name = input.Name;
    		this._ImagePath = input.ImagePath;
    	}
    	
        private int _Id;
    	public int Id
    	{
            get
            { return _Id; }
            set
            {
                if (_Id == value)
                    return;
                _Id = value;
                _Name = (string)IkaUtil.GearPowerDict[_Id];
                RaisePropertyChanged();
            }
        }

        private string _Name;
    	public string Name
    	{
            get
            { return _Name; }
            set
            {
                if (_Name == value)
                    return;
                _Name = value;
                if (IkaUtil.GearPowerDict.ContainsValue(value))
                {
                    _Id = (int)IkaUtil.GearPowerDict.First(x => x.Value == value).Key;
                }
                else
                {
                    //アボート対策.
                    _Id = 0;
                }
                RaisePropertyChanged();
            }
        }

        private string _ImagePath;//現在未使用
    	public string ImagePath
    	{
            get
            { return _ImagePath; }
            set
            {
                if (_ImagePath == value)
                    return;
                _ImagePath = value;
                RaisePropertyChanged();
            }
        }

    }
}//namespace IkaStylist.Models
