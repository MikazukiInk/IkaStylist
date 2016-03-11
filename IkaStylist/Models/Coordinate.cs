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
            this.points = new int[Enum.GetNames(typeof(GearPowerKind)).Length];
        }

        public Coordinate( Coordinate input )
        {
            //this.points = new int[Enum.GetNames(typeof(GearPowerKind)).Length];
            this.points = new int[Enum.GetNames(typeof(GearPowerKind)).Length];
            for (int i = 0; i < input.points.Length; i++)
            {
                this.points[i] = input.points[i];
            }
            points.CopyTo(input.points, 0);
            this._HeadGear   = new Gear(input.HeadGear());
            this._ClothGear  = new Gear(input.ClothGear());
            this._ShoesGear  = new Gear(input.ShoesGear());
            this._PowersText = input._PowersText;

        }

        public Coordinate( Gear head, Gear cloth, Gear shoes )
        {
            SetGears(head, cloth, shoes);
            this.points = new int[Enum.GetNames(typeof(GearPowerKind)).Length];
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
        	points[ gear.MainPower.GearPowerId ] += 10;   //メインは10ポイント
            points[ gear.SubPower1.GearPowerId ] += 3;    //サブは3ポイントとして計算
            points[ gear.SubPower2.GearPowerId ] += 3;
            points[ gear.SubPower3.GearPowerId ] += 3;
        }
            
    	// メンバ変数.
    	private Gear   _HeadGear;
    	private Gear   _ClothGear;
    	private Gear   _ShoesGear;
    	public  string _PowersText;
    	public  int[]  points;
    	
    	// API
    	public Gear HeadGear(){ return _HeadGear; }
    	public void HeadGear( Gear gear ){ _HeadGear = gear; }
    	public Gear ClothGear(){ return _ClothGear; }
    	public void ClothGear( Gear gear ){ _ClothGear = gear; }
    	public Gear ShoesGear(){ return _ShoesGear; }
    	public void ShoesGear( Gear gear ){ _ShoesGear = gear; }
    	public void SetGears( Gear head, Gear cloth, Gear shoes )
    	{
    		_HeadGear = head;
            _ClothGear = cloth;
            _ShoesGear = shoes;
    	}

		//Head Name
    	public string HeadGearName
        {
            get
            { return _HeadGear.Name; }
            set
            {
                if (_HeadGear.Name == value)
                    return;
                _HeadGear.Name = value;
                RaisePropertyChanged();
            }
        }
        //Head Name
    	public string ClothGearName
        {
            get
            { return _ClothGear.Name; }
            set
            {
                if (_ClothGear.Name == value)
                    return;
                _ClothGear.Name = value;
                RaisePropertyChanged();
            }
        }
        //Head Name
    	public string ShoesGearName
        {
            get
            { return _ShoesGear.Name; }
            set
            {
                if (_ShoesGear.Name == value)
                    return;
                _ShoesGear.Name = value;
                RaisePropertyChanged();
            }
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
    }
}//namespace IkaStylist.Models
