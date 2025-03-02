using System;
using System.Collections;

namespace WeDonekRpc.Helper.BloomFilter
{
    public class BloomFilter
    {
        private readonly int _bitSize = 0;
        private readonly int _hashNum = 8;
        private readonly int _setSize;
        private readonly BitArray _bitArray;

        #region Constructors
        /// <summary>
        /// 初始化bloom滤波器并设置hash散列的最佳数目
        /// </summary>
        /// <param name="bitSize">布隆过滤器的大小(m)</param>
        /// <param name="setSize">集合的大小 (n)</param>
        public BloomFilter ( int size, int accuracyRate = 1, int hashNum = 8 )
        {
            if ( hashNum > 8 )
            {
                hashNum = 8;
            }
            this._hashNum = hashNum;
            this._setSize = size;
            this._bitSize = this._GetBitArrayNum(accuracyRate);
            if ( this._bitSize < 0 )
            {
                this._bitSize = int.MaxValue;
            }
            this._bitArray = new BitArray(this._bitSize);
        }
        #endregion

        #region 属性
        public int HashNum => this._hashNum;
        public int SetSize => this._setSize;
        public int BitSize => this._bitSize;
        #endregion

        #region 公共方法
        public void Add ( string item )
        {
            for ( int i = 0 ; i < this._hashNum ; i++ )
            {
                this._bitArray[this.Hash(item, i)] = true;
            }
        }
        public bool Contains ( string item )
        {
            for ( int i = 0 ; i < this._hashNum ; i++ )
            {
                int num = this.Hash(item, i);
                if ( !this._bitArray[num] )
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查列表中的任何项是否可能是在集合。
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool ContainsAny ( string[] items )
        {
            return Array.FindIndex(items, a =>
            {
                return this.Contains(a);
            }) != -1;
        }

        //检查列表中的所有项目是否都在集合。
        public bool ContainsAll ( string[] items )
        {
            return Array.TrueForAll(items, a =>
            {
                return this.Contains(a);
            });
        }

        /// <summary>
        /// 计算遇到误检率的概率。
        /// </summary>
        /// <returns>Probability of a false positive</returns>
        public double FalsePositiveProbability ()
        {
            return Math.Pow(1 - Math.Exp(-this._hashNum * this._setSize / (double)this._bitSize), this._hashNum);
        }
        #endregion

        #region 私有方法
        private int Hash ( string str, int index )
        {
            string name = HashCodeHelper.HashList[index % 9];
            int code = HashCodeHelper.Hash(str, name);
            return Math.Abs(code % ( this._bitSize / 8 ) - 1);
        }

        //计算基于布隆过滤器散列的最佳数量
        private int _GetBitArrayNum ( int accuracyRate )
        {
            return (int)Math.Ceiling(this._hashNum * this._setSize / ( accuracyRate / 100.0 ));
        }
        #endregion

    }
}
