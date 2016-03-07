using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace IkaStylist.Models
{
    ///<summary>コーデの検索を行うクラス</summary>
    public class Searcher : NotificationObject
    {
        ///<summary>リクエストの最大サイズ</summary>
        const int MaxRequestSize = 5;
        ///<summary> </summary>
        Request[] Requests = new Request[MaxRequestSize];

        public List<Gear.TotalPoints> totalPointsList;

        /// <summary>検索クラスの初期化
        /// 全パターンのギアパワー値を計算して持っておく
        /// </summary>
        /// <param name="head">アタマのギアリスト</param>
        /// <param name="cloth">フクのギアリスト</param>
        /// <param name="shoes">クツのギアリスト</param>
        public void Init(List<Gear> head, List<Gear> cloth, List<Gear> shoes)
        {
            var h_points = new List<Gear.Points>();
            var c_points = new List<Gear.Points>();
            var s_points = new List<Gear.Points>();
            this.totalPointsList = new List<Gear.TotalPoints>();
            int offset = 0;

            for (int i = 0; i < head.Count; i++)
            {
                h_points.Add(new Gear.Points(head[i]));
            }
            for (int i = 0; i < cloth.Count; i++)
            {
                c_points.Add(new Gear.Points(cloth[i]));
            }
            for (int i = 0; i < shoes.Count; i++)
            {
                s_points.Add(new Gear.Points(shoes[i]));
            }

            /**とりあえず全組み合わせを列挙するんだぜ（物理）**/
            for (int h = 0; h < head.Count; h++)
            {
                for (int c = 0; c < cloth.Count; c++)
                {
                    for (int s = 0; s < shoes.Count; s++)
                    {
                        var temp = new Gear.TotalPoints(h_points[h] + c_points[c] + s_points[s]);
                        this.totalPointsList.Add(temp);
                        this.totalPointsList[offset].HeadID = h;
                        this.totalPointsList[offset].ClothID = c;
                        this.totalPointsList[offset].ShoesID = s;
                        offset++;
                    }
                }
            }
            /***/
        }

        /// <summary>検索処理実行メソッド</summary>
        /// <param name="request">検索の要求値</param>
        /// <returns>ギアパワー配列のリスト</returns>
        public List<Gear.TotalPoints> Start(Request[] request)
        {
            var candidate = new List<Gear.TotalPoints>(this.totalPointsList);
            for (int i = 0; i < request.Length; i++)
            {
                if (request[i].GearPowerID != 0)
                {
                    candidate.RemoveAll(x => x.points[request[i].GearPowerID] < request[i].Point);
                }
                if (candidate.Count <= 1)
                {
                    break;
                }
            }
            return candidate;
        }

        /// <summary>検索処理実行メソッド</summary>
        /// <param name="request">検索の要求値</param>
        /// <param name="onlyEnhanced">強化済みのみを使用するか？</param>
        /// <returns>ギアパワー配列のリスト</returns>
        public List<Gear.TotalPoints> Start(Request[] request, bool onlyEnhanced)
        {
            var candidate = new List<Gear.TotalPoints>(this.totalPointsList);

            //「なし」2個までは許容する。それ以上は除外する。
            if(onlyEnhanced)
            {
                candidate.RemoveAll(x => x.points[0] > 8);
            }

            for (int i = 0; i < request.Length; i++)
            {
                if (request[i].GearPowerID != 0)
                {
                    candidate.RemoveAll(x => x.points[request[i].GearPowerID] < request[i].Point);
                }
                if (candidate.Count <= 1)
                {
                    break;
                }
            }
            return candidate;
        }

    }

    public class Request : NotificationObject
    {
        public Request()
        {
            Point = 0;
            GearPowerID = 0;
        }

        #region Point変更通知プロパティ
        private int _Point;

        public int Point
        {
            get
            { return _Point; }
            set
            {
                if (_Point == value)
                    return;
                _Point = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region GearPowerID変更通知プロパティ
        private int _GearPowerID;

        public int GearPowerID
        {
            get
            { return _GearPowerID; }
            set
            {
                if (_GearPowerID == value)
                    return;
                _GearPowerID = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
