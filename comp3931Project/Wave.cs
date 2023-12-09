using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace comp3931Project
{
    /**
     * Class that represents a wave. Contains header information
     */
    public class Wave
    {
        // HEADER = 44 bytes

        // RIFF Chunk
        private int ChunkID;
        private int ChunkSize;
        private int Format;

        // FORMAT Chunk
        private int FMTID;
        private int FMTSize;
        private short FMTFormatTag;
        private short FMTChannels;
        private int FMTSampleRate;
        private int FMTByteRate;
        private short FMBlock;
        private short FMTBPS;

        // DATA Chunk
        private int DataID;
        private int DataSize;
        //private Byte[] LeftChannel;
        //private Byte[] RightChannel;
        private double[] R;
        private double[] L;
        private byte[] Data;

        /**
         * Wave constructor
         */
        public Wave()
        {
        }

        /**
         * Purpose: Reads the opened wav file
         * 
         * @param filename: The name of the file
         * 
         * @return: None
         */
        public void ReadWavFile(String filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);

            BinaryReader reader = new BinaryReader(fs);

            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            // Reads the header information from the file
            this.ChunkID = reader.ReadInt32();
            this.ChunkSize = reader.ReadInt32();
            this.Format = reader.ReadInt32();

            this.FMTID = reader.ReadInt32();
            this.FMTSize = reader.ReadInt32();
            this.FMTFormatTag = reader.ReadInt16();
            this.FMTChannels = reader.ReadInt16();
            this.FMTSampleRate = reader.ReadInt32();
            this.FMTByteRate = reader.ReadInt32();
            this.FMBlock = reader.ReadInt16();
            this.FMTBPS = reader.ReadInt16();
           


            this.DataID = reader.ReadInt32();
            this.DataSize = reader.ReadInt32();

            int bytesPerSample = FMTBPS / 8;
            int samples = DataSize / bytesPerSample;

            // possibly read all data into a buffer then process into the proper format, then split channels

            byte[] buffer = new byte[DataSize];
            buffer = reader.ReadBytes(DataSize);
            Data = new byte[DataSize];
            for (int i = 0; i < DataSize; i++)

            {
                Data[i] = buffer[i];
            }

           // buffer containing amplitudes as bytes

            double[] doubleArr;

            switch (this.FMTBPS)
            {
                case 8:
                    byte[] byteBuffer = new byte[DataSize];
                    Buffer.BlockCopy(buffer, 0, byteBuffer, 0, DataSize);
                    doubleArr = new double[byteBuffer.Length];
                    for (int i = 0; i < byteBuffer.Length; i++)
                    {
                        doubleArr[i] = Convert.ToDouble(byteBuffer[i]);
                    }
                    break;
                case 16:
                    short[] shortBuffer = new short[DataSize / 2];
                    Buffer.BlockCopy(buffer, 0, shortBuffer, 0, DataSize);
                    doubleArr = new double[shortBuffer.Length];
                    for (int i = 0; i < shortBuffer.Length; i++)
                    {
                        doubleArr[i] = Convert.ToDouble(shortBuffer[i]);
                    }
                    break;
                case 32:
                    int[] intBuffer = new int[DataSize / 4];
                    Buffer.BlockCopy(buffer, 0, intBuffer, 0, DataSize);
                    doubleArr = new double[intBuffer.Length];
                    for (int i = 0; i < intBuffer.Length; i++)
                    {
                        doubleArr[i] = Convert.ToDouble(intBuffer[i]);
                    }
                    break;
                default:
                    //maybe pop an error message?
                    throw new Exception("Difficulty Reading File.");
            }


            if (FMTChannels == 1)
            {
                this.L = new double[samples];
                this.L = doubleArr;
            }
            else
            {
                this.L = new double[samples / 2];
                this.R = new double[samples / 2];


                for (int i = 0, interleavedValue = 0; i < doubleArr.Length; i++)
                {
                    this.L[i] = doubleArr[interleavedValue++];
                    this.R[i] = doubleArr[interleavedValue++];
                }

            }
        }

        /**
         * Purpose: Reads the byte array
         * 
         * @param bArr: The byte array with data from the buffer
         * 
         * @return: None
         */
        public void readByteArr(byte[] bArr)
        {

        {
            using (MemoryStream memoryStream = new MemoryStream(bArr))
            using (BinaryReader reader = new BinaryReader(memoryStream))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);

                this.ChunkID = 1179011410;
                this.ChunkSize = bArr.Length + 44;
                this.Format = 1163280727;

                this.FMTID = 544501094;
                this.FMTSize = 16;
                this.FMTFormatTag = (short)wf.wFormatTag;
                this.FMTChannels = (short)wf.nChannels;
                this.FMTSampleRate = (int)wf.nSamplesPerSec;
                this.FMTByteRate = (int)wf.nAvgBytesPerSec;
                this.FMBlock = (short)wf.nBlockAlign;
                this.FMTBPS = (short)wf.wBitsPerSample;
                this.DataID = 1635017060;

                this.DataSize = intValue;

                this.Data = new byte[DataSize];
                for (int i = 0; i < DataSize; i++)
                {
                    Data[i] = bArr[i];
                }

           
                int bytesPerSample = FMTBPS / 8;
                int samples = DataSize / bytesPerSample;

                double[] doubleArr = bArr.Select(b => Convert.ToDouble(b)).ToArray();

                this.L = new double[this.DataSize];
               
                    this.L = doubleArr;
               
            }
        }
    

    public double[] getL()
        {
            return L;
        }

        public int getDataSize()
        {
            return this.DataSize;
        }

        /**
         * Purpose: Returns data from the left channel
         * 
         * @return: A double array containing left channel data
         */
        public double[] getL()
        {
            return Data;
        }

        /**
         * Purpose: Returns data from the right channel
         * 
         * @return: A double array containing right channel data
         */
        public double[] getR()
        {
            return R;
        }

        /**
         * Purpose: Returns the sample rate of the wave
         * 
         * @return: The sample rate
         */
        public int getSampleRate()
        {
            return FMTSampleRate;
        }

        /**
         * Purpose: Writes a new wav file
         * 
         * @param filename: The name of the file
         * 
         * @return: None
         */
        public void WriteWavFile(String filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(fs);

            writer.Write(this.ChunkID);
            writer.Write(this.ChunkSize);
            writer.Write(this.Format);
            writer.Write(this.FMTID);
            writer.Write(this.FMTSize);
            writer.Write(this.FMTFormatTag);
            writer.Write(this.FMTChannels);
            writer.Write(this.FMTSampleRate);
            writer.Write(this.FMTByteRate);
            writer.Write(this.FMBlock);
            writer.Write(this.FMTBPS);
            writer.Write(this.DataID);
            writer.Write(this.DataSize);

            switch (this.FMTBPS)
            {
                case 8:
                    for (int i = 0; i < this.L.Length; i++)
                    {
                        writer.Write((byte)this.L[i]);
                        if (this.FMTChannels == 2)
                        {
                            writer.Write((byte)this.R[i]);
                        }
                    }
                    break;
                case 16:
                    for (int i = 0; i < this.L.Length; i++)
                    {
                        writer.Write((Int16)this.L[i]);
                        if (this.FMTChannels == 2)
                        {
                            writer.Write((Int16)this.R[i]);
                        }
                    }
                    break;
                case 32:
                    for (int i = 0; i < this.L.Length; i++)
                    {
                        writer.Write(this.L[i]);
                        if (this.FMTChannels == 2)
                        {
                            writer.Write(this.R[i]);
                        }
                    }
                    break;
                default:
                    break;
            }
            fs.Close();
        }
    }
}
