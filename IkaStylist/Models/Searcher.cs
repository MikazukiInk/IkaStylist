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
        private OptManager OptMgr;
        public Searcher(OptManager opt)
        {
        	OptMgr = opt;
        }
        
        ///<summary> </summary>

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
        public List<Coordinate> Start()
        {
            var candidate = new List<Coordinate>(this.CoordinateList);

            //「なし」2個までは許容する。それ以上は除外する。
            if (OptMgr.OnlyEnhanced)
            {
                candidate.RemoveAll(x => x.points[0] > 8);
            }

            for (int i = 0; i < OptMgr.Requests.Length; i++)
            {
                if (OptMgr.Requests[i].GearPowerID != 0)
                {
                    candidate.RemoveAll(x => x.points[OptMgr.Requests[i].GearPowerID] < (OptMgr.Requests[i].Point - OptMgr.Tolerance));
                    for (int ci = candidate.Count - 1; ci >= 0; ci--)
                    {
                        if (OptMgr.Tolerance > 0)
                        {
                            if (candidate[ci].points[OptMgr.Requests[i].GearPowerID] < OptMgr.Requests[i].Point)
                            {
                                candidate[ci].IsTolerance[OptMgr.Requests[i].GearPowerID] = true;
                            }
                            else
                            {
                                candidate[ci].IsTolerance[OptMgr.Requests[i].GearPowerID] = false;
                            }
                        }
                        else
                        {
                            candidate[ci].IsTolerance = Enumerable.Repeat<bool>(false, Enum.GetNames(typeof(GearPowerKind)).Length).ToArray();
                        }
                    }

                }
                if (candidate.Count <= 1)
                {
                    break;
                }
            }
            return candidate;
        }

    }
}
