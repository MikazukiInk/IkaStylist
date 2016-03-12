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

        public List<Coordinate> CoordinateList = new List<Coordinate>();

        /// <summary>検索クラスの初期化
        /// 全パターンのギアパワー値を計算して持っておく
        /// </summary>
        /// <param name="head">アタマのギアリスト</param>
        /// <param name="cloth">フクのギアリスト</param>
        /// <param name="shoes">クツのギアリスト</param>
        public void Init(List<Gear> head, List<Gear> cloth, List<Gear> shoes)
        {
            /**とりあえず全組み合わせを列挙するんだぜ（物理）**/
            for (int h = 0; h < head.Count; h++)
            {
                for (int c = 0; c < cloth.Count; c++)
                {
                    for (int s = 0; s < shoes.Count; s++)
                    {
                        var temp = new Coordinate(head[h], cloth[c], shoes[s]);
                        this.CoordinateList.Add(temp);
                    }
                }
            }
            /***/
        }

        /// <summary>検索処理実行メソッド</summary>
        /// <param name="request">検索の要求値</param>
        /// <param name="onlyEnhanced">強化済みのみを使用するか？</param>
        /// <returns>ギアパワー配列のリスト</returns>
        public List<Coordinate> Start(Request[] request, bool onlyEnhanced = false)
        {
            var candidate = new List<Coordinate>(this.CoordinateList);

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
