using System;
using System.Collections.Generic;
using UnityEngine;

namespace script.Buffer
{
    //数值，可以进行增益的数值
    //使用列表存储增益项
    //增益项（名称，说明，类型【+-*/】，计算数）
    [Serializable]
    public class BufferValue
    {
        
        public decimal BaseVal { get; set; }//初始值
       
        public decimal ResultVal        //增益计算后的值
        {
            get
            {
                var val = BaseVal;
                foreach (var unit in _units)
                {
                    val = unit.Cal(val);
                }
                return val;
            }
        } //结算伤害
        private List<BufferUnit> _units;

        public float GetResultValF()
        {
            return (float)ResultVal;
        }
        public BufferValue( int baseVal,List<BufferUnit> units)
        {
            _units = units;
            BaseVal = baseVal;
        }

        public static  implicit operator BufferValue(int baseVal)
        {
            return new BufferValue(baseVal);
        }
        
        public static  implicit operator BufferValue(float baseVal)
        {
            return new BufferValue(baseVal);
        }
        
        public BufferValue(int baseVal)
        {
            BaseVal = baseVal;
            _units = new List<BufferUnit>();
        }
        
        public BufferValue(float baseVal)
        {
            BaseVal =(decimal) baseVal;
            _units = new List<BufferUnit>();
        }

        public BufferValue Clone()
        {
            BufferValue clone = new BufferValue((int) BaseVal, _units);
            return clone;
        }
        public void AddBufferUnit(BufferUnit unit)
        {
            _units.Add(unit);
        }
    }
}