/*
 * Major props to Greg for leting me use this!
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kf
{
    class MemBlock
    {
        byte[] data;
        int index;
        public MemBlock(byte[] d)
        {
            data = d;
            index = 0;
        }
        public void seek(int pos)
        {
            index = pos;
            if (index > data.Length - 1)
                index = data.Length - 1;
        }
        public void skip(int offset)
        {
            index = index + offset;
            if (index > data.Length - 1)
                index = data.Length - 1;
            if (index < 0)
                index = 0;
        }
        public int current()
        {
            return index;
        }
        public int size()
        {
            return data.Length;
        }
        public byte getU8()
        {
            if (index <= data.Length)
                return data[index++];
            return 0;
        }
        public ushort getU16()
        {
            if (index <= data.Length - 2)
            {
                ushort value = BitConverter.ToUInt16(data, index);
                index += 2;
                return value;
            }
            return 0;
        }
        public uint getU32()
        {
            if (index <= data.Length - 4)
            {
                uint value = BitConverter.ToUInt32(data, index);
                index += 4;
                return value;
            }
            return 0;
        }
        public sbyte getS8()
        {
            if (index <= data.Length)
                return (sbyte)data[index++];
            return 0;
        }
        public short getS16()
        {
            if (index <= data.Length - 2)
            {
                short value = BitConverter.ToInt16(data, index);
                index += 2;
                return value;
            }
            return 0;
        }
        public int getS32()
        {
            if (index <= data.Length - 4)
            {
                int value = BitConverter.ToInt32(data, index);
                index += 4;
                return value;
            }
            return 0;
        }
        public float getFloat()
        {
            if (index <= data.Length - 4)
            {
                float value = BitConverter.ToSingle(data, index);
                index += 4;
                return value;
            }
            return 0;
        }
        public double getDouble()
        {
            if (index <= data.Length - 4)
            {
                double value = BitConverter.ToDouble(data, index);
                index += 4;
                return value;
            }
            return 0;
        }

        public void setU8(byte value)
        {
            if (index <= data.Length)
            {
                data[index++] = value;
            }
        }
        public void setU16(ushort value)
        {
            if (index < data.Length - 2)
            {
                BitConverter.GetBytes(value).CopyTo(data, index);
                index += 2;
            }
        }
        public void setU32(uint value)
        {
            if (index <= data.Length - 4)
            {
                BitConverter.GetBytes(value).CopyTo(data, index);
                index += 4;
            }
        }
        public void setS8(sbyte value)
        {
            if (index <= data.Length)
            {
                data[index++] = (byte)value;
            }
        }
        public void setS16(short value)
        {
            if (index <= data.Length - 2)
            {
                BitConverter.GetBytes(value).CopyTo(data, index);
                index += 2;
            }
        }
        public void setS32(int value)
        {
            if (index <= data.Length - 4)
            {
                BitConverter.GetBytes(value).CopyTo(data, index);
                index += 4;
            }
        }
        public void setFloat(float value)
        {
            if (index <= data.Length - 4)
            {
                BitConverter.GetBytes(value).CopyTo(data, index);
                index += 4;
            }
        }
        public void setDouble(double value)
        {
            if (index <= data.Length - 8)
            {
                BitConverter.GetBytes(value).CopyTo(data, index);
                index += 8;
            }
        }
    }
}