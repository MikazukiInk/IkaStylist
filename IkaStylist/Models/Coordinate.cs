using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;

using Livet;

namespace IkaStylist.Models
{
	///<summary>ギアパワーを管理するためのクラス</summary>
    public class Coordinate : NotificationObject
    {
    	public Coordinate()
        {
        	this.points = Enumerable.Repeat<int>(0, Enum.GetNames(typeof(GearPowerKind)).Length).ToArray();
            //this.points = new int[Enum.GetNames(typeof(GearPowerKind)).Length];
            this._IsTolerance = Enumerable.Repeat<bool>(false, Enum.GetNames(typeof(GearPowerKind)).Length).ToArray();
            //this._IsTolerance = new bool[Enum.GetNames(typeof(GearPowerKind)).Length];
        }

        public Coordinate( Coordinate input )
        {
        	this.points = Enumerable.Repeat<int>(0, Enum.GetNames(typeof(GearPowerKind)).Length).ToArray();
        	this._IsTolerance = Enumerable.Repeat<bool>(false, Enum.GetNames(typeof(GearPowerKind)).Length).ToArray();
            for (int i = 0; i < input.points.Length; i++)
            {
                this.points[i] = input.points[i];
            }
            for (int i = 0; i < input._IsTolerance.Length; i++)
            {
                this._IsTolerance[i] = input._IsTolerance[i];
            }
            points.CopyTo(input.points, 0);
            this.HeadGear   = new Gear(input.HeadGear);
            this.ClothGear  = new Gear(input.ClothGear);
            this.ShoesGear  = new Gear(input.ShoesGear);
            this._PowersText = input._PowersText;
            this.IsTolerance = input.IsTolerance;

        }

        public Coordinate( Gear head, Gear cloth, Gear shoes )
        {
        	this.points = Enumerable.Repeat<int>(0, Enum.GetNames(typeof(GearPowerKind)).Length).ToArray();
        	this._IsTolerance = Enumerable.Repeat<bool>(false, Enum.GetNames(typeof(GearPowerKind)).Length).ToArray();
            SetGears(head, cloth, shoes);
            CalcPoints();
        }

        public void CalcPoints()
        {
            addGearPowerPoint( _HeadGear );
            addGearPowerPoint( _ClothGear );
            addGearPowerPoint( _ShoesGear );
        }

        private void addGearPowerPoint( Gear gear )
        {
        	points[ gear.MainPower.Id ] += 10;   //メインは10ポイント
            points[ gear.SubPower1.Id ] += 3;    //サブは3ポイントとして計算
            points[ gear.SubPower2.Id ] += 3;
            points[ gear.SubPower3.Id ] += 3;
        }
            
    	// メンバ変数.
    	public  string _PowersText;
    	public  int[]  points;
    	
    	// API
    	private Gear   _HeadGear;
        public Gear HeadGear
        {
            get
            { return _HeadGear; }
            set
            {
                if (_HeadGear == value)
                    return;
                _HeadGear = value;
                RaisePropertyChanged();
            }
        }
        
        private Gear   _ClothGear;
        public Gear ClothGear
        {
            get
            { return _ClothGear; }
            set
            {
                if (_ClothGear == value)
                    return;
                _ClothGear = value;
                RaisePropertyChanged();
            }
        }
        
        private Gear   _ShoesGear;
        public Gear ShoesGear
        {
            get
            { return _ShoesGear; }
            set
            {
                if (_ShoesGear == value)
                    return;
                _ShoesGear = value;
                RaisePropertyChanged();
            }
        }
        
    	public void SetGears( Gear head, Gear cloth, Gear shoes )
    	{
    		HeadGear  = head;
            ClothGear = cloth;
            ShoesGear = shoes;
    	}

        //Total Points
        public int[] TotalPoint
        {
            get
            { return points; }
            set
            { 
                if (points == value)
                    return;
                points = value;
                RaisePropertyChanged();
            }
        }

        //許容値検索で抽出対象となったフラグ.
        private bool[] _IsTolerance;
        public bool[] IsTolerance
        {
            get
            { return _IsTolerance; }
        	set
        	{
        		if (_IsTolerance == value)
                    return;
                _IsTolerance = value;
                RaisePropertyChanged();
        	}
        }
    }
}//namespace IkaStylist.Models
