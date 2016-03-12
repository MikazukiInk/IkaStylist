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

        #region Id変更通知プロパティ
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
                Name = (string)IkaUtil.GearPowerDict[_Id];
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
                _Name = value;

                if (IkaUtil.GearPowerDict.ContainsValue(value))
                {
                    Id = (int)IkaUtil.GearPowerDict.First(x => x.Value == value).Key;
                }
                else
                {
                    //アボート対策.
                    Id = 0;
                }
                RaisePropertyChanged();
            }
        }
        #endregion

        //未使用
        #region ImagePath変更通知プロパティ
        private string _ImagePath;

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
        #endregion

    }
}//namespace IkaStylist.Models
